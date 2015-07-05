namespace Bookie.Common
{
    public interface ISplashScreen
    {
        void AddMessage(string message);

        void LoadComplete();
    }
}