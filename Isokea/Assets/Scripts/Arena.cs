using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField]
    GameObject walls;
    [HideInInspector]
    public int enemiesKilled;
    [SerializeField]
    GameObject[] wave1;
    [SerializeField]
    GameObject[] wave2;
    [SerializeField]
    GameObject[] wave3;
    [SerializeField]
    GameObject[] wave4;
    [SerializeField]
    GameObject[] wave5;
    int currentWave;
    int waveLength;
    // Start is called before the first frame update
    void Start()
    {
        walls.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesKilled >= waveLength && walls.activeSelf == true)
        {
            enemiesKilled = 0;
            currentWave += 1;
            spawnWave(currentWave);
        }
    }

    void spawnWave(int currentWave)
    {
        switch (currentWave)
        {
            case 1:
                waveLength = wave1.Length;
                if (waveLength == 0)
                    endArena();
                for (int i = 0; i < wave1.Length; i++)
                {
                    GameObject enemy = Instantiate(wave1[i]);
                    enemy.GetComponent<ArenaEnemy>().currentArena = this;
                    enemy.transform.parent = this.transform.parent;
                    enemy.transform.position = this.transform.position;
                    enemy.transform.position = new Vector3(enemy.transform.position.x + Random.Range(-this.transform.localScale.x / 3, this.transform.localScale.x / 3), enemy.transform.position.y, enemy.transform.position.z + Random.Range(-this.transform.localScale.z / 3, this.transform.localScale.z / 3));
                }
                break;
            case 2:
                waveLength = wave2.Length;
                if (waveLength == 0)
                    endArena();
                for (int i = 0; i < wave2.Length; i++)
                {
                    GameObject enemy = Instantiate(wave2[i]);
                    enemy.GetComponent<ArenaEnemy>().currentArena = this;
                    enemy.transform.parent = this.transform.parent;
                    enemy.transform.position = this.transform.position;
                    enemy.transform.position = new Vector3(enemy.transform.position.x + Random.Range(-this.transform.localScale.x / 3, this.transform.localScale.x / 3), enemy.transform.position.y, enemy.transform.position.z + Random.Range(-this.transform.localScale.z / 3, this.transform.localScale.z / 3));
                }
                break;
            case 3:
                waveLength = wave3.Length;
                if (waveLength == 0)
                    endArena();
                for (int i = 0; i < wave3.Length; i++)
                {
                    GameObject enemy = Instantiate(wave3[i]);
                    enemy.GetComponent<ArenaEnemy>().currentArena = this;
                    enemy.transform.parent = this.transform.parent;
                    enemy.transform.position = this.transform.position;
                    enemy.transform.position = new Vector3(enemy.transform.position.x + Random.Range(-this.transform.localScale.x / 3, this.transform.localScale.x / 3), enemy.transform.position.y, enemy.transform.position.z + Random.Range(-this.transform.localScale.z / 3, this.transform.localScale.z / 3));
                }
                break;
            case 4:
                waveLength = wave4.Length;
                if (waveLength == 0)
                    endArena();
                for (int i = 0; i < wave4.Length; i++)
                {
                    GameObject enemy = Instantiate(wave4[i]);
                    enemy.GetComponent<ArenaEnemy>().currentArena = this;
                    enemy.transform.parent = this.transform.parent;
                    enemy.transform.position = this.transform.position;
                    enemy.transform.position = new Vector3(enemy.transform.position.x + Random.Range(-this.transform.localScale.x / 3, this.transform.localScale.x / 3), enemy.transform.position.y, enemy.transform.position.z + Random.Range(-this.transform.localScale.z / 3, this.transform.localScale.z / 3));
                }
                break;
            case 5:
                waveLength = wave5.Length;
                if (waveLength == 0)
                    endArena();
                for (int i = 0; i < wave5.Length; i++)
                {
                    GameObject enemy = Instantiate(wave5[i]);
                    enemy.GetComponent<ArenaEnemy>().currentArena = this;
                    enemy.transform.parent = this.transform.parent;
                    enemy.transform.position = this.transform.position;
                    enemy.transform.position = new Vector3(enemy.transform.position.x + Random.Range(-this.transform.localScale.x / 3, this.transform.localScale.x / 3), enemy.transform.position.y, enemy.transform.position.z + Random.Range(-this.transform.localScale.z / 3, this.transform.localScale.z / 3));
                }
                break;
        }
    }

    void endArena()
    {
        walls.SetActive(false);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            walls.SetActive(true);
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
