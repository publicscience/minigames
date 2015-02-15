using UnityEngine;

public class PuckTarget : MonoBehaviour {
    public float points = 1;

    void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject);
    }
}
