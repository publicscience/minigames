using UnityEngine;
using UnityEngine.UI;
using System;

public enum Feature {
    Design,
    Engineering,
    Marketing
}

public class ProductMinigame : MonoBehaviour {
    private float[] scores = new float[3];

    public int pucks = 3;
    public Text[] labels;
    public GameObject bonusTargetPrefab;

    void Start() {
        int numBonusTargets = UnityEngine.Random.Range(0, 10);

        for (int i=0; i<numBonusTargets; i++) {
            GameObject bonusTarget = Instantiate(bonusTargetPrefab) as GameObject;

            // TO DO this should be limited to actual stage bounds.
            float x = -1.5f + UnityEngine.Random.value * 3f;
            float y = -1.5f + UnityEngine.Random.value * 3f;

            bonusTarget.transform.parent = transform;
            bonusTarget.transform.localPosition = new Vector2(x, y);
        }
    }

    void OnEnable() {
        Puck.Scored += Scored;
    }

    void OnDisable() {
        Puck.Scored -= Scored;
    }

    void Scored(Puck puck, float[] points) {
        foreach (Feature f in Enum.GetValues(typeof(Feature))) {
            scores[(int)f] += points[(int)f];
        }
        DisplayScore();

        pucks--;
        puck.Reset();

        if (pucks <= 0) {
            puck.gameObject.SetActive(false);

            Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~");
            Debug.Log("GAME OVER");
            Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~");
        }
    }

    void DisplayScore() {
        foreach (Feature f in Enum.GetValues(typeof(Feature))) {
            labels[(int)f].text = string.Format("{0}: {1}", f, scores[(int)f]);
        }
    }
}
