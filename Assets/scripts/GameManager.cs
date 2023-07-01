using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject barrier;
    public Transform spawnPoint;
    int score = 0;
    public TextMeshProUGUI scoreText;
    public GameObject playButton;
    public GameObject player;
    public Rigidbody playerbody;
    public int physicsForce;
    public bool isOnUnderside;
    public bool isOnFrontside;
    public bool isOnTopside;

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
    public bool underSideReturn() { return isOnUnderside; }
    public void toggleSides(bool up, bool side, bool down)
    {
        isOnTopside = up; isOnFrontside = side; isOnUnderside = down;
        // physics for if on top side
        if (isOnTopside) {
            Physics.gravity = new Vector3(0, -1 * physicsForce, 0);
            playerbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            
        }
        // physics for if on front side
        if (isOnFrontside) {
            Physics.gravity = new Vector3(physicsForce, 0, 0);
            playerbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
        // physics for if on underside
        if (isOnUnderside) {
            Physics.gravity = new Vector3(0, physicsForce, 0);
            playerbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
    }

    IEnumerator SpawnBarriers()
    {

        while (true)
        {
            float waitTime = Random.Range(0.5f, 2f);
            yield return new WaitForSeconds(waitTime);
            Instantiate(barrier, spawnPoint.position, Quaternion.identity);
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
        toggleSides(true, false, false);
        StartCoroutine("SpawnBarriers");
        InvokeRepeating("scoreUp", 2f, 1f);
    }
}
