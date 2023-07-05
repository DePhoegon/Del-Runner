using System;
using System.Collections;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class GameManager : MonoBehaviour
{
    public GameObject barrier;
    public ObjectMover barrierRespawn;
    public Transform NeutralSpawnPoint;
    public Transform BottomRail;
    public Transform TopRail;
    public GameObject playButton;
    public GameObject arrowPad;
    public GameObject player;
    public GameObject scoreLine;
    public Rigidbody playerbody;
    public int physicsForce;
    public float spinPerUpdate;
    public bool spinPlayer;
    private bool isOnUnderside;
    private bool isOnFrontside;
    private bool isOnTopside;
    private bool isOnBackside;
    private bool isOnTopRail;
    private bool isTwisted = false;
    private Vector3 spinAxis;


    private void Update()
    {
        if (spinPlayer) { player.transform.Rotate(spinPerUpdate * spinAxis.x, spinPerUpdate * spinAxis.y, spinPerUpdate * spinAxis.z); }
    }
    public bool topSideReturn() { return isOnTopside; }
    public bool frontSideReturn() { return isOnFrontside; }
    public bool backSideReturn() { return isOnBackside; }
    public bool underSideReturn() { return isOnUnderside; }
    public bool topRailReturn() { return isOnTopRail; }
    public void toggleSides(bool up, bool side, bool back, bool down, bool topRail) {
        isOnTopRail = topRail;
        toggleSides(up, side, back, down);
    }
    private Vector3 railSnap(Vector3 vect)
    {
        float BrX = isOnTopRail ? TopRail.position.x : BottomRail.transform.position.x;
        float BrY = isOnTopRail ? TopRail.transform.position.y : BottomRail.transform.position.y;
        return new Vector3(BrX+ vect.x, BrY+ vect.y, vect.z);
    }
    public void toggleSides(bool up, bool side, bool back, bool down)
    {
        isOnTopside = up; isOnFrontside = side; isOnUnderside = down; isOnBackside = back; spinAxis = new Vector3(0,0,0);
        playerbody.constraints = RigidbodyConstraints.None;
        // physics for if on top side
        if (isOnTopside) {
            Physics.gravity = new Vector3(0, -1 * physicsForce, 0);
            playerbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ |RigidbodyConstraints.FreezeRotationY;
            player.transform.position = railSnap(Vector3.up); 
            spinAxis = Vector3.right;
        }
        // physics for if on front side
        if (isOnFrontside) {
            Physics.gravity = new Vector3(-1 * physicsForce, 0, 0);
            playerbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ |RigidbodyConstraints.FreezeRotationX;
            player.transform.position = railSnap(Vector3.right);
            spinAxis = Vector3.down;
        }
        // physics for if on back side
        if (isOnBackside) {
            Physics.gravity = new Vector3(physicsForce, 0, 0);
            playerbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
            player.transform.position = railSnap(Vector3.left);
            spinAxis = Vector3.up;
        }
        // physics for if on underside
        if (isOnUnderside) {
            Physics.gravity = new Vector3(0, physicsForce, 0);
            playerbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ |RigidbodyConstraints.FreezeRotationY;
            player.transform.position = railSnap(Vector3.down);
            spinAxis = Vector3.left;
        }
    }
    private Vector3 spawnPointShuffle(Transform rail)
    {
        float spXb = rail.position.x;
        float spYb = rail.position.y;
        float spZ = NeutralSpawnPoint.position.z;
        float x, y, z;
        isTwisted = UnityEngine.Random.Range(0, 20) > 15;
        float offset = isTwisted ? 1.3f : 0.76f;
        switch (UnityEngine.Random.Range(0, 9))
        {
            case 1: x = spXb; y = spYb + offset; z = spZ; break;
            case 2: x = spXb + offset; y = spYb; z = spZ; break;
            case 3: x = spXb; y = spYb - offset; z = spZ; break;
            case 4: x = spXb - offset; y = spYb; z = spZ; break;
            default: x = spXb; y = spYb; z = spZ; break;
        }
        return new Vector3(x, y, z);
    }
    IEnumerator SpawnBarriers()
    {
        while (true)
        {
            float waitTime = UnityEngine.Random.Range(0.2f, 1f);
            yield return new WaitForSeconds(waitTime);
            Quaternion rotation = isTwisted ? Quaternion.identity : Quaternion.AngleAxis(45, Vector3.back);
            barrierRespawn.RespawnObjects(spawnPointShuffle(BottomRail), rotation);
            yield return new WaitForSeconds(waitTime);
            rotation = isTwisted ? Quaternion.identity : Quaternion.AngleAxis(45, Vector3.back);
            barrierRespawn.RespawnObjects(spawnPointShuffle(TopRail), rotation);
        }
    }
    
    public void GameStart()
    {
        player.SetActive(true);
        playButton.SetActive(false);
        arrowPad.SetActive(true);
        toggleSides(true, false, false, false, false);
        StartCoroutine("SpawnBarriers");
        // InvokeRepeating("scoreUp", 2f, 1f);
    }
}
