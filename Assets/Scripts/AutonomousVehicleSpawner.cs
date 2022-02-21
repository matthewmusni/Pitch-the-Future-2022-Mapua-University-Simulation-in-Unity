using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutonomousVehicleSpawner : MonoBehaviour
{
    public SimulationManager sM;
    public CanvasScript cS;

    private float timer;
    private float timeInterval;
    private bool firstSpawn;

    public bool isNorthTrail;
    public bool isEastTrail;
    public bool isWestTrail;
    public bool isSouthTrail;

    public GameObject emergencyVehicle;
    public bool emergencyEvent;
    public bool isEmergencySpawner;

    public GameObject av;

    public bool limitReached;

    private void Awake()
    {
        sM = FindObjectOfType<SimulationManager>();
        cS = FindObjectOfType<CanvasScript>();
    }

    void Start()
    {
        SpawnAV();
    }

    void Update()
    {
        timer += Time.deltaTime;

        SpawnAV();
    }

    private void SpawnAV()
    {
        if (emergencyEvent && isEmergencySpawner)
        {
            SpawnEmergencyVehicle();
            sM.stopSpawn = true;
        }

        else
        {
            if (!sM.stopSpawn && !limitReached)
            {
                if (timer >= timeInterval || !firstSpawn)
                {
                    GameObject autoV;
                    autoV = (GameObject)Instantiate(av, transform.position, transform.rotation);

                    if (isNorthTrail)
                    {
                        autoV.GetComponent<AutonomousVehicleBehavior>().RotateToNorth();
                    }
                    else if (isEastTrail)
                    {
                        autoV.GetComponent<AutonomousVehicleBehavior>().RotateToEast();
                    }
                    else if (isWestTrail)
                    {
                        autoV.GetComponent<AutonomousVehicleBehavior>().RotateToWest();
                    }
                    else if (isSouthTrail)
                    {
                        autoV.GetComponent<AutonomousVehicleBehavior>().RotateToSouth();
                    }

                    GenerateTimeInterval();
                    timer = 0f;
                    firstSpawn = true;
                }
            }
        }

    }

    private void GenerateTimeInterval()
    {
        timeInterval = Random.Range(1f, 3f);
    }

    private void SpawnEmergencyVehicle()
    {
        Debug.Log("Emergency!");
        GameObject eV = Instantiate(emergencyVehicle, transform.position, transform.rotation);
        RotateToNorth(eV);
        emergencyEvent = false;
        cS.DisableButton();
    }

    public void RotateToNorth(GameObject eV)
    {
        Vector3 newRotation = new Vector3(0, 0, 0);
        eV.transform.eulerAngles = newRotation;
    }
}
