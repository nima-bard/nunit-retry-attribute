using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework.Interfaces;

namespace RetryAttributes
{
    /// <summary>
    /// Extend the implementation of <see cref="IReflectionInfo.GetCustomAttributes{T}"/> by returning
    /// an extra attribute list. It resembles that the given method has the extended attributes in its type.  
    /// </summary>
    public sealed class CustomAttributeMethodWrapper : IMethodInfo
    {
        private readonly IMethodInfo _baseInfo;
        private readonly Attribute[] _extraAttributes;

        public CustomAttributeMethodWrapper(IMethodInfo baseInfo, Attribute[] extraAttributes)
        {
            _baseInfo = baseInfo;
            _extraAttributes = extraAttributes;
        }

        public T[] GetCustomAttributes<T>(bool inherit) where T : class
            => _baseInfo.GetCustomAttributes<T>(inherit)
                .Concat(_extraAttributes.OfType<T>())
                .ToArray();

        public ITypeInfo TypeInfo => _baseInfo.TypeInfo;

        public MethodInfo MethodInfo => _baseInfo.MethodInfo;

        public string Name => _baseInfo.Name;

        public bool IsAbstract => _baseInfo.IsAbstract;

        public bool IsPublic => _baseInfo.IsPublic;

        public bool IsStatic => _baseInfo.IsStatic;

        public bool ContainsGenericParameters => _baseInfo.ContainsGenericParameters;

        public bool IsGenericMethod => _baseInfo.IsGenericMethod;

        public bool IsGenericMethodDefinition => _baseInfo.IsGenericMethodDefinition;

        public ITypeInfo ReturnType => _baseInfo.ReturnType;

        public Type[] GetGenericArguments()
            => _baseInfo.GetGenericArguments();

        public IParameterInfo[] GetParameters()
            => _baseInfo.GetParameters();

        public object Invoke(object fixture, params object[] args)
            => _baseInfo.Invoke(fixture, args);

        public bool IsDefined<T>(bool inherit) where T : class
            => _baseInfo.IsDefined<T>(inherit);

        public IMethodInfo MakeGenericMethod(params Type[] typeArguments)
            => _baseInfo.MakeGenericMethod(typeArguments);
    }
}