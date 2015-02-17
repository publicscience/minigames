using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class HypeTarget : MonoBehaviour {
    public static Color cascadeColor = new Color(0.74f, 1.0f, 0.95f, 0.2f);
    public static Color firstColor = new Color(0.74f, 1.0f, 0.95f, 0.4f);

    public enum Function {
        Linear,
        Sin,
        Cos
    }

    public float hypePoints = 1f;
    public float opinionPoints = 1f;
    public float speed = 1f;
    public float distance = 5f;
    public float cascadeRadius = 10f;
    public Function function;
    public GameObject rangeDisplay;

    private Vector2 start;
    new private bool enabled = true;

    public static event System.Action<float, float> Scored;
    public static event System.Action<HypePuck> Completed;
    private static int activeCascades = 0;
    private static int[] notes = new int[] { 0, 5, 7, -3, -5, -7, -12, -24, -19, -17, -15 };

    float Y(float x) {
        switch (function) {
            case Function.Linear:
                return start.y;
            case Function.Sin:
                return start.y + Mathf.Sin(x);
            case Function.Cos:
                return start.y + Mathf.Cos(x);
        }
        return start.y;
    }

    void Start() {
        start = transform.position;
        rangeDisplay.transform.localScale = Vector2.zero;
        rangeDisplay.GetComponent<SpriteRenderer>().color = cascadeColor;

        StartCoroutine("Move");
    }

    IEnumerator Move() {
        while (true) {
            float i = Mathf.PingPong(Time.time * speed, distance);
            float x = start.x + i;
            float y = Y(x);
            Vector2 end = new Vector2(x, y);
            transform.position = end;
            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        other.gameObject.SetActive(false);

        rangeDisplay.GetComponent<SpriteRenderer>().color = firstColor;

        HypePuck puck = other.GetComponent<HypePuck>();
        if (enabled) {
            DoCascade(puck, this, this);
            enabled = false;
        } else {
            if (Completed != null)
                Completed(puck);
        }
    }

    public IEnumerator Cascade(HypePuck puck, HypeTarget root) {
        activeCascades++;

        Vector2 center = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(center, cascadeRadius);
        List<HypeTarget> targets = new List<HypeTarget>();

        for (int i=0; i<colliders.Length; i++) {
            HypeTarget ht = colliders[i].GetComponent<HypeTarget>();
            if (ht != null && ht.enabled) {
                targets.Add(ht);
            }
        }

        foreach (HypeTarget ht in targets.OrderBy(t => Vector2.Distance(t.transform.position, center))) {
            if (ht.enabled) {
                ht.enabled = false;
                yield return new WaitForSeconds(0.2f);
                DoCascade(puck, ht, root);
            }
        }

        activeCascades--;

        if (activeCascades == 0) {
            if (Completed != null)
                Completed(puck);
        }
    }
    void DoCascade(HypePuck puck, HypeTarget ht, HypeTarget root) {
        StartCoroutine(ht.Highlight());

        if (Scored != null)
            Scored(ht.hypePoints, ht.opinionPoints);

        StartCoroutine(ht.Cascade(puck, root));
    }

    public IEnumerator Highlight() {
        yield return new WaitForSeconds(Random.value * 0.2f);
        StartCoroutine(ShowHit());

        float note = notes[Random.Range(0, notes.Length)];
        audio.pitch =  Mathf.Pow(2, note/12.0f);
        audio.Play();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(0f,1f,0f);
        yield return new WaitForSeconds(0.1f);
        sr.color = new Color(0.4f, 0.4f, 0.4f);
    }

    IEnumerator ShowHit() {
        Vector2 endScale = transform.localScale * cascadeRadius * 2;
        float step = 0.05f;
        for (float f = 0f; f <= 1f + step; f += step) {
            rangeDisplay.transform.localScale = Vector2.Lerp(rangeDisplay.transform.localScale, endScale, Mathf.SmoothStep(0f, 1f, f));
            //transform.Rotate(0f, Mathf.SmoothStep(0f, 180f, f), 0f);
            yield return null;
        }

        for (float f = 0f; f <= 1f + step; f += step) {
            rangeDisplay.transform.localScale = Vector2.Lerp(endScale, Vector2.zero, Mathf.SmoothStep(0f, 1f, f));
            yield return null;
        }
    }
}
