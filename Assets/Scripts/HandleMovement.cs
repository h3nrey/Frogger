using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HandleMovement : MonoBehaviour {
    public MoveableEntity data;

    public float xDir;

    private Rigidbody2D rb;
    private SpriteRenderer sprRenderer;
    private BoxCollider2D collider;

    private bool wasEnabled;

    [SerializeField]
    private bool Debuging;

    //private bool

    private void OnEnable() {
        rb = GetComponent<Rigidbody2D>();
        sprRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();

        if (Debuging) {
            Setup();
        }
    }

    public void Setup() {
        // tag and layer
        this.tag = GameTags.tags[(int)data.tag];
        this.gameObject.layer = (int)data.layer;

        if (xDir == 1) {
            transform.localScale = new Vector2(-1, 1);
        }
        if (xDir == -1) {
            transform.localScale = new Vector2(1, 1);
        }

        if (data.isSimpleSprite) {
            sprRenderer.enabled = true;
            sprRenderer.sprite = data.sprite;
        }
        else {
            sprRenderer.enabled = false;
            Instantiate(data.graphic_holder, transform.position, Quaternion.identity, transform);
        }

        // Add extra scripts
        foreach (var scriptName in data.extraScripts) {
            Type scriptType = Type.GetType(scriptName + ",Assembly-CSharp");
            gameObject.AddComponent(scriptType);
        }

        // collider
        collider.size = data.collSize;
        collider.offset = data.collOffset;

        wasEnabled = true;
    }

    private void FixedUpdate() {
        if (wasEnabled) {
            Move();
        }
    }

    private void OnBecameInvisible() {
        GameManager.Game.activeEntities.Release(this.gameObject);
    }

    private void Move() {
        Vector2 pos = rb.position;
        rb.velocity = new Vector2(data.speed * xDir * Time.fixedDeltaTime, 0);
    }
}