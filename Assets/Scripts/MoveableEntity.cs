using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "moveable entity")]
public class MoveableEntity : ScriptableObject {
    public float speed;
    public GameObject obj;
    public GameTagsMapper tag;
    public GameLayers layer;
    public Color entityColor;

    //public Sprite sprite;

    //public SpriteRenderer sprRenderer;

    private void Enable() {
        //sprRenderer.sprite = sprite;
    }
}