using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoFade : MonoBehaviour
{
    [SerializeField]
    Color startColor;
    [SerializeField]
    Color endColor;
    [SerializeField]
    float destroyTime;
    SpriteRenderer renderer_;
    // Start is called before the first frame update
    void Start()
    {
        renderer_ = this.GetComponent<SpriteRenderer>();
        renderer_.sprite = FindObjectOfType<PlayerSprite>().gameObject.GetComponent<SpriteRenderer>().sprite;
        renderer_.color = startColor;
        Invoke("DestroyObject", destroyTime);
    }

    void DestroyObject()
    {
        Destroy(this.gameObject);
    }

    float elapsedTime = 0;
    void Update()
    {
        elapsedTime += Time.deltaTime;
        float percentComp = elapsedTime / destroyTime;
        renderer_.color = Color.Lerp(startColor, endColor, percentComp);
    }
}
