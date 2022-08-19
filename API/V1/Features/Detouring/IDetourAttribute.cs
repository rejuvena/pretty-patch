using Terraria.ModLoader;

namespace PrettyPatch.API.V1.Features.Detouring
{
    public interface IDetourAttribute
    {
        IMethodProvider MethodProvider { get; }

        void Contextualize(Mod mod);
    }
}