using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualResetObjects : MonoBehaviour
{
    public ObjectMover object1Respawn;
    public ObjectMover object2Respawn;
    public bool resetObjectWithRNG;
    public bool enableRespawn;
    public bool enableShiftPos;
    public float objectOneShiftAmmount;
    public float objectTwoShiftAmmount;
    public string shiftAxises;
    public bool restrictX;
    public bool restrictY;
    public bool restrictZ;
    public float maxX;
    public float maxY;
    public float maxZ;
    public float minX;
    public float minY;
    public float minZ;
    string boundX;
    string boundY;
    string boundZ;

    private void Start()
    {
        boundX = restrictX ? "x" : "none";
        boundY = restrictY ? "y" : "none";
        boundZ = restrictZ ? "z" : "none";
        if (!enableShiftPos) { objectOneShiftAmmount = 0; shiftAxises = null; }
        if (object2Respawn == null) { object2Respawn = object1Respawn; }
    }
    private float adjustedRange(float maxShift, float basePos, string xyz)
    {
        float outfloat = UnityEngine.Random.Range(0, maxShift) + basePos;
        return xyz.Equals("z") ? Mathf.Min(Mathf.Max(minZ, outfloat), maxZ) : xyz.Equals("y") ? Mathf.Min(Mathf.Max(minY, outfloat), maxY) : xyz.Equals("x") ? Mathf.Min(Mathf.Max(minX, outfloat), maxX) : outfloat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (object1Respawn.gameObject.tag == other.gameObject.tag || other.gameObject.tag == object2Respawn.gameObject.tag)
        {
            Destroy(other.gameObject);
            if (enableRespawn)
            {
                Vector3 objectOnePos = object1Respawn.gameObject.transform.position;
                Vector3 objectTwoPos = object2Respawn.gameObject.transform.position;
                if (shiftAxises != null)
                {
                    
                    objectOnePos = new Vector3(shiftAxises.Contains("x") ? adjustedRange(objectOneShiftAmmount, objectOnePos.x, boundX) : objectOnePos.x, shiftAxises.Contains("y") ? adjustedRange(objectOneShiftAmmount, objectOnePos.y, boundY) : objectOnePos.y , shiftAxises.Contains("z") ? adjustedRange(objectOneShiftAmmount, objectOnePos.z, boundZ) : objectOnePos.z);
                    objectTwoPos = new Vector3(shiftAxises.Contains("x") ? adjustedRange(objectTwoShiftAmmount, objectTwoPos.x, boundX) : objectTwoPos.x, shiftAxises.Contains("y") ? adjustedRange(objectTwoShiftAmmount, objectTwoPos.y, boundY) : objectTwoPos.y, shiftAxises.Contains("z") ? adjustedRange(objectTwoShiftAmmount, objectTwoPos.z, boundZ) : objectTwoPos.z);
                }
                if (resetObjectWithRNG)
                {
                    bool isObjectOne = UnityEngine.Random.Range(0, 20) < 10;
                    ObjectMover respawnMe = resetObjectWithRNG ? isObjectOne ? object1Respawn : object2Respawn : object1Respawn;
                    Vector3 outPos = resetObjectWithRNG ? isObjectOne ? objectOnePos : objectTwoPos : objectOnePos;
                    respawnMe.RespawnObjects(outPos, respawnMe.transform.rotation);
                }
                else
                {
                    if (object1Respawn.gameObject.tag == other.gameObject.tag) { object1Respawn.RespawnObjects(objectOnePos, object1Respawn.transform.rotation); }
                    else { object2Respawn.RespawnObjects(objectTwoPos, object2Respawn.transform.rotation); }
                }
            }
        }
    }
}
