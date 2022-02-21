using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyVehicleBehavior : MonoBehaviour
{
    

    private SimulationManager sM;
    private CanvasScript cS;

    private Vector2 northVelocity = new Vector2(0f, 1f);

    public bool isNorthTrail = true;

    public bool prepareNorth = true;

    public bool followingGhostCar;

    private Rigidbody2D rb;

    public float eVSpeed;

    public float destroyTimer;
    public bool startTimer;

    private void Awake()
    {
        sM = FindObjectOfType<SimulationManager>();
        cS = FindObjectOfType<CanvasScript>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        VelocityRegulator();

        ActivateTimer();
    }

    private void ActivateTimer()
    {
        if (startTimer && destroyTimer <= 3f)
        {
            destroyTimer += Time.deltaTime;
        }
        else if (startTimer)
        {
            startTimer = false;
            Destroy(this.gameObject);
            sM.stopSpawn = false;
            cS.ResumeButton();
        }
    }

    private void VelocityRegulator()
    {
        if (isNorthTrail)
        {
            rb.velocity = northVelocity * eVSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Slowdown"))
        {
            eVSpeed = 1.5f;
        }

        else if (other.CompareTag("Slowdown Intersection"))
        {
            eVSpeed = 1.5f;
        }

        else if (other.CompareTag("Fast Lane"))
        {
            eVSpeed = 1.75f;
        }

        else if (other.CompareTag("Intersection Zone"))
        {
            eVSpeed = 0.5f;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Hospital Drop Off") && other.transform.position.y >= transform.position.y && other.transform.position.y - transform.position.y <= 1f)
        {
            eVSpeed = 0;
            startTimer = true;
        }

        if (isNorthTrail)
        {
            if (prepareNorth)
            {
                if (other.CompareTag("Ghost Car N") && other.transform.position.y >= transform.position.y && other.transform.position.y - transform.position.y <= 0.1f)
                {
                    if (!other.GetComponent<GhostCarBehavior>().occupied && !followingGhostCar)
                    {
                        followingGhostCar = true;
                        FollowGhostCar(other.gameObject);

                        other.GetComponent<GhostCarBehavior>().occupied = true;
                    }
                    else if (followingGhostCar)
                    {
                        FollowGhostCar(other.gameObject);
                    }

                }
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Intersection Zone"))
        {
            eVSpeed = 1.5f;
        }

        if (isNorthTrail)
        {
            if (prepareNorth)
            {
                if (other.CompareTag("Ghost Car N"))
                {
                    if (followingGhostCar)
                    {
                        followingGhostCar = false;
                    }
                }
            }
        }
    }

    private void FollowGhostCar(GameObject ghostCar)
    {
        if (ghostCar)
        {
            if (followingGhostCar)
            {
                transform.position = ghostCar.transform.position;
            }
        }
        else
        {
            followingGhostCar = false;
        }
    }
}
