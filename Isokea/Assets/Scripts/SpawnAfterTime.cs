using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAfterTime : MonoBehaviour
{
    [SerializeField]
    float spawnDelay;
    [SerializeField]
    GameObject objectToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Spawn", spawnDelay);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Spawn()
    {
        GameObject temp = Instantiate(objectToSpawn);
        temp.transform.position = this.transform.position;
    }
}
