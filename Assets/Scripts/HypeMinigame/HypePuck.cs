using UnityEngine;

public class HypePuck : MonoBehaviour {
    float speed = 12000f;

    void Update() {
        if (Input.GetMouseButton(0)) {
            rigidbody2D.AddForce(new Vector2(0 * speed * Time.deltaTime, 1f * speed * Time.deltaTime));
        }
    }

    public void Reset() {
        gameObject.SetActive(true);
        transform.position = new Vector3(0,0,0);
        rigidbody2D.velocity = Vector3.zero;
    }

}
