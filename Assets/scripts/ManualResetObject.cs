using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualResetObjects : MonoBehaviour
{
    public ObjectMover object1Respawn;
    public ObjectMover object2Respawn;
    public bool resetObjectWithRNG;
    public bool enableRespawn;
    public bool respawnOnX;
    public bool enableShiftPos;
    public float objectOneShiftXAmmount;
    public float objectOneShiftYAmmount;
    public float objectOneShiftZAmmount;
    public float objectTwoShiftXAmmount;
    public float objectTwoShiftYAmmount;
    public float objectTwoShiftZAmmount;
    public string shiftAxises;
    public int possibleSpeedIncrease;
    public bool increaseSpeed;
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
    private int respawn1Speed;
    private int respawn2Speed;

    private void Start()
    {
        boundX = restrictX ? "x" : "none";
        boundY = restrictY ? "y" : "none";
        boundZ = restrictZ ? "z" : "none";
        if (!enableShiftPos) { objectOneShiftXAmmount = 0; shiftAxises = null; }
        if (object2Respawn == null) { object2Respawn = object1Respawn; }
        respawn1Speed = object1Respawn.GetSpeed();
        respawn2Speed = object2Respawn.GetSpeed();
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
            float oX = other.transform.position.x;
            float oY = other.transform.position.y;
            float oZ = other.transform.position.z;
            Destroy(other.gameObject);
            if (enableRespawn)
            {
                Vector3 objectOnePos = object1Respawn.gameObject.transform.position;
                Vector3 objectTwoPos = object2Respawn.gameObject.transform.position;
                if (respawnOnX)
                {
                    objectOnePos.Set(oX, objectOnePos.y, objectOnePos.z);
                    objectTwoPos.Set(oX, objectTwoPos.y, objectTwoPos.z);
                }
                if (shiftAxises != null)
                {
                    
                    objectOnePos = new Vector3(shiftAxises.Contains("x") ? adjustedRange(objectOneShiftXAmmount, objectOnePos.x, boundX) : objectOnePos.x, shiftAxises.Contains("y") ? adjustedRange(objectOneShiftYAmmount, objectOnePos.y, boundY) : objectOnePos.y , shiftAxises.Contains("z") ? adjustedRange(objectOneShiftZAmmount, objectOnePos.z, boundZ) : objectOnePos.z);
                    objectTwoPos = new Vector3(shiftAxises.Contains("x") ? adjustedRange(objectTwoShiftXAmmount, objectTwoPos.x, boundX) : objectTwoPos.x, shiftAxises.Contains("y") ? adjustedRange(objectTwoShiftYAmmount, objectTwoPos.y, boundY) : objectTwoPos.y, shiftAxises.Contains("z") ? adjustedRange(objectTwoShiftZAmmount, objectTwoPos.z, boundZ) : objectTwoPos.z);
                }
                if (resetObjectWithRNG)
                {
                    bool isObjectOne = UnityEngine.Random.Range(0, 20) < 10;
                    ObjectMover respawnMe = resetObjectWithRNG ? isObjectOne ? object1Respawn : object2Respawn : object1Respawn;
                    Vector3 outPos = resetObjectWithRNG ? isObjectOne ? objectOnePos : objectTwoPos : objectOnePos;
                    variableRespawn(respawnMe, outPos, respawnMe.transform.rotation, respawnMe.Equals(object1Respawn));
                }
                else 
                {
                    if (object1Respawn.gameObject.tag == other.gameObject.tag) { variableRespawn(object1Respawn, objectOnePos, object1Respawn.transform.rotation, true); }
                    else { variableRespawn(object2Respawn, objectTwoPos, object2Respawn.transform.rotation, false); }
                }
            }
        }
    }
     private void variableRespawn(ObjectMover target, Vector3 postion, Quaternion rotation, bool isObjectOne)
    {
        if (increaseSpeed) 
        {
            int targetSpeed = isObjectOne ? respawn1Speed : respawn2Speed;
            int spd = Random.Range(targetSpeed, possibleSpeedIncrease + targetSpeed);
            target.RespawnObjects(postion, rotation, spd);
        } else { target.RespawnObjects(postion, rotation); }
    }
}
