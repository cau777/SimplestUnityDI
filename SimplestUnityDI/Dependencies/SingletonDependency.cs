using System;
using JetBrains.Annotations;
using SimplestUnityDI.Dependencies.Providers;

namespace SimplestUnityDI.Dependencies
{
    public class SingletonDependency : Dependency
    {
        private object _instance;
        
        public SingletonDependency([NotNull] IProvider provider, [NotNull] Type contractType, [NotNull] string id) : 
            base(provider, contractType, id) { }
        
        public override object GetInstance(DiContainer container)
        {
            return _instance ?? (_instance = Provider.Provide(container));
        }
    }
}