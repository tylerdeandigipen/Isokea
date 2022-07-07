using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoFade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<PlayerSprite>().gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
