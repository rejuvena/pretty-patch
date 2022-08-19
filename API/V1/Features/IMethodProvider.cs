using System.Reflection;

namespace PrettyPatch.API.V1.Features
{
    public interface IMethodProvider
    {
        MethodInfo ResolveMethod();
    }
}