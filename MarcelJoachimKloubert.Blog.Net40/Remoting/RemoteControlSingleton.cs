// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace MarcelJoachimKloubert.Blog.Remoting
{
    /// <summary>
    /// <see cref="IRemoteControl" /> als Singleton bessen Logiken auf Delegates beruht.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                     IncludeExceptionDetailInFaults = true,
                     ConcurrencyMode = ConcurrencyMode.Multiple)]
    public sealed class RemoteControlSingleton : IRemoteControl
    {
        #region Fields (4)

        private readonly IDictionary<string, IRemoteControlCallback> _CALLBACKS;
        private readonly SendMessageAction _SEND_MESSAGE;
        private readonly StartPredicate _START;
        private readonly StartResultFactory _START_RESULT;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="RemoteControlSingleton" />.
        /// </summary>
        /// <param name="start">Die optionale Logik für <see cref="RemoteControlSingleton.Start(string, string, byte[], out bool)" />.</param>
        /// <param name="startResult">Die optionale Logik, um die Rückgabedaten für <see cref="RemoteControlSingleton.Start(string, string, byte[], out bool)" /> zu generieren.</param>
        /// <param name="sendMessage">Die optionale Logik für <see cref="RemoteControlSingleton.SendMessage(int, string)" />.</param>
        public RemoteControlSingleton(StartPredicate start = null,
                                      StartResultFactory startResult = null,
                                      SendMessageAction sendMessage = null)
        {
            this._START = start;
            this._SEND_MESSAGE = sendMessage;
            this._START_RESULT = startResult;

            this._CALLBACKS = new ConcurrentDictionary<string, IRemoteControlCallback>();

            OperationContext.Current.InstanceContext.Closed += this.OperationContext_Closed;
            OperationContext.Current.InstanceContext.Faulted += this.OperationContext_Faulted;
        }

        #endregion Constructors

        #region Delegates and Events (3)

        // Delegates (3) 

        /// <summary>
        /// Beschreibt Logik für die <see cref="RemoteControlSingleton.SendMessage(int, string)" /> Methode.
        /// </summary>
        /// <param name="id">Die ID der Nachricht.</param>
        /// <param name="args">Die Argumente für die Nachricht.</param>
        /// <param name="resultBuilder">Der <see cref="StringBuilder" />, der für das Bauen der Rückgabe verwendet wird.</param>
        public delegate void SendMessageAction(int id, string args,
                                               ref StringBuilder resultBuilder);
        /// <summary>
        /// Beschreibt eine Methode / Funktion für die <see cref="RemoteControlSingleton.Start(string, string, byte[], out bool)" /> Methode.
        /// </summary>
        /// <param name="user">Der Benutzername, der geprüft werden soll.</param>
        /// <param name="password">Das Passwort, das geprüft werden soll.</param>
        /// <param name="clientId">Die Client ID, die geprüft werden soll.</param>
        /// <param name="errMsg">Die Variabel, in die ggf. die Fehlermeldung für die Exception geschrieben werden soll.</param>
        /// <returns>Kriterien sind erfüllt oder nicht.</returns>
        public delegate bool StartPredicate(string user,
                                            string password,
                                            byte[] clientId,
                                            ref IEnumerable<char> errMsg);
        /// <summary>
        /// Beschreibt eine Methode / Funktion, die die Rückgabe für die
        /// <see cref="RemoteControlSingleton.Start(string, string, byte[], out bool)" /> Methode generiert.
        /// </summary>
        /// <param name="context">Der zugrundeliegende <see cref="OperationContext" />.</param>
        /// <param name="resultStream">Der Stream, der die Rückgabedaten speichert.</param>
        public delegate void StartResultFactory(OperationContext context,
                                                Stream resultStream);

        #endregion Delegates and Events

        #region Methods (7)

        // Public Methods (3) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IRemoteControl.Close()" />
        public void Close()
        {
            this.TryRemoveCallback();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IRemoteControl.SendMessage(int, string)" />
        public string SendMessage(int id, string args)
        {
            if (this._SEND_MESSAGE != null)
            {
                var resultBuilder = new StringBuilder();
                this._SEND_MESSAGE(id, args,
                                   ref resultBuilder);

                return resultBuilder != null ? resultBuilder.ToString() : null;
            }

            throw new NotImplementedException(id.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IRemoteControl.Start(string, string, byte[], out bool)" />
        public byte[] Start(string user, string password, byte[] clientId, out bool isResultCompressed)
        {
            IEnumerable<char> errMsg = null;
            if (!this._START(user, password, clientId, ref errMsg))
            {
                var strErrorMsg = errMsg.AsString();
                if (strErrorMsg.IsNullOrWhiteSpace())
                {
                    throw new SecurityException();
                }
                else
                {
                    throw new SecurityException(strErrorMsg.Trim());
                }
            }

            byte[] result = null;
            isResultCompressed = false;    // TODO

            var remoteAddr = TryGetRemoteAddress();
            if (remoteAddr != null)
            {
                if (this._START_RESULT != null)
                {
                    using (var resultStream = new MemoryStream())
                    {
                        this._START_RESULT(OperationContext.Current,
                                           resultStream);
                    }
                }

                var cb = OperationContext.Current.GetCallbackChannel<IRemoteControlCallback>();
                this._CALLBACKS[remoteAddr] = cb;
            }

            OperationContext.Current.Channel.Closed += this.OperationContext_Closed;
            OperationContext.Current.Channel.Faulted += this.OperationContext_Faulted;

            return result;
        }
        // Private Methods (4) 

        private void OperationContext_Closed(object sender, EventArgs e)
        {
            this.TryRemoveCallback();
        }

        private void OperationContext_Faulted(object sender, EventArgs e)
        {
            this.TryRemoveCallback();
        }

        private static string TryGetRemoteAddress()
        {
            string result = null;

            try
            {
                var context = OperationContext.Current;
                if (context != null)
                {
                    var prop = context.IncomingMessageProperties;
                    if (prop != null)
                    {
                        var ep = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                        if (ep != null)
                        {
                            if (!string.IsNullOrWhiteSpace(ep.Address))
                            {
                                result = ep.Address.ToLower().Trim();
                            }
                        }
                    }
                }
            }
            catch
            {
                result = null;
            }

            return result;
        }

        private void TryRemoveCallback()
        {
            var remoteAddr = TryGetRemoteAddress();
            if (remoteAddr != null)
            {
                IRemoteControlCallback cb;
                if (this._CALLBACKS.TryGetValue(remoteAddr, out cb))
                {
                    this._CALLBACKS
                        .Remove(remoteAddr);
                }
            }
        }

        #endregion Methods
    }
}
