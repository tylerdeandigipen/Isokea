using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField]
    float destroyDelay;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", destroyDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Destroy()
    {
        Destroy(this.gameObject);
    }
}
