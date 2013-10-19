// s. http://blog.marcel-kloubert.de

// s. http://www.codeproject.com/Articles/19192/How-to-capture-a-Window-as-an-Image-and-save-it


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using MarcelJoachimKloubert.Blog.WindowSnapShot.Values;
using MarcelJoachimKloubert.Blog.WindowSnapShot.Win32;

namespace MarcelJoachimKloubert.Blog.WindowSnapShot.UI
{
    /// <summary>
    /// Verwaltet einen <see cref="Process" /> als GUI Applikation.
    /// </summary>
    public sealed class GuiApp
    {
        #region Fields (4)

        private readonly Process _PROCESS;
        private readonly IntPtr _REAL_HWND;
        private readonly IntPtr[] _WINDOW_HANDLES;
        /// <summary>
        /// Speichert die aktuelle Grösse des Desktops.
        /// </summary>
        public RECT DESKTOP_SIZE;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="GuiApp" />.
        /// </summary>
        /// <param name="proc">Der zugrundeliegende Prozess.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="proc" /> ist <see langword="null" />.
        /// </exception>
        public GuiApp(Process proc)
        {
            if (proc == null)
            {
                throw new ArgumentNullException("proc");
            }

            var desktopHandle = Win32Api.GetDesktopWindow();
            Win32Api.GetWindowRect(desktopHandle,
                                   out DESKTOP_SIZE);

            this._PROCESS = proc;
            this._REAL_HWND = IntPtr.Zero;

            var windowHandleList = new List<IntPtr>();

            var listHandle = default(GCHandle);
            try
            {
                if (proc.MainWindowHandle == IntPtr.Zero)
                {
                    throw new ApplicationException("Can't add a process with no MainFrame");
                }

                var maxRect = default(RECT);    // init with 0
                if (IsValidGUIWnd(proc.MainWindowHandle, DESKTOP_SIZE))
                {
                    this._REAL_HWND = proc.MainWindowHandle;
                    return;
                }

                // the mainFrame is size == 0, so we look for the 'real' window
                listHandle = GCHandle.Alloc(windowHandleList);
                foreach (ProcessThread pt in proc.Threads)
                {
                    Win32Api.EnumThreadWindows((uint)pt.Id,
                                               new Win32Api.EnumThreadDelegate(EnumThreadCallback),
                                               GCHandle.ToIntPtr(listHandle));
                }

                this._WINDOW_HANDLES = windowHandleList.ToArray();

                // get the biggest visible window in the current proc
                var maxHWnd = IntPtr.Zero;
                foreach (var hWnd in _WINDOW_HANDLES)
                {
                    RECT crtWndRect;

                    // do we have a valid rect for this window
                    if (Win32Api.IsWindowVisible(hWnd) && Win32Api.GetWindowRect(hWnd, out crtWndRect) &&
                        crtWndRect.Height > maxRect.Height && crtWndRect.Width > maxRect.Width)
                    {
                        // if the rect is outside the desktop, it's a dummy window

                        RECT visibleRect;
                        if (Win32Api.IntersectRect(out visibleRect, ref this.DESKTOP_SIZE, ref crtWndRect)
                            && !Win32Api.IsRectEmpty(ref visibleRect))
                        {
                            maxHWnd = hWnd;
                            maxRect = crtWndRect;
                        }
                    }
                }

                if (maxHWnd != IntPtr.Zero &&
                    maxRect.Width > 0 &&
                    maxRect.Height > 0)
                {
                    this._REAL_HWND = maxHWnd;
                }
                else
                {
                    this._REAL_HWND = proc.MainWindowHandle;    // just add something even if it's a bad window
                }
            }
            finally
            {
                if (listHandle != default(GCHandle) &&
                    listHandle.IsAllocated)
                {
                    listHandle.Free();
                }
            }
        }

        #endregion Constructors

        #region Properties (4)

        /// <summary>
        /// Gibt den Titel des Prozesses zurück.
        /// </summary>
        public string Caption
        {
            get
            {
                // allocate correct string length first
                int length = Win32Api.GetWindowTextLength(this.HWnd);

                var sb = new StringBuilder(length + 1);
                Win32Api.GetWindowText(this.HWnd, sb, sb.Capacity);

                return sb.ToString();
            }
        }

