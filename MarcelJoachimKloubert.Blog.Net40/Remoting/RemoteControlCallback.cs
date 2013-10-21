// s. http://blog.marcel-kloubert.de


using System;

namespace MarcelJoachimKloubert.Blog.Remoting
{
    /// <summary>
    /// Einfache Implementation von <see cref="IRemoteControlCallback" /> basierend auf Delegates.
    /// </summary>
    public sealed class RemoteControlCallback : IRemoteControlCallback
    {
        #region Fields (3)

        private readonly ReceiveConsoleTextAction _RECEIVE_CONSOLE_ERROR;
        private readonly ReceiveConsoleTextAction _RECEIVE_CONSOLE_STANDARD;
        private readonly ReceiveLogAction _RECEIVE_LOG;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="RemoteControlCallback" />.
        /// </summary>
        /// <param name="receiveLog">Die Logik für <see cref="RemoteControlCallback.ReceiveLog(RemoteLogMessage)" />.</param>
        /// <param name="receiveConsoleError">Die Logik für <see cref="RemoteControlCallback.ReceiveConsoleError(string, int?, int?)" />.</param>
        /// <param name="receiveConsoleStandard">Die Logik für <see cref="RemoteControlCallback.ReceiveConsoleStandard(string, int?, int?)" />.</param>
        public RemoteControlCallback(ReceiveLogAction receiveLog = null,
                                     ReceiveConsoleTextAction receiveConsoleError = null,
                                     ReceiveConsoleTextAction receiveConsoleStandard = null)
        {
            this._RECEIVE_LOG = receiveLog;
            this._RECEIVE_CONSOLE_ERROR = receiveConsoleError;
            this._RECEIVE_CONSOLE_STANDARD = receiveConsoleStandard;
        }

        #endregion Constructors

        #region Delegates and Events (2)

        // Delegates (2) 

        /// <summary>
        /// Beschreibt Logik für <see cref="RemoteControlCallback.ReceiveConsoleError(string, int?, int?)" /> und
        /// <see cref="RemoteControlCallback.ReceiveConsoleStandard(string, int?, int?)" />.
        /// </summary>
        /// <param name="txt">Der empfangene Text.</param>
        /// <param name="foregroundColor">Die Vordergrundfarbe.</param>
        /// <param name="backgroundColor">Die Hintergrundfarbe.</param>
        public delegate void ReceiveConsoleTextAction(string txt, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor);

        /// <summary>
        /// Beschreibt Logik für <see cref="RemoteControlCallback.ReceiveLog(RemoteLogMessage)" />.
        /// </summary>
        /// <param name="msg">Die empfangene Nachricht.</param>
        public delegate void ReceiveLogAction(RemoteLogMessage msg);

        #endregion Delegates and Events

        #region Methods (4)

        // Public Methods (3) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IRemoteControlCallback.ReceiveConsoleError(string, int?, int?)" />
        public void ReceiveConsoleError(string txt, int? foregroundColor, int? backgroundColor)
        {
            if (this._RECEIVE_CONSOLE_ERROR != null)
            {
                this._RECEIVE_CONSOLE_ERROR(txt: txt,
                                            foregroundColor: TryGetConsoleColor(foregroundColor),
                                            backgroundColor: TryGetConsoleColor(backgroundColor));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IRemoteControlCallback.ReceiveConsoleStandard(string, int?, int?)" />
        public void ReceiveConsoleStandard(string txt, int? foregroundColor, int? backgroundColor)
        {
            if (this._RECEIVE_CONSOLE_STANDARD != null)
            {
                this._RECEIVE_CONSOLE_STANDARD(txt: txt,
                                               foregroundColor: TryGetConsoleColor(foregroundColor),
                                               backgroundColor: TryGetConsoleColor(backgroundColor));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IRemoteControlCallback.ReceiveLog(RemoteLogMessage)" />
        public void ReceiveLog(RemoteLogMessage msg)
        {
            if (this._RECEIVE_LOG != null)
            {
                if (msg != null)
                {
                    this._RECEIVE_LOG(msg);
                }
            }
        }
        // Private Methods (1) 

        private static ConsoleColor? TryGetConsoleColor(int? value)
        {
            ConsoleColor? result = null;

            if (value.HasValue)
            {
                ConsoleColor temp;
                if (Enum.TryParse<ConsoleColor>(value.ToString(), false, out temp))
                {
                    result = temp;
                }
            }

            return result;
        }

        #endregion Methods
    }
}
