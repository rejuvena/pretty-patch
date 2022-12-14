using System;
using PrettyPatch.Util;
using Terraria.ModLoader;

namespace PrettyPatch.API.V1.Detouring
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class DetourAttribute : Attribute, IDetourAttribute
    {
        public IMethodProvider MethodProvider { get; }

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
                typeProvider.Contexts = Utilities.GetLoadContexts(mod);
        }
    }
}