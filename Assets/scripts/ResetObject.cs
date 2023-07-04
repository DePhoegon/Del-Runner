using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjects : MonoBehaviour
{
    public string objectTag;
    public GameObject objectPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == objectTag)
        {
            Destroy(other.gameObject);
            Instantiate(objectPrefab, objectPrefab.transform.position, Quaternion.identity);
        }
    }
}
