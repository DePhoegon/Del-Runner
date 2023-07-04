using UnityEngine;

public class ObjectMover : MonoBehaviour {
    public float speed;

    // Update is called once per frame
    void Update() {
        transform.Translate(Vector3.forward * -speed * Time.deltaTime);
    }
    public void SetSpeed(float speedIn) { speed = speedIn; }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
