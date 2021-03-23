namespace SocketCommon.Contracts
{
    public partial interface IObserver
    {
        void Notify(Pattern.Observable sender, object eventArgs);
    }
}
