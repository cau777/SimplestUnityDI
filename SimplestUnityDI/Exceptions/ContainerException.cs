using System;

namespace SimplestUnityDI.Exceptions
{
    public class ContainerException : Exception
    {
        public ContainerException(string message) : base(message) { }
    }
}