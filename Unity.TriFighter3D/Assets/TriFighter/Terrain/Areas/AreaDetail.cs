using TriFighter.Types;

using UnityEngine;

namespace TriFighter.Terrain {
    public interface IAreaDetail {
        Rect Rect { get; set; }
    }

    public sealed class AreaDetail : IAreaDetail {
        public Rect Rect { get; set; }

        public Transform Parent;
    }
}