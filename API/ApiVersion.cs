using System;
using PrettyPatch.API.V1;
using Terraria.ModLoader;

namespace PrettyPatch.API
{
    public abstract class ApiVersion
    {
        public static readonly ApiVersion One = new ApiVersionOne();

        public abstract void Initialize();
        
        /// <summary>
        ///     Whether a <paramref name="mod"/> has already been bootstrapped.
        /// </summary>
        /// <param name="mod">The mod to check.</param>
        /// <returns>True if the mod has been bootstrapped, otherwise false.</returns>
        public abstract bool HasBootstrapped(Mod mod);

        /// <summary>
        ///     Bootstraps the <paramref name="mod"/>.
        /// </summary>
        /// <param name="mod">The mod to bootstrap.</param>
        public abstract void Bootstrap(Mod mod);

        /// <summary>
        ///     Reverses any changes made by bootstrapping, if necessary.
        /// </summary>
        /// <param name="mod">The mod to bootstrap.</param>
        /// <param name="failed">If bootstrapping is being unapplied due to a fatal error thrown during bootstrapping.</param>
        public abstract void Unbootstrap(Mod mod, bool failed);
        
        /// <summary>
        ///     Get the actual API version, represented as a <see cref="Version"/> object.
        /// </summary>
        public abstract Version GetVersion();
    }
}