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
    [SerializeField]
    ParticleSystem otherParticles;
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem temp = this.GetComponent<ParticleSystem>();
        var main = temp.main;
        main.simulationSpeed = .3f;
        var main2 = otherParticles.main;
        main2.simulationSpeed = .3f;
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
