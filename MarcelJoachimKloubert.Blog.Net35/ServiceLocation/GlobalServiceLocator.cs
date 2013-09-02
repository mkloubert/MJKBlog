// s. http://blog.marcel-kloubert.de


using System;
using MarcelJoachimKloubert.Blog.ServiceLocation;

/// <summary>
/// Verwaltet die globale Instanz eines <see cref="IServiceLocator" />
/// für die aktuelle <see cref="AppDomain" />.
/// </summary>
public static class GlobalServiceLocator
{
    #region Fields (1)

    private static Func<IServiceLocator> _provider;

    #endregion Fields

    #region Properties (1)

    /// <summary>
    /// Gibt die globale <see cref="IServiceLocator" /> Instanz zurück.
    /// </summary>
    public static IServiceLocator Current
    {
        get { return _provider(); }
    }

    #endregion Properties

    #region Methods (1)

    // Public Methods (1) 

    /// <summary>
    /// Definiert das Factory Delegate, dass die Instanz für die EIgenschaft
    /// <see cref="GlobalServiceLocator.Current" /> zurückgibt.
    /// </summary>
    /// <param name="newProvider">Das neue Delegate.</param>
    public static void SetProvider(Func<IServiceLocator> newProvider)
    {
        _provider = newProvider;
    }

    #endregion Methods
}
