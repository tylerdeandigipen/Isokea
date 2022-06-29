using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 1;
    [SerializeField]
    float speedMultiplier = 2;
    [SerializeField]
    bool canSprint = false;
    [SerializeField]
    int _gravity = 1;
    CharacterController controller;
    [SerializeField]
    Vector3 movementDir = new Vector3(0,0,0);
    [SerializeField]
    GameObject weapon;
    float yVelocity = 0;
    bool canMove = true;
    bool doHitstun = false;
    Vector3 hitDirection;
    float hitForce;
    float stunDuration;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponentInChildren<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
            GetInput();
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        //DoAnimations();
    }
    void FixedUpdate()
    {
        DoMovements();
    }

    void GetInput()
    {
        float sprintMultiplier = 1;
        if (Input.GetKey(KeyCode.LeftShift) && canSprint)
            sprintMultiplier = speedMultiplier;
        movementDir = new Vector3(Input.GetAxis("Vertical") * sprintMultiplier, 0, -Input.GetAxis("Horizontal") * sprintMultiplier);
    }

    void DoMovements()
    {
        if (canMove)
        {
            controller.Move(moveSpeed * Time.deltaTime * movementDir);          
        }
        else if (doHitstun && movementDir != new Vector3(0,0,0))
        {
            controller.Move(hitForce * Time.deltaTime * Vector3.Lerp(movementDir, new Vector3(0,movementDir.y,0), stunDuration));     
        }
        else
            controller.Move(hitForce * Time.deltaTime * hitDirection);
        //make gravity
        if (!controller.isGrounded)
        {
            yVelocity -= _gravity;
            controller.Move(new Vector3(0, yVelocity, 0) * Time.deltaTime);
        }
        else
            yVelocity = 0;
    }

    //make hitstun slow player to stop instead of instant stop
    public void EnterHitstun(Vector3 hitDirection_, float hitForce_, float stunDuration_)
    {
        canMove = false;
        doHitstun = true;
        hitDirection = hitDirection_.normalized;
        hitForce = hitForce_;
        stunDuration = stunDuration_;
        Invoke("EndHitstun", stunDuration);
    }
    void EndHitstun()
    {
        doHitstun = false;
        canMove = true;
    }
    private string currentState;
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
            return;
        animator.Play(newState);
        currentState = newState;
    }

    void DoAnimations()
    {
        if (movementDir.x > 0) //moving up
        {
            if (movementDir.z > 0) //moving left
            {
                //ChangeAnimationState("player_up_left");
                Debug.Log("up left");
            }
            else if (movementDir.z < 0) //moving right
            {
                //ChangeAnimationState("player_up_right");
                Debug.Log("up right");
            }
            else //pure up
            {
                //ChangeAnimationState("player_up");
                Debug.Log("up");
            }

        }
        else if (movementDir.x < 0) //moving down
        {
            if (movementDir.z > 0) //moving left
            {
                //ChangeAnimationState("player_down_left");
                Debug.Log("down left");
            }
            else if (movementDir.z < 0) //moving right
            {
                //ChangeAnimationState("player_down_right");
                Debug.Log("down right");
            }
            else //pure down
            {
                //ChangeAnimationState("player_down");
                Debug.Log("down");
            }
        }
        else if (movementDir.z > 0) //moving left
        {
            //ChangeAnimationState("player_left");
            Debug.Log("left");
        }
        else if (movementDir.z < 0) //moving right
        {
            //ChangeAnimationState("player_right");
            Debug.Log("right");
        }
    }
}
