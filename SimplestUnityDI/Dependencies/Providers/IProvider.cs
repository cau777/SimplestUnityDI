using JetBrains.Annotations;

namespace SimplestUnityDI.Dependencies.Providers
{
    public interface IProvider
    {
        /// <summary>
        /// Gets an object that has the dependency's type
        /// </summary>
        /// <param name="container"></param>
        /// <returns>An object or null</returns>
        [CanBeNull]
        object Provide([NotNull] DiContainer container);
    }
}