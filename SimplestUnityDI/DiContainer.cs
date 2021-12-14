using System;
using System.Linq;
using SimplestUnityDI.Exceptions;
using System.Collections.Generic;
using SimplestUnityDI.Baking;
using SimplestUnityDI.Dependencies;

namespace SimplestUnityDI
{
    public class DiContainer
    {
        public static DiContainer Instance => _instance ?? (_instance = new DiContainer());
        private static DiContainer _instance;

        private readonly IDictionary<Type, Dependency[]> _dependencies;

        private DiContainer()
        {
            _dependencies = new Dictionary<Type, Dependency[]>();
        }

        public T Resolve<T>(string id = "")
        {
            return (T) Resolve(typeof(T), id);
        }

        public object Resolve(Type type, string id = "")
        {
            if (!_dependencies.ContainsKey(type))
                throw new ContainerException($"Type {type.FullName} is not registered");

            Dependency[] all = _dependencies[type];

            // Gets the dependency with the same id or the first one
            Dependency first = all[0];
            foreach (Dependency d in all)
            {
                if (d.Id == id)
                {
                    first = d;
                    break;
                }
            }

            return first.GetInstance(this) ?? throw new ContainerException($"Type {type.FullName} provided null");
        }

        public object[] ResolveParameters(BakedParameter[] bakedParameters)
        {
            object[] result = new object[bakedParameters.Length];

            for (int i = 0; i < bakedParameters.Length; i++)
            {
                BakedParameter parameter = bakedParameters[i];
                result[i] = Resolve(parameter.ParamType, parameter.Name);
            }

            return result;
        }

        public DependencyBuilder<TConcrete, TConcrete> Register<TConcrete>()
        {
            return Register<TConcrete, TConcrete>();
        }

        public DependencyBuilder<TContract, TConcrete> Register<TContract, TConcrete>()
            where TConcrete : TContract
        {
            return new DependencyBuilder<TContract, TConcrete>(AddDependency);
        }

        private void AddDependency(Dependency dependency)
        {
            Type type = dependency.ContractType;

            if (!_dependencies.TryGetValue(type, out Dependency[] array))
                array = Array.Empty<Dependency>();

            if (array.Contains(dependency))
                throw new ContainerException($"{dependency} is already registered");

            _dependencies[type] = array.Append(dependency).ToArray();
        }
    }
}