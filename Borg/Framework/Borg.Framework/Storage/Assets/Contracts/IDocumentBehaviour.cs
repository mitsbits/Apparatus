namespace Borg.Framework.Storage.Assets.Contracts
{
    public interface IDocumentBehaviour
    {
        DocumentBehaviourState DocumentBehaviourState { get; }
    }

    public enum DocumentBehaviourState
    {
        Commited = 0,
        Processing = 1
    }
}