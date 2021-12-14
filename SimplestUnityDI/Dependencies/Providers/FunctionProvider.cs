using System;

namespace SimplestUnityDI.Dependencies.Providers
{
    public class FunctionProvider<TConcrete> : IProvider
    {
        private readonly Func<DiContainer, TConcrete> _function;
        
        public FunctionProvider(Func<DiContainer, TConcrete> function)
        {
            _function = function;
        }

        public object Provide(DiContainer container)
        {
            return _function(container);
        }
    }
}