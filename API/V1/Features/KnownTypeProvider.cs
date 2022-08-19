﻿using System;

namespace PrettyPatch.API.V1.Features
{
    public class KnownTypeProvider : ITypeProvider
    {
        private Type Type { get; }

        public KnownTypeProvider(Type type) {
            Type = type;
        }

        public Type ResolveType() {
            return Type;
        }
    }
}