using System;

namespace PrettyPatch.Util
{
    internal static class Messages
    {
        internal static string RepeatedBootstrapRequest(string name, string display, string ver) {
            return $"{name} ({display}) attempted to bootstrap itself with PrettyPatch API version {ver}, but has already been bootstrapped with that version!";
        }

        internal static string CaughtFatalError(string name, string display, string ver, Exception e) {
            return $"Caught a fatal error while bootstrapping {name} ({display}) using API version {ver}:\n{e}";
        }

        internal static string BootstrapFailedPrettyPatch(string ver) {
            return $"Failed to bootstrap PrettyPatch using API version {ver}.";
        }
    }
}