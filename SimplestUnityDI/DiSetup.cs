using System;
using System.Collections.Generic;
using SimplestUnityDI.Dependencies;
using UnityEngine;

namespace SimplestUnityDI
{
    [DefaultExecutionOrder(-1)] // Executes before most of the scripts
    public abstract class DiSetup : MonoBehaviour, IDisposingEvent
    {
        public event Action Disposing;
        protected abstract void SetupDependencies(DiContainer diContainer);
        
        private void Awake()
        {
            DiContainer container = DiContainer.Instance;
            container.CurrentDisposingEvent = this;
            SetupDependencies(container);
            AfterSetup(container);
        }

        private void OnDestroy()
        {
            Disposing?.Invoke();
        }

        protected virtual void AfterSetup(DiContainer diContainer)
        {
            
        }
    }
}