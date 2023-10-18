using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class EndPointBehaviour : MonoBehaviour {

    [ReadOnly]
    [SerializeField]
    public bool isAvaiable;

    [SerializeField]
    private GameObject fillChild;

    private void Start() {
        Clean();
    }

    public bool Fill() {
        if (!isAvaiable) return false;
        transform.parent.GetComponent<EndPointsController>().UpdateAvaiableLilypads(transform.name);

        fillChild.SetActive(true);
        // update the sprite;
        isAvaiable = false;
        return true;
    }

    public void Clean() {
        isAvaiable = true;
    }
}