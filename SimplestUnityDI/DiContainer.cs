using System;
using System.Linq;
using SimplestUnityDI.Exceptions;
using System.Collections.Generic;
using SimplestUnityDI.Dependencies;
using UnityEngine;

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
        
        public DependencyBuilder<TConcrete, TConcrete> Register<TConcrete>()
        {
            return Register<TConcrete, TConcrete>();
        }

        public DependencyBuilder<TContract, TConcrete> Register<TContract, TConcrete>()
            where TConcrete : TContract
        {
            Type concreteType = typeof(TConcrete);
            
            if (concreteType.IsAbstract)
                throw new ContainerException($"The concrete type {concreteType} must not be abstract");

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