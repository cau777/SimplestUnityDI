using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using SimplestUnityDI;

namespace Tests
{
    public static class Utils
    {
        public static DiContainer Container
        {
            get
            {
                if (_containerConstructor is null)
                    _containerConstructor = () =>
                        typeof(DiContainer).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0]
                            .Invoke(Array.Empty<object>()) as DiContainer;

                DiContainer container = _containerConstructor();
                container.CurrentDisposingEvent = new DisposingEvent();
                return container;
            }
        }

        private static Func<DiContainer> _containerConstructor;

        public static void Log(object obj)
        {
            TestContext.Out.WriteLine(obj);
        }

        public static void InvokeVoid<T>(string name, T obj, params object[] parameters)
        {
            typeof(T).GetRuntimeMethods().First(o => string.Equals(o.Name, name, StringComparison.OrdinalIgnoreCase))
                .Invoke(obj, parameters);
        }
    }
}