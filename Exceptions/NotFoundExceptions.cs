using System;
using System.Runtime.Serialization;

namespace PrettyPatch.Exceptions
{
    [Serializable]
    public class TypeNotFoundException : Exception
    {
        public TypeNotFoundException() { }
        public TypeNotFoundException(string message) : base(message) { }
        public TypeNotFoundException(string message, Exception inner) : base(message, inner) { }

        protected TypeNotFoundException(
            SerializationInfo info,
            StreamingContext context
        ) : base(info, context) { }
    }
    
    [Serializable]
    public class MethodNotFoundException : Exception
    {
        public MethodNotFoundException() { }
        public MethodNotFoundException(string message) : base(message) { }
        public MethodNotFoundException(string message, Exception inner) : base(message, inner) { }

        protected MethodNotFoundException(
            SerializationInfo info,
            StreamingContext context
        ) : base(info, context) { }
    }
}