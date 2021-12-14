using System;
using SimplestUnityDI;

namespace Tests
{
    public class DisposingEvent : IDisposingEvent
    {
        public event Action Disposing;

        public void OnDisposing()
        {
            Disposing?.Invoke();
        }
    }
}