namespace SimplestUnityDI.Dependencies.Providers
{
    public interface IProvider
    {
        object Provide(DiContainer container);
    }
}