using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player")]
public class Player : ScriptableObject {
    public int tries;
    public float baseSpeed;
    public int scorePerEndPoint;
    public int scoreFly;
}