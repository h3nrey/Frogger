using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using UnityEngine.Pool;

public class LaneSpawnerController : MonoBehaviour {

    [SerializeField]
    private float direction;

    [SerializeField]
    private GameObject EntityPrefab;

    public ObjectPool<GameObject> spawnPool;

    [SerializeField]
    private MoveableEntity[] TypesOfEntities;

    [SerializeField]
    private float spawnCooldown;

    [SerializeField]
    private float startCooldown;

    private bool canSpawn;

    private void Start() {
        canSpawn = true;

        Coroutines.DoAfter(() => {
            StartCoroutine(Spawn());
        }, startCooldown, this);
    }

    private IEnumerator Spawn() {
        while (true) {
            int randomIndex = Random.Range(0, TypesOfEntities.Length);
            MoveableEntity entityData = TypesOfEntities[randomIndex];
            //GameObject entity = Instantiate(EntityPrefab, transform.position, Quaternion.identity, this.transform);
            GameObject entity = GameManager.Game.activeEntities.Get();

            entity.transform.SetParent(this.transform);
            entity.transform.position = this.transform.position;

            entity.GetComponent<HandleMovement>().xDir = direction;
            entity.GetComponent<HandleMovement>().data = entityData;
            entity.GetComponent<HandleMovement>().Setup();
            entity.SetActive(true);
            yield return new WaitForSeconds(spawnCooldown);
        }
    }
}