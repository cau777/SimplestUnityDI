using System;

namespace SimplestUnityDI
{
    public interface IDisposingEvent
    {
        event Action Disposing;
    }
}