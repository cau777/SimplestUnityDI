using System;
using System.Reflection;

namespace SimplestUnityDI.Baking
{
    public class BakedParameter
    {
        public Type ParamType { get; }
        public string Name { get; }

        public BakedParameter(Type paramType, string name)
        {
            ParamType = paramType;
            Name = name;
        }
        
        public static BakedParameter[] BakeParameters(MethodBase method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            BakedParameter[] result = new BakedParameter[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo info = parameters[i];

                result[i] = new BakedParameter(info.ParameterType, info.Name.ToLower());
            }

            return result;
        }
    }
}