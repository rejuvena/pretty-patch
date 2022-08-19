using System;
using System.Reflection;
using PrettyPatch.Exceptions;
using PrettyPatch.Util;

namespace PrettyPatch.API.V1
{
    public class SimpleMethodProvider : IMethodProvider
    {
        internal ITypeProvider TypeProvider { get; }

        private string MethodName { get; }

        public SimpleMethodProvider(ITypeProvider typeProvider, string methodName) {
            TypeProvider = typeProvider;
            MethodName = methodName;
        }

        public MethodInfo ResolveMethod() {
            Type type = TypeProvider.ResolveType();
            MethodInfo? method = Utilities.AdvancedGetMethod(type, MethodName);
            return method ?? throw new MethodNotFoundException(type.FullName + "::" + MethodName);
        }
    }
}