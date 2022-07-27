using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    [SerializeField]
    Vector3 camOffset;
    [SerializeField]
    GameObject objectToFollow;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (objectToFollow != null)
        {
            this.transform.position = new Vector3(objectToFollow.transform.position.x + camOffset.x, objectToFollow.transform.position.y + camOffset.y, objectToFollow.transform.position.z + camOffset.z);
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
    }
}
