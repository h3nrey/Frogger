using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HandleMovement : MonoBehaviour {

    [SerializeField]
    private MoveableEntity data;

    [SerializeField]
    private Rigidbody2D rb;

    public float xDir;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        Vector2 pos = rb.position;
        rb.velocity = new Vector2(data.speed * xDir * Time.fixedDeltaTime, 0);
    }
}