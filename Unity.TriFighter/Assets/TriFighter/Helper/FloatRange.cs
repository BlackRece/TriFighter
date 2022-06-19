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

        public bool IsInRange(float value) => value >= min && value <= max;
        public float Delta() => max - min;
    }
}