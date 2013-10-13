// s. http://blog.marcel-kloubert.de



using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MarcelJoachimKloubert.Blog.Diagnostics
{
    /// <summary>
    /// Eine <see cref="ILoggerFacade" />, die in eine XML-Datei schreibt.
    /// </summary>
    public sealed class XmlFileLogger : LoggerFacadeBase
    {
        #region Fields (1)

        /// <summary>
        /// Name des Attributs, das den Inhaltstyps eines Log-Elements definiert.
        /// </summary>
        public const string LOG_ATTRIB_MIME = "mime";

        #endregion Fields

        #region Constructors (6)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="XmlFileLogger" />.
        /// </summary>
        /// <param name="logDir">
        /// Der Wert für die <see cref="XmlFileLogger.LogDirectory" /> Eigenschaft.
        /// </param>
        /// <param name="filePrefix">
        /// Der Wert für die <see cref="XmlFileLogger.FilePrefix" /> Eigenschaft.
        /// </param>
        /// <param name="fileSuffix">
        /// Der Wert für die <see cref="XmlFileLogger.FileSuffix" /> Eigenschaft.
        /// </param>
        public XmlFileLogger(IEnumerable<char> logDir,
                             IEnumerable<char> filePrefix,
                             IEnumerable<char> fileSuffix)
            : base(true)
        {
            this.LogDirectory = Path.GetFullPath(AsString(logDir));
            this.FilePrefix = AsString(filePrefix);
            this.FileSuffix = AsString(fileSuffix);
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="XmlFileLogger" />.
        /// </summary>
        /// <param name="logDir">
        /// Der Wert für die <see cref="XmlFileLogger.LogDirectory" /> Eigenschaft.
        /// </param>
        /// <param name="filePrefix">
        /// Der Wert für die <see cref="XmlFileLogger.FilePrefix" /> Eigenschaft.
        /// </param>
        /// <param name="fileSuffix">
        /// Der Wert für die <see cref="XmlFileLogger.FileSuffix" /> Eigenschaft.
        /// </param>
        public XmlFileLogger(DirectoryInfo logDir,
                             IEnumerable<char> filePrefix,
                             IEnumerable<char> fileSuffix)
            : this(logDir.FullName,
                   filePrefix,
                   fileSuffix)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="XmlFileLogger" />.
        /// </summary>
        /// <param name="logDir">
        /// Der Wert für die <see cref="XmlFileLogger.LogDirectory" /> Eigenschaft.
        /// </param>
        /// <param name="filePrefix">
        /// Der Wert für die <see cref="XmlFileLogger.FilePrefix" /> Eigenschaft.
        /// </param>
        public XmlFileLogger(IEnumerable<char> logDir,
                             IEnumerable<char> filePrefix)
            : this(logDir,
                   filePrefix,
                   null)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="XmlFileLogger" />.
        /// </summary>
        /// <param name="logDir">
        /// Der Wert für die <see cref="XmlFileLogger.LogDirectory" /> Eigenschaft.
        /// </param>
        /// <param name="filePrefix">
        /// Der Wert für die <see cref="XmlFileLogger.FilePrefix" /> Eigenschaft.
        /// </param>
        public XmlFileLogger(DirectoryInfo logDir,
                             IEnumerable<char> filePrefix)
            : this(logDir.FullName,
                   filePrefix)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="XmlFileLogger" />.
        /// </summary>
        /// <param name="logDir">
        /// Der Wert für die <see cref="XmlFileLogger.LogDirectory" /> Eigenschaft.
        /// </param>
        public XmlFileLogger(IEnumerable<char> logDir)
            : this(logDir,
                   null)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="XmlFileLogger" />.
        /// </summary>
        /// <param name="logDir">
        /// Der Wert für die <see cref="XmlFileLogger.LogDirectory" /> Eigenschaft.
        /// </param>
        public XmlFileLogger(DirectoryInfo logDir)
            : this(logDir.FullName)
        {

        }

        #endregion Constructors

        #region Properties (3)

        /// <summary>
        /// Gibt das Präfix für den Dateinamen zurück.
        /// </summary>
        public string FilePrefix
        {
            get;
            private set;
        }

        /// <summary>
        /// Gibt das Suffix für den Dateinamen zurück.
        /// </summary>
        public string FileSuffix
        {
            get;
            private set;
        }

        /// <summary>
        /// Gibt das Verzeichnis zurück in das die XML-Dateien geschrieben werden sollen.
        /// </summary>
        public string LogDirectory
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods (3)

        // Protected Methods (1) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="LoggerFacadeBase.OnLog(ILogMessage)" />
        protected override void OnLog(ILogMessage msg)
        {
            var xmlFile = new FileInfo(Path.Combine(this.LogDirectory,
                                                    string.Format("{0}{1:yyyyMMdd}{2}.xml",
                                                                  this.FilePrefix,
                                                                  msg.Time,
                                                                  this.FileSuffix)));

            XDocument logDoc = null;
            if (xmlFile.Exists)
            {
                try
                {
                    using (var fileStream = xmlFile.OpenRead())
                    {
                        using (var reader = XmlReader.Create(fileStream))
                        {
                            logDoc = XDocument.Load(reader);
                        }
                    }
                }
                catch
                {
                    logDoc = null;
                }
            }

            if (logDoc == null)
            {
                logDoc = new XDocument(new XDeclaration("1.0", Encoding.UTF8.WebName, "yes"));
                logDoc.Add(new XElement("logs"));
            }

            var logElement = new XElement("log");
            logElement.SetAttributeValue("time", msg.Time.ToString("yyyy-MM-dd HH:mm:ss.fffff (zzz)"));
            logElement.SetAttributeValue("id", msg.Id.ToString("N"));
            logElement.SetAttributeValue("categories", string.Join("|",
                                                                   msg.Categories
                                                                      .Select(c => c.ToString())
                                                                      .ToArray()));
            if (msg.Tag != null)
            {
                logElement.SetAttributeValue("tag", msg.Tag);
            }

            if (msg.Message is Exception)
            {
                var ex = (Exception)msg.Message;

                var exceptionElement = new XElement("exception");
                logElement.SetAttributeValue(LOG_ATTRIB_MIME, "(clrexception)");

                if (SetupExceptionElement(exceptionElement, ex))
                {
                    logElement.Add(exceptionElement);
                }
            }
            else if (msg.Message is XNode ||
                     msg.Message is XmlNode)
            {
                XElement xmlElement = null;
                if (msg.Message is XNode)
                {
                    // LINQ to XML

                    xmlElement = msg.Message as XElement;
                    if (xmlElement == null)
                    {
                        // in ein XELement umwandeln

                        var xmlDoc = msg.Message as XDocument;
                        if (xmlDoc != null)
                        {
                            // aus Stammelement
                            xmlElement = xmlDoc.Root;
                        }
                        else
                        {
                            // Umweg über XDocument

                            var xml = (msg.Message.ToString() ?? string.Empty).Trim();
                            if (xml != string.Empty)
                            {
                                xmlElement = XDocument.Parse(xml).Root;
                            }
                        }
                    }
                }
                else
                {
                    // in ein XElement über ein
                    // XDocument umwandeln

                    var xml = (((XmlNode)msg.Message).OuterXml ?? string.Empty).Trim();
                    if (xml != string.Empty)
                    {
                        xmlElement = XDocument.Parse(xml).Root;
                    }
                }

                logElement.SetAttributeValue(LOG_ATTRIB_MIME, MediaTypeNames.Text.Xml);
                if (xmlElement != null)
                {
                    logElement.Add(xmlElement);
                }
            }
            else
            {
                // Standard: als XML-Element-Wert (String)

                string strMsgValue;
                if (msg.Message is IEnumerable<char>)
                {
                    strMsgValue = AsString((IEnumerable<char>)msg.Message);
                }
                else
                {
                    strMsgValue = msg.Message.ToString();
                }

                logElement.SetAttributeValue(LOG_ATTRIB_MIME, MediaTypeNames.Text.Plain);
                if (strMsgValue != null)
                {
                    logElement.Value = strMsgValue;
                }
            }

            logDoc.Root
                  .AddFirst(logElement);

            using (var fileStream = new FileStream(xmlFile.FullName,
                                                   FileMode.Create,
                                                   FileAccess.ReadWrite))
            {

                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = false;

                using (var writer = XmlWriter.Create(fileStream, settings))
                {
                    logDoc.Save(writer);

                    writer.Flush();
                    writer.Close();
                }
            }
        }
        // Private Methods (2) 

        private static bool SetupExceptionElement(XElement element, Exception ex)
        {
            return SetupExceptionElement(element, ex, 0, new HashSet<Exception>());
        }

        private static bool SetupExceptionElement(XElement element, Exception ex, int level, ICollection<Exception> usedExceptions)
        {
            if (level >= 32)
            {
                return false;
            }

            if (usedExceptions.Contains(ex))
            {
                return false;
            }

            element.SetAttributeValue("type", ex.GetType().FullName);

            var contentElement = new XElement("content");
            {
                var strEx = ex.ToString();
                if (strEx != null)
                {
                    contentElement.Value = strEx;
                }

                element.Add(contentElement);
            }

            var stackTraceElement = new XElement("stackTrace");
            {
#if DEBUG
                var st = new StackTrace(ex, true);
#else
                var st = new StackTrace(ex, false);
#endif

                var frames = st.GetFrames();
                if (frames != null)
                {
                    for (long i = 0; i < frames.LongLength; i++)
                    {
                        var f = frames[i];

                        var frameElement = new XElement("frame");
                        frameElement.SetAttributeValue("index", i);

                        var file = f.GetFileName();
                        if (file != null)
                        {
                            frameElement.SetAttributeValue("file", file);
                        }

                        var member = f.GetMethod();
                        if (member != null)
                        {
                            var memberElement = new XElement("member");
                            memberElement.SetAttributeValue("name", member.Name);
                            memberElement.SetAttributeValue("type", member.MemberType);
                            memberElement.SetAttributeValue("declaringType", member.DeclaringType.FullName);
                            memberElement.SetAttributeValue("assembly", member.DeclaringType.Assembly.FullName);

                            var paramsElement = new XElement("params");
                            {
                                foreach (var p in member.GetParameters())
                                {
                                    var newParamElement = new XElement("param");
                                    newParamElement.SetAttributeValue("pos", p.Position);
                                    newParamElement.SetAttributeValue("name", p.Name);
                                    newParamElement.SetAttributeValue("type", p.ParameterType.FullName);
                                    newParamElement.SetAttributeValue("assembly", p.ParameterType.Assembly.FullName);

                                    paramsElement.Add(newParamElement);
                                }

                                memberElement.Add(paramsElement);
                            }

                            frameElement.Add(memberElement);
                        }

                        frameElement.SetAttributeValue("line", f.GetFileLineNumber());
                        frameElement.SetAttributeValue("column", f.GetFileColumnNumber());
                        frameElement.SetAttributeValue("ilOffset", f.GetILOffset());
                        frameElement.SetAttributeValue("nativeOffset", f.GetNativeOffset());

                        stackTraceElement.Add(frameElement);
                    }
                }

                element.Add(stackTraceElement);
            }

            usedExceptions.Add(ex);

            var innerEx = ex.InnerException;
            if (innerEx != null)
            {
                var innerElement = new XElement("inner");
                if (SetupExceptionElement(innerElement, innerEx, level + 1, usedExceptions))
                {
                    element.Add(innerElement);
                }
            }

            return true;
        }

        #endregion Methods
    }
}
