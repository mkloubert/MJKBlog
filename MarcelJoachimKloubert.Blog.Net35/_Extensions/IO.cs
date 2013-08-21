// s. http://blog.marcel-kloubert.de


using System;
using System.IO;
using System.Security.AccessControl;

/// <summary>
/// Extension Methoden für Eingabe-/Ausgabeoperationen.
/// </summary>
public static partial class __IOExtensionMethods
{
    #region Methods (6)

    // Public Methods (6) 

    /// <summary>
    /// Erstellt eine komplette Verzeichnisstruktur samt übergeordneten Verzeichnissen
    /// ohne Rechte auf die Ordner anzuwenden..
    /// Bevor und nachdem die Struktur erstellt wurde, wird jeweils die
    /// <see cref="FileSystemInfo.Refresh()" /> Methode von <paramref name="dir" /> aufgerufen.
    /// Die Methode wird ebenfalls aufgerufen, wenn das Verzeichnis von <paramref name="dir" />
    /// bereits existiert.
    /// </summary>
    /// <param name="dir">Das Verzeichnis, das erstellt werden soll.</param>
    /// <returns><paramref name="dir" /> selbst.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="dir" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static DirectoryInfo CreateDeep(this DirectoryInfo dir)
    {
        return CreateDeep(dir, null);
    }

    /// <summary>
    /// Erstellt eine komplette Verzeichnisstruktur samt übergeordneten Verzeichnissen.
    /// Bevor und nachdem die Struktur erstellt wurde, wird jeweils die
    /// <see cref="FileSystemInfo.Refresh()" /> Methode von <paramref name="dir" /> aufgerufen.
    /// Die Methode wird ebenfalls aufgerufen, wenn das Verzeichnis von <paramref name="dir" />
    /// bereits existiert.
    /// </summary>
    /// <param name="dir">Das Verzeichnis, das erstellt werden soll.</param>
    /// <param name="sec">Die optionalen Rechte, die auf alle Verzechnisse angewendet werden sollen.</param>
    /// <returns><paramref name="dir" /> selbst.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="dir" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static DirectoryInfo CreateDeep(this DirectoryInfo dir,
                                           DirectorySecurity sec)
    {
        return CreateDeep(dir, sec, true);
    }

    /// <summary>
    /// Erstellt eine komplette Verzeichnisstruktur samt übergeordneten Verzeichnissen
    /// ohne Rechte auf die Ordner anzuwenden.
    /// Nachdem die Struktur erstellt wurde, wird die <see cref="FileSystemInfo.Refresh()" /> Methode
    /// von <paramref name="dir" /> aufgerufen.
    /// Die Methode wird ebenfalls aufgerufen, wenn das Verzeichnis von <paramref name="dir" />
    /// bereits existiert.
    /// </summary>
    /// <param name="dir">Das Verzeichnis, das erstellt werden soll.</param>
    /// <param name="refreshBefore">
    /// <see cref="FileSystemInfo.Refresh()" /> Methode von <paramref name="dir" /> aufrufen
    /// bevor das Verzeichnis erstellt wird.
    /// </param>
    /// <returns><paramref name="dir" /> selbst.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="dir" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static DirectoryInfo CreateDeep(this DirectoryInfo dir,
                                           bool refreshBefore)
    {
        return CreateDeep(dir, null, refreshBefore, true);
    }

    /// <summary>
    /// Erstellt eine komplette Verzeichnisstruktur samt übergeordneten Verzeichnissen.
    /// Nachdem die Struktur erstellt wurde, wird die <see cref="FileSystemInfo.Refresh()" /> Methode
    /// von <paramref name="dir" /> aufgerufen.
    /// Die Methode wird ebenfalls aufgerufen, wenn das Verzeichnis von <paramref name="dir" />
    /// bereits existiert.
    /// </summary>
    /// <param name="dir">Das Verzeichnis, das erstellt werden soll.</param>
    /// <param name="sec">Die optionalen Rechte, die auf alle Verzechnisse angewendet werden sollen.</param>
    /// <param name="refreshBefore">
    /// <see cref="FileSystemInfo.Refresh()" /> Methode von <paramref name="dir" /> aufrufen
    /// bevor das Verzeichnis erstellt wird.
    /// </param>
    /// <returns><paramref name="dir" /> selbst.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="dir" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static DirectoryInfo CreateDeep(this DirectoryInfo dir,
                                           DirectorySecurity sec,
                                           bool refreshBefore)
    {
        return CreateDeep(dir, sec, refreshBefore, true);
    }

    /// <summary>
    /// Erstellt eine komplette Verzeichnisstruktur samt übergeordneten Verzeichnissen
    /// ohne Rechte auf die Ordner anzuwenden.
    /// </summary>
    /// <param name="dir">Das Verzeichnis, das erstellt werden soll.</param>
    /// <param name="refreshBefore">
    /// <see cref="FileSystemInfo.Refresh()" /> Methode von <paramref name="dir" /> aufrufen
    /// bevor das Verzeichnis erstellt wird.
    /// </param>
    /// <param name="refreshAfter">
    /// <see cref="FileSystemInfo.Refresh()" /> Methode von <paramref name="dir" /> aufrufen
    /// nachdem das Verzeichnis erstellt wurde.
    /// Die Methode wird ebenfalls aufgerufen, wenn das Verzeichnis von <paramref name="dir" />
    /// bereits existiert.
    /// </param>
    /// <returns><paramref name="dir" /> selbst.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="dir" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static DirectoryInfo CreateDeep(this DirectoryInfo dir,
                                           bool refreshBefore,
                                           bool refreshAfter)
    {
        return CreateDeep(dir, null, refreshBefore, true);
    }

    /// <summary>
    /// Erstellt eine komplette Verzeichnisstruktur samt übergeordneten Verzeichnissen.
    /// </summary>
    /// <param name="dir">Das Verzeichnis, das erstellt werden soll.</param>
    /// <param name="sec">Die optionalen Rechte, die auf alle Verzechnisse angewendet werden sollen.</param>
    /// <param name="refreshBefore">
    /// <see cref="FileSystemInfo.Refresh()" /> Methode von <paramref name="dir" /> aufrufen
    /// bevor das Verzeichnis erstellt wird.
    /// </param>
    /// <param name="refreshAfter">
    /// <see cref="FileSystemInfo.Refresh()" /> Methode von <paramref name="dir" /> aufrufen
    /// nachdem das Verzeichnis erstellt wurde.
    /// Die Methode wird ebenfalls aufgerufen, wenn das Verzeichnis von <paramref name="dir" />
    /// bereits existiert.
    /// </param>
    /// <returns><paramref name="dir" /> selbst.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="dir" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static DirectoryInfo CreateDeep(this DirectoryInfo dir,
                                           DirectorySecurity sec,
                                           bool refreshBefore,
                                           bool refreshAfter)
    {
        if (dir == null)
        {
            throw new ArgumentNullException("dir");
        }

        var parent = dir.Parent;
        if (parent != null)
        {
            CreateDeep(new DirectoryInfo(parent.FullName), sec, false, false);
        }

        if (refreshBefore)
        {
            dir.Refresh();
        }

        if (!dir.Exists)
        {
            if (sec == null)
            {
                dir.Create();
            }
            else
            {
                dir.Create(sec);
            }
        }
        else
        {
            if (sec != null)
            {
                dir.SetAccessControl(sec);
            }
        }

        if (refreshAfter)
        {
            dir.Refresh();
        }

        return dir;
    }

    #endregion Methods
}