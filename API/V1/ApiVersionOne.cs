using System;
using System.Collections.Generic;
using System.Reflection;
using MonoMod.RuntimeDetour;
using PrettyPatch.API.V1.Detouring;
using PrettyPatch.Util;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace PrettyPatch.API.V1
{
    public sealed class ApiVersionOne : ApiVersion
    {
        private readonly List<Mod> BootstrappedMods = new();
        private bool Initialized;
        
        public override void Initialize() {
            if (Initialized) return;
            Initialized = true;
            MonoModHooks.RequestNativeAccess();
        }

        public override bool HasBootstrapped(Mod mod) {
            return BootstrappedMods.Contains(mod);
        }

        public override void Bootstrap(Mod mod) {
            MonoModHooks.RequestNativeAccess();
            BootstrappedMods.Add(mod);

            foreach (Type type in AssemblyManager.GetLoadableTypes(mod.GetType().Assembly)) {
                BootstrapType(type, mod);
            }
        }

        public override void Unbootstrap(Mod mod, bool failed) {
            if (failed) {
                // TODO: Handle logic on failure here. We need to determine what that entails.
            }

            BootstrappedMods.Remove(mod);
        }

        public override Version GetVersion() {
            // return PrettyPatchMod.Get().Version;
            return new Version(1, 0, 0);
        }

        private static void BootstrapType(Type type, Mod mod) {
            MethodInfo[] methods = type.GetMethods();
            ApplyDetours(methods, mod);
        }
        
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