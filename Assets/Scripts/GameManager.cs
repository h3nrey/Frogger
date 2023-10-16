using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Game;

    public ObjectPool<GameObject> activeEntities;

    [SerializeField]
    private GameObject entityPrefab;

    [Header("UI")]
    [SerializeField] private Slider timeSlider;

    [SerializeField] private Transform triesHolder;

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

    private void Update() {
        UpdateTriesIcon();
    }

    public void UpdateTriesIcon() {
        int totalIcons = triesHolder.childCount;
        int tries = PlayerController.instance.currentTries;

        for (int i = 0; i < totalIcons; i++) {
            GameObject icon = triesHolder.GetChild(i).gameObject;
            icon.SetActive(false);

            if (i < tries) icon.SetActive(true);
        }
    }

    public void UpdateTimeSlideValue(float curTime, float totalTime) {
        timeSlider.value = curTime / totalTime;
    }
}