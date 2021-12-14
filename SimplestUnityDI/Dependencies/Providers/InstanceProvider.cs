namespace SimplestUnityDI.Dependencies.Providers
{
    public class InstanceProvider : IProvider
    {
        private readonly object _instance;
        
        public InstanceProvider(object instance)
        {
            _instance = instance;
        }

        public object Provide(DiContainer container)
        {
            return _instance;
        }
    }
}