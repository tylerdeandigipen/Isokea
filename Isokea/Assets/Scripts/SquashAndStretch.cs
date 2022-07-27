using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashAndStretch : MonoBehaviour
{
    Vector3 startingScale;
    [SerializeField]
    float duration;
    // Start is called before the first frame update
    void Start()
    {
        startingScale = transform.localScale;
    }

    float timer = 0;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        this.transform.localScale = Vector3.Slerp(this.transform.localScale, startingScale * 1.5f, timer / duration);
    }
}
