// s. http://blog.marcel-kloubert.de


using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace MarcelJoachimKloubert.Blog.MEF
{
    /// <summary>
    /// Hilfsklasse zum Erzeugen und Speichern einer einzelnen Instanz, die durch
    /// einen <see cref="CompositionContainer" /> erzeugt wird.
    /// </summary>
    /// <typeparam name="C">Typ der erzeugten Instanzen / des Vertrages.</typeparam>
    public sealed class SingleInstanceComposer<C>
    {
        #region Constructors (2)

        /// <summary>
        /// Initialisiert eine neue Instance der <see cref="SingleInstanceComposer{C}" /> Klasse.
        /// </summary>
        /// <param name="container">
        /// Der Wert für die <see cref="SingleInstanceComposer{C}.Container" />
        /// Eigenschaft.
        /// </param>
        /// <param name="doRefresh">
        /// <see cref="SingleInstanceComposer{C}.Refresh()" /> Methode aufrufen oder nicht.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="container" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public SingleInstanceComposer(CompositionContainer container,
                                      bool doRefresh)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.Container = container;

            if (doRefresh)
            {
                this.Refresh();
            }
        }

        /// <summary>
        /// Initialisiert eine neue Instance der <see cref="SingleInstanceComposer{C}" /> Klasse
        /// ohne die <see cref="SingleInstanceComposer{C}.Refresh()" /> Methode aufzurufen.
        /// </summary>
        /// <param name="container">
        /// Der Wert für die <see cref="SingleInstanceComposer{C}.Container" />
        /// Eigenschaft.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="container" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public SingleInstanceComposer(CompositionContainer container)
            : this(container, false)
        {

        }

        #endregion Constructors

        #region Properties (2)

        /// <summary>
        /// Gibt den zugrundeliegenden <see cref="CompositionContainer" /> zurück.
        /// </summary>
        public CompositionContainer Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Gibt die zuletzt erzeugte Instanz zurück.
        /// </summary>
        [Import(AllowRecomposition = true)]
        public C Instance
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods (1)

        // Public Methods (1) 

        /// <summary>
        /// Aktualisiert die aktuelle Instanz in der
        /// <see cref="SingleInstanceComposer{C}.Instance" /> Eigenschaft.
        /// </summary>
        public void Refresh()
        {
            this.Container
                .ComposeParts(this);
        }

        #endregion Methods
    }
}
