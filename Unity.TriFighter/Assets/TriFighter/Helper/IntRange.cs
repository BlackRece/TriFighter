using System;

namespace TriFighter.Types {
    [Serializable]
    public sealed class IntRange {
        public int min;
        public int max;

        public bool IsInRange(int value) => value >= min && value <= max;
        
        public IntRange(int min, int max) {
            this.min = min;
            this.max = max;
        }

        public int Random() => 
            UnityEngine.Random.Range(min, max);
    }
}