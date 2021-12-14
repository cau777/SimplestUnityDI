using System;
using JetBrains.Annotations;
using SimplestUnityDI.Dependencies.Providers;

namespace SimplestUnityDI.Dependencies
{
    public interface IDependency
    {
        object GetInstance(DiContainer container);
    }

    public abstract class Dependency : IDependency, IEquatable<Dependency>
    {
        [NotNull]
        public Type ContractType { get; }

        [NotNull]
        public string Id { get; }

        [NotNull] 
        protected readonly IProvider Provider;

        public abstract object GetInstance(DiContainer container);

        protected Dependency([NotNull] IProvider provider, [NotNull] Type contractType, [NotNull] string id)
        {
            Provider = provider;
            ContractType = contractType;
            Id = id;
        }

        public override string ToString()
        {
            return
                $"Dependency(ContractType: {ContractType.FullName}, Id: {Id}, ProviderType: {Provider.GetType().Name})";
        }

        public bool Equals(Dependency other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ContractType == other.ContractType && Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Dependency) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ContractType.GetHashCode() * 397) ^ (Id != null ? Id.GetHashCode() : 0);
            }
        }
    }
}