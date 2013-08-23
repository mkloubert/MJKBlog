// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace MarcelJoachimKloubert.Blog.MEF
{
    /// <summary>
    /// Hilfsklasse zum Erzeugen und Speichern mehrerer Instanzen, die durch
    /// einen <see cref="CompositionContainer" /> erzeugt werden.
    /// </summary>
    /// <typeparam name="C">Typ der erzeugten Instanzen / des Vertrages.</typeparam>
    public sealed class MultiInstanceComposer<C>
    {
        #region Constructors (2)

        /// <summary>
        /// Initialisiert eine neue Instance der <see cref="MultiInstanceComposer{C}" /> Klasse.
        /// </summary>
        /// <param name="container">
        /// Der Wert für die <see cref="MultiInstanceComposer{C}.Container" />
        /// Eigenschaft.
        /// </param>
        /// <param name="doRefresh">
        /// <see cref="MultiInstanceComposer{C}.Refresh()" /> Methode aufrufen oder nicht.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="container" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public MultiInstanceComposer(CompositionContainer container,
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
        /// Initialisiert eine neue Instance der <see cref="MultiInstanceComposer{C}" /> Klasse
        /// ohne die <see cref="MultiInstanceComposer{C}.Refresh()" /> Methode aufzurufen.
        /// </summary>
        /// <param name="container">
        /// Der Wert für die <see cref="MultiInstanceComposer{C}.Container" />
        /// Eigenschaft.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="container" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public MultiInstanceComposer(CompositionContainer container)
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
        /// Gibt das Objekt zurück, das die zulegt erzeugten Instanzen
        /// speichert.
        /// </summary>
        /// <remarks>Ist zu Beginn <see langword="null" />.</remarks>
        [ImportMany(AllowRecomposition = true)]
        public List<C> Instances
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods (2)

        // Public Methods (2) 

        /// <summary>
        /// Aktualisiert die aktuelle Instanzliste der
        /// <see cref="MultiInstanceComposer{C}.Instances" /> Eigenschaft.
        /// </summary>
        public void Refresh()
        {
            this.Container
                .ComposeParts(this);
        }

        /// <summary>
        /// Aktualisiert die aktuelle Instanzliste der
        /// <see cref="MultiInstanceComposer{C}.Instances" /> Eigenschaft,
        /// wenn dies noch nicht geschehen ist.
        /// </summary>
        public bool RefreshIfNeeded()
        {
            if (this.Instances == null)
            {
                this.Refresh();
                return true;
            }

            return false;
        }

        #endregion Methods
    }
}
