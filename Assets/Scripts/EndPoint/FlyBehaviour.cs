using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class FlyBehaviour : MonoBehaviour {
    [SerializeField] private EndPointsController spawner;
    private Collider2D collider;
    private GameObject graphic;
    [SerializeField] private float aliveTime;

    // Start is called before the first frame update
    private void Start() {
        collider = GetComponent<Collider2D>();
        graphic = transform.GetChild(0).gameObject;

        collider.enabled = false;
        graphic.SetActive(false);
    }

    public void SetSpawner(EndPointsController instance) {
        spawner = instance;
    }

    public void EnableFly() {
        collider.enabled = true;
        graphic.SetActive(true);

        Coroutines.DoAfter(() => Die(), 3f, this);
    }

    public void Die() {
        collider.enabled = false;
        graphic.SetActive(false);

        spawner.CallSpawn();
    }
}