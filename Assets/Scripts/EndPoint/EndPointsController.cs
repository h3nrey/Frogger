using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class EndPointsController : MonoBehaviour {

    [SerializeField]
    private EndPointBehaviour[] points;

    [SerializeField] private Transform[] lilypads;

    [ReadOnly] [SerializeField] private List<Transform> avaiableLilypads;

    [Header("fly")]
    [SerializeField] private GameObject flyObject;

    [SerializeField] private bool hasFly;
    [SerializeField] private float spawnFlyCooldown;

    public void RestartPoints() {
        foreach (EndPointBehaviour point in points) {
            point.Clean();
        }
    }

    public void UpdateAvaiableLilypads(string lilyName) {
        int index = 0;
        for (int i = 0; i < avaiableLilypads.Count; i++) {
            if (avaiableLilypads[i].name == lilyName) index = i;
        }
        avaiableLilypads.RemoveAt(index);
    }

    private void Start() {
        avaiableLilypads.AddRange(lilypads);
        CallSpawn();
    }

    public void CallSpawn() {
        if (avaiableLilypads.Count < 1) return;
        StartCoroutine(InstantiateFly());
    }

    public IEnumerator InstantiateFly() {
        if (avaiableLilypads.Count < 1) yield return new WaitForSeconds(0);
        yield return new WaitForSeconds(spawnFlyCooldown);
        Transform lilypad = GetRandomLily();

        flyObject.GetComponent<FlyBehaviour>().EnableFly();
        flyObject.transform.position = lilypad.position;
    }

    private Transform GetRandomLily() {
        if (avaiableLilypads.Count < 1) return null;
        int randomIndex = Random.Range(0, avaiableLilypads.Count);
        Transform point = avaiableLilypads[randomIndex];
        return point;
    }
}