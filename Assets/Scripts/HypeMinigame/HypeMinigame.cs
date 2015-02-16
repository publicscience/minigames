using UnityEngine;

public class HypeMinigame : MonoBehaviour {
    public int pucks = 3;

    private float score = 0;

    void OnEnable() {
        HypeTarget.Scored += Scored;
        HypeTarget.Completed += Completed;
    }

    void OnDisable() {
        HypeTarget.Scored -= Scored;
        HypeTarget.Completed -= Completed;
    }

    void Scored(float points) {
        score += points;
    }

    void Completed(HypePuck puck) {
        if (pucks > 0) {
            pucks--;
            puck.Reset();
        } else {
            puck.gameObject.SetActive(false);
        }
        Debug.Log(string.Format("Scored {0} points!", score));
    }
}
