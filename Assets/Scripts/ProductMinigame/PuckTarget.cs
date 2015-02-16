using UnityEngine;

public class PuckTarget : MonoBehaviour {
    public float points = 1;
    public Feature type;

    void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject);
    }
}
