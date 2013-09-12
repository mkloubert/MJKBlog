// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MarcelJoachimKloubert.Blog.MEF
{
    /// <summary>
    /// Ein <see cref="ComposablePartCatalog" />, der auf einer Liste von vertrauenswürdigen
    /// und öffentlichen Assembly-Signatur-Schlüsseln beruht.
    /// </summary>
    public sealed partial class StrongNamedAssemblyPartCatalog : ComposablePartCatalog
    {
        #region Fields (2)

        private readonly AggregateCatalog _INNER_CATALOG = new AggregateCatalog();
        private readonly byte[][] _TRUSTED_ASM_KEYS;

        #endregion Fields

        #region Constructors (2)

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="StrongNamedAssemblyPartCatalog"/> class.
        /// </summary>
        /// <param name="trustedKeys">
        /// Die Liste mit verstrauenswürdigen, öffentlichen Assembly-Signatur-Schlüsseln.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="trustedKeys" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public StrongNamedAssemblyPartCatalog(IEnumerable<byte[]> trustedKeys)
        {
            if (trustedKeys == null)
            {
                throw new ArgumentNullException("trustedKeys");
            }

            this._TRUSTED_ASM_KEYS = trustedKeys as byte[][];
            if (this._TRUSTED_ASM_KEYS == null)
            {
                var list = trustedKeys as List<byte[]>;
                if (list != null)
                {
                    this._TRUSTED_ASM_KEYS = list.ToArray();
                }
                else
                {
                    this._TRUSTED_ASM_KEYS = trustedKeys.ToArray();
                }
            }
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="StrongNamedAssemblyPartCatalog"/> class.
        /// </summary>
        /// <param name="trustedKeys">
        /// Die Liste mit verstrauenswürdigen, öffentlichen Assembly-Signatur-Schlüsseln.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Mindestens ein Element aus <paramref name="trustedKeys" />
        /// ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="NullReferenceException">
        /// <paramref name="trustedKeys" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public StrongNamedAssemblyPartCatalog(IEnumerable<IEnumerable<byte>> trustedKeys)
            : this(trustedKeys.Select(k => k is byte[] ? (byte[])k : k.ToArray()))
        {

        }

        #endregion Constructors

        #region Properties (2)

        private bool HasEmptyPublicKey
        {
            get
            {
                return this._TRUSTED_ASM_KEYS
                           .Any(k => k.Length < 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ComposablePartCatalog.Parts" />
        public override IQueryable<ComposablePartDefinition> Parts
        {
            get { return this._INNER_CATALOG.Parts; }
        }

        #endregion Properties

        #region Methods (20)

        // Public Methods (18) 

        /// <summary>
        /// Fügt eine Liste von vertrauenswürdigen Assemblies diesem Katalog hinzu.
        /// </summary>
        /// <param name="asms">Die Liste, die hinzugefügt werden soll.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="asms" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Mindestens ein Element aus <paramref name="asms" /> ist nicht vertrauenswürdig.
        /// </exception>
        /// <exception cref="NullReferenceException">
        /// Mindestens ein Element aus <paramref name="asms" />
        /// ist eine <see langword="null" /> Referenz.
        /// </exception>
        public StrongNamedAssemblyPartCatalog AddAssemblies(params Assembly[] asms)
        {
            return this.AddAssemblies(asms as IEnumerable<Assembly>);
        }

        /// <summary>
        /// Fügt eine Liste von vertrauenswürdigen Assemblies diesem Katalog hinzu.
        /// </summary>
        /// <param name="asms">Die Liste, die hinzugefügt werden soll.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="asms" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Mindestens ein Element aus <paramref name="asms" /> ist nicht vertrauenswürdig.
        /// </exception>
        /// <exception cref="NullReferenceException">
        /// Mindestens ein Element aus <paramref name="asms" />
        /// ist eine <see langword="null" /> Referenz.
        /// </exception>
        public StrongNamedAssemblyPartCatalog AddAssemblies(IEnumerable<Assembly> asms)
        {
            if (asms == null)
            {
                throw new ArgumentNullException("asms");
            }

            foreach (var a in asms)
            {
                if (a == null)
                {
                    throw new NullReferenceException();
                }

                if (!this.IsTrustedAssembly(a))
                {
                    throw new InvalidOperationException(a.FullName);
                }

                this._INNER_CATALOG
                    .Catalogs
                    .Add(new AssemblyCatalog(a));
            }

            return this;
        }

        /// <summary>
        /// Fügt eine Liste von Assemblies, die in Dateien gespeichert sind, dem
        /// zugrundeliegenden Katalog hinzu.
        /// </summary>
        /// <param name="paths">Die Liste mit Dateipfaden, die zu Assemblies führen.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="paths" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Mindestens ein Element aus <paramref name="paths" /> ist nicht vertrauenswürdig.
        /// </exception>
        /// <exception cref="NullReferenceException">
        /// Mindestens ein Element aus <paramref name="paths" />
        /// ist eine <see langword="null" /> Referenz.
        /// </exception>
        public StrongNamedAssemblyPartCatalog AddAssemblyFiles(params IEnumerable<char>[] paths)
        {
            return this.AddAssemblyFiles(paths as IEnumerable<IEnumerable<char>>);
        }

        /// <summary>
        /// Fügt eine Liste von Assemblies, die in Dateien gespeichert sind, dem
        /// zugrundeliegenden Katalog hinzu.
        /// </summary>
        /// <param name="paths">Die Liste mit Dateipfaden, die zu Assemblies führen.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="paths" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Mindestens ein Element aus <paramref name="paths" /> ist nicht vertrauenswürdig.
        /// </exception>
        /// <exception cref="NullReferenceException">
        /// Mindestens ein Element aus <paramref name="paths" />
        /// ist eine <see langword="null" /> Referenz.
        /// </exception>
        public StrongNamedAssemblyPartCatalog AddAssemblyFiles(IEnumerable<IEnumerable<char>> paths)
        {
            if (paths == null)
            {
                throw new ArgumentNullException("paths");
            }

            return this.AddAssemblyFiles(paths.Select(p => AsString(p)));
        }

        /// <summary>
        /// Fügt eine Liste von Assemblies, die in Dateien gespeichert sind, dem
        /// zugrundeliegenden Katalog hinzu.
        /// </summary>
        /// <param name="paths">Die Liste mit Dateipfaden, die zu Assemblies führen.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="paths" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Mindestens ein Element aus <paramref name="paths" /> ist nicht vertrauenswürdig.
        /// </exception>
        /// <exception cref="NullReferenceException">
        /// Mindestens ein Element aus <paramref name="paths" />
        /// ist eine <see langword="null" /> Referenz.
        /// </exception>
        public StrongNamedAssemblyPartCatalog AddAssemblyFiles(IEnumerable<string> paths)
        {
            if (paths == null)
            {
                throw new ArgumentNullException("paths");
            }

            return this.AddAssemblyFiles(paths.Select(p => new FileInfo(p)));
        }

        /// <summary>
        /// Fügt eine Liste von Assemblies, die in Dateien gespeichert sind, dem
        /// zugrundeliegenden Katalog hinzu.
        /// </summary>
        /// <param name="files">Die Liste mit Dateien, die zu Assemblies führen.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="files" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Mindestens ein Element aus <paramref name="files" /> ist nicht vertrauenswürdig.
        /// </exception>
        /// <exception cref="NullReferenceException">
        /// Mindestens ein Element aus <paramref name="files" />
        /// ist eine <see langword="null" /> Referenz.
        /// </exception>
        public StrongNamedAssemblyPartCatalog AddAssemblyFiles(params FileInfo[] files)
        {
            return this.AddAssemblyFiles(files as IEnumerable<FileInfo>);
        }

        /// <summary>
        /// Fügt eine Liste von Assemblies, die in Dateien gespeichert sind, dem
        /// zugrundeliegenden Katalog hinzu.
        /// </summary>
        /// <param name="files">Die Liste mit Dateien, die zu Assemblies führen.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="files" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Mindestens ein Element aus <paramref name="files" /> ist nicht vertrauenswürdig.
        /// </exception>
        /// <exception cref="NullReferenceException">
        /// Mindestens ein Element aus <paramref name="files" />
        /// ist eine <see langword="null" /> Referenz.
        /// </exception>
        public StrongNamedAssemblyPartCatalog AddAssemblyFiles(IEnumerable<FileInfo> files)
        {
            if (files == null)
            {
                throw new ArgumentNullException("files");
            }

            foreach (var f in files)
            {
                if (f == null)
                {
                    throw new NullReferenceException();
                }

                if (!f.Exists)
                {
                    throw new FileNotFoundException("Assembly Datei wurde nicht gefunden!",
                                                    f.FullName);
                }

                if (!this.IsTrustedAssemblyFile(f))
                {
                    throw new InvalidOperationException(f.FullName);
                }

                this.AddAssemblies(new Assembly[]
            {
                Assembly.Load(File.ReadAllBytes(f.FullName)),
            });
            }

            return this;
        }

        /// <summary>
        /// Fügt eine Liste von vertrauenswürdigen Typen diesem Katalog hinzu.
        /// </summary>
        /// <param name="types">Die Liste, die hinzugefügt werden soll.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Mindestens ein Element aus <paramref name="types" /> ist nicht vertrauenswürdig.
        /// </exception>
        /// <exception cref="NullReferenceException">
        /// Mindestens ein Element aus <paramref name="types" />
        /// ist eine <see langword="null" /> Referenz.
        /// </exception>
        public StrongNamedAssemblyPartCatalog AddTypes(params Type[] types)
        {
            return this.AddTypes(types as IEnumerable<Type>);
        }

        /// <summary>
        /// Fügt eine Liste von vertrauenswürdigen Typen diesem Katalog hinzu.
        /// </summary>
        /// <param name="types">Die Liste, die hinzugefügt werden soll.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Mindestens ein Element aus <paramref name="types" /> ist nicht vertrauenswürdig.
        /// </exception>
        /// <exception cref="NullReferenceException">
        /// Mindestens ein Element aus <paramref name="types" />
        /// ist eine <see langword="null" /> Referenz.
        /// </exception>
        public StrongNamedAssemblyPartCatalog AddTypes(IEnumerable<Type> types)
        {
            if (types == null)
            {
                throw new ArgumentNullException("types");
            }

            var typeArray = types as Type[];
            if (typeArray == null)
            {
                var list = types as List<Type>;
                if (list != null)
                {
                    typeArray = list.ToArray();
                }
                else
                {
                    typeArray = types.ToArray();
                }
            }

            if (typeArray.Any(t => t == null))
            {
                throw new NullReferenceException();
            }

            var nonTrustedAssemblies = typeArray.Where(t => !this.IsTrustedType(t));
            if (nonTrustedAssemblies.Any())
            {
                throw new InvalidOperationException(string.Join("; ",
                                                                nonTrustedAssemblies.Select(t => t.FullName)));
            }

            this._INNER_CATALOG
                .Catalogs
                .Add(new TypeCatalog(typeArray));

            return this;
        }

        /// <summary>
        /// Leert den inneren MEF-Katalog.
        /// </summary>
        /// <returns>Diese Instanz.</returns>
        public StrongNamedAssemblyPartCatalog Clear()
        {
            this._INNER_CATALOG
                .Catalogs
                .Clear();

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ComposablePartCatalog.GetExports(ImportDefinition)" />
        public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
        {
            return this._INNER_CATALOG
                       .GetExports(definition);
        }

        /// <summary>
        /// Gibt eine neue Liste der zugrundeliegenden <see cref="ComposablePartCatalog" />
        /// Instanzen zurück.
        /// </summary>
        /// <returns>Die List der zugrundeliegenden <see cref="ComposablePartCatalog" /> Instanzen.</returns>
        public List<ComposablePartCatalog> GetInnerCatalogs()
        {
            return new List<ComposablePartCatalog>(this._INNER_CATALOG
                                                       .Catalogs);
        }

        /// <summary>
        /// Gibt eine neue Liste aller zugrundeliegenden vertrauenswürdigen und
        /// öffentlichen Assembly-Signatur-Schlüsseln zurück.
        /// </summary>
        /// <returns>Die List </returns>
        public List<byte[]> GetTrustedAssemblyKeys()
        {
            return new List<byte[]>(this._TRUSTED_ASM_KEYS);
        }

        /// <summary>
        /// Prüft, ob ein <see cref="Assembly" /> vertrauenswürdig ist oder nicht.
        /// </summary>
        /// <param name="asm">Das zu prüfende Assembly.</param>
        /// <returns>Ist vertrauenswürdig oder nicht.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="asm" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public bool IsTrustedAssembly(Assembly asm)
        {
            if (asm == null)
            {
                throw new ArgumentNullException("asm");
            }

            return this.IsTrustedAssembly(asm.GetName());
        }

        /// <summary>
        /// Prüft, ob ein <see cref="AssemblyName" /> ein vertrauenswürdiges
        /// <see cref="Assembly" /> repräsentiert oder nicht.
        /// </summary>
        /// <param name="asmName">Der Name des zu prüfenden Assemblies.</param>
        /// <returns>Ist vertrauenswürdig oder nicht.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="asmName" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public bool IsTrustedAssembly(AssemblyName asmName)
        {
            if (asmName == null)
            {
                throw new ArgumentNullException("asmName");
            }

            var pKey = asmName.GetPublicKey();
            if (pKey != null &&
                pKey.LongLength > 0)
            {
                return this._TRUSTED_ASM_KEYS
                           .Any(k => k != null &&
                                     k.SequenceEqual(pKey));
            }

            return this.HasEmptyPublicKey;
        }

        /// <summary>
        /// Prüft, ob eine Assembly-Datei vertrauenswürdig ist.
        /// </summary>
        /// <param name="path">Der Pfad zur Assembly-Datei.</param>
        /// <returns>Ist vertrauenswürdig oder nicht.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// Die Datei zu <paramref name="path" /> existiert nicht.
        /// </exception>
        public bool IsTrustedAssemblyFile(IEnumerable<char> path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            return this.IsTrustedAssemblyFile(new FileInfo(AsString(path)));
        }

        /// <summary>
        /// Prüft, ob eine Assembly-Datei vertrauenswürdig ist.
        /// </summary>
        /// <param name="file">Die Assembly-Datei.</param>
        /// <returns>Ist vertrauenswürdig oder nicht.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="file" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// Die Datei zu <paramref name="file" /> existiert nicht.
        /// </exception>
        public bool IsTrustedAssemblyFile(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            if (!file.Exists)
            {
                throw new FileNotFoundException("Assembly wurde nicht gefunden!",
                                                file.FullName);
            }

            return this.IsTrustedAssembly(AssemblyName.GetAssemblyName(file.FullName));
        }

        /// <summary>
        /// Prüft, ob ein <see cref="Type" /> zu einem vertrauenswürdigen
        /// Assmebly gehört oder nicht.
        /// </summary>
        /// <param name="type">Der zu prüfende Typ.</param>
        /// <returns>Ist vertrauenswürdig oder nicht.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public bool IsTrustedType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var asm = type.Assembly;
            if (asm == null)
            {
                return this.HasEmptyPublicKey;
            }

            return this.IsTrustedAssembly(asm);
        }
        // Protected Methods (1) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ComposablePartCatalog.Dispose(bool)" />
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                this._INNER_CATALOG
                    .Dispose();
            }
        }
        // Private Methods (1) 

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

        #endregion Methods
    }
}
