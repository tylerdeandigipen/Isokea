using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField]
    float timeBetweenAttacks = 1;
    [SerializeField]
    float attackDuration = 1;
    [SerializeField]
    float forwardForce = 1;
    [SerializeField]
    GameObject weaponObject;
    [SerializeField]
    GameObject playerGameobject;
    [SerializeField]
    Movement plMovement;

    [Header("Keybinds")]
    [SerializeField]
    KeyCode LightAttackKey = KeyCode.Mouse0;


    bool canAttack = true;
    Vector3 movementDir = new Vector3(0,0,1);
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
        if (Input.GetKeyDown(LightAttackKey) && canAttack)
        {
            LightAttack();
        }
    }


    void LightAttack()
    {
        canAttack = false;
        weaponObject.SetActive(true);
        plMovement.EnterHitstun(movementDir, forwardForce, attackDuration);
        Invoke("EndAttack", attackDuration);
    }
    void EndAttack()
    {
        weaponObject.SetActive(false);
        Invoke("AllowAttack", timeBetweenAttacks);
    }

    void AllowAttack()
    {
        canAttack = true;
    }
    
}
