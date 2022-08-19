using Terraria.ModLoader;

namespace PrettyPatch.API.V1.Detouring
{
    public interface IDetourAttribute
    {
        IMethodProvider MethodProvider { get; }

        void Contextualize(Mod mod);
    }
}