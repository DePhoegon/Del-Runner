using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scoreLine : MonoBehaviour
{
    public GameObject whiteStrips;
    private int score = 0;
    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public int getScore() { return score; }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstical")
        {
            scoreUp();
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "wline")
        {
            Destroy(other.gameObject);
            Instantiate(whiteStrips, whiteStrips.transform.position, Quaternion.identity);
        }
    }
    public void scoreUp()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
