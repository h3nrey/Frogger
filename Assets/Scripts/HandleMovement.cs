using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HandleMovement : MonoBehaviour {
    public MoveableEntity data;

    [SerializeField]
    private Rigidbody2D rb;

    public float xDir;

    //private bool

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup() {
        this.tag = GameTags.tags[(int)data.tag];
        this.gameObject.layer = (int)data.layer;
        GetComponent<SpriteRenderer>().color = data.entityColor;
    }

    private void FixedUpdate() {
        Move();
    }

    private void OnBecameInvisible() {
        GameManager.Game.activeEntities.Release(this.gameObject);
    }

    private void Move() {
        Vector2 pos = rb.position;
        rb.velocity = new Vector2(data.speed * xDir * Time.fixedDeltaTime, 0);
    }
}