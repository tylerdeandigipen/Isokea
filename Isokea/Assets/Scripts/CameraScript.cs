using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    float smoothSpeed = 1;
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
        Vector3 desiredPos = new Vector3(objectToFollow.transform.position.x + camOffset.x, objectToFollow.transform.position.y + camOffset.y, objectToFollow.transform.position.z + camOffset.z);
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        this.transform.position = smoothedPos;
    }
}
