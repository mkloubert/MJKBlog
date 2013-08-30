// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MarcelJoachimKloubert.Blog.Collections.ObjectModel
{
    /// <summary>
    /// Eine Thread-sichere <see cref="ObservableCollection{T}" />,
    /// </summary>
    /// <typeparam name="T">Typ der Elemente.</typeparam>
    public class SynchronizedObservableCollection<T> : ObservableCollection<T>
    {
        #region Constructors (6)

        /// <summary>
        /// Initialisiert eine neue Instanz der
        /// Klasse <see cref="SynchronizedObservableCollection{T}"/>.
        /// </summary>
        /// <param name="syncRoot">Das Objekt für Thread-sichere Operationen.</param>
        /// <param name="collection">Die Elemente, die von Anfang an Teil dieser List sein sollen.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="syncRoot" /> und/oder <paramref name="collection" />
        /// ist eine <see langword="null" /> Referenz.
        /// </exception>
        public SynchronizedObservableCollection(object syncRoot, IEnumerable<T> collection)
            : base(collection)
        {
            if (syncRoot == null)
            {
                throw new ArgumentNullException("syncRoot");
            }

            this.SyncRoot = syncRoot;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der
        /// Klasse <see cref="SynchronizedObservableCollection{T}"/>.
        /// </summary>
        /// <param name="syncRoot">Das Objekt für Thread-sichere Operationen.</param>
        /// <param name="list">Die Elemente, die von Anfang an Teil dieser List sein sollen.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="syncRoot" /> und/oder <paramref name="list" />
        /// ist eine <see langword="null" /> Referenz.
        /// </exception>
        public SynchronizedObservableCollection(object syncRoot, List<T> list)
            : base(list)
        {
            if (syncRoot == null)
            {
                throw new ArgumentNullException("syncRoot");
            }

            this.SyncRoot = syncRoot;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der
        /// Klasse <see cref="SynchronizedObservableCollection{T}"/>.
        /// </summary>
        /// <param name="list">Die Elemente, die von Anfang an Teil dieser List sein sollen.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="list" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public SynchronizedObservableCollection(List<T> list)
            : this(new object(),
                   list)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der
        /// Klasse <see cref="SynchronizedObservableCollection{T}"/>.
        /// </summary>
        /// <param name="syncRoot">Das Objekt für Thread-sichere Operationen.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="syncRoot" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public SynchronizedObservableCollection(object syncRoot)
            : base()
        {
            if (syncRoot == null)
            {
                throw new ArgumentNullException("syncRoot");
            }

            this.SyncRoot = syncRoot;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der
        /// Klasse <see cref="SynchronizedObservableCollection{T}"/>.
        /// </summary>
        /// <param name="collection">Die Elemente, die von Anfang an Teil dieser List sein sollen.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public SynchronizedObservableCollection(IEnumerable<T> collection)
            : this(new object(),
                   collection)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der
        /// Klasse <see cref="SynchronizedObservableCollection{T}"/>.
        /// </summary>
        public SynchronizedObservableCollection()
            : this(new object())
        {

        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Gibt das Objekt zurück, das für Thread-sichere Operationen
        /// verwendet werden soll.
        /// </summary>
        public object SyncRoot
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods (5)

        // Protected Methods (5) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ObservableCollection{T}.ClearItems()" />
        protected override void ClearItems()
        {
            lock (this.SyncRoot)
            {
                base.ClearItems();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ObservableCollection{T}.InsertItem(int, T)" />
        protected override void InsertItem(int index, T item)
        {
            lock (this.SyncRoot)
            {
                base.InsertItem(index, item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ObservableCollection{T}.MoveItem(int, int)" />
        protected override void MoveItem(int oldIndex, int newIndex)
        {
            lock (this.SyncRoot)
            {
                base.MoveItem(oldIndex, newIndex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ObservableCollection{T}.RemoveItem(int)" />
        protected override void RemoveItem(int index)
        {
            lock (this.SyncRoot)
            {
                base.RemoveItem(index);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ObservableCollection{T}.SetItem(int, T)" />
        protected override void SetItem(int index, T item)
        {
            lock (this.SyncRoot)
            {
                base.SetItem(index, item);
            }
        }

        #endregion Methods
    }
}
