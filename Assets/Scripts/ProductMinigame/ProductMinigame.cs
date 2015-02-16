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
