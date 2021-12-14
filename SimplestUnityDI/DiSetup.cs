using UnityEngine;

namespace SimplestUnityDI
{
    [DefaultExecutionOrder(-1)] // Executes before most of the scripts
    public abstract class DiSetup : MonoBehaviour
    {
        protected abstract void SetupDependencies(DiContainer diContainer);
        
        private void Awake()
        {
            DiContainer container = DiContainer.Instance;
            SetupDependencies(container);
            AfterSetup(container);
        }

        protected virtual void AfterSetup(DiContainer diContainer)
        {
            
        }
    }
}