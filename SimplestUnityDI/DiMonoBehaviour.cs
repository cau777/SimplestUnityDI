using UnityEngine;

namespace SimplestUnityDI
{
    public abstract class DiMonoBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            AfterInjection();
        }

        protected virtual void AfterInjection() { }
    }
}