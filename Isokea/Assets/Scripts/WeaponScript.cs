using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraShake;
public class WeaponScript : MonoBehaviour
{
    [Header("Light Attack Settings")]
    [SerializeField]
    float lightTimeBetweenAttacks = 1;
    [SerializeField]
    float lightAttackDuration = 1;
    [SerializeField]
    float lightForwardForce = 1;
    [SerializeField]
    float lightComboDelay = .2f;
    [SerializeField]
    float lightDamage;
    [SerializeField]
    float lightFinalDamage;
    [SerializeField]
    public float hitstopDuration;
    [SerializeField]
    float lightAttackDurationEndCombo = 1;
    [SerializeField]
    float lightForwardForceEndCombo = 1;
    [SerializeField]
    public float knockbackForce = 1;
    [SerializeField]
    public float upForce = 1;

    [Header("Ranged Attack Settings")]
    [SerializeField]
    float projectileSpeed = 20;
    [SerializeField]
    float shootDelay = 1;
    [SerializeField]
    float shotSpread = 1;

    [Header("Keybinds")]
    [SerializeField]
    KeyCode LightAttackKey = KeyCode.Mouse0;
    [SerializeField]
    KeyCode RangedAttackKey = KeyCode.LeftControl;

    [Header("Object References")]
    [SerializeField]
    GameObject weaponObject;
    [SerializeField]
    GameObject playerGameobject;
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    Movement plMovement;

    [HideInInspector]
    public float currentDamage;
    public Vector3 rotToVector;
    int comboThreshold = 2;
    int lightAttackNum;
    float timer = 0;
    bool canAttack = true;
    [HideInInspector]
    public Vector3 movementDir = new Vector3(0,0,1);
    bool canCombo = false;
    float shootingTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        weaponObject.SetActive(false);        
    }

    // Update is called once per frame
    void Update()
    {
        rotToVector = transform.rotation.eulerAngles.normalized;
        this.transform.position = playerGameobject.transform.position;
        GetInput();
        ComboTimer();
    }
    void ComboTimer()
    {
        if(canAttack)
            timer += Time.deltaTime;
        if (timer <= lightComboDelay)
        {
            canCombo = false;
        }
        else
        {
            canCombo = true;
            lightAttackNum = 0;
        }

    }

    void GetInput()
    {
        if (canAttack) //check to freeze rotation when attacking 
        {
            movementDir = new Vector3(Input.GetAxisRaw("Vertical"), 0, -Input.GetAxisRaw("Horizontal"));
            if(movementDir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(movementDir);     
        }

        if (Input.GetKeyDown(LightAttackKey) && canAttack && !plMovement.isDashing && plMovement.isGrounded && !plMovement.isShooting)
        {
            if (canCombo = true && lightAttackNum >= comboThreshold)
            {
                LightComboFinalAttack();
            }
            else
                LightAttack();
        }

        if (Input.GetKey(RangedAttackKey) && canAttack && !plMovement.isDashing && plMovement.isGrounded && new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal")) != Vector3.zero)
        {
            plMovement.isShooting = true;
            if (shootingTimer > shootDelay)
            {
                RangedAttack();
                shootingTimer = 0;
            }
            shootingTimer += Time.deltaTime;
        }
        if (Input.GetKeyUp(RangedAttackKey))
        {
            plMovement.isShooting = false;
        }
    }
    void RangedAttack()
    {
        Vector3 lookDir = new Vector3(Input.GetAxis("Vertical") + Random.Range(-shotSpread, shotSpread), 0, -Input.GetAxis("Horizontal") +Random.Range(-shotSpread, shotSpread));
        lookDir = lookDir.normalized;
        GameObject bullet = Instantiate(projectile, playerGameobject.transform.position + lookDir, Quaternion.Euler(lookDir));
        bullet.GetComponent<Rigidbody>().velocity = lookDir * projectileSpeed;
    }

    void LightAttack()
    {
        currentDamage = lightDamage;
        plMovement.isAttacking = true;
        canAttack = false;
        weaponObject.SetActive(true);
        lightAttackNum += 1;
        timer = 0;
        plMovement.EnterHitstun(transform.rotation.eulerAngles.normalized, lightForwardForce, lightAttackDuration);
        Invoke("EndAttack", lightAttackDuration);
    }
    void LightComboFinalAttack()
    {
        currentDamage = lightFinalDamage;
        plMovement.isAttacking = true;
        lightAttackNum = 0;
        canAttack = false;
        weaponObject.SetActive(true);
        plMovement.EnterHitstun(transform.rotation.eulerAngles.normalized, lightForwardForceEndCombo, lightAttackDurationEndCombo);
        Invoke("EndAttack", lightAttackDurationEndCombo);
    }
    void EndAttack()
    {
        plMovement.isAttacking = false;
        weaponObject.SetActive(false);
        Invoke("AllowAttack", lightTimeBetweenAttacks);
    }

    void AllowAttack()
    {
        canAttack = true;
    }
    
}
