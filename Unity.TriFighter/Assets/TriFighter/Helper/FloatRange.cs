using System;

namespace TriFighter.Types {
    [Serializable]
    public sealed class FloatRange {
        public float min;
        public float max;

        public FloatRange(float min, float max) {
            this.min = min;
            this.max = max;
        }
    }
}