using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaEnemy : MonoBehaviour
{
    [HideInInspector]
    public Arena currentArena;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        currentArena.enemiesKilled += 1;
    }
}
