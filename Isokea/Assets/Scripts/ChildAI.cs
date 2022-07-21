using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ChildAI : MonoBehaviour
{
    [SerializeField]
    float maxHealth = 10;
    [SerializeField]
    public float currentHealth;
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
    float maxPoise = 1;
    [SerializeField]
    float flashDuration = .05f;
    [SerializeField]
    Animator animator;
    private string currentState;
    [SerializeField]
    float groundCheckDistance = 1;
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
    bool isGrounded;
    private Vector3 moveDir;
    private Vector3 prevDir;
    private Vector3 prevPos;
    HitStop hitstopScript;
    bool isColliding;
    [SerializeField]
    SpriteRenderer spRenderer;
    [SerializeField]
    Material flashMat;
    Material normalMat;
    float currentPoise;
    // Start is called before the first frame update
    void Start()
    {
        normalMat = spRenderer.material;
        currentHealth = maxHealth;
        hitstopScript = FindObjectOfType<HitStop>();
        rb = this.GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerSprite>().gameObject;
        agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        GotAttacked -= Time.deltaTime;
        if (agent.enabled == true)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
            if (playerInSightRange && !playerInAttackRange)
                ChasePlayer();
            else if (playerInSightRange && playerInAttackRange)
                AttackPlayer();
            else if (agent.velocity != Vector3.zero)
                isWalking = true;
            else
                isWalking = false;
        }
        prevDir = (transform.position + moveDir) - prevPos;
        prevDir = prevDir.normalized;
        prevPos = transform.position;
        DoAnimations();
    }

    void GroundCheck()
    {
        //check if grounded
        RaycastHit hit;
        Debug.DrawRay(this.transform.position, transform.TransformDirection(Vector3.down) * groundCheckDistance, Color.red);
        if (Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.down) * groundCheckDistance, out hit, Mathf.Infinity, groundLayer))
        {
            if (hit.distance < groundCheckDistance)
            {
                isGrounded = true;
            }
            else
                isGrounded = false;
        }
        else
        {
            isGrounded = false;
        }

        //activate rb is not grounded
        if (!isGrounded)
        { 
        agent.enabled = false;
        rb.isKinematic = false;
        }
    }
    public void TakeKnockback(float knockbackForce, Vector3 knockbackDirection)
    {
        agent.enabled = false;
        rb.isKinematic = false;
        DamageFlash();
        if (currentPoise <= 0)
        {
            rb.AddForce(knockbackDirection * knockbackForce);
            currentPoise = maxPoise;

        }
        else
        {
            currentPoise -= knockbackDirection.magnitude;
        }
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
    void DamageFlash()
    {
        spRenderer.material = flashMat;
        StartCoroutine(ResetColor(flashDuration));
    }
    public void TakeDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
        }

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Destroy(this.gameObject.transform.parent.gameObject);
    }
    IEnumerator ResetColor(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        spRenderer.material = normalMat;
    }
    public float GotAttackedDelay = 0.2f;//seconds
    float GotAttacked = 0f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "playerWeapon")
        {            
            WeaponScript temp = other.GetComponentInParent<WeaponScript>();
            if (GotAttacked <= 0)
            {
                if (temp.movementDir == Vector3.zero)
                {
                    TakeKnockback(1, other.transform.parent.transform.rotation.eulerAngles.normalized * temp.knockbackForce);
                }
                else
                    TakeKnockback(1, new Vector3(temp.movementDir.x * temp.knockbackForce, temp.upForce, temp.movementDir.z * temp.knockbackForce));
                TakeDamage(temp.currentDamage);
                hitstopScript.Stop(temp.hitstopDuration);
                GotAttacked = GotAttackedDelay;
            }
        }
        if (other.gameObject.tag == "playerProjectile")
        {
            bulletScript temp = other.GetComponent<bulletScript>();
            TakeDamage(temp.damage);
            TakeKnockback(1, temp.gameObject.GetComponent<Rigidbody>().velocity.normalized * temp.bulletKnockback);
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
