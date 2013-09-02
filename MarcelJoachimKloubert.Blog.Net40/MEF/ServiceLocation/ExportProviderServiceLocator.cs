// s. http://blog.marcel-kloubert.de
// s. http://commonservicelocator.codeplex.com/


using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using MarcelJoachimKloubert.Blog.ServiceLocation;

namespace MarcelJoachimKloubert.Blog.MEF.ServiceLocation
{
    /// <summary>
    /// Ein <see cref="IServiceLocator" />, der auf einem
    /// <see cref="ExportProvider" />, wie bspw. einem
    /// <see cref="CompositionContainer" /> basiert.
    /// </summary>
    public class ExportProviderServiceLocator : ServiceLocatorBase
    {
        #region Fields (1)

        /// <summary>
        /// Speichert den zugrundeliegenden <see cref="ExportProvider" />.
        /// </summary>
        protected readonly ExportProvider _PROVIDER;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz dieser Klasse.
        /// </summary>
        /// <param name="provider">Der zugrundeliegende <see cref="ExportProvider" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="provider" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public ExportProviderServiceLocator(ExportProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            this._PROVIDER = provider;
        }

        #endregion Constructors

        #region Methods (2)

        // Protected Methods (2) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ServiceLocatorBase.OnGetAllInstances(Type)" />
        protected override IEnumerable<object> OnGetAllInstances(Type serviceType)
        {
            return this._PROVIDER
                       .GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ServiceLocatorBase.OnGetInstance(Type, object)" />
        protected override object OnGetInstance(Type serviceType, object key)
        {
            var strKey = key.AsString(true);
            if (string.IsNullOrWhiteSpace(strKey))
            {
                strKey = AttributedModelServices.GetContractName(serviceType);
            }

            var lazyInstance = this._PROVIDER
                                   .GetExports<object>(strKey)
                                   .FirstOrDefault();
            if (lazyInstance != null)
            {
                return lazyInstance.Value;
            }

            return null;
        }

        #endregion Methods
    }
}
