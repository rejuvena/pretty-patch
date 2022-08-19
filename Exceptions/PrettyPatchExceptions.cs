using System;
using System.Runtime.Serialization;

namespace PrettyPatch.Exceptions
{
    [Serializable]
    public class BootstrapFailedModLoadException : Exception
    {
        public BootstrapFailedModLoadException() { }
        public BootstrapFailedModLoadException(string message) : base(message) { }
        public BootstrapFailedModLoadException(string message, Exception inner) : base(message, inner) { }

        protected BootstrapFailedModLoadException(
            SerializationInfo info,
            StreamingContext context
        ) : base(info, context) { }
    }
}