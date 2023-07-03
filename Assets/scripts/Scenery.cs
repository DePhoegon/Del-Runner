using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenery : MonoBehaviour
{
    public GameObject building1;
    public GameObject building2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject place = UnityEngine.Random.Range(0,20) < 10 ? building1 : building2;
        if (other.gameObject.tag == "Building")
        {
            Destroy(other.gameObject);
            Instantiate(place, place.transform.position, Quaternion.identity);
        }
    }
}
