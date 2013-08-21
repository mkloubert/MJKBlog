using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

/// <summary>
/// Implementation von <see cref="IWcfHttpServer" />
/// </summary>
[ServiceBehavior(AddressFilterMode = AddressFilterMode.Prefix,
                 InstanceContextMode = InstanceContextMode.Single,
                 ConcurrencyMode = ConcurrencyMode.Multiple)]
public sealed class WcfHttpServer : IWcfHttpServer
{
    #region Fields (1)

    private readonly MessageEncoder _WEB_ENCODER = CreateWebMessageBindingEncoder().CreateMessageEncoderFactory().Encoder;

    #endregion Fields

    #region Methods (2)

    // Public Methods (2) 

    /// <summary>
    /// Erstellt ein vorkonfiguriertes
    /// <see cref="WebMessageEncodingBindingElement" />.
    /// </summary>
    /// <returns>Das vorkonfigurierte Objekt.</returns>
    public static WebMessageEncodingBindingElement CreateWebMessageBindingEncoder()
    {
        var encoding = new WebMessageEncodingBindingElement();
        encoding.MaxReadPoolSize = int.MaxValue;
        encoding.ContentTypeMapper = new RawContentTypeMapper();
        encoding.ReaderQuotas.MaxArrayLength = int.MaxValue;

        return encoding;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <see cref="IWcfHttpServer.Request(Message)" />
    public Message Request(Message message)
    {
        using (var uncompressedResponse = new MemoryStream())
        {
            var request = (HttpRequestMessageProperty)message.Properties[HttpRequestMessageProperty.Name];
            var response = new HttpResponseMessageProperty();

            // HTTP-Methode: bspw. GET oder POST
            var method = request.Method;

            // Kopfdaten der Anfrage
            var requestHeaders = new Dictionary<string, string>();
            foreach (var key in request.Headers.AllKeys)
            {
                requestHeaders[key] = request.Headers[key];
            }

            // Rohdaten der Anfrage (nur Body) ermitteln
            byte[] requestBody;
            using (var requestStream = new MemoryStream())
            {
                this._WEB_ENCODER.WriteMessage(message, requestStream);

                requestBody = requestStream.ToArray();
            }

            // Beispiel: Antwort definieren
            byte[] responseData;
            {
                // eigene Kopfdaten definieren
                {
                    //TODO: Dictionary füllen
                    var responseHeaders = new Dictionary<string, string>();

                    foreach (var item in responseHeaders)
                    {
                        response.Headers[item.Key] = item.Value;
                    }
                }

                // Beispiel HTML-Ausgabe
                {
                    var html = new StringBuilder().Append("<html>")
                                                  .Append("<body>")
                                                  .AppendFormat("Hallo, es ist: {0}",
                                                                DateTimeOffset.Now)
                                                  .Append("</body>")
                                                  .Append("</html>");

                    var utf8Html = Encoding.UTF8
                                           .GetBytes(html.ToString());

                    uncompressedResponse.Write(utf8Html, 0, utf8Html.Length);

                    response.Headers[HttpResponseHeader.ContentType]
                        = "text/html; charset=utf-8";
                }

                // komprimieren?
                var compress = true;
                if (compress)
                {
                    // mit GZIP komprimieren

                    using (var compressedResponse = new MemoryStream())
                    {
                        using (var gzip = new GZipStream(compressedResponse,
                                                         CompressionMode.Compress))
                        {
                            long oldPos = uncompressedResponse.Position;
                            try
                            {
                                uncompressedResponse.Position = 0;

                                var buffer = new byte[81920];
                                int bytesRead;
                                while ((bytesRead = uncompressedResponse.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    gzip.Write(buffer, 0, bytesRead);
                                }
                            }
                            finally
                            {
                                uncompressedResponse.Position = oldPos;
                            }

                            gzip.Flush();
                            gzip.Close();

                            responseData = compressedResponse.ToArray();
                        }
                    }

                    response.Headers[HttpResponseHeader.ContentEncoding] = "gzip";
                }
                else
                {
                    responseData = uncompressedResponse.ToArray();
                }
            }

            // HTTP-Status Code (hier: 200)
            response.StatusCode = HttpStatusCode.OK;

            // WCF-Antwort erstellen
            var responseMessage = new BinaryMessage(responseData);
            responseMessage.Properties[HttpResponseMessageProperty.Name] = response;

            return responseMessage;
        }
    }

    #endregion Methods
}