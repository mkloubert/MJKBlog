// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Linq;

namespace MarcelJoachimKloubert.Blog.ServiceLocation
{
    /// <summary>
    /// Ein Grundgerüst für einen Service Locator.
    /// </summary>
    public abstract class ServiceLocatorBase : IServiceLocator
    {
        #region Constructors (2)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="ServiceLocatorBase" />.
        /// </summary>
        /// <param name="syncRoot">Das Objekt, das für Thread-sichere Operationen verwendet werden soll.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="syncRoot" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        protected ServiceLocatorBase(object syncRoot)
        {
            if (syncRoot == null)
            {
                throw new ArgumentNullException("syncRoot");
            }

            this.SyncRoot = syncRoot;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="ServiceLocatorBase" />.
        /// </summary>
        protected ServiceLocatorBase()
            : this(new object())
        {

        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Gibt das Objekt für Thread-sichere Operationen zurück.
        /// </summary>
        public object SyncRoot
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods (10)

        // Public Methods (6) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IServiceLocator.GetAllInstances{S}()" />
        public IEnumerable<S> GetAllInstances<S>()
        {
            return this.GetAllInstances(typeof(S))
                       .Cast<S>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IServiceLocator.GetAllInstances(Type)" />
        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            IEnumerable<object> result = null;

            ServiceActivationException exceptionToThrow = null;
            try
            {
                result = this.OnGetAllInstances(serviceType);

                if (result == null)
                {
                    exceptionToThrow = new ServiceActivationException(serviceType,
                                                                      null);
                }
            }
            catch (Exception ex)
            {
                exceptionToThrow = new ServiceActivationException(serviceType,
                                                                  null,
                                                                  ex);
            }

            if (exceptionToThrow != null)
            {
                throw exceptionToThrow;
            }

            return result.Select(s => !DBNull.Value.Equals(s) ? s : null)
                         .OfType<object>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IServiceLocator.GetInstance{S}()" />
        public S GetInstance<S>()
        {
            return (S)this.GetInstance(typeof(S));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IServiceLocator.GetInstance{S}(object)" />
        public S GetInstance<S>(object key)
        {
            return (S)this.GetInstance(typeof(S),
                                       key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IServiceLocator.GetInstance(Type)" />
        public object GetInstance(Type serviceType)
        {
            return this.GetInstance(serviceType,
                                    key: null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IServiceLocator.GetInstance(Type, object)" />
        public object GetInstance(Type serviceType, object key)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            object result = null;

            ServiceActivationException exceptionToThrow = null;
            try
            {
                result = ParseValue(this.OnGetInstance(serviceType,
                                                       ParseValue(key)));

                if (result == null)
                {
                    exceptionToThrow = new ServiceActivationException(serviceType,
                                                                      key);
                }
            }
            catch (Exception ex)
            {
                exceptionToThrow = new ServiceActivationException(serviceType,
                                                                  key,
                                                                  ex);
            }

            if (exceptionToThrow != null)
            {
                throw exceptionToThrow;
            }

            return result;
        }
        // Protected Methods (2) 

        /// <summary>
        /// Implementiert die Logik für die Methode
        /// <see cref="ServiceLocatorBase.GetAllInstances(Type)" />.
        /// </summary>
        /// <param name="serviceType">Der Typ des Dienstes.</param>
        /// <returns>Die Liste mit Instanzen des Dienstes.</returns>
        protected abstract IEnumerable<object> OnGetAllInstances(Type serviceType);

        /// <summary>
        /// Implementiert die Logik für die Methode
        /// <see cref="ServiceLocatorBase.GetInstance(Type, object)" />.
        /// </summary>
        /// <param name="serviceType">Der Typ des Dienstes.</param>
        /// <param name="key">Der Schlüssel.</param>
        /// <returns>Die einzige Instanz des Dienstes.</returns>
        protected abstract object OnGetInstance(Type serviceType,
                                                object key);
        // Private Methods (2) 

        object IServiceProvider.GetService(Type serviceType)
        {
            try
            {
                return this.GetInstance(serviceType);
            }
            catch (ServiceActivationException sae)
            {
                var innerEx = sae.InnerException;
                if (innerEx != null)
                {
                    throw innerEx;
                }

                return null;
            }
        }

        private static object ParseValue(object value)
        {
            var result = value;
            if (DBNull.Value.Equals(result))
            {
                result = null;
            }

            return result;
        }

        #endregion Methods
    }
}
