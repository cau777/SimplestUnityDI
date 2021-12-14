using SimplestUnityDI.Exceptions;
using UnityEngine;

namespace SimplestUnityDI.Dependencies.Providers
{
    /// <summary>
    /// Uses Unity's Resources.Load to get an object from the Resources folder
    /// </summary>
    public class ResourceProvider : IProvider
    {
        private readonly string _path;
        public ResourceProvider(string path)
        {
            _path = path;
        }

        public object Provide(DiContainer container)
        {
            return Resources.Load(_path) ?? throw new ContainerException($"Resource {_path} was not found");
        }
    }
}