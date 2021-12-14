using System;
using System.Reflection;
using SimplestUnityDI.Baking;
using SimplestUnityDI.Exceptions;

namespace SimplestUnityDI.Dependencies.Providers
{
    /// <summary>
    /// Instantiates a class 
    /// </summary>
    public class ConstructorProvider : IProvider
    {
        private readonly BakedConstructor _baked;
        
        public ConstructorProvider(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            if (constructors.Length != 1)
                throw new ContainerException($"Type {type} should have exactly 1 constructor");

            ConstructorInfo constructor = constructors[0];
            _baked = new BakedConstructor(constructor);
        }

        public object Provide(DiContainer container)
        {
            return _baked.Action(container.ResolveParameters(_baked.Parameters));
        }
    }
}