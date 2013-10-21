// s. http://blog.marcel-kloubert.de


using System;
using System.ServiceModel;

namespace MarcelJoachimKloubert.Blog.Remoting
{
    /// <summary>
    /// Beschreibt den Callback für <see cref="IRemoteControl" />.
    /// </summary>
    [ServiceContract]
    public interface IRemoteControlCallback
    {
        #region Operations (3)

        /// <summary>
        /// Empfängt Text für <see cref="Console.Error" />.
        /// </summary>
        /// <param name="txt">Der empfangene Text.</param>
        /// <param name="foregroundColor">Die <see cref="int" /> Repräsentation von <see cref="Console.ForegroundColor" />.</param>
        /// <param name="backgroundColor">Die <see cref="int" /> Repräsentation von <see cref="Console.BackgroundColor" />.</param>
        void ReceiveConsoleError(string txt,
                                 int? foregroundColor,
                                 int? backgroundColor);

        /// <summary>
        /// Empfängt Text für <see cref="Console.Out" />.
        /// </summary>
        /// <param name="txt">Der empfangene Text.</param>
        /// <param name="foregroundColor">Die <see cref="int" /> Repräsentation von <see cref="Console.ForegroundColor" />.</param>
        /// <param name="backgroundColor">Die <see cref="int" /> Repräsentation von <see cref="Console.BackgroundColor" />.</param>
        void ReceiveConsoleStandard(string txt,
                                    int? foregroundColor,
                                    int? backgroundColor);

        /// <summary>
        /// Empfängt eine Lognachricht.
        /// </summary>
        /// <param name="msg">Die empfangene Nachricht.</param>
        void ReceiveLog(RemoteLogMessage msg);

        #endregion Operations
    }
}
