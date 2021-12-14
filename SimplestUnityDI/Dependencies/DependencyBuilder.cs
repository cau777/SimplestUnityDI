using System;
using JetBrains.Annotations;
using SimplestUnityDI.Dependencies.Providers;
using SimplestUnityDI.Exceptions;

namespace SimplestUnityDI.Dependencies
{
    public class DependencyBuilder<TContract, TConcrete>
    {
        private readonly Action<Dependency> _finished;
        private IProvider _provider;
        private string _id;
        
        public DependencyBuilder(Action<Dependency> finished)
        {
            _finished = finished;
            _provider = new ConstructorProvider();
        }
        
        public DependencyBuilder<TContract, TConcrete> FromInstance([NotNull] TConcrete instance)
        { 
            _provider = new InstanceProvider(instance);
            return this;
        }

        public DependencyBuilder<TContract, TConcrete> WithId([NotNull] string id)
        {
            _id = id;
            return this;
        }

        public void AsTransient()
        {
            Validate();
            Dependency dependency = new TransientDependency(_provider, typeof(TContract), _id);
            _finished(dependency);
        }

        private void Validate()
        {
            
        }
    }
}