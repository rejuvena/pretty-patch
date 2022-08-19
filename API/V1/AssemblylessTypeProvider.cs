using System;
using System.Reflection;
using System.Runtime.Loader;
using PrettyPatch.Exceptions;

namespace PrettyPatch.API.V1
{
    public class AssemblylessTypeProvider : ITypeProvider
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
}