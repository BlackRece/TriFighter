using UnityEngine;

namespace TriFighter.Types {
    public sealed class LinePath {
        private Vector2 _source, _target;

        public Vector2 Source {
            get => _source;
            set => _source = value;
        }

        public Vector2 Target {
            get => _target;
            set => _target = value;
        }

        public float Distance => Vector2.Distance(_source, _target);
        public int Intervals(float width) => (int)(Distance / width);

        public bool ContainsX(float value) {
            var result = false;

            if (_source.x < _target.x) 
                result = new FloatRange(_source.x, _target.x).IsInRange(value);
            if (_source.x > _target.x) 
                result = new FloatRange(_target.x, _source.x).IsInRange(value);

            return result;
        }

        public bool ContainsY(float value) =>
            new FloatRange(_source.y, _target.y).IsInRange(value);

        public LinePath() {
            _source = new Vector2();
            _target = new Vector2();
        }
        
        public LinePath(Vector2 source, Vector2 target) {
            _source = source;
            _target = target;
        }
    }
}