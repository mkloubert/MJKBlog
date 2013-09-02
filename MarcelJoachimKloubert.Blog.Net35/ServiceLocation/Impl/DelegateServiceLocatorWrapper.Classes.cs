// s. http://blog.marcel-kloubert.de


using System;

namespace MarcelJoachimKloubert.Blog.ServiceLocation.Impl
{
    partial class DelegateServiceLocatorWrapper
    {
        #region Nested Classes (3)

        private abstract class InstanceFactoryBase
        {
            #region Fields (2)

            internal readonly Delegate DELEGATE;
            internal readonly Type SERVICE_TYPE;

            #endregion Fields

            #region Constructors (1)

            protected InstanceFactoryBase(Type serviceType,
                                          Delegate provider)
            {
                this.SERVICE_TYPE = serviceType;
                this.DELEGATE = provider;
            }

            #endregion Constructors
        }

        private sealed class MultiInstanceFactory<T> : InstanceFactoryBase
        {
            #region Constructors (1)

            internal MultiInstanceFactory(MultiInstanceProvider<T> provider)
                : base(typeof(T),
                       provider)
            {

            }

            #endregion Constructors

            #region Properties (1)

            internal MultiInstanceProvider<T> Provider
            {
                get { return (MultiInstanceProvider<T>)this.DELEGATE; }
            }

            #endregion Properties
        }

        private sealed class SingleInstanceFactory<T> : InstanceFactoryBase
        {
            #region Constructors (1)

            internal SingleInstanceFactory(SingleInstanceProvider<T> provider)
                : base(typeof(T),
                       provider)
            {

            }

            #endregion Constructors

            #region Properties (1)

            internal SingleInstanceProvider<T> Provider
            {
                get { return (SingleInstanceProvider<T>)this.DELEGATE; }
            }

            #endregion Properties
        }

        #endregion Nested Classes
    }
}
