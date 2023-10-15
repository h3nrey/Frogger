using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;
using NaughtyAttributes;

public class PlayerController : MonoBehaviour {
    public static PlayerController instance;

    [Expandable]
    public Player data;

    #region Events

    // Events
    public UnityEvent<Vector2> onMove;

    public UnityEvent onDie;
    public UnityEvent onReachEndPoint;
    public UnityEvent onReachAllEndPoints;

    #endregion Events

    #region Properties

    // Tries
    [ReadOnly]
    public int currentTries;

    // Movement
    [SerializeField]
    private float speed;

    [Header("Initialization")]
    [SerializeField]
    private Transform startPoint;

    // Score
    [ReadOnly]
    public int score;

    // Filled end points
    [ReadOnly]
    public int reachedEndPoints;

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
    public bool isAlive;

    public bool isTouchingPlataform;

    // Components
    [Header("Components")]
    [SerializeField]
    private Rigidbody2D rb;

    #endregion Properties

    #region Defaul Methods

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    private void Start() {
        // initialization
        isAlive = true;
        currentTries = data.tries;

        // events
        onMove.AddListener((Vector2 dir) => Move(dir));
        onDie.AddListener(Die);

        onReachEndPoint.AddListener(handleWhenReachEndPoint);
        onReachAllEndPoints.AddListener(winLevel);
    }

    private void OnDestroy() {
        onMove.RemoveAllListeners();
        onDie.RemoveAllListeners();
        onReachEndPoint.RemoveAllListeners();
        onReachAllEndPoints.RemoveAllListeners();
    }

    private void FixedUpdate() {
        if (isAlive) {
            CheckLogCollision();
            CheckLakeCollision();
        }
    }

    #endregion Defaul Methods

    #region Movement

    public void Move(Vector2 dir) {
        if (!isAlive) return;

        Vector2 pos = rb.position;
        rb.MovePosition(new Vector2(pos.x + (dir.x * speed), pos.y + (dir.y * speed)));
    }

    #endregion Movement

    #region Collision

    public void CheckLogCollision() {
        Collider2D[] touchingPlataforms = Physics2D.OverlapCircleAll(plataformPoint.position, logRange, plataformLayer);

        if (touchingPlataforms.Length > 0) {
            isTouchingPlataform = true;
            Transform plataform = touchingPlataforms[0].transform;
            transform.SetParent(plataform);
        }
        else {
            isTouchingPlataform = false;
            transform.SetParent(null);
        }
    }

    public void CheckLakeCollision() {
        bool touchingLake = Physics2D.OverlapCircle(plataformPoint.position, lakeRange, lakeLayer);

        if (touchingLake && !isTouchingPlataform) {
            onDie?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        GameObject otherObj = other.gameObject;

        if (otherObj.CompareTag(GameTags.tags[(int)GameTagsMapper.CAR])) {
            onDie?.Invoke();
        }

        if (otherObj.CompareTag(GameTags.tags[(int)GameTagsMapper.ENDPOINT])) {
            bool isAvaiable = otherObj.GetComponent<EndPointBehaviour>().Fill();

            if (isAvaiable) {
                onReachEndPoint?.Invoke();
            }
            else {
                onDie?.Invoke();
            }
        }
    }

    #endregion Collision

    private void Die() {
        if (isAlive == false) return;
        isAlive = false;

        currentTries -= 1;
        Coroutines.DoAfter(() => {
            RestartPlayer();
        }, 0.5f, this);
    }

    private void handleWhenReachEndPoint() {
        score += data.scorePerEndPoint;
        isAlive = false;
        reachedEndPoints++;

        if (reachedEndPoints >= 5) {
            onReachAllEndPoints?.Invoke();
            return;
        }

        Coroutines.DoAfter(() => RestartPlayer(), 0.5f, this);
    }

    private void winLevel() {
        Coroutines.DoAfter(() => RestartPlayer(), 1f, this);
    }

    private void RestartPlayer() {
        transform.position = startPoint.position;
        isAlive = true;
    }

    #region Gizmos

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(plataformPoint.position, logRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(plataformPoint.position, lakeRange);
    }

    #endregion Gizmos
}