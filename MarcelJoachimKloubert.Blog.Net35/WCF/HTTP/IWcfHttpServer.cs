using System.ServiceModel;
using System.ServiceModel.Channels;

/// <summary>
/// Beschreibt einen Service contract für einen HTTP Service.
/// </summary>
[ServiceContract]
public interface IWcfHttpServer
{
    #region Operations (1)

    /// <summary>
    /// Bearbeitet eine Anfrage.
    /// </summary>
    /// <param name="message">Die Anfrage.</param>
    /// <returns>Die Anwort.</returns>
    [OperationContract(Action = "*", ReplyAction = "*")]
    Message Request(Message message);

    #endregion Operations
}