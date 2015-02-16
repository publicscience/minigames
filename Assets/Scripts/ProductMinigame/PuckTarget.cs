using UnityEngine;

public class PuckTarget : MonoBehaviour {
    public float points = 1;
    public Feature type;

    void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log(other.gameObject);
    }
}