        /// <summary>
        /// Gibt die Beschreibung des Prozesses zurück.
        /// </summary>
        public string Description
        {
            get
            {
                return string.Format("{0}:{1}",
                                     this._PROCESS.ProcessName,
                                     this.Caption);
            }
        }

        /// <summary>
        /// Gibt das Handle des Prozesses zurück.
        /// </summary>
        public IntPtr HWnd
        {
            get { return this._REAL_HWND; }
        }

        /// <summary>
        /// Gibt den Namen der Klasse des Fensters dieses Prozesses zurück.
        /// </summary>
        public string WindowClass
        {
            get
            {
                var classNameBuilder = new StringBuilder(256);
                Win32Api.GetClassName(this.HWnd,
                                      classNameBuilder,
                                      classNameBuilder.Capacity);

                return classNameBuilder.ToString();
            }
        }

        #endregion Properties

        #region Methods (8)

        // Public Methods (6) 

        /// <summary>
        /// Gibt <see cref="GuiApp" />-Instanzen zurück, die aus validen
        /// GUI Applikationen bestehen.
        /// </summary>
        /// <returns>Die Liste der validen GUI Applikationen.</returns>
        /// <remarks>
        /// Die zugrundeliegende Liste wird über <see cref="Process.GetProcesses()" /> ermittelt.
        /// </remarks>
        public static List<GuiApp> GetValidGuiProcesses()
        {
            return GetValidGuiProcesses(Process.GetProcesses());
        }

