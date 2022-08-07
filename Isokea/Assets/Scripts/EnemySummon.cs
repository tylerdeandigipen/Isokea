using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySummon : MonoBehaviour
{
    [SerializeField]
    float spawnDelay;
    [SerializeField]
    GameObject EnemyController;
    [SerializeField]
    GameObject EnemySprite;
    // Start is called before the first frame update
    void Start()
    {
        EnemyController.SetActive(false);
        EnemySprite.SetActive(false);
        Invoke("UnhideEnemy", spawnDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UnhideEnemy()
    {
        EnemyController.SetActive(true);
        EnemySprite.SetActive(true);
    }
}
