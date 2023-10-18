using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "moveable entity/turtle")]
public class Turtle : MoveableEntity {
    public float[] turnCooldownRange;
}