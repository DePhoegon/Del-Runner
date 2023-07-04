using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject barrier;
    public Transform NeutralSpawnPoint;
    public Transform BottomRail;
    public Transform TopRail;
    public GameObject playButton;
    public GameObject arrowPad;
    public GameObject player;
    public GameObject scoreLine;
    public Rigidbody playerbody;
    public int physicsForce;
    private bool isOnUnderside;
    private bool isOnFrontside;
    private bool isOnTopside;
    private bool isOnBackside;
    private bool isOnTopRail;
    private bool isTwisted = false;

    public bool topSideReturn() { return isOnTopside; }
    public bool frontSideReturn() { return isOnFrontside; }
    public bool backSideReturn() { return isOnBackside; }
    public bool underSideReturn() { return isOnUnderside; }
    public bool topRailReturn() { return isOnTopRail; }
    public void toggleSides(bool up, bool side, bool back, bool down, bool topRail) {
        isOnTopRail = topRail;
        toggleSides(up, side, back, down);
    }
    private Vector3 railSnap(float x, float y, float z)
    {
        float BrX = isOnTopRail ? TopRail.position.x : BottomRail.transform.position.x;
        float BrY = isOnTopRail ? TopRail.transform.position.y : BottomRail.transform.position.y;
        return new Vector3(BrX+x, BrY+y, z);
    }
    public void toggleSides(bool up, bool side, bool back, bool down)
    {
        isOnTopside = up; isOnFrontside = side; isOnUnderside = down; isOnBackside = back;
        // physics for if on top side
        if (isOnTopside) {
            Physics.gravity = new Vector3(0, -1 * physicsForce, 0);
            playerbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            player.transform.position = railSnap(0f, 1f, 0f);
            
        }
        // physics for if on front side
        if (isOnFrontside) {
            Physics.gravity = new Vector3(-1 * physicsForce, 0, 0);
            playerbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            player.transform.position = railSnap(1f, 0f, 0f);
        }// physics for if on back side
        if (isOnBackside) {
            Physics.gravity = new Vector3(physicsForce, 0, 0);
            playerbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            player.transform.position = railSnap(-1f, 0f, 0f);
        }
        // physics for if on underside
        if (isOnUnderside) {
            Physics.gravity = new Vector3(0, physicsForce, 0);
            playerbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            player.transform.position = railSnap(0f, -1f, 0f);
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
            Instantiate(barrier, spawnPointShuffle(BottomRail), rotation);
            yield return new WaitForSeconds(waitTime);
            rotation = isTwisted ? Quaternion.identity : Quaternion.AngleAxis(45, Vector3.back);
            Instantiate(barrier, spawnPointShuffle(TopRail), rotation);
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
