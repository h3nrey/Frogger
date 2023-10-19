using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

public class GameManager : MonoBehaviour {
    public static GameManager Game;

    public ObjectPool<GameObject> activeEntities;

    [SerializeField]
    private GameObject entityPrefab;

    [Header("UI")]
    [SerializeField] private Slider timeSlider;

    [SerializeField] private Transform triesHolder;

    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverModal;

    [SerializeField] private GameObject gameWonModal;

    [Header("AUDIO")]
    [SerializeField] private Sound[] sounds;

    [ReadOnly] public List<AudioSource> audioSrcs;

    private void Awake() {
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
        UpdateScoreText();
    }

    #region UI

    public void UpdateScoreText() {
        scoreText.text = PlayerController.instance.score.ToString();
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

    public void ShowGameOverModal() {
        gameOverModal.SetActive(true);
    }

    public void ShowGameWonModal() {
        gameWonModal.SetActive(true);
    }

    public void UpdateTimeSlideValue(float curTime, float totalTime) {
        timeSlider.value = curTime / totalTime;
    }

    #endregion UI

    #region Audio

    [Button("Populate Sound")]
    public void PopulateSounds() {
        foreach (Sound s in sounds) {
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            s.source = newAudioSource;
            audioSrcs.Add(newAudioSource);
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.playOnAwake = s.playOnAwake;
        }
    }

    [Button("Clear Sounds")]
    public void ClearSound() {
        foreach (AudioSource src in audioSrcs) {
            DestroyImmediate(src);
        }
        audioSrcs.Clear();
    }

    public void PlaySound(string name) {
        Sound currentSound = null;

        foreach (Sound s in sounds) {
            if (s.name == name) {
                currentSound = s;
                currentSound.source.Play();
                return;
            }
        }

        if (currentSound == null) {
            Debug.LogError($"Sound: {name} not found!");
            return;
        }
    }

    #endregion Audio
}