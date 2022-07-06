using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    [Header("Normal Movement")]
    [SerializeField]
    float moveSpeed = 1;
    [SerializeField]
    float speedMultiplier = 2;
    [SerializeField]
    bool canSprint = false;
    [SerializeField]
    int _gravity = 1;
    
    [Header("Dash Settings")]
    [SerializeField]
    float dashSpeed = 1;
    [SerializeField]
    float dashTurnSpeed = 1;
    [SerializeField]
    float dashDuration = 1;
    [SerializeField]
    KeyCode dashKey = KeyCode.LeftShift;
    [SerializeField]
    float dashCooldown = .2f;
    public Vector3 dashDirection;

    [Header("Misc")]
    [SerializeField]
    Vector3 movementDir = new Vector3(0,0,0);
    [SerializeField]
    GameObject weapon;
    float yVelocity = 0;
    CharacterController controller;
    bool canMove = true;
    bool doHitstun = false;
    Vector3 hitDirection;
    float hitForce;
    float stunDuration;
    Animator animator;
    [HideInInspector]
    public bool isDashing = false;
    bool canDash = true;
    [HideInInspector]
    public bool isAttacking = false;
    void Start()
    {
        controller = this.GetComponentInChildren<CharacterController>();
        animator = this.GetComponentInChildren<Animator>();
    }
    
    void Update()
    {
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
        DoAnimations();
    }

    void GetInput()
    {
        float sprintMultiplier = 1;
        if (Input.GetKey(KeyCode.LeftShift) && canSprint)
            sprintMultiplier = speedMultiplier;
        if (Input.GetKeyDown(dashKey) && !isDashing && canDash)
        {
            canMove = false;
            doHitstun = false;
            isDashing = true;
            canDash = false;
            dashDirection = movementDir;
            Invoke("EndHitstun", dashDuration);
            Invoke("RefreshDash", dashDuration + dashCooldown);
        }
        movementDir = new Vector3(Input.GetAxis("Vertical") * sprintMultiplier, 0, -Input.GetAxis("Horizontal") * sprintMultiplier);
    }

    void DoMovements()
    {
        if (canMove)
        {
            controller.Move(moveSpeed * Time.deltaTime * movementDir);
        }
        else if (doHitstun && movementDir != new Vector3(0, 0, 0))
        {
            controller.Move(hitForce * Time.deltaTime * Vector3.Lerp(movementDir, new Vector3(0, movementDir.y, 0), stunDuration));
        }
        else if (doHitstun)
        {
            controller.Move(hitForce * Time.deltaTime * hitDirection);
        }
        else if (isDashing)
        {
            dashDirection = Vector3.Lerp(dashDirection, movementDir, dashTurnSpeed);
            controller.Move(dashSpeed * Time.deltaTime * dashDirection.normalized);            
        }

        //make gravity
        if (!controller.isGrounded)
        {
            yVelocity -= _gravity; 
            controller.Move(new Vector3(0, yVelocity, 0) * Time.deltaTime);
        }
        else
            yVelocity = 0;
    }
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
        isDashing = false;
        canMove = true;
    }
    void RefreshDash()
    {
        canDash = true;
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
                if (isAttacking)
                    ChangeAnimationState("Attack_Up_Left");
                else if (isDashing)
                    ChangeAnimationState("Dash_Up_Left");
                else
                    ChangeAnimationState("Up_Left");
            }
            else if (movementDir.z < 0) //moving right
            {
                if (isAttacking)
                    ChangeAnimationState("Attack_Up_Right");
                else if (isDashing)
                    ChangeAnimationState("Dash_Up_Right");
                else
                    ChangeAnimationState("Up_Right");
            }
           // else //pure up
            //{
                //ChangeAnimationState("Up");
           // }

        }
        else if (movementDir.x < 0) //moving down
        {
            if (movementDir.z > 0) //moving left
            {
                if (isAttacking)
                    ChangeAnimationState("Attack_Down_Left");
                else if (isDashing)
                    ChangeAnimationState("Dash_Down_Left");
                else
                    ChangeAnimationState("Down_Left");
            }
            else if (movementDir.z < 0) //moving right
            {
                if (isAttacking)
                    ChangeAnimationState("Attack_Down_Right");
                else if (isDashing)
                    ChangeAnimationState("Dash_Down_Right");
                else
                    ChangeAnimationState("Down_Right");
            }
            //else //pure down
            //{
                //ChangeAnimationState("player_down");
          //  }
        }
        //else if (movementDir.z > 0) //moving left
        //{
            //ChangeAnimationState("Left");
        //}
        //else if (movementDir.z < 0) //moving right
        //{
            //ChangeAnimationState("Right");
        //}
        else
        {
            ChangeAnimationState("Idle_Temp");
        }
    }
}
