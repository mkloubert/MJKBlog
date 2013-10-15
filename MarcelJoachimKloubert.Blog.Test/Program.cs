// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using ColorCode;
using MarcelJoachimKloubert.Blog.Diagnostics;
using MarcelJoachimKloubert.Blog.IO;
using MarcelJoachimKloubert.Blog.MEF;
using MarcelJoachimKloubert.Blog.MEF.ServiceLocation;
using MarcelJoachimKloubert.Blog.Net.Http.Impl;
using MarcelJoachimKloubert.Blog.Serialization.Xml;
using MarcelJoachimKloubert.Blog.ServiceLocation;
using MarcelJoachimKloubert.Blog.ServiceLocation.Impl;
using MarcelJoachimKloubert.Blog.Values;
using RemObjects.Script;

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
        #region Methods (1)

        // Public Methods (1) 

        public override string ToString()
        {
            return this.GetType().FullName + "::" + this.GetHashCode();
        }

        #endregion Methods
    }

    [Export(typeof(IA))]
    class A : IA
    {
        #region Methods (2)

        // Public Methods (2) 

        public void test()
        {
            Console.WriteLine("A:test()");
        }

        public override string ToString()
        {
            return this.GetType().FullName + "::" + this.GetHashCode();
        }

        #endregion Methods
    }

    [Export(typeof(IB))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    class B1 : IB
    {
        #region Methods (1)

        // Public Methods (1) 

        public override string ToString()
        {
            return this.GetType().FullName + "::" + this.GetHashCode();
        }

        #endregion Methods
    }

    internal static class Program
    {
        private static void Test_RemObjectsScript()
        {
            // Funktionen in der JavaScript-Engine
            // werden über Delegates definiert
            var myWriteLineAction = new Action<object>(
                (obj) =>
                {
                    Console.WriteLine(obj);
                });

            using (var javascript = new EcmaScriptComponent())
            {


                // Variabel 'a' mit dem Wert 5979
                javascript.Globals.SetVariable("a", 5979);
                // Funktion 'myWriteLine' (s.o.)
                javascript.Globals.SetVariable("myWriteLine", myWriteLineAction);

                // die eigene Klasse 'A' als
                // 'KlasseA' registrieren
                javascript.ExposeType(typeof(A), "KlasseA");

                // Quelltext festlegen
                javascript.Source = @"
myWriteLine(a);    // 5979

a = 23979;
myWriteLine(a);    // 23979

var objA = new KlasseA();
objA.test();
";

                javascript.Run();
            }
        }

        private static void Test_Resources()
        {
            var asm = Assembly.GetExecutingAssembly();

            var str = asm.GetManifestResourceString("MarcelJoachimKloubert.Blog.Test._Resources.LoremIpsum.txt");
        }

        private static void Main(string[] args)
        {
            try
            {
                dynamic d = new ExpandoObject();
                d.Test = "Wurst";
                d.Methode = new Action<object>(
                    (x) =>
                    {
                        Console.WriteLine(x);
                    });

                d.Methode(d.Test);

                // Test_Resources();
                // Test_HttpServer();
                Test_ValueRouter();
                // Test_Mef();
                // Test_AsyncEncryption();
                // Test_Xslt();
                // Test_XmlObjectSerializer();
                // Test_GroupedCollection();
                // Test_RemObjectsScript();
                // Test_ColorCode();
                // Test_UnpackArchiv();
                // Test_ForAll();
                // Test_ServiceLocator();

                //var knownOs = Environment.OSVersion.TryGetKnownOS();

                //var times = GetTimes();
                //foreach (var t in times.ToArray())
                //{

                //}

                //times.ForEach(x => Console.WriteLine(x));
                //Thread.Sleep(2000);
                //times.ForEach(x => Console.WriteLine(x));

                //var tokenSource2 = new CancellationTokenSource();

                //var t = Task.Factory
                //    .StartNewTask((state, ct) =>
                //    {


                //        for (ulong i = 0; i <= ulong.MaxValue; i++)
                //        {
                //            if (ct.IsCancellationRequested)
                //            {
                //                ct.ThrowIfCancellationRequested();
                //            }

                //            Thread.Sleep(100);
                //        }
                //    }, new
                //    {
                //    }, cancellationToken: tokenSource2.Token);

                //Thread.Sleep(5000);
                //tokenSource2.Cancel(false);

                //t.Wait();

                //                Time s = (Time)"13:00:12";
                //                Time? c = null;
                //                TimeSpan? ts = c;
                //                string s2 = (string)c;

                //                var test = XDocument.Parse(@"<?xml version=""1.0"" ?>
                //<root xmlns:ms=""http://www.microsoft.com""
                //      ms:testAttrib=""hallo"">
                //</root>");

                //                var dict = test.Root.GetAttributeDictionary();


                //                var list = (c.Value * 2).ToArray();

                //                Console.WriteLine(c);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException() ?? ex);
            }

            Console.ReadLine();
        }

        private static IEnumerable<DateTimeOffset> GetTimes()
        {
            yield return DateTimeOffset.Now;
        }

        private static void Test_Mef()
        {
            var asmCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());

            var container = new CompositionContainer(asmCatalog, true);

            var mi1 = new MultiInstanceComposer<IA>(container);
            mi1.Refresh();
        }

        private static void Test_UnpackArchiv()
        {
            using (var stream = File.OpenRead(@"./7zDLLs.7z"))
            {
                foreach (var file in new FileInfo(@"./7zDLLs.7z")
                                         .UnpackArchive()
                                         .Where(f => f.Type == CompressedArchiveItemType.File)
                                         .OrderBy(f => (f.Name ?? string.Empty).ToLower().Trim()))
                {

                }
            }
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
            Console.WriteLine(xml);

            Console.WriteLine();
            Console.WriteLine("OK");
        }

        private static void Test_ColorCode()
        {
            var cc = new CodeColorizer();

            var vbnet = cc.Colorize(@"Module Module1
    Sub Main()
        Console.WriteLine(""Hallo Welt!"")
    End Sub
End Module",
                                    Languages.VbDotNet);

            var html = cc.Colorize(@"",
                                   Languages.Html);

            var aspNet = cc.Colorize(@"",
                                     Languages.AspxCs);

            var css = cc.Colorize(@"",
                                  Languages.Css);

            var js = cc.Colorize(@"",
                                 Languages.Php);
        }

        private static void Test_Xslt()
        {

        }

        private static void Test_ValueRouter(dynamic test = null,
                                             bool test3 = false,
                                             string test2 = "wurst")
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            var logger = new DelegateLogger();
            logger.Add((msg) =>
                {
                    Console.WriteLine("\t" + msg.Thread.ManagedThreadId);
                    Console.WriteLine("\t" + Thread.CurrentThread.ManagedThreadId);
                });

            var taskLogger = new TaskLogger(logger,
                                            TaskScheduler.Current);

            var a1 = new EnumValueRouter<TrafficLightState>(TrafficLightState.Yellow);

            var b1 = new EnumValueRouter<TrafficLightState>();
            var b2 = new EnumValueRouter<TrafficLightState>();

            var c1 = new EnumValueRouter<TrafficLightState>();
            var c2 = new EnumValueRouter<TrafficLightState>();
            var c3 = new EnumValueRouter<TrafficLightState>();

            // Struktur festlegen
            {
                a1.AddMediator(b1);
                a1.AddMediator(b2);

                b1.AddMediator(c1);
                b1.AddMediator(c2);

                b2.AddMediator(c3);
            }

            c1.MyValue = TrafficLightState.Green;
            b1.MyValue = TrafficLightState.None;
            c3.MyValue = TrafficLightState.Red;

            taskLogger.Log(c3, LoggerFacadeCategories.Warnings | LoggerFacadeCategories.Errors);

            var xmlLogger = new XmlFileLogger(@"D:\logfiles");
            //try
            //{
            //    throw new Exception("test");
            //}
            //catch (Exception ex)
            //{
            //    xmlLogger.Log(ex,
            //                  LoggerFacadeCategories.Warnings | LoggerFacadeCategories.Errors);
            //}

            //var tasks = Enumerable.Range(0, 999)
            //          .Select(i => Task.Factory.StartNew((state) =>
            //              {
            //                  var xl = (XmlFileLogger)state;

            //                  xmlLogger.Log(new Exception(),
            //                                LoggerFacadeCategories.Warnings | LoggerFacadeCategories.Errors);
            //              }, xmlLogger))
            //          .ToArray();

            //Task.WaitAll(tasks);
        }

        private static void Test_HttpServer()
        {
            using (var server = new HttpListenerServer())
            {
                server.TcpPort = 23979;
                server.Start();

                while (server.IsRunning)
                {

                }
            }
        }

        private static void Test_ServiceLocator()
        {
            var catalogs = new AggregateCatalog();
            catalogs.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            var container = new CompositionContainer(catalogs, true);

            var innerServiceLocator = new ExportProviderServiceLocator(container);

            var serviceLocator = new DelegateServiceLocatorWrapper(innerServiceLocator);
            serviceLocator.RegisterSingle<IA>(Test_ServiceLocator3);
            serviceLocator.RegisterMulti<IB>(Test_ServiceLocator2);

            var a1 = serviceLocator.GetAllInstances<IA>().ToArray();
            var a2 = serviceLocator.GetAllInstances<IA>().ToArray();
            // var a3 = serviceLocator.GetInstance<IA>();

            var b1 = serviceLocator.GetAllInstances<IB>().ToArray();
            var b2 = serviceLocator.GetAllInstances<IB>().ToArray();
            var b3 = serviceLocator.GetInstance<IB>();
        }

        private static Lazy<IB> lazyB = new Lazy<IB>(() => new B1(), true);

        private static IEnumerable<IB> Test_ServiceLocator2(IServiceLocator serviceLocator)
        {
            yield return lazyB.Value;
        }

        private static IA Test_ServiceLocator3(IServiceLocator serviceLocator, object key)
        {
            return new A();
        }

        private static void Test_ForAll()
        {
            var test = Enumerable.Range(0, 1000).ToArray();
            var ex2 = test.ForAllAsync(
                (i, s) =>
                {
                    if ((i + s) % 3 == 0)
                    {
                        throw new Exception(i.ToString());
                    }
                }, 1, false);

            try
            {
                Console.WriteLine("A...");

                test.ForAllAsync((i) =>
                {
                    if (i % 3 == 0)
                    {
                        throw new Exception(i.ToString());
                    }
                });
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Console.WriteLine("Fertig");
            }

            Console.ReadLine();
        }
    }
}
