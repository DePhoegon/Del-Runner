using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject barrier;
    public Transform NeutralSpawnPoint;
    public Transform PosXSpawnPoint;
    public Transform NegXSpawnPoint;
    public Transform PosYSpawnPoint;
    public Transform NegYSpawnPoint;
    int score = 0;
    public TextMeshProUGUI scoreText;
    public GameObject playButton;
    public GameObject player;
    public Rigidbody playerbody;
    public int physicsForce;
    public bool isOnUnderside;
    public bool isOnFrontside;
    public bool isOnTopside;
    public bool isOnBackside;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool topSideReturn() { return isOnTopside; }
    public bool frontSideReturn() { return isOnFrontside; }
    public bool backSideReturn() { return isOnBackside; }
    public bool underSideReturn() { return isOnUnderside; }
    public void toggleSides(bool up, bool side, bool back, bool down)
    {
        isOnTopside = up; isOnFrontside = side; isOnUnderside = down; isOnBackside = back;
        // physics for if on top side
        if (isOnTopside) {
            Physics.gravity = new Vector3(0, -1 * physicsForce, 0);
            playerbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            player.transform.position = new Vector3(0f, 1f, 0f);
            
        }
        // physics for if on front side
        if (isOnFrontside) {
            Physics.gravity = new Vector3(-1 * physicsForce, 0, 0);
            playerbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            player.transform.position = new Vector3(1f, 0f, 0f);
        }// physics for if on back side
        if (isOnBackside) {
            Physics.gravity = new Vector3(physicsForce, 0, 0);
            playerbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            player.transform.position = new Vector3(-1f, 0f, 0f);
        }
        // physics for if on underside
        if (isOnUnderside) {
            Physics.gravity = new Vector3(0, physicsForce, 0);
            playerbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            player.transform.position = new Vector3(0f, -1f, 0f);
        }
    }
    private Vector3 spawnPointShuffle()
    {
        switch (Random.Range(0, 4))
        {
            case 1: return PosYSpawnPoint.position;
            case 2: return PosXSpawnPoint.position;
            case 3: return NegYSpawnPoint.position;
            case 4: return NegXSpawnPoint.position;
            default: return NeutralSpawnPoint.position;
        }
    }

    IEnumerator SpawnBarriers()
    {
        while (true)
        {
            float waitTime = Random.Range(0.5f, 2f);
            yield return new WaitForSeconds(waitTime);
            Instantiate(barrier, spawnPointShuffle(), Quaternion.identity);
        }
    }

    void scoreUp()
    {
        score++;
        scoreText.text = score.ToString();
    }
    
    public void GameStart()
    {
        player.SetActive(true);
        playButton.SetActive(false);
        toggleSides(true, false, false, false);
        StartCoroutine("SpawnBarriers");
        InvokeRepeating("scoreUp", 2f, 1f);
    }
}
