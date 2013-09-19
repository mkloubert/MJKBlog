// s. http://blog.marcel-kloubert.de


using System;
using MarcelJoachimKloubert.Blog;

/// <summary>
/// Extension Methoden für Systemoperationen.
/// </summary>
public static partial class __SystemExtensionMethods
{
    #region Methods (1)

    // Public Methods (1) 

    /// <summary>
    /// Versucht ein <see cref="OperatingSystem" /> Objekt in einen
    /// <see cref="KnownOperatingSystem" /> umzuwandeln.
    /// </summary>
    /// <param name="os">Das Eigabeobjekt.</param>
    /// <returns>
    /// Der umgewandelte Wert oder <see langword="null" />, wenn <paramref name="os" />
    /// einem unbekannten Betriebssystem entspricht.
    /// </returns>
    public static KnownOperatingSystem? TryGetKnownOS(this OperatingSystem os)
    {
        if (os == null)
        {
            throw new ArgumentNullException("os");
        }

        switch (os.Platform)
        {
            case PlatformID.MacOSX:
                return KnownOperatingSystem.MacOSX;

            case PlatformID.Win32S:
                return KnownOperatingSystem.Windows_3_1;

            case PlatformID.Win32Windows:
                switch (os.Version.Major)
                {
                    case 0:
                        return KnownOperatingSystem.Windows_95;

                    case 10:
                        return KnownOperatingSystem.Windows_98;

                    case 90:
                        return KnownOperatingSystem.Windows_ME;
                }
                break;

            case PlatformID.Win32NT:
                switch (os.Version.Major)
                {
                    case 3:
                        return KnownOperatingSystem.Windows_NT_3_51;

                    case 4:
                        return KnownOperatingSystem.Windows_NT_4;

                    case 5:
                        switch (os.Version.Minor)
                        {
                            case 0:
                                return KnownOperatingSystem.Windows_2000;

                            case 1:
                                return KnownOperatingSystem.Windows_XP;

                            case 2:
                                return KnownOperatingSystem.Windows_2003;
                        }
                        break;

                    case 6:
                        switch (os.Version.Minor)
                        {
                            case 0:
                                return KnownOperatingSystem.Windows_Vista_or_Server_2008;

                            case 1:
                                return KnownOperatingSystem.Windows_7_or_Server_2008_R2;

                            case 2:
                                return KnownOperatingSystem.Windows_8_or_Server_2012;
                        }
                        break;
                }
                break;

            case PlatformID.WinCE:
                return KnownOperatingSystem.Windows_CE;

            case PlatformID.Xbox:
                return KnownOperatingSystem.XBox_360;

            case PlatformID.Unix:
                return KnownOperatingSystem.Unix;
        }

        return null;
    }

    #endregion Methods
}
