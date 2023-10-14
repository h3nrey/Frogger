using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {
    public static PlayerController instance;

    // Events
    public UnityEvent<Vector2> onMove;

    // Properties
    [SerializeField]
    private float speed;

    // Log
    [Header("Log Collision")]
    [SerializeField]
    private float logRange;

    [SerializeField]
    private LayerMask plataformLayer;

    [SerializeField]
    private Transform plataformPoint;

    //Lake
    [Header("Lake Collision")]
    [SerializeField]
    private float lakeRange;

    [SerializeField]
    private LayerMask lakeLayer;

    // Flags
    [Header("Flags")]
    public bool isTouchingPlataform;

    // Components
    [Header("Components")]
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

    private void FixedUpdate() {
        CheckLogCollision();
    }

    public void Move(Vector2 dir) {
        Vector2 pos = rb.position;
        rb.MovePosition(new Vector2(pos.x + (dir.x * speed), pos.y + (dir.y * speed)));
    }

    public void CheckLogCollision() {
        Collider2D[] touchingPlataforms = Physics2D.OverlapCircleAll(plataformPoint.position, logRange, plataformLayer);
        bool touchingLake = Physics2D.OverlapCircle(plataformPoint.position, lakeRange, lakeLayer);

        if (touchingPlataforms.Length > 0) {
            isTouchingPlataform = true;
            Transform plataform = touchingPlataforms[0].transform;
            transform.SetParent(plataform);
        }
        else {
            isTouchingPlataform = false;
            transform.SetParent(null);
        }

        if (touchingLake && touchingPlataforms.Length < 1) {
            print("Touching lake");
        }
    }

    //public void OnTriggerStay2D(Collider2D other) {
    //    GameObject otherObj = other.gameObject;
    //    if (otherObj.CompareTag("lake") && !isTouchingPlataform) {
    //        print("Touching lake");
    //    }
    //}

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(plataformPoint.position, logRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(plataformPoint.position, lakeRange);
    }
}