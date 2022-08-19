using System.Collections.Generic;
using System.Reflection;
using MonoMod.RuntimeDetour;
using PrettyPatch.API.V1.Detouring;
using PrettyPatch.Util;
using Terraria.ModLoader;

namespace PrettyPatch.API.V1
{
    partial class ApiVersionOne
    {
        private static void ApplyDetours(IEnumerable<MethodInfo> methods, Mod mod) {
            foreach (MethodInfo method in methods) {
                if (!method.IsStatic) continue;
                
                IDetourAttribute[] detours = method.GetInterfaceAttributes<IDetourAttribute>();
                if (detours.Length == 0) continue;

                foreach (IDetourAttribute detour in detours) {
                    detour.Contextualize(mod);
                    MethodInfo origMethod = detour.MethodProvider.ResolveMethod();
                    IDetour hook = new Hook(origMethod, method);
                    hook.Apply();
                }
            }
        }
    }
}