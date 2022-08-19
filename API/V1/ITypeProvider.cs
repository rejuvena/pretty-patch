using System;

namespace PrettyPatch.API.V1
{
    public interface ITypeProvider
    {
        Type ResolveType();
    }
}