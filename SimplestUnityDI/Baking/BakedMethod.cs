using System;
using System.Reflection;

namespace SimplestUnityDI.Baking
{
    public class BakedMethod
    {
        public Func<object, object[], object> Action { get; }
        public BakedParameter[] Parameters { get; }

        public BakedMethod(MethodInfo info)
        {
            Action = info.Invoke;
            Parameters = BakedParameter.BakeParameters(info);
        }
    }
}