// s. http://blog.marcel-kloubert.de



using System.Collections.Generic;
using System.IO;
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

        #region Methods (1)

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

            XDocument xmlDoc;
            try
            {
                using (var fileStream = xmlFile.OpenRead())
                {
                    using (var reader = XmlReader.Create(fileStream))
                    {
                        xmlDoc = XDocument.Load(reader);
                    }
                }
            }
            catch
            {
                xmlDoc = null;
            }

            if (xmlDoc == null)
            {
                xmlDoc = new XDocument(new XDeclaration("1.0", Encoding.UTF8.WebName, "yes"));
                xmlDoc.Add(new XElement("logs"));
            }

            var logElement = new XElement("log");
            logElement.SetAttributeValue("time", msg.Time.ToString("yyyy-MM-dd HH:mm:ss.fffff (zzz)"));
            logElement.SetAttributeValue("id", msg.Id.ToString("N"));

            xmlDoc.Root
                  .AddFirst(logElement);

            using (var fileStream = new FileStream(xmlFile.FullName,
                                                   FileMode.Create,
                                                   FileAccess.ReadWrite))
            {
                using (var writer = XmlWriter.Create(fileStream))
                {
                    xmlDoc.Save(writer);

                    writer.Flush();
                    writer.Close();
                }
            }
        }

        #endregion Methods
    }
}
