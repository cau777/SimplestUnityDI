using System;
using System.Collections;
using SimplestUnityDI.Exceptions;
using System.Collections.Generic;
using System.Linq;
using SimplestUnityDI.Baking;
using SimplestUnityDI.Dependencies;

namespace SimplestUnityDI
{
    /// <summary>
    /// The main class of the library, responsible for storing Dependencies and Types
    /// </summary>
    public class DiContainer : ICollection<Dependency>
    {
        public static DiContainer Instance => _instance ?? (_instance = new DiContainer());
        public IDisposingEvent CurrentDisposingEvent { private get; set; }
        public int Count => _dependencies.Values.SelectMany(o => o).Count();
        public bool IsReadOnly => false;

        private static DiContainer _instance;

        private readonly IDictionary<Type, List<Dependency>> _dependencies;

        private DiContainer()
        {
            _dependencies = new Dictionary<Type, List<Dependency>>();
        }

        /// <summary>
        /// Gets an object of the specified registered type
        /// </summary>
        /// <param name="id">Used to distinguish objects with the same type</param>
        /// <typeparam name="T">The type to receive</typeparam>
        /// <returns></returns>
        public T Resolve<T>(string id = "")
        {
            return (T) Resolve(typeof(T), id);
        }

        /// <summary>
        /// Gets an object of the specified registered type
        /// </summary>
        /// <param name="type">The type to receive</param>
        /// <param name="id">Used to distinguish objects with the same type</param>
        /// <returns></returns>
        /// <exception cref="ContainerException"></exception>
        public object Resolve(Type type, string id = "")
        {
            if (!_dependencies.TryGetValue(type, out List<Dependency> list) || list.Count == 0)
                throw new ContainerException($"Type {type.FullName} is not registered");

            // Gets the dependency with the same id or the first one
            Dependency first = list[0];
            foreach (Dependency d in list)
            {
                if (d.Id == id)
                {
                    first = d;
                    break;
                }
            }

            return first.GetInstance(this) ?? throw new ContainerException($"Type {type.FullName} provided null");
        }

        /// <summary>
        /// Gets an object array for the specified parameters. Use BakedParameter.BakeParameters to extract them from a method or constructor
        /// </summary>
        /// <param name="bakedParameters">The objects containing information about the parameters</param>
        /// <returns>An object array with the same length as backedParameters</returns>
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

        /// <summary>
        /// Registers a dependency in the container
        /// </summary>
        /// <typeparam name="TConcrete">The type to register the dependency as. Use this type to get the added dependency, using the Resolve method.</typeparam>
        /// <returns></returns>
        public DependencyBuilder<TConcrete, TConcrete> Register<TConcrete>()
        {
            return Register<TConcrete, TConcrete>();
        }

        /// <summary>
        /// Registers a dependency in the container
        /// </summary>
        /// <typeparam name="TContract">The type to register the dependency as. Use this type to get the added dependency, using the Resolve method.</typeparam>
        /// <typeparam name="TConcrete">The implementation of TContract</typeparam>
        /// <returns></returns>
        public DependencyBuilder<TContract, TConcrete> Register<TContract, TConcrete>()
            where TConcrete : TContract
        {
            return new DependencyBuilder<TContract, TConcrete>(Add,
                o => CurrentDisposingEvent.Disposing += () => Remove(o));
        }

        public IEnumerator<Dependency> GetEnumerator()
        {
            return _dependencies.Values.SelectMany(o => o).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Dependency item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));

            Type type = item.ContractType;

            if (!_dependencies.TryGetValue(type, out List<Dependency> list))
                list = new List<Dependency>(2);

            if (list.Contains(item))
                throw new ContainerException($"{item} is already registered");

            list.Add(item);
            _dependencies[type] = list;
        }

        public void Clear()
        {
            _dependencies.Clear();
        }

        public bool Contains(Dependency item)
        {
            return _dependencies.Values.SelectMany(o => o).Any(o => o.Equals(item));
        }

        public void CopyTo(Dependency[] array, int arrayIndex)
        {
            _dependencies.Values.SelectMany(o => o).ToArray().CopyTo(array, arrayIndex);
        }

        public bool Remove(Dependency item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));

            return _dependencies.TryGetValue(item.ContractType, out List<Dependency> value) && value.Remove(item);
        }
    }
}