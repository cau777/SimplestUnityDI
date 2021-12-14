using System;
using SimplestUnityDI.Exceptions;
using UnityEngine;

namespace SimplestUnityDI.Dependencies.Providers
{
    public class GameObjectProvider : IProvider
    {
        private readonly string _name;
        private readonly Type _type;
        
        public GameObjectProvider(string name, Type type)
        {
            _name = name;
            _type = type;
        }

        public object Provide(DiContainer container)
        {
            GameObject gameObject = GameObject.Find(_name) ?? throw new ContainerException($"Can't find GameObject named {_name}");
            return gameObject.GetComponent(_type) ?? throw new ContainerException($"GameObject {_name} doesn't contain {_type.Name}");
        }
    }
}