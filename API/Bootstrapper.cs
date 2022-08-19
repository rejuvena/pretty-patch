using System;
using PrettyPatch.Util;
using Terraria.ModLoader;

namespace PrettyPatch.API
{
    /// <summary>
    ///     Used to bootstrap the loading of mods making use of the PrettyPatch API.
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        ///     Bootstraps the loading of a <paramref name="mod"/> in accordance to the API <paramref name="version"/>.
        /// </summary>
        /// <param name="mod">The mod to bootstrap.</param>
        /// <param name="version">The API version to bootstrap with.</param>
        /// <returns>True if bootstrapping was successful, otherwise false.</returns>
        public static bool Bootstrap(Mod mod, ApiVersion version) {
            version.Initialize(); // Versions can just check if they're already initialized themselves.

            if (version.HasBootstrapped(mod)) {
                PrettyPatchMod.Get().Logger.Warn(Messages.RepeatedBootstrapRequest(mod.Name, mod.DisplayName, version.GetVersion().ToString()));
                return false;
            }

            try {
                version.Bootstrap(mod);
                return true;
            }
            catch (Exception e) {
                version.Unbootstrap(mod, true);
                return false;
            }
        }
    }
}