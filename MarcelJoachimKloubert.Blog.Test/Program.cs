// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Security.Cryptography;
using MarcelJoachimKloubert.Blog.MEF;
using MarcelJoachimKloubert.Blog.Serialization.Xml;

namespace MarcelJoachimKloubert.Blog.Test
{
    interface IA
    {

    }
    interface IB
    {

    }

    [Export(typeof(IA))]
    class A1 : IA
    {

    }
    [Export(typeof(IA))]
    class A2 : IA
    {

    }
    [Export(typeof(IB))]
    class B1 : IB
    {

    }

    internal static class Program
    {
        #region Methods (3)

        // Private Methods (3) 

        private static void Main(string[] args)
        {
            try
            {
                // Test_Mef();
                // Test_AsyncEncryption();
                // Test_Xslt();
                Test_XmlObjectSerializer();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException() ?? ex);
            }

            Console.ReadLine();
        }

        private static void Test_Mef()
        {
            var asmCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());

            var container = new CompositionContainer(asmCatalog, true);

            var mi1 = new MultiInstanceComposer<IA>(container);
            mi1.Refresh();
        }

        private static void Test_AsyncEncryption()
        {
            using (var rsa = new RSACryptoServiceProvider())    // generate new/random key pair
            {
                var export = rsa.ExportParameters(false);
            }
        }

        private static void Test_XmlObjectSerializer()
        {
            var bin = new byte[16];
            new Random().NextBytes(bin);

            var dict1 = new Dictionary<string, object>()
            {
                { "a1", short.MinValue },
                { "a2", short.MaxValue },
                
                { "b1", ushort.MinValue },
                { "b2", ushort.MaxValue },

                { "c1", int.MinValue },
                { "c2", int.MaxValue },
                
                { "d1", uint.MinValue },
                { "d2", uint.MaxValue },

                { "e1", long.MinValue },
                { "e2", long.MaxValue },
                
                { "f1", ulong.MinValue },
                { "f2", ulong.MaxValue },

                { "g", float.Parse("59,23979") },

                { "h", double.Parse("59,23979") },

                { "i", decimal.Parse("59,23979") },

                { "j", new List<object>()
                {
                    1,2,3,
                }},

                { "k", new Dictionary<string, object>()
                {
                    {"TM", 5979},
                    {"MK", 23979},
                }},

                { "l", bin },

                { "m1", sbyte.MinValue },
                { "m2", sbyte.MaxValue },
                
                { "n1", byte.MinValue },
                { "n2", byte.MaxValue },
            };

            var xml = XmlObjectSerializer.ToXml(dict1);

            var dict2 = XmlObjectSerializer.FromXml(xml);

            if (dict1.Count != dict2.Count)
            {
                throw new Exception("dict1.Count != dict2.Count");
            }

            foreach (var item1 in dict1)
            {
                var key1 = item1.Key;
                var value1 = item1.Value;

                object value2;
                if (!dict2.TryGetValue(key1.ToUpper().Trim(), out value2))
                {
                    throw new IndexOutOfRangeException(key1 ?? string.Empty);
                }

                var type1 = value1 != null ? value1.GetType() : null;
                var type2 = value2 != null ? value2.GetType() : null;

                if (!EqualityComparer<Type>.Default.Equals(type1, type2))
                {
                    throw new Exception(string.Format("{0}: {1} != {2}",
                                                      key1,
                                                      type1,
                                                      type2));
                }

                Console.WriteLine("{0}: {1} == {2}",
                                  key1,
                                  type1,
                                  type2);
            }

            Console.WriteLine();
            Console.WriteLine("OK");
        }

        private static void Test_Xslt()
        {

        }

        #endregion Methods
    }
}
