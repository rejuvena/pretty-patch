using System;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using PrettyPatch.API;
using PrettyPatch.API.V1.Detouring;
using PrettyPatch.API.V1.ILEditing;
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
        
        [ILEdit("Terraria.Main", "System.Void DrawCursor(Microsoft.Xna.Framework.Vector2,System.Boolean)")]
        public static void DrawCursorEdit(ILContext il) {
            ILCursor c = new(il);

            c.Emit(OpCodes.Ldarg_1);
            c.Emit(OpCodes.Ldc_I4_0);
            c.Emit(OpCodes.Ceq);
            c.Emit(OpCodes.Starg, 1);
        }

        // TODO: Null checking here? :thinking:
        internal static PrettyPatchMod Get() {
            return ModContent.GetInstance<PrettyPatchMod>() ?? BackupInstanceUnsafe;
        }
    }
}