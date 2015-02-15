using UnityEngine;

public class Puck : MonoBehaviour {
    float speed = 8000f;
    bool fired = false;
    bool done = false;
    private CircleCollider2D _collider;

    void Start() {
        _collider = (CircleCollider2D)collider2D;
    }

    void Update() {
        if (Input.GetMouseButton(0)) {
            fired = true;
            rigidbody2D.AddForce(new Vector2(0 * speed * Time.deltaTime, 1f * speed * Time.deltaTime));
        }

        if (rigidbody2D.IsSleeping() && fired) {
            Score();
        }

        //Debug.Log(Input.GetAxis("Horizontal"));
        //Debug.Log(Input.GetAxis("Vertical"));
        //Debug.Log(Input.GetAxis("Mouse X"));
        //Debug.Log(Input.GetAxis("Mouse Y"));
        //Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
    }

    void Score() {
        float points = 0;
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, _collider.radius);

        // First collider is the puck itself.
        if (colliders.Length > 1) {
            for (int i=1; i<colliders.Length; i++) {
                PuckTarget t = colliders[i].gameObject.GetComponent<PuckTarget>();
                if (t != null &&
                    CenterInCollider((CircleCollider2D)colliders[i]) &&
                    t.points > points) {
                    points = t.points;
                }
            }
        }
        Debug.Log(string.Format("Scored {0} points", points));

        Reset();
    }

    void Reset() {
        transform.position = new Vector3(0,0,0);
        fired = false;
    }

    bool CenterInCollider(CircleCollider2D c) {
        float radius = c.bounds.extents.x;
        Vector2 center = c.gameObject.transform.position;
        return Mathf.Pow((transform.position.x - center.x), 2f) + Mathf.Pow((transform.position.y - center.y), 2f) < Mathf.Pow(radius, 2f);
    }
}