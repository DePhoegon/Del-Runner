using UnityEngine;

public class Reset2RngObjects : MonoBehaviour
{
    public GameObject objectPrefab1;
    public GameObject objectPrefab2;
    public string objectTag;
    private void OnTriggerEnter(Collider other)
    {
        GameObject place = UnityEngine.Random.Range(0,20) < 10 ? objectPrefab1 : objectPrefab2;
        if (other.gameObject.tag == objectTag)
        {
            Destroy(other.gameObject);
            Instantiate(place, place.transform.position, Quaternion.identity);
        }
    }
}
