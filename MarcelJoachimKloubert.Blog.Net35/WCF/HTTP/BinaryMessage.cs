// s. http://blog.marcel-kloubert.de


using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Xml;

namespace MarcelJoachimKloubert.Blog.Net.HTTP
{
    /// <summary>
    /// Eine binäre <see cref="Message" />.
    /// </summary>
    public sealed class BinaryMessage : Message
    {
        #region Fields (2)

        private MessageHeaders _HEADERS;
        private MessageProperties _PROPERTIES;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="BinaryMessage"/> Klasse.
        /// </summary>
        /// <param name="data">
        /// Die Daten für die <see cref="BinaryMessage.Data" /> Eigenschaft.
        /// </param>
        public BinaryMessage(IEnumerable<byte> data)
        {
            if (data != null)
            {
                this.Data = data is byte[] ? (byte[])data : data.ToArray();
            }

            this._HEADERS = new MessageHeaders(MessageVersion.None);

            this._PROPERTIES = new MessageProperties();
            this.Properties
                .Add(WebBodyFormatMessageProperty.Name,
                     new WebBodyFormatMessageProperty(WebContentFormat.Raw));
        }

        #endregion Constructors

        #region Properties (4)

        /// <summary>
        /// Gets the zugrundliegenden Daten zurück.
        /// </summary>
        public byte[] Data
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Message.Headers" />
        public override MessageHeaders Headers
        {
            get { return this._HEADERS; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Message.Properties" />
        public override MessageProperties Properties
        {
            get { return this._PROPERTIES; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Message.Version" />
        public override MessageVersion Version
        {
            get { return MessageVersion.None; }
        }

        #endregion Properties

        #region Methods (1)

        // Protected Methods (1) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Message.OnWriteBodyContents(XmlDictionaryWriter)" />
        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            var writeState = writer.WriteState;

            if (writeState == WriteState.Start)
            {
                writer.WriteStartElement("Binary");
            }

            var data = this.Data ?? new byte[0];
            writer.WriteBase64(data, 0, data.Length);

            if (writeState == WriteState.Start)
            {
                writer.WriteEndElement();
            }
        }

        #endregion Methods
    }
}
