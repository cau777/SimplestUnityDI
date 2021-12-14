using System;
using JetBrains.Annotations;
using SimplestUnityDI.Dependencies.Providers;

namespace SimplestUnityDI.Dependencies
{
    public class TransientDependency : Dependency
    {
        public TransientDependency([NotNull] IProvider provider, [NotNull] Type contractType, [NotNull] string id) :
            base(provider, contractType, id) { }

        public override object GetInstance(DiContainer container)
        {
            return Provider.Provide(container);
        }
    }
}