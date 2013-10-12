// s. http://blog.marcel-kloubert.de


using System;
using MarcelJoachimKloubert.Blog.Diagnostics;


/// <summary>
/// Klasse zum globalen Zugriff auf eine <see cref="ILoggerFacade" />.
/// </summary>
public static class GlobalLogger
{
    #region Fields (1)

    private static Func<ILoggerFacade> _provider;

    #endregion Fields

    #region Properties (1)

    /// <summary>
    /// Gibt den aktuellen logger zurück.
    /// </summary>
    public static ILoggerFacade Current
    {
        get { return _provider(); }
    }

    #endregion Properties

    #region Methods (1)

    // Public Methods (1) 

    /// <summary>
    /// Definiert die Funktion, die den Wert für die
    /// <see cref="GlobalLogger.Current" /> Eigenschaft zurückgibt.
    /// </summary>
    /// <param name="newProvider">Die neue Funktion.</param>
    public static void SetLoggerProvider(Func<ILoggerFacade> newProvider)
    {
        _provider = newProvider;
    }

    #endregion Methods
}
