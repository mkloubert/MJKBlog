// s. http://blog.marcel-kloubert.de


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MarcelJoachimKloubert.Blog.ServiceLocation.Impl
{
    /// <summary>
    /// Ein <see cref="IServiceLocator" />, der auf Delegates beruht.
    /// Ist kein Delegate für einen Dienst definiert, wird versucht eine Instanz
    /// über eine Basis-Instanz aus der <see cref="DelegateServiceLocatorWrapper.InnerLocator" />
    /// Eigenschaft zu nutzen, sofern diese definiert ist / wurde.
    /// </summary>
    public sealed partial class DelegateServiceLocatorWrapper : ServiceLocatorBase
    {
        #region Fields (2)

        private readonly IList<InstanceFactoryBase> _MULTI_FACTORIES = new List<InstanceFactoryBase>();
        private readonly IList<InstanceFactoryBase> _SINGLE_FACTORIES = new List<InstanceFactoryBase>();

        #endregion Fields

        #region Constructors (4)

        /// <summary>
        /// Initialisiert eine neue Instanz dieser Klasse.
        /// </summary>
        /// <param name="syncRoot">Das Objekt, das für Thread-sichere Operationen verwendet werden soll.</param>
        /// <param name="innerLocator">
        /// Der Wert für die <see cref="DelegateServiceLocatorWrapper.InnerLocator" />
        /// Eigenschaft.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="syncRoot" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public DelegateServiceLocatorWrapper(object syncRoot,
                                             IServiceLocator innerLocator)
            : base(syncRoot)
        {
            this.InnerLocator = innerLocator;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz dieser Klasse.
        /// </summary>
        /// <param name="innerLocator">
        /// Der Wert für die <see cref="DelegateServiceLocatorWrapper.InnerLocator" />
        /// Eigenschaft.
        /// </param>
        public DelegateServiceLocatorWrapper(IServiceLocator innerLocator)
            : this(new object(),
                   innerLocator)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz dieser Klasse.
        /// </summary>
        /// <param name="syncRoot">Das Objekt, das für Thread-sichere Operationen verwendet werden soll.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="syncRoot" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public DelegateServiceLocatorWrapper(object syncRoot)
            : this(syncRoot,
                   (IServiceLocator)null)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz dieser Klasse.
        /// </summary>
        public DelegateServiceLocatorWrapper()
            : this(new object(),
                   (IServiceLocator)null)
        {

        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Gibt die Instanz des Basis <see cref="IServiceLocator" />
        /// zurück, oder <see langword="null" />, wenn keiner definiert wurde.
        /// </summary>
        public IServiceLocator InnerLocator
        {
            get;
            private set;
        }

        #endregion Properties

        #region Delegates and Events (2)

        // Delegates (2) 

        /// <summary>
        /// Beschreibt eine Funktion oder Methode, die eine Liste von Dienstinstanzen erzeugt.
        /// </summary>
        /// <typeparam name="T">Typ des zugrundeliegenden Dienstes.</typeparam>
        /// <param name="sender">Das aufrufende Objekt.</param>
        /// <returns>Die Liste mit erzeugten Instanzen.</returns>
        public delegate IEnumerable<T> MultiInstanceProvider<T>(DelegateServiceLocatorWrapper sender);

        /// <summary>
        /// Beschreibt eine Funktion oder Methode, die eine einzige Instanz eines Dienstes erzeugt.
        /// </summary>
        /// <typeparam name="T">Typ des zugrundeliegenden Dienstes.</typeparam>
        /// <param name="sender">Das aufrufende Objekt.</param>
        /// <param name="key">Der Schlüssel.</param>
        /// <returns>Die Liste mit erzeugten Instanzen.</returns>
        public delegate T SingleInstanceProvider<T>(DelegateServiceLocatorWrapper sender, object key);

        #endregion Delegates and Events

        #region Methods (13)

        // Public Methods (7) 

        /// <summary>
        /// Löscht alle registrierten Delegates zum Erzeugen von Dienst-Instanzen.
        /// </summary>
        /// <returns>Diese Instanz.</returns>
        public DelegateServiceLocatorWrapper Clear()
        {
            this.ClearSingle();
            this.ClearMulti();

            return this;
        }

        /// <summary>
        /// Löscht alle registrierten Delegates zum Erzeugen von Instanzlisten.
        /// </summary>
        /// <returns>Diese Instanz.</returns>
        public DelegateServiceLocatorWrapper ClearMulti()
        {
            this.InvokeForFactoryLists(
                (singleList, multiList) =>
                {
                    multiList.Clear();
                });

            return this;
        }

        /// <summary>
        /// Löscht alle registrierten Delegates zum Erzeugen von einzelner Instanzen.
        /// </summary>
        /// <returns>Diese Instanz.</returns>
        public DelegateServiceLocatorWrapper ClearSingle()
        {
            this.InvokeForFactoryLists(
                (singleList, multiList) =>
                {
                    singleList.Clear();
                });

            return this;
        }

        /// <summary>
        /// Registriert ein Delegate zum Erzeugen einer Liste von Dienstinstanzen
        /// und fügt zusätzlich ein Standard-Delegate zum Erzeugen einer einzelnen Instanz
        /// hinzu.
        /// </summary>
        /// <typeparam name="T">Typ des Dienstes.</typeparam>
        /// <param name="provider">Das Delegate, das Registriert werden soll.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="provider" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="DuplicateWaitObjectException">
        /// Ein Delegate wurde bereits registriert.
        /// </exception>
        public DelegateServiceLocatorWrapper RegisterMulti<T>(MultiInstanceProvider<T> provider)
        {
            return this.RegisterMulti<T>(provider,
                                         true);
        }

        /// <summary>
        /// Registriert ein Delegate zum Erzeugen einer Liste von Dienstinstanzen.
        /// </summary>
        /// <typeparam name="T">Typ des Dienstes.</typeparam>
        /// <param name="provider">Das Delegate, das Registriert werden soll.</param>
        /// <param name="addDefaultSingleProvider">
        /// Zusätzliches Delegate zum Erzeugen einer einzelnen Instanz
        /// hinzufügen oder nicht.
        /// </param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="provider" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="DuplicateWaitObjectException">
        /// Ein Delegate wurde bereits registriert.
        /// </exception>
        public DelegateServiceLocatorWrapper RegisterMulti<T>(MultiInstanceProvider<T> provider,
                                                              bool addDefaultSingleProvider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            this.InvokeForFactoryLists(
                (singleList, multiList, args) =>
                {
                    var factory = multiList.FirstOrDefault(f => args.ServiceType
                                                                    .Equals(f.SERVICE_TYPE));
                    if (factory != null)
                    {
                        throw new DuplicateWaitObjectException(args.ServiceType
                                                                   .FullName);
                    }

                    multiList.Add(new MultiInstanceFactory<T>(args.InstanceProvider));
                }, new
                {
                    InstanceProvider = provider,
                    ServiceType = typeof(T),
                });

            if (addDefaultSingleProvider)
            {
                this.RegisterSingle<T>(MultiToSingle<T>(provider),
                                       false);
            }

            return this;
        }

        /// <summary>
        /// Registriert ein Delegate zum Erzeugen einer einzelnen Dienstinstanz
        /// und fügt zusätzlich ein Standard-Delegate zum Erzeugen einer Liste von Instanzen
        /// hinzu.
        /// </summary>
        /// <typeparam name="T">Typ des Dienstes.</typeparam>
        /// <param name="provider">Das Delegate, das Registriert werden soll.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="provider" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="DuplicateWaitObjectException">
        /// Ein Delegate wurde bereits registriert.
        /// </exception>
        public DelegateServiceLocatorWrapper RegisterSingle<T>(SingleInstanceProvider<T> provider)
        {
            return this.RegisterSingle<T>(provider,
                                          true);
        }

        /// <summary>
        /// Registriert ein Delegate zum Erzeugen einer einzelnen Dienstinstanz.
        /// </summary>
        /// <typeparam name="T">Typ des Dienstes.</typeparam>
        /// <param name="provider">Das Delegate, das Registriert werden soll.</param>
        /// <param name="addDefaultMultiProvider">
        /// Zusätzliches Delegate zum Erzeugen einer Liste von Instanzen
        /// hinzufügen oder nicht.
        /// </param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="provider" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="DuplicateWaitObjectException">
        /// Ein Delegate wurde bereits registriert.
        /// </exception>
        public DelegateServiceLocatorWrapper RegisterSingle<T>(SingleInstanceProvider<T> provider,
                                                               bool addDefaultMultiProvider)
        {
            this.InvokeForFactoryLists(
                (singleList, multiList, args) =>
                {
                    var factory = singleList.FirstOrDefault(f => args.ServiceType
                                                                     .Equals(f.SERVICE_TYPE));
                    if (factory != null)
                    {
                        throw new DuplicateWaitObjectException(args.ServiceType
                                                                   .FullName);
                    }

                    singleList.Add(new SingleInstanceFactory<T>(args.InstanceProvider));
                }, new
                   {
                       InstanceProvider = provider,
                       ServiceType = typeof(T),
                   });

            if (addDefaultMultiProvider)
            {
                this.RegisterMulti<T>(SingleToMulti<T>(provider),
                                      false);
            }

            return this;
        }
        // Protected Methods (2) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ServiceLocatorBase.OnGetAllInstances(Type)" />
        protected override IEnumerable<object> OnGetAllInstances(Type serviceType)
        {
            InstanceFactoryBase factory = null;
            this.InvokeForFactoryLists(
                (singleList, multiList, args) =>
                {
                    factory = multiList.FirstOrDefault(f => args.ServiceType
                                                                .Equals(f.SERVICE_TYPE));
                }, new
                {
                    ServiceType = serviceType,
                });

            IEnumerable<object> result = null;

            if (factory == null)
            {
                var innerLoc = this.InnerLocator;
                if (innerLoc != null)
                {
                    result = innerLoc.GetAllInstances(serviceType);
                }
            }
            else
            {
                var seq = (IEnumerable)factory.DELEGATE
                                              .Method
                                              .Invoke(factory.DELEGATE
                                                             .Target,
                                                      new object[] { this });

                if (seq != null)
                {
                    result = seq as IEnumerable<object>;
                    if (result == null)
                    {
                        result = seq.Cast<object>();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ServiceLocatorBase.OnGetInstance(Type, object)" />
        protected override object OnGetInstance(Type serviceType, object key)
        {
            InstanceFactoryBase factory = null;
            this.InvokeForFactoryLists(
                (singleList, multiList, args) =>
                {
                    factory = singleList.FirstOrDefault(f => args.ServiceType
                                                                 .Equals(f.SERVICE_TYPE));
                }, new
                {
                    ServiceType = serviceType,
                });

            object result = null;

            if (factory == null)
            {
                var innerLoc = this.InnerLocator;
                if (innerLoc != null)
                {
                    result = innerLoc.GetInstance(serviceType,
                                                  key);
                }
            }
            else
            {
                result = factory.DELEGATE
                                .Method
                                .Invoke(factory.DELEGATE
                                               .Target,
                                        new object[] { this, key });
            }

            return result;
        }
        // Private Methods (4) 

        private void InvokeForFactoryLists(Action<IList<InstanceFactoryBase>, IList<InstanceFactoryBase>> action)
        {
            this.InvokeForFactoryLists<object>((singleList, multiList, actionState) => action(singleList, multiList),
                                               null);
        }

        private void InvokeForFactoryLists<T>(Action<IList<InstanceFactoryBase>, IList<InstanceFactoryBase>, T> action,
                                              T actionState)
        {
            lock (this.SyncRoot)
            {
                action(this._SINGLE_FACTORIES,
                       this._MULTI_FACTORIES,
                       actionState);
            }
        }

        private static SingleInstanceProvider<T> MultiToSingle<T>(MultiInstanceProvider<T> multiProvider)
        {
            if (multiProvider != null)
            {
                return new SingleInstanceProvider<T>(
                    (sender, key) =>
                    {
                        if (DBNull.Value.Equals(key))
                        {
                            key = null;
                        }

                        if (key != null)
                        {
                            throw new ArgumentException("key");
                        }

                        var instances = multiProvider(sender);
                        if (instances == null)
                        {
                            throw new ServiceActivationException(typeof(T),
                                                                 key);
                        }

                        return instances.Single();
                    });
            }

            return null;
        }

        private static MultiInstanceProvider<T> SingleToMulti<T>(SingleInstanceProvider<T> singleProvider)
        {
            if (singleProvider != null)
            {
                return new MultiInstanceProvider<T>(
                    (sender) =>
                    {
                        var instance = singleProvider(sender, null);
                        if (instance == null)
                        {
                            throw new ServiceActivationException(typeof(T),
                                                                 null);
                        }

                        return new T[] { instance };
                    });
            }

            return null;
        }

        #endregion Methods
    }
}
