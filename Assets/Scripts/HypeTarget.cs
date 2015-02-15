using UnityEngine;
using System.Collections;

public class HypeTarget : MonoBehaviour {
    public float points = 1f;
    public float speed = 1f;
    public float distance = 5f;
    public float cascadeRadius = 10f;
    public GameObject rangeDisplay;

    private Vector2 start;
    private Vector2 end;

    public static event System.Action<HypeTarget> TargetHit;

    void Start() {
        start = transform.position;
        end = (Vector2)transform.position + new Vector2(distance, 0f);

        rangeDisplay.transform.localScale = Vector2.zero;

        StartCoroutine("Move");
    }

    IEnumerator ShowCascade() {
        Vector2 endScale = transform.localScale * cascadeRadius * 2;
        float step = 0.05f;
        for (float f = 0f; f <= 1f + step; f += step) {
            rangeDisplay.transform.localScale = Vector2.Lerp(rangeDisplay.transform.localScale, endScale, Mathf.SmoothStep(0f, 1f, f));
            yield return null;
        }
    }

    void OnEnable() {
        TargetHit += OnTargetHit;
    }

    void OnDisable() {
        TargetHit -= OnTargetHit;
    }

    void OnTargetHit(HypeTarget ht) {
        StopCoroutine("Move");
    }

    IEnumerator Move() {
        while (true) {
            float i = Mathf.PingPong(Time.time * speed, 1f);
            Vector2 end = new Vector2(i, i);
            transform.position = Vector2.Lerp(start, end, i);
            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (TargetHit != null)
            TargetHit(this);

        FindNearbyTargets();
        other.GetComponent<HypePuck>().Reset();
    }

    void FindNearbyTargets() {
        float total = 0;
        Vector2 center = gameObject.transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(center, cascadeRadius);

        for (int i=0; i<colliders.Length; i++) {
            HypeTarget ht = colliders[i].GetComponent<HypeTarget>();
            if (ht != null) {
                StartCoroutine(ht.Highlight());
                total += ht.points;
            }
        }

        Debug.Log(string.Format("Scored {0} points", total));
    }

    public IEnumerator Highlight() {
        StartCoroutine(ShowCascade());

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color orig = sr.color;
        sr.color = new Color(0f,1f,0f);
        yield return new WaitForSeconds(0.5f);
        sr.color = orig;
    }
}
