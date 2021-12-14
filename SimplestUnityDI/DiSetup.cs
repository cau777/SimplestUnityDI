using System;
using UnityEngine;

namespace SimplestUnityDI
{
    /// <summary>
    /// Inherit this class to add dependencies to the container. It automatically executes before other scripts
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public abstract class DiSetup : MonoBehaviour, IDisposingEvent
    {
        public event Action Disposing;
        protected abstract void SetupDependencies(DiContainer diContainer);
        
        private void Awake()
        {
            DiContainer container = DiContainer.Instance;
            container.CurrentDisposingEvent = this;
            container.Register<DiContainer>().FromInstance(container).AsSingleton();
            SetupDependencies(container);
            AfterSetup(container);
        }

        private void OnDestroy()
        {
            Disposing?.Invoke();
        }

        /// <summary>
        /// Will be called after setting up the Dependency Injection container 
        /// </summary>
        /// <param name="diContainer"></param>
        protected virtual void AfterSetup(DiContainer diContainer)
        {
            
        }
    }
}