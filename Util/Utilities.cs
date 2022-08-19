using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Mono.Cecil;
using PrettyPatch.Exceptions;
using Terraria.ModLoader;

namespace PrettyPatch.Util
{
    internal static class Utilities
    {
        private static readonly ModuleDefinition FakeModule = ModuleDefinition.CreateModule("FakeModule", ModuleKind.Dll);

        internal static MethodInfo? AdvancedGetMethod(Type? type, string methodName) {
            if (type is null) return null;

            // TODO: Better check here? lol.
            if (!methodName.Contains(' '))
                return type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            MethodInfo[] methods = type.GetMethods();
            MethodInfo? match = methods.FirstOrDefault(x =>
            {
                MethodReference mRef = FakeModule.ImportReference(x);
                return x.ToString() == methodName || mRef.ToString().Replace($"{mRef.DeclaringType.FullName}::", "") == methodName;
            });

            return match ?? throw new TypeNotFoundException(methodName);
        }

        internal static T? GetInterfaceAttribute<T>(this MethodInfo element)
            where T : class {
            return element.GetCustomAttributes().FirstOrDefault(x => x is T) as T;
        }

        internal static T[] GetInterfaceAttributes<T>(this MethodInfo element)
            where T : class {
            return element.GetCustomAttributes().Where(x => x is T).Select(x => (x as T)!).ToArray();
        }

        internal static AssemblyLoadContext?[] GetLoadContexts(Mod mod) {
            return new[]
            {
                // TODO: default should satisfy this, but this is preliminary for coremods
                AssemblyLoadContext.GetLoadContext(typeof(Terraria.Program).Assembly),
                AssemblyLoadContext.Default,
                AssemblyLoadContext.GetLoadContext(mod.GetType().Assembly),
            };
        }
    }
}