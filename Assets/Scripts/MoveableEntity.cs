using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "moveable entity")]
public class MoveableEntity : ScriptableObject {
    public float speed;
    public GameObject obj;
    public GameTagsMapper tag;
    public GameLayers layer;
    public Vector2 collSize;
    public Vector2 collOffset;

    public bool isSimpleSprite;
    public GameObject graphic_holder;
    public Sprite sprite;

    public string[] extraScripts;
}