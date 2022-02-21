using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCarSpawner : MonoBehaviour
{

    public SimulationManager sM;

    public GameObject ghostCar;

    public Transform intersection;

    private float currentXDistanceTime;
    private float currentYDistanceTime;

    private float distanceXCalculated;
    private float distanceYCalculated;

    private bool isXtrail;
    private bool isGhostCarSpawned;

    public float currentTime;
    public float waitTime;


    // Start is called before the first frame update
    public void Awake()
    {
        sM = FindObjectOfType<SimulationManager>();
    }

    void Start()
    {
        CalculateWaitTime();
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        MonitorWaitTimeSpawn();
        DistanceChecker();
    }

    public void SpawnGhostCar()
    {
        Instantiate(ghostCar, transform.position, transform.rotation);
    }

    public void CalculateWaitTime()
    {
        waitTime = (sM.northSpawner.position.y * -1f - sM.eastSpawner.position.x * -1f + sM.ghostCarDistance / 2)/ sM.ghostCarVelocity; 
    }

    public void MonitorWaitTimeSpawn()
    {
        if (waitTime > 0)
        {
            if (isXtrail)
            {
                if (currentTime >= waitTime && !isGhostCarSpawned)
                {
                    SpawnGhostCar();
                    isGhostCarSpawned = true;
                    currentXDistanceTime = 0f;
                }
                else
                {
                    CalculateWaitTime();
                }

            }
            else
            {
                if (!isGhostCarSpawned)
                {
                    SpawnGhostCar();
                    isGhostCarSpawned = true;
                }
            }
        }
    }

    public void DistanceChecker()
    {
        if (waitTime > 0)
        {
            if (isXtrail)
            {
                if (isGhostCarSpawned)
                {
                    currentXDistanceTime += Time.deltaTime;
                    distanceXCalculated = sM.ghostCarVelocity * currentXDistanceTime;

                    if(distanceXCalculated >= sM.ghostCarDistance)
                    {
                        SpawnGhostCar();
                        currentXDistanceTime = 0f;
                    }
                }
            }

            else
            {
                currentYDistanceTime += Time.deltaTime;
                distanceYCalculated = sM.ghostCarVelocity * currentYDistanceTime;

                if(distanceYCalculated >= sM.ghostCarDistance)
                {
                    SpawnGhostCar();
                    currentYDistanceTime = 0f;
                }
            }
        }
    }
}
