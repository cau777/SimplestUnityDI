using System;
using System.Reflection;

namespace SimplestUnityDI.Baking
{
    public class BakedConstructor
    {
        public Func<object[], object> Action { get; }
        public BakedParameter[] Parameters { get; }

        public BakedConstructor(ConstructorInfo info)
        {
            Action = info.Invoke;
            Parameters = BakedParameter.BakeParameters(info);
        }
    }
}