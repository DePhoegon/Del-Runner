using UnityEngine;
using UnityEngine.EventSystems;
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
    bool topRail;
    private Vector2 starTouchPos;
    private Vector2 moveTouchPos;
    private bool swipelocked = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("swipeUnlocker", 0.5f, 0.3f);
    }
    private void swipeUnlocker() { swipelocked = false; }
    //update bools
    void boolUpdate()
    {
        up = Manager.topSideReturn();
        fside = Manager.frontSideReturn();
        bside = Manager.backSideReturn();
        down = Manager.underSideReturn();
        topRail = Manager.topRailReturn();
    }
    private float posDif(float one, float two) { return one < two ? two - one : one - two; }
    // Update is called once per frame
    void Update()
    {
        bool swipeup = false;
        bool swipedown = false;
        bool swiperight = false;
        bool swiperleft = false;
        bool noswipe = false;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {  starTouchPos = Input.GetTouch(0).position; }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Ended)
        {
            if (swipelocked == false) 
            { 
                float threshold = 15f;
                moveTouchPos = Input.GetTouch(0).position;
                float xdif = posDif(starTouchPos.x, moveTouchPos.x);
                float ydif = posDif(starTouchPos.y, moveTouchPos.y);
                xdif = xdif > threshold ? xdif : 0f;
                ydif = ydif > threshold ? ydif : 0f;
                if (xdif == ydif)
                {
                    if (xdif < 5) { ydif = 0f; xdif = 0f; }
                    else { if (RandomRange(0, 31, 16)) { ydif = ydif - 1f; } else { ydif = ydif + 1f; } }
                }
                if (xdif > ydif) { if (moveTouchPos.x < starTouchPos.x) { swiperleft = true; } else { swiperight = true; } }
                else if (ydif != xdif) { if (moveTouchPos.y > starTouchPos.y) { swipeup = true; } else { swipedown = true; } }
                else { noswipe = true; }
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || swipedown) { pressDown(); } 
        else if (Input.GetKeyDown(KeyCode.UpArrow) || swipeup) { pressUp(); } 
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || swiperleft) { pressLeft(); } 
        else if (Input.GetKeyDown(KeyCode.RightArrow) || swiperight) { pressRight(); } 
        else if (Input.GetKeyDown(KeyCode.Space) || noswipe) { pressJump(); }
    }
    public void pressUp() { playermovement(true, false, false, false); }
    public void pressDown() { playermovement(false, true, false, false); }
    public void pressLeft() { playermovement(false, false, true, false); }
    public void pressRight() { playermovement(false, false, false, true); }
    public void pressJump() { playermovement(false, false, false, false); }
    private void playermovement(bool upInput, bool downInput, bool leftInput, bool rightInput)
    {
        swipelocked = true;
        canMove = true;
        canjump = isGrounded;
        boolUpdate();
        bool hldUp = up;
        bool hldDown = down;
        bool hldFntSide = fside;
        bool hldBckSide = bside;
        if (upInput)
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
            }
            else if (hldUp)
            {
                if (canjump) { rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); }
                else { if (!topRail) Manager.toggleSides(false, false, false, true, true); }
            }
        }
        if (downInput)
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
            }
            else if (down)
            {
                if (canjump) { rb.AddForce(Vector3.down * jumpForce, ForceMode.Impulse); }
                else { if (topRail) Manager.toggleSides(true, false, false, false, false); }
            }
        }
        if (leftInput)
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
                hldUp = sidePicker(); hldDown = !hldUp; hldFntSide = false; hldBckSide = false;
                canMove = false;
            }
            if (bside) { if (canjump) { rb.AddForce(Vector3.left * jumpForce, ForceMode.Impulse); } }
        }
        if (rightInput)
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
        if (hldUp != up || hldDown != down || hldFntSide != fside || hldBckSide != bside) { Manager.toggleSides(hldUp, hldFntSide, hldBckSide, hldDown); }
        else if (!(upInput || downInput || leftInput || rightInput)) { if (canjump) { rb.AddForce(directionalVector * jumpForce, ForceMode.Impulse); } }
    }
    private bool sidePicker() { return RandomRange(0, 20, 10) ? true : false; }
    private void OnCollisionEnter(Collision collision) { if (collision.gameObject.tag == "Ground") { isGrounded = true; } }
    private void OnCollisionExit(Collision collision) { if (collision.gameObject.tag == "Ground") { isGrounded=false; } }
    private void OnTriggerEnter(Collider other) { if (other.gameObject.tag == "Obstical") { SceneManager.LoadScene("Game"); } }
    private bool RandomRange(float min, float max, float lessThan) { return UnityEngine.Random.Range(min, max) < lessThan; }
}