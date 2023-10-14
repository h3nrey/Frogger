using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {
    public static PlayerController instance;
    public UnityEvent<Vector2> onMove;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    private void Start() {
        onMove.AddListener((Vector2 dir) => print(dir));
    }

    public void Move(Vector2 dir) {
        print($"moved to: {dir}");
    }
}