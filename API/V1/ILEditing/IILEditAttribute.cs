using Terraria.ModLoader;

namespace PrettyPatch.API.V1.ILEditing
{
    public interface IILEditAttribute
    {
        IMethodProvider MethodProvider { get; }

        void Contextualize(Mod mod);
    }
}