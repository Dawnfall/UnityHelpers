﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dawnfall.FullSerializer
{
    partial class fsConverterRegistrar
    {
        public static Internal.DirectConverters.RectOffset_DirectConverter Register_RectOffset_DirectConverter;
    }
}

namespace Dawnfall.FullSerializer.Internal.DirectConverters
{
    public class RectOffset_DirectConverter : fsDirectConverter<RectOffset>
    {
        protected override fsResult DoSerialize(RectOffset model, Dictionary<string, fsData> serialized, object other)
        {
            var result = fsResult.Success;

            result += SerializeMember(serialized, null, "bottom", model.bottom, other);
            result += SerializeMember(serialized, null, "left", model.left, other);
            result += SerializeMember(serialized, null, "right", model.right, other);
            result += SerializeMember(serialized, null, "top", model.top, other);

            return result;
        }

        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref RectOffset model, object other)
        {
            var result = fsResult.Success;

            var t0 = model.bottom;
            result += DeserializeMember(data, null, "bottom", out t0, other);
            model.bottom = t0;

            var t2 = model.left;
            result += DeserializeMember(data, null, "left", out t2, other);
            model.left = t2;

            var t3 = model.right;
            result += DeserializeMember(data, null, "right", out t3, other);
            model.right = t3;

            var t4 = model.top;
            result += DeserializeMember(data, null, "top", out t4, other);
            model.top = t4;

            return result;
        }

        public override object CreateInstance(fsData data, Type storageType)
        {
            return new RectOffset();
        }
    }
}