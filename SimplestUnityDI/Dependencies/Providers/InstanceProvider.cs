namespace SimplestUnityDI.Dependencies.Providers
{
    /// <summary>
    /// Returns an instance
    /// </summary>
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