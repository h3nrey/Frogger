using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {
    public static PlayerController instance;
    public UnityEvent<Vector2> onMove;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Rigidbody2D rb;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    private void Start() {
        onMove.AddListener((Vector2 dir) => Move(dir));
    }

    public void Move(Vector2 dir) {
        Vector2 pos = rb.position;
        rb.MovePosition(new Vector2(pos.x + (dir.x * speed), pos.y + (dir.y * speed)));
    }
}