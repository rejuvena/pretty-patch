using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace PrettyPatch.API.V1
{
    public sealed partial class ApiVersionOne : ApiVersion
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
    }
}