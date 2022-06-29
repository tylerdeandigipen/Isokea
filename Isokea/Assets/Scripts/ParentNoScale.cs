using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentNoScale : MonoBehaviour
{
    [SerializeField]
    GameObject objectToFollow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = objectToFollow.transform.position;
    }
}
