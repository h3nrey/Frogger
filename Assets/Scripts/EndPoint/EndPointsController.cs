using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointsController : MonoBehaviour {

    [SerializeField]
    private EndPointBehaviour[] points;

    public void RestartPoints() {
        foreach (EndPointBehaviour point in points) {
            point.Clean();
        }
    }
}