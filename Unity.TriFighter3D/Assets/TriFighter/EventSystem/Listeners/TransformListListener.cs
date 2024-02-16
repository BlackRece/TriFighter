using System.Collections.Generic;
using UnityEngine;

namespace BlackRece.Events {
public class TransformListListener : BaseGameEventListener<List<Transform>, 
    TransformListEvent, UnityTransformListEvent> { }
}