        /// <summary>
        /// Gibt <see cref="GuiApp" />-Instanzen zurück, die aus validen
        /// GUI Applikationen bestehen.
        /// </summary>
        /// <param name="processes">Die zugrundliegende Liste der Prozesse.</param>
        /// <returns>Die Liste der validen GUI Applikationen.</returns>
        public static List<GuiApp> GetValidGuiProcesses(IEnumerable<Process> processes)
        {
            var result = new List<GuiApp>();

            if (processes != null)
            {
                foreach (var proc in processes.OfType<Process>())
                {
                    if (proc.MainWindowHandle != IntPtr.Zero)
                    {
                        var entry = new GuiApp(proc);
                        if (entry.IsValidGUIWnd())
                        {
                            result.Add(entry);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gibt alle Fester-Handle des Prozesses zurück.
        /// </summary>
        /// <returns>Die Liste der Handles.</returns>
        public List<IntPtr> GetWindowHandles()
        {
            return new List<IntPtr>(this._WINDOW_HANDLES);
        }

        /// <summary>
        /// Gibt zurück, ob es sich um einen Prozess mit GUI handelt.
        /// </summary>
        /// <returns>Ist ein GUI Prozess oder nicht.</returns>
        public bool IsValidGUIWnd()
        {
            return IsValidGUIWnd(this.HWnd, this.DESKTOP_SIZE);
        }

        /// <summary>
        /// Erzeugt einen Screenshot dieses Fensters.
        /// </summary>
        /// <param name="isClientWnd">Handelt es sich um das Handle eines Client-Fensters oder nícht.</param>
        /// <param name="nCmdShow">Der WindowShowStyle.</param>
        /// <param name="timeForRedrawing">Die Zeit, die gewartet werden soll, bis der Screenshot gemacht wird.</param>
        /// <returns>Der Screenshot oder <see langword="null" />, wenn nicht ermittelbar.</returns>
        public Bitmap MakeScreenshot(bool isClientWnd = false,
                                     Win32Api.WindowShowStyle nCmdShow = Win32Api.WindowShowStyle.Restore,
                                     TimeSpan? timeForRedrawing = null)
        {
            var appWndHandle = this.HWnd;

            if (appWndHandle == IntPtr.Zero ||
                !Win32Api.IsWindow(appWndHandle) ||
                !Win32Api.IsWindowVisible(appWndHandle))
            {
                return null;
            }

            if (Win32Api.IsIconic(appWndHandle))
            {
                Win32Api.ShowWindow(appWndHandle, nCmdShow);    //show it
            }

            if (!Win32Api.SetForegroundWindow(appWndHandle))
            {
                return null;    // can't bring it to front
            }

            if (timeForRedrawing.HasValue)
            {
                // give it some time to redraw
                Thread.Sleep(timeForRedrawing.Value);
            }

            RECT appRect;
            bool res = isClientWnd ? Win32Api.GetClientRect(appWndHandle, out appRect) : Win32Api.GetWindowRect(appWndHandle, out appRect);
            if (!res ||
                appRect.Height == 0 ||
                appRect.Width == 0)
            {
                return null;    // some hidden window
            }

            if (isClientWnd)
            {
                var lt = new Point(appRect.Left, appRect.Top);
                var rb = new Point(appRect.Right, appRect.Bottom);
                Win32Api.ClientToScreen(appWndHandle, ref lt);
                Win32Api.ClientToScreen(appWndHandle, ref rb);

                appRect.Left = lt.X;
                appRect.Top = lt.Y;
                appRect.Right = rb.X;
                appRect.Bottom = rb.Y;
            }

            // intersect with the Desktop rectangle and get what's visible
            IntPtr desktopHandle = Win32Api.GetDesktopWindow();
            RECT desktopRect;
            Win32Api.GetWindowRect(desktopHandle, out desktopRect);

            RECT visibleRect;
            if (!Win32Api.IntersectRect(out visibleRect, ref desktopRect, ref appRect))
            {
                visibleRect = appRect;
            }

            if (Win32Api.IsRectEmpty(ref visibleRect))
            {
                return null;
            }

            var width = visibleRect.Width;
            var height = visibleRect.Height;
            var hdcTo = IntPtr.Zero;
            var hdcFrom = IntPtr.Zero;
            var hBitmap = IntPtr.Zero;
            try
            {
                Bitmap clsRet = null;

                // get device context of the window...
                hdcFrom = isClientWnd ? Win32Api.GetDC(appWndHandle) : Win32Api.GetWindowDC(appWndHandle);

                // create dc that we can draw to...
                hdcTo = Win32Api.CreateCompatibleDC(hdcFrom);
                hBitmap = Win32Api.CreateCompatibleBitmap(hdcFrom, width, height);

                //  validate...
                if (hBitmap != IntPtr.Zero)
                {
                    // copy...
                    var x = appRect.Left < 0 ? -appRect.Left : 0;
                    var y = appRect.Top < 0 ? -appRect.Top : 0;

                    var hLocalBitmap = Win32Api.SelectObject(hdcTo, hBitmap);
                    Win32Api.BitBlt(hdcTo, 0, 0, width, height, hdcFrom, x, y, Win32Api.SRCCOPY);
                    Win32Api.SelectObject(hdcTo, hLocalBitmap);

                    // create bitmap for window image...
                    clsRet = System.Drawing.Image.FromHbitmap(hBitmap);
                }

                return clsRet;
            }
            finally
            {
                //  release...

                if (hdcFrom != IntPtr.Zero)
                {
                    Win32Api.ReleaseDC(appWndHandle, hdcFrom);
                }

                if (hdcTo != IntPtr.Zero)
                {
                    Win32Api.DeleteDC(hdcTo);
                }

                if (hBitmap != IntPtr.Zero)
                {
                    Win32Api.DeleteObject(hBitmap);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="object.ToString()" />
        public override string ToString()
        {
            var result = this.Caption;
            if (result.IsNullOrWhiteSpace())
            {
                result = this.WindowClass;
            }

            return result ?? string.Empty;
        }
        // Private Methods (2) 

        private static bool EnumThreadCallback(IntPtr hWnd, IntPtr lParam)
        {
            var gch = GCHandle.FromIntPtr(lParam);

            var coll = gch.Target as ICollection<IntPtr>;
            if (coll == null)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as ICollection<IntPtr>");
            }

            coll.Add(hWnd);
            return true;
        }

        private static bool IsValidGUIWnd(IntPtr hWnd, RECT desktopSize)
        {
            if (hWnd == IntPtr.Zero ||
                !Win32Api.IsWindow(hWnd) ||
                !Win32Api.IsWindowVisible(hWnd))
            {
                return false;
            }

            RECT crtWndRect;
            if (!Win32Api.GetWindowRect(hWnd, out crtWndRect))
            {
                return false;
            }

            var res = false;

            if (crtWndRect.Height > 0 &&
                crtWndRect.Width > 0)
            {
                // a valid rectangle means the right window is the mainframe and it intersects the desktop

                // if the rectangle is outside the desktop, it's a dummy window
                RECT visibleRect;
                if (Win32Api.IntersectRect(out visibleRect, ref desktopSize, ref crtWndRect) &&
                    !Win32Api.IsRectEmpty(ref visibleRect))
                {
                    res = true;
                }
            }

            return res;
        }

        #endregion Methods
    }
}
