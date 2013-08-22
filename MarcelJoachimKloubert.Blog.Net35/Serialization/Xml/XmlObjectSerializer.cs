// s. http://blog.marcel-kloubert.de


using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MarcelJoachimKloubert.Blog.Serialization.Xml
{
    /// <summary>
    /// Eine Hilfsklasse, die Objekte von und nach XML serialisiert.
    /// </summary>
    public static partial class XmlObjectSerializer
    {
        #region Fields (24)

        /// <summary>
        /// Interner Name eines <see cref="Boolean" />.
        /// </summary>
        public const string TYPE_BOOLEAN = "bool";
        /// <summary>
        /// Interner Name eines <see cref="Decimal" />.
        /// </summary>
        public const string TYPE_DECIMAL = "decimal";
        /// <summary>
        /// Interner Name eines <see cref="Double" />.
        /// </summary>
        public const string TYPE_DOUBLE = "double";
        /// <summary>
        /// Interner Name eines <see cref="Enum" />s.
        /// </summary>
        public const string TYPE_ENUM = "enum";
        /// <summary>
        /// Interner Name eines <see cref="Single" />.
        /// </summary>
        public const string TYPE_FLOAT = "float";
        /// <summary>
        /// Interner Name eines <see cref="Int16" />.
        /// </summary>
        public const string TYPE_INT16 = "int16";
        /// <summary>
        /// Interner Name eines <see cref="Int32" />.
        /// </summary>
        public const string TYPE_INT32 = "int32";
        /// <summary>
        /// Interner Name eines <see cref="Int64" />.
        /// </summary>
        public const string TYPE_INT64 = "int64";
        /// <summary>
        /// Interner Name einer <see cref="IList{T}" />.
        /// </summary>
        public const string TYPE_LIST = "list";
        /// <summary>
        /// Interner Name einer <see langword="null" /> Referenz.
        /// </summary>
        public const string TYPE_NULL = "<null>";
        /// <summary>
        /// Interner Name eines beliebigen <see cref="IXmlSerializable" />.
        /// </summary>
        public const string TYPE_OBJECT = "object";
        /// <summary>
        /// Interner Name eines <see cref="String" />s.
        /// </summary>
        public const string TYPE_STRING = "string";
        /// <summary>
        /// Interner Name eines <see cref="UInt16" />.
        /// </summary>
        public const string TYPE_UINT16 = "uint16";
        /// <summary>
        /// Interner Name eines <see cref="UInt32" />.
        /// </summary>
        public const string TYPE_UINT32 = "uint32";
        /// <summary>
        /// Interner Name eines <see cref="UInt64" />.
        /// </summary>
        public const string TYPE_UINT64 = "uint64";
        /// <summary>
        /// Interner Name einer <see cref="Version" />.
        /// </summary>
        public const string TYPE_VERSION = "version";
        /// <summary>
        /// Name des Attributs, das den CLR Datentyp eines Wertes speichert.
        /// </summary>
        public const string XML_ATTRIB_CLRTYPE = "clrType";
        /// <summary>
        /// Name des Attributs, das den Namen eines Wertes speichert.
        /// </summary>
        public const string XML_ATTRIB_NAME = "name";
        /// <summary>
        /// Name des Attributs, das den Datentyp eines Wertes speichert.
        /// </summary>
        public const string XML_ATTRIB_TYPE = "type";
        /// <summary>
        /// Name des Elements, das den Eintrag einer Liste speichert.
        /// </summary>
        public const string XML_ELEMENT_LISTITEM = "item";
        /// <summary>
        /// Name des Stamm-Elements, das alle Werte speichert.
        /// </summary>
        public const string XML_ELEMENT_ROOT = "values";
        /// <summary>
        /// Name des Elements, das den Wert speichert.
        /// </summary>
        public const string XML_ELEMENT_VALUE = "value";
        /// <summary>
        /// Speichert den XML-Wert für ein boolisches <see langword="false" />.
        /// </summary>
        public const string XML_VALUE_BOOLEAN_FALSE = "0";
        /// <summary>
        /// Speichert den XML-Wert für ein boolisches <see langword="true" />.
        /// </summary>
        public const string XML_VALUE_BOOLEAN_TRUE = "1";

        #endregion Fields

        #region Properties (1)

        /// <summary>
        /// Gibt das Format zurück, das zum Serialisieren von Zahlen genutzt werden soll.
        /// </summary>
        public static NumberFormatInfo NumberCultureFormat
        {
            get { return CultureInfo.InvariantCulture.NumberFormat; }
        }

        #endregion Properties

        #region Methods (10)

        // Public Methods (6) 

        /// <summary>
        /// Erzeugt ein Wörterbuch aus XML-Daten.
        /// </summary>
        /// <param name="xml">The XML-Daten aus denen das Wörterbuch erstellt werden soll.</param>
        /// <param name="keyComparer">
        /// Der optionale, eigene <see cref="IEqualityComparer{T}" />, der die Schlüssel des
        /// zurückgegeben Wörterbuchs vergleicht.
        /// </param>
        /// <returns>Das Wörterbuch mit den Werten.</returns>
        /// <remarks>
        /// Ist <paramref name="keyComparer" /> eine <see langword="null" /> Referenz, wird ein
        /// Standard <see cref="IEqualityComparer{T}" /> Objekt genutzt, das die Schlüssel nicht
        /// nach Groß- und Kleinschreibung vergleicht.
        /// </remarks>
        public static Dictionary<string, object> FromXml(IEnumerable<char> xml,
                                                         IEqualityComparer<string> keyComparer = null)
        {
            if (xml != null)
            {
                var strXml = AsString(xml).Trim();
                if (strXml != string.Empty)
                {
                    return FromXml(strXml,
                                   keyComparer);
                }
            }

            return new Dictionary<string, object>(GetEqualityComparer(keyComparer));
        }

        /// <summary>
        /// Erzeugt ein Wörterbuch aus XML-Daten.
        /// </summary>
        /// <param name="xml">The XML-Daten aus denen das Wörterbuch erstellt werden soll.</param>
        /// <param name="keyComparer">
        /// Der optionale, eigene <see cref="IEqualityComparer{T}" />, der die Schlüssel des
        /// zurückgegeben Wörterbuchs vergleicht.
        /// </param>
        /// <returns>Das Wörterbuch mit den Werten.</returns>
        /// <remarks>
        /// Ist <paramref name="keyComparer" /> eine <see langword="null" /> Referenz, wird ein
        /// Standard <see cref="IEqualityComparer{T}" /> Objekt genutzt, das die Schlüssel nicht
        /// nach Groß- und Kleinschreibung vergleicht.
        /// </remarks>
        public static Dictionary<string, object> FromXml(XmlNode xml,
                                                         IEqualityComparer<string> keyComparer = null)
        {
            return FromXml(xml != null ? xml.OuterXml : null,
                           keyComparer);
        }

        /// <summary>
        /// Erzeugt ein Wörterbuch aus XML-Daten.
        /// </summary>
        /// <param name="xml">The XML-Daten aus denen das Wörterbuch erstellt werden soll.</param>
        /// <param name="keyComparer">
        /// Der optionale, eigene <see cref="IEqualityComparer{T}" />, der die Schlüssel des
        /// zurückgegeben Wörterbuchs vergleicht.
        /// </param>
        /// <returns>Das Wörterbuch mit den Werten.</returns>
        /// <remarks>
        /// Ist <paramref name="keyComparer" /> eine <see langword="null" /> Referenz, wird ein
        /// Standard <see cref="IEqualityComparer{T}" /> Objekt genutzt, das die Schlüssel nicht
        /// nach Groß- und Kleinschreibung vergleicht.
        /// </remarks>
        public static Dictionary<string, object> FromXml(XNode xml,
                                                         IEqualityComparer<string> keyComparer = null)
        {
            var result = new Dictionary<string, object>(GetEqualityComparer(keyComparer));

            var xmlElement = ToXElement(xml);

            if (xmlElement != null)
            {
                foreach (var item in xmlElement.Elements(XML_ELEMENT_VALUE))
                {
                    var value = XmlToKeyValuePair(item);
                    if (value.HasValue)
                    {
                        result.Add((value.Value.Key ?? string.Empty).Trim(),
                                   value.Value.Value);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Erzeugt XML-Daten aus einem Schlüssel/Wert-Paar.
        /// </summary>
        /// <param name="pair">Das Schlüssel/Wert-Paar.</param>
        /// <returns>Die XML-Daten.</returns>
        /// <exception cref="NotSupportedException">
        /// Der Datentyp des Wertes kann nicht serialisiert werden.
        /// </exception>
        public static XElement KeyValuePairToXml(KeyValuePair<string, object> pair)
        {
            var result = new XElement(XML_ELEMENT_VALUE);

            var name = (pair.Key ?? string.Empty).Trim();
            if (name != string.Empty)
            {
                result.SetAttributeValue(XML_ATTRIB_NAME, name);
            }

            var value = pair.Value;
            if (DBNull.Value.Equals(value))
            {
                value = null;
            }

            var type = TYPE_NULL;
            if (value != null)
            {
                string xmlValue = null;

                if (value is IEnumerable<char>)
                {
                    xmlValue = AsString(value as IEnumerable<char>);
                    type = TYPE_STRING;
                }
                else if (value is short)
                {
                    xmlValue = ((short)value).ToString(NumberCultureFormat);
                    type = TYPE_INT16;
                }
                else if (value is int)
                {
                    xmlValue = ((int)value).ToString(NumberCultureFormat);
                    type = TYPE_INT32;
                }
                else if (value is long)
                {
                    xmlValue = ((long)value).ToString(NumberCultureFormat);
                    type = TYPE_INT64;
                }
                else if (value is ushort)
                {
                    xmlValue = ((ushort)value).ToString(NumberCultureFormat);
                    type = TYPE_UINT16;
                }
                else if (value is uint)
                {
                    xmlValue = ((uint)value).ToString(NumberCultureFormat);
                    type = TYPE_UINT32;
                }
                else if (value is ulong)
                {
                    xmlValue = ((ulong)value).ToString(NumberCultureFormat);
                    type = TYPE_UINT64;
                }
                else if (value is decimal)
                {
                    xmlValue = ((decimal)value).ToString(NumberCultureFormat);
                    type = TYPE_DECIMAL;
                }
                else if (value is double)
                {
                    xmlValue = ((double)value).ToString(NumberCultureFormat);
                    type = TYPE_DOUBLE;
                }
                else if (value is float)
                {
                    xmlValue = ((float)value).ToString(NumberCultureFormat);
                    type = TYPE_FLOAT;
                }
                else if (value is bool)
                {
                    xmlValue = (bool)value ? XML_VALUE_BOOLEAN_TRUE : XML_VALUE_BOOLEAN_FALSE;
                    type = TYPE_BOOLEAN;
                }
                else if (value is Version)
                {
                    xmlValue = value.ToString();
                    type = TYPE_VERSION;
                }
                else if (value is IXmlSerializable)
                {
                    var xmlObj = value as IXmlSerializable;

                    var xmlBuilder = new StringBuilder();
                    using (var writer = XmlWriter.Create(xmlBuilder))
                    {
                        xmlObj.WriteXml(writer);
                    }

                    XElement xml = null;
                    var xmlStr = xmlBuilder.ToString().Trim();
                    if (xmlStr != string.Empty)
                    {
                        xmlBuilder.Remove(0, xmlBuilder.Length);

                        xml = XDocument.Parse(xmlStr).Root;
                    }

                    if (xml != null)
                    {
                        result.Add(xml);
                    }

                    type = TYPE_OBJECT;
                    result.SetAttributeValue(XML_ATTRIB_CLRTYPE,
                                             xmlObj.GetType().FullName);
                }
                else if (value is IEnumerable)
                {
                    foreach (var item in (IEnumerable)value)
                    {
                        var xml = KeyValuePairToXml(new KeyValuePair<string, object>(string.Empty, value));
                        if (xml != null)
                        {
                            xml.Name = XML_ELEMENT_LISTITEM;
                            result.Add(xml);
                        }
                    }

                    type = TYPE_LIST;
                }
                else
                {
                    if (value.GetType().IsEnum)
                    {
                        xmlValue = value.ToString();

                        type = TYPE_ENUM;
                        result.SetAttributeValue(XML_ATTRIB_CLRTYPE,
                                                 value.GetType().FullName);
                    }
                    else
                    {
                        throw new NotSupportedException(value.GetType().FullName);
                    }
                }

                if (xmlValue != null)
                {
                    result.Value = xmlValue;
                }
            }

            result.SetAttributeValue(XML_ATTRIB_TYPE, type);
            return result;
        }

        /// <summary>
        /// Erzeugt XML-Daten aus einer Liste von Werten.
        /// </summary>
        /// <param name="values">Die Werte.</param>
        /// <returns>Die XML-Daten.</returns>
        public static XElement ToXml(IEnumerable<KeyValuePair<IEnumerable<char>, object>> values)
        {
            return ToXml(values != null ? values.Select(v => new KeyValuePair<string, object>(AsString(v.Key), v.Value))
                                        : null);
        }

        /// <summary>
        /// Erzeugt XML-Daten aus einer Liste von Werten.
        /// </summary>
        /// <param name="values">Die Werte.</param>
        /// <returns>Die XML-Daten.</returns>
        public static XElement ToXml(IEnumerable<KeyValuePair<string, object>> values)
        {
            var result = new XElement(XML_ELEMENT_ROOT);

            if (values != null)
            {
                foreach (var item in values)
                {
                    var newElement = KeyValuePairToXml(item);
                    if (newElement != null)
                    {
                        result.Add(newElement);
                    }
                }
            }

            return result;
        }
        // Private Methods (4) 

        private static string AsString(IEnumerable<char> chars)
        {
            if (chars == null)
            {
                return null;
            }

            if (chars is string)
            {
                return (string)chars;
            }

            if (chars is char[])
            {
                return new string((char[])chars);
            }

            return new string(chars.ToArray());
        }

        private static IEqualityComparer<string> GetEqualityComparer(IEqualityComparer<string> keyComparer)
        {
            return keyComparer ?? new __IgnoreCaseEqualityComparer();
        }

        private static string NormalizeXmlValue(string value)
        {
            return (value ?? string.Empty).Trim();
        }

        private static XElement ToXElement(XNode node)
        {
            XElement result = null;

            if (node != null)
            {
                result = node as XElement;
                if (result == null)
                {
                    var xmlDoc = node as XDocument;
                    if (xmlDoc != null)
                    {
                        result = xmlDoc.Root;
                    }
                    else
                    {
                        result = XDocument.Parse(node.ToString()).Root;
                    }
                }
            }

            return result;
        }

        #endregion Methods

        /// <summary>
        /// Erzeugt ein Schlüssel/Wert-Paar aus XML-Daten.
        /// </summary>
        /// <param name="xml">Dei XML-Daten.</param>
        /// <returns>
        /// Das Schlüssel/Wert-Paar oder <see langword="null" />, wenn nicht genug Daten in
        /// <paramref name="xml" /> vorhanden sind bzw. dieses ebenfalls <see langword="null" />
        /// ist.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Die XML-Daten konnten nicht deserialisiert werden.
        /// </exception>
        public static KeyValuePair<string, object>? XmlToKeyValuePair(XNode xml)
        {
            KeyValuePair<string, object>? result = null;

            var xmlElement = ToXElement(xml);
            if (xmlElement != null)
            {
                var name = string.Empty;
                {
                    var nameAttrib = xmlElement.Attribute(XML_ATTRIB_NAME);
                    if (nameAttrib != null)
                    {
                        name = (nameAttrib.Value ?? string.Empty).Trim();
                    }
                }

                var type = string.Empty;
                {
                    var typeAttrib = xmlElement.Attribute(XML_ATTRIB_TYPE);
                    if (typeAttrib != null)
                    {
                        type = (typeAttrib.Value ?? string.Empty).ToLower().Trim();
                    }
                }

                object value = null;

                switch (type)
                {
                    case "":
                    case TYPE_STRING:
                        value = xmlElement.Value;
                        break;

                    case TYPE_BOOLEAN:
                        {
                            var s = NormalizeXmlValue(xmlElement.Value);
                            if (s != string.Empty)
                            {
                                switch (s)
                                {
                                    case XML_VALUE_BOOLEAN_FALSE:
                                    case XML_VALUE_BOOLEAN_TRUE:
                                        value = s == XML_VALUE_BOOLEAN_TRUE;
                                        break;

                                    default:
                                        throw new InvalidCastException(string.Format("'{0}' ist kein boolischer Wert!",
                                                                                     s));
                                }
                            }
                        }
                        break;

                    case TYPE_DECIMAL:
                        {
                            var s = NormalizeXmlValue(xmlElement.Value);
                            if (s != string.Empty)
                            {
                                value = decimal.Parse(s, NumberCultureFormat);
                            }
                        }
                        break;

                    case TYPE_DOUBLE:
                        {
                            var s = NormalizeXmlValue(xmlElement.Value);
                            if (s != string.Empty)
                            {
                                value = double.Parse(s, NumberCultureFormat);
                            }
                        }
                        break;

                    case TYPE_ENUM:
                        {
                            var clrTypeAttrib = xmlElement.Attribute(XML_ATTRIB_CLRTYPE);
                            if (clrTypeAttrib == null)
                            {
                                throw new FormatException("Der CLR-Datentype ist nicht definiert!");
                            }

                            var clrTypeName = clrTypeAttrib.Value.Trim();

                            var clrType = AppDomain.CurrentDomain
                                                   .GetAssemblies()
                                                   .SelectMany(a => a.GetTypes())
                                                   .SingleOrDefault(t => t.FullName == clrTypeName);
                            if (clrType == null)
                            {
                                throw new IndexOutOfRangeException(XML_ATTRIB_CLRTYPE);
                            }

                            var s = NormalizeXmlValue(xmlElement.Value);
                            if (s != string.Empty)
                            {
                                value = Enum.Parse(clrType, s, false);
                            }
                        }
                        break;

                    case TYPE_FLOAT:
                        {
                            var s = NormalizeXmlValue(xmlElement.Value);
                            if (s != string.Empty)
                            {
                                value = float.Parse(s, NumberCultureFormat);
                            }
                        }
                        break;

                    case TYPE_INT16:
                        {
                            var s = NormalizeXmlValue(xmlElement.Value);
                            if (s != string.Empty)
                            {
                                value = short.Parse(s, NumberCultureFormat);
                            }
                        }
                        break;

                    case TYPE_INT32:
                        {
                            var s = NormalizeXmlValue(xmlElement.Value);
                            if (s != string.Empty)
                            {
                                value = int.Parse(s, NumberCultureFormat);
                            }
                        }
                        break;

                    case TYPE_INT64:
                        {
                            var s = NormalizeXmlValue(xmlElement.Value);
                            if (s != string.Empty)
                            {
                                value = long.Parse(s, NumberCultureFormat);
                            }
                        }
                        break;

                    case TYPE_LIST:
                        {
                            var list = new List<object>();
                            foreach (var item in xmlElement.Elements(XML_ELEMENT_LISTITEM))
                            {
                                var pair = XmlToKeyValuePair(item);
                                if (pair.HasValue)
                                {
                                    list.Add(pair.Value.Value);
                                }
                            }

                            value = list;
                        }
                        break;

                    case TYPE_NULL:
                        if (!string.IsNullOrEmpty(xmlElement.Value))
                        {
                            throw new FormatException("xmlElement.Value != null");
                        }
                        break;

                    case TYPE_OBJECT:
                        {
                            var clrTypeAttrib = xmlElement.Attribute(XML_ATTRIB_CLRTYPE);
                            if (clrTypeAttrib == null)
                            {
                                throw new FormatException("Der CLR-Datentype ist nicht definiert!");
                            }

                            var clrTypeName = clrTypeAttrib.Value.Trim();

                            var clrType = AppDomain.CurrentDomain
                                                   .GetAssemblies()
                                                   .SelectMany(a => a.GetTypes())
                                                   .SingleOrDefault(t => t.FullName == clrTypeName);
                            if (clrType == null)
                            {
                                throw new IndexOutOfRangeException(XML_ATTRIB_CLRTYPE);
                            }

                            var xmlObj = Activator.CreateInstance(clrType) as IXmlSerializable;
                            if (xmlObj == null)
                            {
                                throw new InvalidCastException("Kein gültiges XML-Objekt!");
                            }

                            using (var strReader = new StringReader(xmlElement.ToString()))
                            {
                                using (var reader = XmlReader.Create(strReader))
                                {
                                    xmlObj.ReadXml(reader);
                                }
                            }

                            value = xmlObj;
                        }
                        break;

                    case TYPE_UINT16:
                        {
                            var s = NormalizeXmlValue(xmlElement.Value);
                            if (s != string.Empty)
                            {
                                value = ushort.Parse(s, NumberCultureFormat);
                            }
                        }
                        break;

                    case TYPE_UINT32:
                        {
                            var s = NormalizeXmlValue(xmlElement.Value);
                            if (s != string.Empty)
                            {
                                value = uint.Parse(s, NumberCultureFormat);
                            }
                        }
                        break;

                    case TYPE_UINT64:
                        {
                            var s = NormalizeXmlValue(xmlElement.Value);
                            if (s != string.Empty)
                            {
                                value = ulong.Parse(s, NumberCultureFormat);
                            }
                        }
                        break;

                    case TYPE_VERSION:
                        {
                            var s = NormalizeXmlValue(xmlElement.Value);
                            if (s != string.Empty)
                            {
                                value = new Version(s);
                            }
                        }
                        break;

                    default:
                        throw new InvalidCastException(type);
                }

                result = new KeyValuePair<string, object>(name,
                                                          value);
            }

            return result;
        }
    }
}
