// s. http://blog.marcel-kloubert.de


using System.ServiceModel;

namespace MarcelJoachimKloubert.Blog.Remoting
{
    /// <summary>
    /// Eine WCF-"Fernbedienung".
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required,
                     CallbackContract = typeof(global::MarcelJoachimKloubert.Blog.Remoting.IRemoteControlCallback))]
    public interface IRemoteControl
    {
        #region Operations (3)

        /// <summary>
        /// Schliesst die aktuelle Session.
        /// </summary>
        [OperationContract(IsInitiating = false,
                           IsOneWay = true,
                           IsTerminating = true)]
        void Close();

        /// <summary>
        /// Sendet eine generische Nachricht.
        /// </summary>
        /// <param name="id">Die ID der Nachricht.</param>
        /// <param name="args">Die serialisierten Argumente für die Nachricht.</param>
        /// <returns>Die Rückgabe der Nachricht.</returns>
        [OperationContract(IsInitiating = false,
                           IsOneWay = false,
                           IsTerminating = false)]
        string SendMessage(int id, string args);

        /// <summary>
        /// Startet eine Session.
        /// </summary>
        /// <param name="user">Der Benutzername.</param>
        /// <param name="password">Das Passwort.</param>
        /// <param name="clientId">Die ID des Clients der sich anmelden will.</param>
        /// <param name="isResultCompressed">Die Variabel, die angibt, ob die Rückgabe GZIP komprimiert ist oder nicht.</param>
        /// <returns>Die XML-Daten, die die Session beschreiben.</returns>
        [OperationContract(IsInitiating = true,
                           IsOneWay = false,
                           IsTerminating = false)]
        byte[] Start(string user, string password,
                     byte[] clientId,
                     out bool isResultCompressed);

        #endregion Operations
    }
}
