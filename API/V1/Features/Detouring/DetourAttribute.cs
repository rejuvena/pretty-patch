using System;
using System.Reflection;
using System.Runtime.Loader;
using PrettyPatch.Exceptions;
using PrettyPatch.Util;
using Terraria.ModLoader;

namespace PrettyPatch.API.V1.Features.Detouring
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class DetourAttribute : Attribute, IDetourAttribute
    {
        private class SimpleMethodProvider : IMethodProvider
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

        private class AssemblylessTypeProvider : ITypeProvider
        {
            internal AssemblyLoadContext?[]? Contexts { get; set; }

            private string TypeName { get; }

            public AssemblylessTypeProvider(string typeName) {
                TypeName = typeName;
            }

            public Type ResolveType() {
                Type? alclessType = Type.GetType(TypeName);
                if (alclessType is not null) return alclessType;
                if (Contexts is null) throw new TypeNotFoundException(TypeName);

                foreach (AssemblyLoadContext alc in Contexts) {
                    if (alc is null) continue;

                    foreach (Assembly assembly in alc.Assemblies) {
                        Type? type = assembly.GetType(TypeName);
                        if (type is not null) return type;
                    }
                }

                throw new TypeNotFoundException(TypeName);
            }
        }

        private class AssemblyTypeProvider : ITypeProvider
        {
            private Type AssemblyProvider { get; }

            private string TypeName { get; }

            public AssemblyTypeProvider(Type assemblyProvider, string typeName) {
                AssemblyProvider = assemblyProvider;
                TypeName = typeName;
            }

            public Type ResolveType() {
                return AssemblyProvider.Assembly.GetType(TypeName) ?? throw new TypeNotFoundException(TypeName);
            }
        }

        private class KnownTypeProvider : ITypeProvider
        {
            private Type Type { get; }

            public KnownTypeProvider(Type type) {
                Type = type;
            }

            public Type ResolveType() {
                return Type;
            }
        }

        public IMethodProvider MethodProvider { get; private set; }

        public DetourAttribute(string typeName, string methodName) {
            MethodProvider = new SimpleMethodProvider(new AssemblylessTypeProvider(typeName), methodName);
        }

        public DetourAttribute(Type assemblyProvider, string typeName, string methodName) {
            MethodProvider = new SimpleMethodProvider(new AssemblyTypeProvider(assemblyProvider, typeName), methodName);
        }

        public DetourAttribute(Type type, string methodName) {
            MethodProvider = new SimpleMethodProvider(new KnownTypeProvider(type), methodName);
        }

        public void Contextualize(Mod mod) {
            if (MethodProvider is SimpleMethodProvider {TypeProvider: AssemblylessTypeProvider typeProvider})
                typeProvider.Contexts = new[]
                {
                    // TODO: default should satisfy this, but this is preliminary for coremods
                    AssemblyLoadContext.GetLoadContext(typeof(Terraria.Program).Assembly),
                    AssemblyLoadContext.Default,
                    AssemblyLoadContext.GetLoadContext(mod.GetType().Assembly),
                };
        }
    }
}