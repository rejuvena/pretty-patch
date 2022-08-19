using System.Reflection;

namespace PrettyPatch.API.V1
{
    public interface IMethodProvider
    {
        MethodInfo ResolveMethod();
    }
}