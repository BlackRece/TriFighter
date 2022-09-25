using System;

namespace TriFighter.Types {
    [Serializable]
    public sealed class FloatRange {
        public float min;
        public float max;

        private bool _isBounded;
        public FloatRange(float min, float max, bool isBounded = false) {
            this.min = min;
            this.max = max;

            _isBounded = isBounded;
        }

        public bool IsInRange(float value) => value >= min && value <= max;
        public float Delta() => max - min;

        public float BoundToRange(float value) {
            if (!_isBounded) return value;
            
            if (value > max) return max;
            if (value < min) return min;
            
            return value;
        }
    }
}