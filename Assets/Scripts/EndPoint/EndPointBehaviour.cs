using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class EndPointBehaviour : MonoBehaviour {

    [ReadOnly]
    [SerializeField]
    private bool isAvaiable;

    private void Start() {
        Clean();
    }

    public bool Fill() {
        if (!isAvaiable) return false;

        print("was filled");
        // update the sprite;
        isAvaiable = false;
        return true;
    }

    public void Clean() {
        isAvaiable = true;
    }
}