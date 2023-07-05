using UnityEngine;

public class ObjectMover : MonoBehaviour {
    public float speed;
    public GameObject objectPrefab;
    public bool enableRespawn;

    // Update is called once per frame
    void Update() {
        transform.Translate(Vector3.forward * -speed * Time.deltaTime);
    }
    public void SetSpeed(float speedIn) { speed = speedIn; }

    private void OnBecameInvisible() 
    {
       
    }
    public void RespawnObjects(Vector3 position, Quaternion rotation) 
    {
        Instantiate(objectPrefab, position, rotation);
    }
}
