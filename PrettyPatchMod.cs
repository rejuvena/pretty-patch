using System;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using PrettyPatch.API;
using PrettyPatch.API.V1.Features.Detouring;
using PrettyPatch.Exceptions;
using PrettyPatch.Util;
using Terraria.ModLoader;

namespace PrettyPatch
{
    [UsedImplicitly]
    public sealed class PrettyPatchMod : Mod
    {
        // Should be used sparingly. Ideally, never.
        internal static PrettyPatchMod BackupInstanceUnsafe = null!;
        
        public PrettyPatchMod() {
            BackupInstanceUnsafe = this;

            if (!Bootstrapper.Bootstrap(this, ApiVersion.One))
                throw new BootstrapFailedModLoadException(Messages.BootstrapFailedPrettyPatch(ApiVersion.One.GetVersion().ToString()));
        }

        // Reflection name: Void DrawCursor(Microsoft.Xna.Framework.Vector2, Boolean)
        // Mono.Cecil name: System.Void DrawCursor(Microsoft.Xna.Framework.Vector2,System.Boolean)
        [Detour("Terraria.Main", "System.Void DrawCursor(Microsoft.Xna.Framework.Vector2,System.Boolean)")]
        // [Detour(typeof(Terraria.Program), "Terraria.Main", "DrawCursor")]
        // [Detour(typeof(Terraria.Main), "DrawCursor")]
        public static void DrawCursorDetour(Action<Vector2, bool> orig, Vector2 bonus, bool smart) {
            orig(bonus, !smart);
        }

        // TODO: Null checking here? :thinking:
        internal static PrettyPatchMod Get() {
            return ModContent.GetInstance<PrettyPatchMod>() ?? BackupInstanceUnsafe;
        }
    }
}