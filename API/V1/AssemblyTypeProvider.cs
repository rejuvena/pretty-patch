using System;
using PrettyPatch.Exceptions;

namespace PrettyPatch.API.V1
{
    public class AssemblyTypeProvider : ITypeProvider
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
}