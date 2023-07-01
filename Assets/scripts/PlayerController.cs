using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    Rigidbody rb;
    public float jumpForce;
    bool canjump = true;
    bool isGrounded = false;
    public GameManager gameManager;
    bool up;
    bool side;
    bool down;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //update bools
    void boolUpdate()
    {
        up = GetComponent<GameManager>().isOnTopside;
        down = GetComponent<GameManager>().isOnUnderside;
        side = GetComponent<GameManager>().isOnFrontside;
    }

    // Update is called once per frame
    void Update()
    {
        canjump = isGrounded;
        boolUpdate();
        bool hldUp = up;
        bool hldDown = down;
        bool hldSide = side;
        if (Input.GetMouseButtonDown(0)) {
            Vector3 directionalVector = up ? Vector3.up : side ? Vector3.right : Vector3.down;
            // jump
            if (canjump) { rb.AddForce(directionalVector * jumpForce, ForceMode.Impulse); }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && !down)
        {
            if (up)
            {
                // code to move to front side
                side = true; up = false;
            }
            else if (side) 
            { 
                // code to move to underside
                side = false; down = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && !up)
        {
            if (side)
            {
                // code to move to topside
                up = true; side = false;
            }
            if (down)
            {
                // code to move to front side
                down = false; side = true;
            }
        }
        if (hldUp != up ||  hldDown != down || hldSide != side) { GetComponent<GameManager>().toggleSides(up, down, side); }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground") { isGrounded = true; }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground") { isGrounded=false; }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstical") { SceneManager.LoadScene("Game"); }
    }
}
