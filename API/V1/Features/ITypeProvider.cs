using System;

namespace PrettyPatch.API.V1.Features
{
    public interface ITypeProvider
    {
        Type ResolveType();
    }
}