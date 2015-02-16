using UnityEngine;
using System;

public class BonusTarget : MonoBehaviour {
    public float points = 1;
    public Feature type;

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(string.Format("Scored {0} bonus {1} points!", points, type));
        gameObject.SetActive(false);
    }

    void Start() {
        // Bonus targets have random points for random features.
        points = Mathf.Floor(1f + UnityEngine.Random.value * 3f);
        Array features = Enum.GetValues(typeof(Feature));
        type = (Feature)features.GetValue(UnityEngine.Random.Range(0, features.Length - 1));
    }
}
