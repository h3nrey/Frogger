using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class TurtleBehaviour : MonoBehaviour {
    [SerializeField] private Turtle data;

    private Collider2D col;
    private List<Animator> anims = new List<Animator>();
    [SerializeField] private AnimationClip turnClip;
    [SerializeField] private float[] turnCooldownRange = { 1.6f, 2.2f };
    [SerializeField] private float simblingThreeshold;

    private void OnEnable() {
        col = GetComponent<Collider2D>();

        Transform graphic = transform.GetChild(0);
        int graphicTurtles = graphic.childCount;

        for (int i = 0; i < graphicTurtles; i++) {
            Transform t = graphic.GetChild(i);
            Animator tAnim = t.GetComponent<Animator>();
            anims.Add(tAnim);
        }

        float turnCooldown = Random.Range(turnCooldownRange[0], turnCooldownRange[1]);

        print($"turn cooldown: {turnCooldown}");
        Coroutines.DoAfter(() => {
            StartCoroutine(Turn());
        }, turnCooldown, this);
        col.enabled = true;
    }

    public IEnumerator Turn() {
        while (true) {
            // Anim
            //foreach (Animator anim in anims) {
            //    anim.SetTrigger("Turn");
            //}

            for (int i = 0; i < anims.Count; i++) {
                yield return new WaitForSeconds(i * 0.05f);
                anims[i].SetTrigger("Turn");
            }

            Coroutines.DoAfter(() => {
                col.enabled = false;
            }, turnClip.length / 2, this);

            yield return new WaitForSeconds(turnClip.length);
            col.enabled = true;

            float turnCooldown = Random.Range(turnCooldownRange[0], turnCooldownRange[1]);
            print($"turn cooldown: {turnCooldown}");
            yield return new WaitForSeconds(turnCooldown);
        }
    }
}