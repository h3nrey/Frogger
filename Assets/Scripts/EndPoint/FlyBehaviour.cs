using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class FlyBehaviour : MonoBehaviour {
    [SerializeField] private EndPointsController spawner;
    private Collider2D coll;
    private GameObject graphic;
    [SerializeField] private float aliveTime;

    // Start is called before the first frame update
    private void Start() {
        coll = GetComponent<Collider2D>();
        graphic = transform.GetChild(0).gameObject;

        coll.enabled = false;
        graphic.SetActive(false);
    }

    public void SetSpawner(EndPointsController instance) {
        spawner = instance;
    }

    public void EnableFly() {
        coll.enabled = true;
        graphic.SetActive(true);

        Coroutines.DoAfter(() => Die(), aliveTime, this);
    }

    public void Die() {
        coll.enabled = false;
        graphic.SetActive(false);

        spawner.CallSpawn();
    }
}