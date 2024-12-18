using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Timer
    float currentTime = 0;
    [SerializeField] float targetTime = 1.2f;

    // What to instantiate
    [Header("What to Instaniate")]
    [SerializeField] GameObject obstacle;
    [SerializeField] GameObject portalObstacle;
    [SerializeField] GameObject meteorObstalces;
    [SerializeField] GameObject superMeteorObstacle;

    // Height
    [Header("Obstacle Settings")]
    [SerializeField] float yPos = 2;

    // When to spawn Portal Obstalce
    int obstaclesSpawned = 0;
    [SerializeField] int targetObstacles;

    // Chance to spawn Super Star
    float chanceToSpawnSuperMeteor = 0.8f;

    // Booleans
    bool spawningIsPaused;
    bool spawnNormals = true;

    // References
    [Header("Script References")]
    [SerializeField] Player playerScript;
    [SerializeField] GameMaster masterScript;

    void Update()
    {
        if (!masterScript.gameIsOver && !spawningIsPaused && masterScript.gameStarted) // Pause everything if Player is dead OR "Warp" Obstacle was spawned
        {
            if (currentTime >= targetTime) // Can Spawn
            {
                if (playerScript.isInFlyMode) // Flying
                {
                    if (obstaclesSpawned >= targetObstacles) // Portal Obstacle
                    {                       
                        SpawnPortalObstacle();
                        spawnNormals = true; // Now spawn normal
                        targetTime = 1.2f;
                        StartCoroutine(SpawnDelay());
                    }
                    else // Meteor Obstacle
                    {
                        if (spawnNormals)
                        {
                            SpawnNormalObstacle();                            
                        }
                        else
                        {
                            SpawnMeteorObstacle();
                        }
                    }
                }
                else // Bouncing
                {
                    if (obstaclesSpawned >= targetObstacles) // Portal Obstacle
                    {                       
                        SpawnPortalObstacle();
                        spawnNormals = false; // Now spawn meteors
                        chanceToSpawnSuperMeteor = 0.8f;
                        targetTime = 1.6f; // 1.6
                        StartCoroutine(SpawnDelay());
                    }
                    else 
                    {
                        if (!spawnNormals)
                        {
                            SpawnMeteorObstacle();
                        }
                        else
                        {
                            SpawnNormalObstacle();
                        }                       
                    }
                }                           
                
                currentTime = 0; // Reset timer
            }

            currentTime += Time.deltaTime; // Counting...
        }       
    }

    #region Spawning Obstacle Methods
    void SpawnPortalObstacle()
    {
        GameObject wo = Instantiate(portalObstacle);
        wo.transform.position = transform.position + new Vector3(0, Random.Range(-yPos, yPos), 0);
        wo.GetComponent<PortalObstacle>().masterScript = masterScript;
        obstaclesSpawned = 0;
    }
    void SpawnNormalObstacle()
    {
        GameObject go = Instantiate(obstacle); // Only instantiate a copy of obstacle - set position afterwards
        go.transform.position = transform.position + new Vector3(0, Random.Range(-1f, 2f), 0); // Set the instantiated object's positon to the spawners position added with random height differences
        go.GetComponent<CrystalObstacle>().masterScript = masterScript;
        go.GetComponent<CrystalObstacle>().playerScript = playerScript;
        obstaclesSpawned++;
    }
    void SpawnMeteorObstacle()
    {       
        if (Random.value > chanceToSpawnSuperMeteor)
        {
            GameObject go = Instantiate(superMeteorObstacle);
            go.transform.position = transform.position + new Vector3(0, Random.Range(-3, 3), 0);
            go.GetComponent<SuperMeteorObstacle>().masterScript = masterScript;
            go.GetComponent<SuperMeteorObstacle>().playerScript = playerScript;
            obstaclesSpawned++;

            chanceToSpawnSuperMeteor = 1f;
        }
        else
        {
            GameObject go = Instantiate(meteorObstalces);
            go.transform.position = transform.position + new Vector3(0, Random.Range(-3, 3), 0);
            go.GetComponent<MeteorObstacle>().masterScript = masterScript;
            go.GetComponent<MeteorObstacle>().playerScript = playerScript;
            obstaclesSpawned++;
        }
    }
    
    #endregion

    IEnumerator SpawnDelay()
    {
        spawningIsPaused = true;

        yield return new WaitForSeconds(0.5f);

        spawningIsPaused = false;
    }
}
