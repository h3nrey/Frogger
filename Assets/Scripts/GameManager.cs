using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour {
    public static GameManager Game;

    public ObjectPool<GameObject> activeEntities;

    [SerializeField]
    private GameObject entityPrefab;

    private void Start() {
        if (Game == null) {
            Game = this;
        }

        activeEntities = new ObjectPool<GameObject>(
            () => {
                return Instantiate(entityPrefab, new Vector2(0, 0), Quaternion.identity);
            },
            projectille => {
                projectille.SetActive(true);
            },
            projectille => {
                projectille.SetActive(false);
            },
            projectille => {
                Destroy(projectille);
            }, false, 5, 20
        );
    }
}