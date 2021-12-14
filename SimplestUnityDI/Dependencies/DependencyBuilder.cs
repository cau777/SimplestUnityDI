using System;
using JetBrains.Annotations;
using SimplestUnityDI.Dependencies.Providers;
using SimplestUnityDI.Exceptions;

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

        private void FromConstructor()
        {
            Type concreteType = typeof(TConcrete);

            if (concreteType.IsAbstract)
                throw new ContainerException($"The type {concreteType} must not be abstract");

            Provider = new ConstructorProvider(concreteType);
        }

        /// <summary>
        /// Gets a component of the GameObject with the specified name
        /// </summary>
        /// <param name="name">The name of the GameObject</param>
        /// <returns>The builder to continue building</returns>
        public DependencyBuilder<TContract, TConcrete> FromGameObject([NotNull] string name)
        {
            Provider = new GameObjectProvider(name, typeof(TConcrete));
            return this;
        }

        /// <summary>
        /// Uses Unity's Resources.Load to get an object from the Resources folder
        /// </summary>
        /// <param name="path">The path to the file without extension and omitting "Resources/". Example: "My Prefab" for file "Resources/My Prefab.prefab"</param>
        /// <returns>The builder to continue building</returns>
        public DependencyBuilder<TContract, TConcrete> FromResource([NotNull] string path)
        {
            Provider = new ResourceProvider(path);
            return this;
        }

        /// <summary>
        /// Calls a function to receive a object
        /// </summary>
        /// <param name="function">The function that provides the object. It should never return null</param>
        /// <returns>The builder to continue building</returns>
        public DependencyBuilder<TContract, TConcrete> FromFunction([NotNull] Func<DiContainer, TConcrete> function)
        {
            Provider = new FunctionProvider<TConcrete>(function);
            return this;
        }

        /// <summary>
        /// Adds an Id to the dependency. In case 2 dependencies are registered with the same Contract type,
        /// the container chooses the one that has the Id equals to the name of the parameter. Case Insensitive. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The builder to continue building</returns>
        public DependencyBuilder<TContract, TConcrete> WithId([NotNull] string id)
        {
            ID = id.ToLower();
            return this;
        }

        /// <summary>
        /// Specifies that a dependency should not be deleted after changing scenes
        /// </summary>
        /// <returns>The builder to continue building</returns>
        public DependencyBuilder<TContract, TConcrete> Permanent()
        {
            PreventDisposal = true;
            return this;
        }

        /// <summary>
        /// Represents a dependency that always evaluate it's provider
        /// </summary>
        public void AsTransient()
        {
            if (Provider is null) FromConstructor();
            Dependency dependency = new TransientDependency(Provider, typeof(TContract), ID);
            Finish(dependency);
        }

        /// <summary>
        /// Represents a dependency that evaluates it's provider only once
        /// </summary>
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