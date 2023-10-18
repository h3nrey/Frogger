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
    public UnityEvent onGameOver;
    public UnityEvent onReachEndPoint;
    public UnityEvent onReachAllEndPoints;
    public UnityEvent onRestartLevel;

    #endregion Events

    #region Properties

    // Tries
    [ReadOnly]
    public int currentTries;

    // Movement
    [ReadOnly]
    [SerializeField] private float speed;

    [ReadOnly]
    [SerializeField] private Vector3 targetRot;

    [SerializeField] private float rotationSpeed;

    [ReadOnly]
    [SerializeField] private float curMaxY;

    [SerializeField]
    private float minY, maxX;

    [Header("Initialization")]
    [SerializeField]
    private Transform startPoint;

    [Header("Score")]
    // Score
    [ReadOnly]
    public int score;

    [Header("Timer")]
    // Time
    [ReadOnly]
    [SerializeField]
    private float currentTime;

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

    public bool gameIsOver;
    public bool isTouchingPlataform;

    // Components
    [Header("Components")]
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private GameObject graphicHolder;

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
        currentTime = data.totalTime;
        curMaxY = transform.position.y - 1;

        // events
        onMove.AddListener((Vector2 dir) => {
            if (!isAlive) return;
            Move(dir);
            anim.SetTrigger("move");
            CheckIfMovedTowards(dir);
        });
        onDie.AddListener(Die);
        onGameOver.AddListener(GameOver);
        onRestartLevel.AddListener(() => {
            if (!gameIsOver) return;
            SceneCaller.CallScene("");
        });

        onReachEndPoint.AddListener(handleWhenReachEndPoint);
        onReachAllEndPoints.AddListener(winLevel);
    }

    private void OnDestroy() {
        onMove.RemoveAllListeners();
        onDie.RemoveAllListeners();
        onReachEndPoint.RemoveAllListeners();
        onReachAllEndPoints.RemoveAllListeners();
    }

    private void Update() {
        if (isAlive) {
            handlePlayerTime();
            SmoothRotate();
        }
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

        // Locking movement to screen limit
        if (dir == Vector2.down && pos.y <= minY) return;
        if ((dir == Vector2.left && pos.x <= -maxX) || (dir == Vector2.right && pos.x >= maxX)) return;

        // handle rotation
        if (dir == Vector2.down) {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        else if (dir == Vector2.up) {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (dir == Vector2.left) {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        else if (dir == Vector2.right) {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
        rb.MovePosition(new Vector2(pos.x + (dir.x * speed), pos.y + (dir.y * speed)));
    }

    private void SmoothRotate() {
        Quaternion curQ = transform.rotation;
        Quaternion targetQ = Quaternion.Euler(targetRot);
        //transform.rotation = Quaternion.Lerp(curQ, targetQ, rotationSpeed * Time.deltaTime);
    }

    private void CheckIfMovedTowards(Vector2 dir) {
        Vector2 pos = transform.position;

        if (dir == Vector2.up && pos.y > curMaxY) {
            curMaxY = pos.y;
            score += data.scoreForMovingTowards;
        }
    }

    #endregion Movement

    #region Collision

    public void CheckLogCollision() {
        Collider2D[] touchingPlataforms = Physics2D.OverlapCircleAll(plataformPoint.position, logRange, plataformLayer);

        if (touchingPlataforms.Length > 0) {
            Transform plataform = touchingPlataforms[0].transform;
            isTouchingPlataform = true;
            if (transform.position.y < plataform.position.y) return;
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

        if (otherObj.CompareTag(GameTags.tags[(int)GameTagsMapper.FLY])) {
            score += data.scoreFly;
            otherObj.GetComponent<FlyBehaviour>().Die();
        }
    }

    #endregion Collision

    #region Time

    private void handlePlayerTime() {
        currentTime -= Time.deltaTime;
        GameManager.Game.UpdateTimeSlideValue(currentTime, data.totalTime);

        if (currentTime <= 0) {
            onDie?.Invoke();
        }
    }

    #endregion Time

    #region Die

    private void Die() {
        if (isAlive == false) return;
        isAlive = false;
        currentTries -= 1;

        if (currentTries < 1) {
            onGameOver.Invoke();
            return;
        }

        currentTime = data.totalTime;

        Coroutines.DoAfter(() => {
            RestartPlayer();
        }, 0.5f, this);
    }

    private void GameOver() {
        gameIsOver = true;
        graphicHolder.GetComponent<SpriteRenderer>().enabled = false;
    }

    #endregion Die

    #region Reach

    private void handleWhenReachEndPoint() {
        score += data.scorePerEndPoint;
        isAlive = false;
        reachedEndPoints++;
        currentTime = data.totalTime;

        if (reachedEndPoints >= 5) {
            onReachAllEndPoints?.Invoke();
            return;
        }

        Coroutines.DoAfter(() => RestartPlayer(), 0.5f, this);
    }

    private void winLevel() {
        print("win");
        score += data.scoreAllEndPoints;
        Coroutines.DoAfter(() => RestartPlayer(), 1f, this);
    }

    private void RestartPlayer() {
        transform.position = startPoint.position;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        isAlive = true;
    }

    #endregion Reach

    #region Gizmos

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(plataformPoint.position, logRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(plataformPoint.position, lakeRange);
    }

    #endregion Gizmos
}