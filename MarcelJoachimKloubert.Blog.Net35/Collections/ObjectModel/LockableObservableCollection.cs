// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MarcelJoachimKloubert.Blog.Collections.ObjectModel
{
    /// <summary>
    /// Eine <see cref="SynchronizedObservableCollection{T}" /> deren
    /// <see cref="ObservableCollection{T}.CollectionChanged" />- und
    /// <see cref="ObservableCollection{T}.PropertyChanged" />-Ereignisaufrufe
    /// man sperren bzw. unterdrücken kann, solange man bspw. eine grosse Zahl
    /// von Elementen hinzufügt und/oder entfernt.
    /// Dies hat den Vorteil, dass nicht bei jeder kleinsten Änderung eines der
    /// Ereignisse ausgelöst wird, sondern erst dann, wenn sie entsperrt ist/wird. 
    /// </summary>
    /// <typeparam name="T">Typ der zugrundeliegenden Elemente.</typeparam>
    public class LockableObservableCollection<T> : SynchronizedObservableCollection<T>
    {
        #region Constructors (6)

        /// <summary>
        /// Initialisiert eine neue Instanz der
        /// Klasse <see cref="LockableObservableCollection{T}"/>.
        /// </summary>
        /// <param name="syncRoot">Das Objekt für Thread-sichere Operationen.</param>
        /// <param name="collection">Die Elemente, die von Anfang an Teil dieser List sein sollen.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="syncRoot" /> und/oder <paramref name="collection" />
        /// ist eine <see langword="null" /> Referenz.
        /// </exception>
        public LockableObservableCollection(object syncRoot, IEnumerable<T> collection)
            : base(syncRoot, collection)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der
        /// Klasse <see cref="LockableObservableCollection{T}"/>.
        /// </summary>
        /// <param name="syncRoot">Das Objekt für Thread-sichere Operationen.</param>
        /// <param name="list">Die Elemente, die von Anfang an Teil dieser List sein sollen.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="syncRoot" /> und/oder <paramref name="list" />
        /// ist eine <see langword="null" /> Referenz.
        /// </exception>
        public LockableObservableCollection(object syncRoot, List<T> list)
            : base(syncRoot, list)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der
        /// Klasse <see cref="LockableObservableCollection{T}"/>.
        /// </summary>
        /// <param name="list">Die Elemente, die von Anfang an Teil dieser List sein sollen.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="list" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public LockableObservableCollection(List<T> list)
            : base(list)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der
        /// Klasse <see cref="LockableObservableCollection{T}"/>.
        /// </summary>
        /// <param name="syncRoot">Das Objekt für Thread-sichere Operationen.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="syncRoot" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public LockableObservableCollection(object syncRoot)
            : base(syncRoot)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der
        /// Klasse <see cref="LockableObservableCollection{T}"/>.
        /// </summary>
        /// <param name="collection">Die Elemente, die von Anfang an Teil dieser List sein sollen.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public LockableObservableCollection(IEnumerable<T> collection)
            : base(collection)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der
        /// Klasse <see cref="LockableObservableCollection{T}"/>.
        /// </summary>
        public LockableObservableCollection()
            : base()
        {

        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Gibt zurück, ob diese Liste derzeit gesperrt ist oder nicht.
        /// </summary>
        public bool IsLocked
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods (5)

        // Public Methods (3) 

        /// <summary>
        /// Sperrt diese Liste.
        /// </summary>
        public void BeginLock()
        {
            lock (this.SyncRoot)
            {
                this.IsLocked = true;
            }
        }

        /// <summary>
        /// Entsperrt diese Liste und führt anschliessend die gesperrten Ereignisse aus.
        /// </summary>
        public void EndLock()
        {
            this.EndLock(true);
        }

        /// <summary>
        /// Entsperrt diese Liste.
        /// </summary>
        /// <param name="raiseEvents">
        /// Anschließend die gesperrten Ereignisse ausführen oder nicht.
        /// </param>
        public void EndLock(bool raiseEvents)
        {
            lock (this.SyncRoot)
            {
                this.IsLocked = false;

                if (raiseEvents)
                {
                    base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    base.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                }
            }
        }
        // Protected Methods (2) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ObservableCollection{T}.OnCollectionChanged(NotifyCollectionChangedEventArgs)" />
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!this.IsLocked)
            {
                // nicht gesperrt
                base.OnCollectionChanged(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ObservableCollection{T}.OnPropertyChanged(PropertyChangedEventArgs)" />
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (!this.IsLocked)
            {
                // entsperrt
                base.OnPropertyChanged(e);
            }
        }

        #endregion Methods
    }
}
