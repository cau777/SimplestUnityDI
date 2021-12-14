using System;
using JetBrains.Annotations;
using SimplestUnityDI.Dependencies.Providers;
using SimplestUnityDI.Exceptions;
using UnityEngine.SceneManagement;

namespace SimplestUnityDI.Dependencies
{
    public class DependencyBuilder<TContract, TConcrete>
    {
        public IProvider Provider { get; set; }
        public string ID { get; set; }
        public bool PreventDisposal { get; set; }

        private readonly Action<Dependency> _finished;
        private readonly Action<Dependency> _addToDisposal;

        public DependencyBuilder(Action<Dependency> finished, Action<Dependency> addToDisposal)
        {
            _finished = finished;
            _addToDisposal = addToDisposal;
            ID = "";
        }

        public DependencyBuilder<TContract, TConcrete> FromInstance([NotNull] TConcrete instance)
        {
            Provider = new InstanceProvider(instance);
            return this;
        }

        public DependencyBuilder<TContract, TConcrete> FromConstructor()
        {
            Type concreteType = typeof(TConcrete);

            if (concreteType.IsAbstract)
                throw new ContainerException($"The type {concreteType} must not be abstract");

            Provider = new ConstructorProvider(concreteType);
            return this;
        }

        public DependencyBuilder<TContract, TConcrete> FromGameObject([NotNull] string name)
        {
            Provider = new GameObjectProvider(name, typeof(TConcrete));
            return this;
        }

        public DependencyBuilder<TContract, TConcrete> FromResource([NotNull] string path)
        {
            Provider = new ResourceProvider(path);
            return this;
        }

        public DependencyBuilder<TContract, TConcrete> FromFunction([NotNull] Func<DiContainer, TConcrete> function)
        {
            Provider = new FunctionProvider<TConcrete>(function);
            return this;
        }

        public DependencyBuilder<TContract, TConcrete> WithId([NotNull] string id)
        {
            ID = id.ToLower();
            return this;
        }

        public DependencyBuilder<TContract, TConcrete> Permanent()
        {
            PreventDisposal = true;
            return this;
        }

        public void AsTransient()
        {
            if (Provider is null) FromConstructor();
            Dependency dependency = new TransientDependency(Provider, typeof(TContract), ID);
            Finish(dependency);
        }

        public void AsSingleton()
        {
            if (Provider is null) FromConstructor();
            Dependency dependency = new SingletonDependency(Provider, typeof(TContract), ID);
            Finish(dependency);
        }

        private void Finish(Dependency dependency)
        {
            if (!PreventDisposal) _addToDisposal(dependency);
            _finished(dependency);
        }
    }
}