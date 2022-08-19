using System;
using System.Collections.Generic;
using System.Reflection;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;
using PrettyPatch.API.V1.Detouring;
using PrettyPatch.API.V1.ILEditing;
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
                foreach (MethodInfo method in type.GetMethods()) {
                    if (!method.IsStatic) continue;

                    IILEditAttribute[] ilEdits = method.GetInterfaceAttributes<IILEditAttribute>();
                    IDetourAttribute[] detours = method.GetInterfaceAttributes<IDetourAttribute>();

                    foreach (IILEditAttribute ilEdit in ilEdits) {
                        ilEdit.Contextualize(mod);
                        MethodInfo origMethod = ilEdit.MethodProvider.ResolveMethod();
                        HookEndpointManager.Modify(origMethod, Delegate.CreateDelegate(typeof(ILContext.Manipulator), method));
                    }
                    
                    foreach (IDetourAttribute detour in detours) {
                        detour.Contextualize(mod);
                        MethodInfo origMethod = detour.MethodProvider.ResolveMethod();
                        IDetour hook = new Hook(origMethod, method);
                        hook.Apply();
                    }
                }
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
    }
}