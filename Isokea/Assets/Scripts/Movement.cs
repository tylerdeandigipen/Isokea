using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CameraShake;
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
    float _gravity = 1;

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
    [SerializeField]
    float numOfDashClones = 4;
    public Vector3 dashDirection;

    [Header("Jumping")]
    [SerializeField]
    float jumpHeight = 1;
    [SerializeField]
    float jumpCheckDistance = 1;
    [SerializeField]
    LayerMask groundLayer;

    [Header("Misc")]
    [SerializeField]
    Vector3 movementDir = new Vector3(0, 0, 0);
    [SerializeField]
    float groundCheckDistance = 1;
    [SerializeField]
    float timeBetweenSteps = 1;
    [SerializeField]
    float footStepVariation = 1;
    [SerializeField]
    GameObject footStep;
    [SerializeField]
    GameObject jumpEffect;
    [SerializeField]
    GameObject footStepSpawnpoint;
    [SerializeField]
    GameObject weapon;
    [SerializeField]
    GameObject dashClone;
    [SerializeField]
    GameObject dashParticles;
    public float yVelocity = 0;
    CharacterController controller;
    [HideInInspector]
    public bool canMove = true;
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
    float dashCloneTimer = 0;
    float footStepTimer = 0;
    bool isWalking;
    TrailRenderer dashTrail;
    int playerLayer;
    [HideInInspector]
    public bool isGrounded;
    [HideInInspector]
    public bool isShooting;
    void Start()
    { 
        numOfDashClones += 1;
        dashTrail = this.GetComponentInChildren<TrailRenderer>();
        controller = this.GetComponentInChildren<CharacterController>();
        animator = this.GetComponentInChildren<Animator>();
        playerLayer = controller.gameObject.layer;
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
        JumpCheck();
        DoMovements();
        DoAnimations();
    }

    void GetInput()
    {
        float sprintMultiplier = 1;
        //sprint in case we want it
        if (Input.GetKey(KeyCode.LeftShift) && canSprint)
            sprintMultiplier = speedMultiplier;
        //dash input
        if (Input.GetKeyDown(dashKey) && !isDashing && canDash && isGrounded && movementDir != Vector3.zero && !isShooting)
        {
            Dash();
        }
        movementDir = new Vector3(Input.GetAxis("Vertical") * sprintMultiplier, 0, -Input.GetAxis("Horizontal") * sprintMultiplier);
    }
    void Dash()
    {
        CameraShaker.Presets.ShortShake2D();
        canMove = false;
        doHitstun = false;
        isDashing = true;
        canDash = false;
        dashDirection = movementDir;
        controller.gameObject.layer = 8;
        Invoke("EndHitstun", dashDuration);
        Invoke("RefreshDash", dashDuration + dashCooldown);
    }
    void DoMovements()
    {
        if (canMove && !isShooting)
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

        //check if grounded
        RaycastHit hit;
        Debug.DrawRay(controller.transform.position, transform.TransformDirection(Vector3.down) * groundCheckDistance, Color.red);
        if (Physics.Raycast(controller.transform.position, transform.TransformDirection(Vector3.down) * groundCheckDistance, out hit, Mathf.Infinity, groundLayer))
        {
            if (hit.distance < groundCheckDistance)
            {
                isGrounded = true;
                controller.Move(new Vector3(0, -20, 0) * Time.deltaTime);
            }
            else
                isGrounded = false;
        }
        else
        {
            isGrounded = false;
        }
        //make gravity
        if (!isGrounded)
        {
            yVelocity -= _gravity; 
            controller.Move(new Vector3(0, yVelocity, 0) * Time.deltaTime);
        }
        else
            yVelocity = 0;
    }

    void JumpCheck()
    {
        RaycastHit hit;
        Debug.DrawRay(controller.transform.position + (movementDir * 2), transform.TransformDirection(Vector3.down) * jumpCheckDistance, Color.green);

        if (Physics.Raycast(controller.transform.position + (movementDir * 2), transform.TransformDirection(Vector3.down) * jumpCheckDistance, out hit, Mathf.Infinity, groundLayer))
        {
            if (hit.distance > jumpCheckDistance && isGrounded && !isDashing)
            {
                Jump();
            }
        }
        else if (isGrounded && !isDashing)
            Jump();
    }
    void Jump()
    {       
        Instantiate(jumpEffect, footStepSpawnpoint.transform.position, Quaternion.identity);
        yVelocity = jumpHeight;
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
        controller.gameObject.layer = playerLayer; 
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
        isWalking = false;
        if (movementDir.x > 0) //moving up
        {
            if (movementDir.z > 0) //moving left
            {
                if (isAttacking)
                    ChangeAnimationState("Attack_Up_Left");
                else if (isDashing)
                    ChangeAnimationState("Dash_Up_Left");
                else
                {
                    isWalking = true;
                    ChangeAnimationState("Up_Left");
                }
            }
            else if (movementDir.z < 0) //moving right
            {
                if (isAttacking)
                    ChangeAnimationState("Attack_Up_Right");
                else if (isDashing)
                    ChangeAnimationState("Dash_Up_Right");
                else
                {
                    isWalking = true;
                    ChangeAnimationState("Up_Right");
                }
            }
           else //pure up
           {
                if (isAttacking)
                    ChangeAnimationState("Attack_Up_Right");
                else if (isDashing)
                    ChangeAnimationState("Dash_Up_Right");
                else
                {
                    isWalking = true;
                    ChangeAnimationState("Up_Right");
                }
                //ChangeAnimationState("Up");
            }
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
                {
                    isWalking = true;
                    ChangeAnimationState("Down_Left");
                }
            }
            else if (movementDir.z < 0) //moving right
            {
                if (isAttacking)
                    ChangeAnimationState("Attack_Down_Right");
                else if (isDashing)
                    ChangeAnimationState("Dash_Down_Right");
                else
                {
                    isWalking = true;
                    ChangeAnimationState("Down_Right");
                }
            }
            else //pure down
            {
                if (isAttacking)
                    ChangeAnimationState("Attack_Down_Right");
                else if (isDashing)
                    ChangeAnimationState("Dash_Down_Right");
                else
                {
                    isWalking = true;
                    ChangeAnimationState("Down_Right");
                }
                //ChangeAnimationState("player_down");
            }
        }
        else if (movementDir.z > 0) //moving left
        {
            if (isAttacking)
                ChangeAnimationState("Attack_Down_Left");
            else if (isDashing)
                ChangeAnimationState("Dash_Down_Left");
            else
            {
                isWalking = true;
                ChangeAnimationState("Down_Left");
            }
            //ChangeAnimationState("Left");
        }
        else if (movementDir.z < 0) //moving right
        {
            if (isAttacking)
                ChangeAnimationState("Attack_Down_Right");
            else if (isDashing)
                ChangeAnimationState("Dash_Down_Right");
            else
            {
                isWalking = true;
                ChangeAnimationState("Down_Right");
            }
            //ChangeAnimationState("Right");
        }
        else
        {
            ChangeAnimationState("Idle_Temp");
        }

        //make dashclones
        if (isDashing)
        {
            //add speed lines for extra effect
            dashParticles.SetActive(true);
            dashTrail.emitting = true;
            if (dashCloneTimer >= (dashDuration / numOfDashClones))
            {
                Instantiate(dashClone, controller.gameObject.transform.position, controller.gameObject.transform.rotation);
                dashCloneTimer = 0;
            }
            else
                dashCloneTimer += Time.deltaTime;
        }
        else
        {
            dashParticles.SetActive(false);
            dashCloneTimer = 0;
            dashTrail.emitting = false;
        }
        if (isWalking && isGrounded)
        {
            if (footStepTimer >= timeBetweenSteps)
            {
                Instantiate(footStep, footStepSpawnpoint.transform.position, Quaternion.identity);
                footStepTimer = 0 + Random.Range(-footStepVariation, footStepVariation);
            }
            else
                footStepTimer += Time.deltaTime;
        }
    }
}
