using System;
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
    public GameManager Manager;
    bool up;
    bool fside;
    bool down;
    bool bside;
    bool canMove = false;

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
        up = Manager.isOnTopside;
        fside = Manager.isOnFrontside;
        bside = Manager.isOnBackside;
        down = Manager.isOnUnderside;
    }

    // Update is called once per frame
    void Update()
    {
        canMove = true;
        canjump = isGrounded;
        boolUpdate();
        bool hldUp = up;
        bool hldDown = down;
        bool hldFntSide = fside;
        bool hldBckSide = bside;
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (up && canMove)
            {
                // code to move to one of the sides
                hldFntSide = sidePicker(); hldBckSide = !hldFntSide; hldUp = false;
                canMove = false;
            }
            else if ((fside || bside) && canMove)
            {
                // code to move to underside
                hldFntSide = false; hldDown = true; hldBckSide = false; hldUp = false;
                canMove = false;
            } else if (down)
            { if (canjump) { rb.AddForce(Vector3.down * jumpForce, ForceMode.Impulse); } }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if ((fside || bside) && canMove)
            {
                // code to move to topside
                hldUp = true; hldFntSide = false; hldBckSide = false;
                canMove = false;
            }
            if (down && canMove)
            {
                // code to move to one of the sides
                hldDown = false; hldFntSide = sidePicker(); hldBckSide = !hldFntSide;
                canMove = false;
            } else if (hldUp) { if (canjump) { rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); } }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if ((up || down) && canMove)
            {
                // Code to move to the backside
                hldFntSide = false; hldDown = false; hldBckSide = true; hldUp = false;
                canMove = false;
            }
            if (fside && canMove)
            {
                // Code to move to the top side or bottom side
                hldUp = sidePicker(); hldDown = !hldUp; hldFntSide = false; hldBckSide=false;
                canMove = false;
            }
            if (bside) { if (canjump) { rb.AddForce(Vector3.left * jumpForce, ForceMode.Impulse); } }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if ((up || down) && canMove)
            {
                // Code to move to the frontside
                hldFntSide = true; hldDown = false; hldBckSide = false; hldUp = false;
                canMove = false;
            }
            if (bside && canMove)
            {
                // Code to move to the top side or bottom side
                hldUp = sidePicker(); hldDown = !hldUp; hldFntSide = false; hldBckSide = false;
                canMove = false;
            }
            if (fside) { if (canjump) { rb.AddForce(Vector3.right * jumpForce, ForceMode.Impulse); } }
        }
        Vector3 directionalVector = hldUp ? Vector3.up : hldFntSide ? Vector3.right : hldDown ? Vector3.down : Vector3.left;
        if (hldUp != up ||  hldDown != down || hldFntSide != fside || hldBckSide != bside) { Manager.toggleSides(hldUp, hldFntSide, hldBckSide, hldDown); } else if (Input.GetMouseButtonDown(0))
        {
            // jump
            if (canjump) { rb.AddForce(directionalVector * jumpForce, ForceMode.Impulse); }
        }
    }
    private bool sidePicker()
    {
        return UnityEngine.Random.Range(0, 20) < 10 ? true : false;
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
