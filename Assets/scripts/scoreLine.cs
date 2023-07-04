using TMPro;
using UnityEngine;

public class scoreLine : MonoBehaviour
{
    private int score = 0;
    public TextMeshProUGUI scoreText;

    public int getScore() { return score; } 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstical")
        {
            scoreUp();
            Destroy(other.gameObject);
        }
    }
    public void scoreUp()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
