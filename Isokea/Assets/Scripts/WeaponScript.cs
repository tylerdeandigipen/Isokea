using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    float lightAttackDurationEndCombo = 1;
    [SerializeField]
    float lightForwardForceEndCombo = 1;

    [Header("Keybinds")]
    [SerializeField]
    KeyCode LightAttackKey = KeyCode.Mouse0;

    [Header("Object References")]
    [SerializeField]
    GameObject weaponObject;
    [SerializeField]
    GameObject playerGameobject;
    [SerializeField]
    Movement plMovement;


    int comboThreshold = 2;
    int lightAttackNum;
    float timer = 0;
    bool canAttack = true;
    Vector3 movementDir = new Vector3(0,0,1);
    bool canCombo = false;
    // Start is called before the first frame update
    void Start()
    {
        weaponObject.SetActive(false);        
    }

    // Update is called once per frame
    void Update()
    {
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
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    if (Input.GetKey(KeyCode.A))
                    {
                        movementDir = new Vector3(1, 0, 1);
                    }
                    else if (Input.GetKey(KeyCode.D))
                    {
                        movementDir = new Vector3(1, 0, -1);
                    }
                    else
                        movementDir = new Vector3(1, 0, 0);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    if (Input.GetKey(KeyCode.A))
                    {
                        movementDir = new Vector3(-1, 0, 1);
                    }
                    else if (Input.GetKey(KeyCode.D))
                    {
                        movementDir = new Vector3(-1, 0, -1);
                    }
                    else
                        movementDir = new Vector3(-1, 0, 0);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    movementDir = new Vector3(0, 0, 1);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    movementDir = new Vector3(0, 0, -1);
                }
                transform.rotation = Quaternion.LookRotation(movementDir);
            }
        }
        if (Input.GetKeyDown(LightAttackKey) && canAttack && plMovement.isDashing == false)
        {
            if (canCombo = true && lightAttackNum >= comboThreshold)
            {
                LightComboFinalAttack();
            }
            else
                LightAttack();
        }
    }


    void LightAttack()
    {
        plMovement.isAttacking = true;
        canAttack = false;
        weaponObject.SetActive(true);
        lightAttackNum += 1;
        timer = 0;
        plMovement.EnterHitstun(movementDir, lightForwardForce, lightAttackDuration);
        Invoke("EndAttack", lightAttackDuration);
    }
    void LightComboFinalAttack()
    {
        plMovement.isAttacking = true;
        lightAttackNum = 0;
        canAttack = false;
        weaponObject.SetActive(true);
        plMovement.EnterHitstun(movementDir, lightForwardForceEndCombo, lightAttackDurationEndCombo);
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
