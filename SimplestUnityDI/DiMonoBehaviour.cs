using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using SimplestUnityDI.Baking;
using UnityEngine;

namespace SimplestUnityDI
{
    public abstract class DiMonoBehaviour : MonoBehaviour
    {
        private static readonly IDictionary<Type, BakedMethod> BakedSetups = new Dictionary<Type, BakedMethod>();

        private void Awake()
        {
            DiContainer container = DiContainer.Instance;
            Type type = GetType();

            if (!BakedSetups.TryGetValue(type, out BakedMethod method))
            {
                MethodInfo setupInfo = type.GetRuntimeMethods().FirstOrDefault(o => o.Name == "Setup");
                if (setupInfo is null)
                    throw new InvalidOperationException($"Type {type.FullName} must declare a method named Setup");

                method = new BakedMethod(setupInfo);
                BakedSetups[type] = method;
            }

            method.Action(this, container.ResolveParameters(method.Parameters));
        }

        /// <summary>
        /// Will execute after dependency injection happens, on Awake
        /// </summary>
        protected virtual void AfterInjection() { }
    }
}