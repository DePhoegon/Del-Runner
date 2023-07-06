using UnityEngine;

public class ObjectMover : MonoBehaviour {
    public int speed;
    public int maxSpeed;
    public GameObject objectPrefab;
    public bool enableRespawn;
    public bool speedVariance;
    public int speedVarianceAmmount;
    private void Start()
    {
        if (speedVariance)
        {
            int lowerspeed = speedVarianceAmmount / 2;
            int variedspeed = Random.Range(0, speedVarianceAmmount) - lowerspeed;
            speed += variedspeed;
            if (maxSpeed != 0) { maxSpeed += variedspeed; }
        }
        if (maxSpeed == 0) { maxSpeed = speed; }
    }

    // Update is called once per frame
    void Update() {
        if (speed > maxSpeed) { speed = maxSpeed; }
        transform.Translate(Vector3.forward * -speed * Time.deltaTime);
    }
    public void SetSpeed(int speedIn) { speed = speedIn; }
    public int GetSpeed() { return speed; }

    private void OnBecameInvisible() 
    {
       
    }
    public void RespawnObjects(Vector3 position, Quaternion rotation, int speed)
    {
        int spd = objectPrefab.GetComponent<ObjectMover>().GetSpeed();
        GameObject newtarget = objectPrefab;
        newtarget.GetComponent<ObjectMover>().SetSpeed(speed);
        Instantiate(newtarget, position, rotation);
        objectPrefab.GetComponent<ObjectMover>().SetSpeed(spd);
    }
    public void RespawnObjects(Vector3 position, Quaternion rotation) 
    {
        Instantiate(objectPrefab, position, rotation);
    }
}
