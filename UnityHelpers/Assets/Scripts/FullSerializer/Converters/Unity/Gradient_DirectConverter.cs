using System;
using System.Collections.Generic;
using UnityEngine;

namespace helper.FullSerializer {
    partial class fsConverterRegistrar {
        public static Internal.DirectConverters.Gradient_DirectConverter Register_Gradient_DirectConverter;
    }
}

namespace helper.FullSerializer.Internal.DirectConverters {
    public class Gradient_DirectConverter : fsDirectConverter<Gradient> {
        protected override fsResult DoSerialize(Gradient model, Dictionary<string, fsData> serialized, object aiAgent) {
            var result = fsResult.Success;

            result += SerializeMember(serialized, null, "alphaKeys", model.alphaKeys,aiAgent);
            result += SerializeMember(serialized, null, "colorKeys", model.colorKeys,aiAgent);

            return result;
        }

        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Gradient model, object aiAgent) {
            var result = fsResult.Success;

            var t0 = model.alphaKeys;
            result += DeserializeMember(data, null, "alphaKeys", out t0,aiAgent);
            model.alphaKeys = t0;

            var t1 = model.colorKeys;
            result += DeserializeMember(data, null, "colorKeys", out t1,aiAgent);
            model.colorKeys = t1;

            return result;
        }

        public override object CreateInstance(fsData data, Type storageType) {
            return new Gradient();
        }
    }
}