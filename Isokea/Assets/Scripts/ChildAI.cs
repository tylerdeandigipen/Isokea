using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ChildAI : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public GameObject player;
    public LayerMask groundLayer, playerLayer;
    [HideInInspector]
    public bool playerInSightRange, playerInAttackRange;
    public float sightRange, attackRange;
    bool isWalking, isAttacking = false;
    [SerializeField]
    Animator animator;
    private string currentState;
    [SerializeField]
    Vector3 movementDir;
    [SerializeField]
    float timeBetweenSteps = 1;
    [SerializeField]
    float footStepVariation = 1;
    [SerializeField]
    GameObject footStep;
    [SerializeField]
    GameObject footStepSpawnpoint;
    float footStepTimer = 0;
    Rigidbody rb;
    private Vector3 moveDir;
    private Vector3 prevDir;
    private Vector3 prevPos;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerSprite>().gameObject;
        agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.enabled == true)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
            if (playerInSightRange && !playerInAttackRange)
                ChasePlayer();
            else if (playerInSightRange && playerInAttackRange)
                AttackPlayer();
            else
                isWalking = false;
        }
        prevDir = (transform.position + moveDir) - prevPos;
        prevDir = prevDir.normalized;
        prevPos = transform.position;
        DoAnimations();
    }

    public void TakeKnockback(float knockbackForce, Vector3 knockbackDirection)
    {
        agent.enabled = false;
        rb.isKinematic = false;
        rb.AddForce(knockbackDirection * knockbackForce);
    }
    void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
        isWalking = true;
    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        isWalking = false;
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
            return;
        animator.Play(newState);
        currentState = newState;
    }

    void DoAnimations()
    {
        movementDir = prevDir;
        if (!isWalking)
        {
            ChangeAnimationState("Idle_Temp");
        }
        else if (movementDir.x > 0) //moving up
        {
            if (movementDir.z > 0) //moving left
            {
                if (isAttacking)
                    ChangeAnimationState("Attack_Up_Left");
                else if(isWalking)
                {
                    ChangeAnimationState("Up_Left");
                }
            }
            else if (movementDir.z < 0) //moving right
            {
                if (isAttacking)
                    ChangeAnimationState("Attack_Up_Right");
                else if(isWalking)
                {
                    ChangeAnimationState("Up_Right");
                }
            }
            else //pure up
            {
                if (isAttacking)
                    ChangeAnimationState("Attack_Up_Right");
                else if (isWalking)
                {
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
                else if (isWalking)
                {
                    ChangeAnimationState("Down_Left");
                }
            }
            else if (movementDir.z < 0) //moving right
            {
                if (isAttacking)
                    ChangeAnimationState("Attack_Down_Right");
                else if (isWalking)
                {
                    ChangeAnimationState("Down_Right");
                }
            }
            else //pure down
            {
                if (isAttacking)
                    ChangeAnimationState("Attack_Down_Right");
                else if (isWalking)
                {
                    ChangeAnimationState("Down_Right");
                }
                //ChangeAnimationState("player_down");
            }
        }
        else if (movementDir.z > 0) //moving left
        {
            if (isAttacking)
                ChangeAnimationState("Attack_Down_Left");
            else if (isWalking)
            {
                ChangeAnimationState("Down_Left");
            }
            //ChangeAnimationState("Left");
        }
        else if (movementDir.z < 0) //moving right
        {
            if (isAttacking)
                ChangeAnimationState("Attack_Down_Right");
            else if (isWalking)
            {
                ChangeAnimationState("Down_Right");
            }
            //ChangeAnimationState("Right");
        }
        else
        {
            ChangeAnimationState("Idle_Temp");
        }
        if (isWalking)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "playerWeapon")
        {
            WeaponScript temp = other.GetComponentInParent<WeaponScript>();
            TakeKnockback(1, new Vector3(temp.movementDir.x * temp.knockbackForce, temp.upForce, temp.movementDir.z * temp.knockbackForce));
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            agent.enabled = true;
            rb.isKinematic = true;
        }
    }
}
