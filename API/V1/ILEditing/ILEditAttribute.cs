using System;
using PrettyPatch.Util;
using Terraria.ModLoader;

namespace PrettyPatch.API.V1.ILEditing
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class ILEditAttribute : Attribute, IILEditAttribute
    {
        public IMethodProvider MethodProvider { get; }

        public ILEditAttribute(string typeName, string methodName) {
            MethodProvider = new SimpleMethodProvider(new AssemblylessTypeProvider(typeName), methodName);
        }

        public ILEditAttribute(Type assemblyProvider, string typeName, string methodName) {
            MethodProvider = new SimpleMethodProvider(new AssemblyTypeProvider(assemblyProvider, typeName), methodName);
        }

        public ILEditAttribute(Type type, string methodName) {
            MethodProvider = new SimpleMethodProvider(new KnownTypeProvider(type), methodName);
        }

        public void Contextualize(Mod mod) {
            if (MethodProvider is SimpleMethodProvider {TypeProvider: AssemblylessTypeProvider typeProvider})
                typeProvider.Contexts = Utilities.GetLoadContexts(mod);
        }
    }
}