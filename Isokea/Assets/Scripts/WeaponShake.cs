using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraShake;
public class WeaponShake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        CameraShaker.Presets.ShortShake2D();
    }
}
