using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutonomousVehicleBehavior : MonoBehaviour
{
    private SimulationManager sM;

    private Vector2 northVelocity = new Vector2(0f,1f);
    private Vector2 eastVelocity = new Vector2(1f,0f);
    private Vector2 westVelocity = new Vector2(-1f,0f);
    private Vector2 southVelocity = new Vector2(0f,-1f);

    public bool isNorthTrail;
    public bool isEastTrail;
    public bool isWestTrail;
    public bool isSouthTrail;

    public bool prepareNorth;
    public bool prepareEast;
    public bool prepareWest;
    public bool prepareSouth;

    public int slowdownIntersectionEncountered;
    public int IntersectionEncountered;

    public bool followingGhostCar;
    
    private Rigidbody2D rb;

    public float avSpeed;
   
    private void Awake()
    {
        sM = FindObjectOfType<SimulationManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sM.vehicleCount += 1;
    }

    void Update()
    {
        VelocityRegulator();
    }

    private void VelocityRegulator()
    {
        if (isNorthTrail)
        {
            rb.velocity = northVelocity * avSpeed;
        }
        else if (isEastTrail)
        {
            rb.velocity = eastVelocity * avSpeed;
        }
        else if (isWestTrail)
        {
            rb.velocity = westVelocity * avSpeed;
        }
        else if (isSouthTrail)
        {
            rb.velocity = southVelocity * avSpeed;
        }
    }

    private void RouteMaker()
    {
        // prepare direction for North = 1 
        // prepare direction for East  = 2 
        // prepare direction for West  = 3 
        // prepare direction for South = 4 

        if (isNorthTrail)
        {
            int n = Random.Range(0, 2);

            if (n == 0)
            {
                PrepareTrail(1);
            }
            else
            {
                PrepareTrail(2);
            }
        }
        else if (isEastTrail)
        {
            int n = Random.Range(0, 2);

            if (n == 0)
            {
                PrepareTrail(2);
            }
            else
            {
                PrepareTrail(4);
            }
        }
        else if (isWestTrail)
        {
            int n = Random.Range(0, 2);

            if (n == 0)
            {
                PrepareTrail(3);
            }
            else
            {
                PrepareTrail(1);
            }
        }
        else if (isSouthTrail)
        {
            int n = Random.Range(0, 2);

            if (n == 0)
            {
                PrepareTrail(4);
            }
            else
            {
                PrepareTrail(3);
            }
        }
    }

    private void PrepareTrail(int dir)
    {
        if (dir == 1)
        {
            prepareNorth = true;
            prepareEast = false;
            prepareWest = false;
            prepareSouth = false;
        }
        else if (dir == 2)
        {
            prepareNorth = false;
            prepareEast = true;
            prepareWest = false;
            prepareSouth = false;

            
        }
        else if (dir == 3)
        {
            prepareNorth = false;
            prepareEast = false;
            prepareWest = true;
            prepareSouth = false;
        }
        else if (dir == 4)
        {
            prepareNorth = false;
            prepareEast = false;
            prepareWest = false;
            prepareSouth = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EV"))
        {
            EmergencySituation();
        }

        if (other.CompareTag("Exit"))
        {
            Destroy(this.gameObject);
            sM.vehicleCount -= 1;
        }

        if (other.CompareTag("Slowdown"))
        {
            avSpeed = 1f;
        }

        else if (other.CompareTag("Slowdown Intersection"))
        {
            if(slowdownIntersectionEncountered == 0)
            {
                avSpeed = 1f;
                RouteMaker();
                slowdownIntersectionEncountered += 1;
            }
            else
            {
                avSpeed = 2f;
                slowdownIntersectionEncountered = 0;
            }
        }

        else if (other.CompareTag("Fast Lane"))
        {
            avSpeed = 1.25f;
        }
       
        else if(other.CompareTag("Intersection Zone"))
        {
            avSpeed = 0f;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
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

            else if (prepareEast)
            {
                if (other.CompareTag("Ghost Car E") /*&& other.transform.position.y >= transform.position.y && other.transform.position.y - transform.position.y <= 0.1f*/)
                {
                    if (!other.GetComponent<GhostCarBehavior>().occupied && !followingGhostCar)
                    {
                        //avSpeed = 1.5f;
                        RotateToEast();

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
        else if (isSouthTrail)
        {
            if (prepareSouth)
            {
                if (other.CompareTag("Ghost Car S") && transform.position.y >= other.transform.position.y && transform.position.y - other.transform.position.y <= 0.1f)
                {
                    if (!other.GetComponent<GhostCarBehavior>().occupied && !followingGhostCar)
                    {
                        //avSpeed = 1.5f;
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
            else if (prepareWest)
            {
                if (other.CompareTag("Ghost Car W") /*&& transform.position.y >= other.transform.position.y && transform.position.y - other.transform.position.y <= 0.1f*/)
                {
                    if (!other.GetComponent<GhostCarBehavior>().occupied && !followingGhostCar)
                    {
                        //avSpeed = 1.5f;
                        RotateToWest();
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
        else if (isEastTrail)
        {
            if (prepareEast)
            {
                if (other.CompareTag("Ghost Car E") && other.transform.position.x >= transform.position.x && other.transform.position.x - transform.position.x <= 0.1f)
                {
                    if (!other.GetComponent<GhostCarBehavior>().occupied && !followingGhostCar)
                    {
                        //avSpeed = 1.5f;
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
            else if (prepareSouth)
            {
                if (other.CompareTag("Ghost Car S") /*&& other.transform.position.x >= transform.position.x && other.transform.position.x - transform.position.x <= 0.1f*/)
                {
                    if (!other.GetComponent<GhostCarBehavior>().occupied && !followingGhostCar)
                    {
                        //avSpeed = 1.5f;
                        RotateToSouth();
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
        else if (isWestTrail)
        {
            if (prepareWest)
            {
                if (other.CompareTag("Ghost Car W") && transform.position.x >= other.transform.position.x && transform.position.x - other.transform.position.x <= 0.1f)
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
            else if (prepareNorth)
            {
                if (other.CompareTag("Ghost Car N") /*&& transform.position.x >= other.transform.position.x && transform.position.x - other.transform.position.x <= 0.1f*/)
                {
                    if (!other.GetComponent<GhostCarBehavior>().occupied && !followingGhostCar)
                    {
                        RotateToNorth();
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
        if (other.CompareTag("EV"))
        {
            ResumeSituation();
        }

        if (other.CompareTag("Intersection Zone"))
        {
            avSpeed = 1.5f;
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

            else if (prepareEast)
            {
                if (other.CompareTag("Ghost Car E"))
                {
                    if (followingGhostCar)
                    {
                        followingGhostCar = false;
                    }
                }
            }
        }
        else if (isSouthTrail)
        {
            if (prepareSouth)
            {
                if (other.CompareTag("Ghost Car S"))
                {
                    if (followingGhostCar)
                    {
                        followingGhostCar = false;
                    }
                }
            }
            else if (prepareWest)
            {
                if (other.CompareTag("Ghost Car W"))
                {
                    if (followingGhostCar)
                    {
                        followingGhostCar = false;
                    }
                }
            }
        }
        else if (isEastTrail)
        {
            if (prepareEast)
            {
                if (other.CompareTag("Ghost Car E"))
                {
                    if (followingGhostCar)
                    {
                        followingGhostCar = false;
                    }
                }
            }
            else if (prepareSouth)
            {
                if (other.CompareTag("Ghost Car S"))
                {
                    if (followingGhostCar)
                    {
                        followingGhostCar = false;
                    }
                }
            }
        }
        else if (isWestTrail)
        {
            if (prepareWest)
            {
                if (other.CompareTag("Ghost Car W"))
                {
                    if (followingGhostCar)
                    {
                        followingGhostCar = false;
                    }
                }
            }
            else if (prepareNorth)
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

    public void RotateToNorth()
    {
        Vector3 newRotation = new Vector3(0, 0, 0);
        transform.eulerAngles = newRotation;

        isNorthTrail = true;
        isEastTrail = false;
        isWestTrail = false;
        isSouthTrail = false;

        PrepareTrail(1);
    }

    public void RotateToEast()
    {
        Vector3 newRotation = new Vector3(0, 0, -90);
        transform.eulerAngles = newRotation;

        isNorthTrail = false;
        isEastTrail = true;
        isWestTrail = false;
        isSouthTrail = false;

        PrepareTrail(2);
    }

    public void RotateToWest()
    {
        Vector3 newRotation = new Vector3(0, 0, 90);
        transform.eulerAngles = newRotation;

        isNorthTrail = false;
        isEastTrail = false;
        isWestTrail = true;
        isSouthTrail = false;

        PrepareTrail(3);
    }

    public void RotateToSouth()
    {
        Vector3 newRotation = new Vector3(0, 0, 180);
        transform.eulerAngles = newRotation;

        isNorthTrail = false;
        isEastTrail = false;
        isWestTrail = false;
        isSouthTrail = true;

        PrepareTrail(4);
    }

    public void EmergencySituation()
    {
        // move aside and stop
        if (!followingGhostCar && !isEastTrail && !isWestTrail)
        {
            avSpeed = 0f;
            Vector2 emergencyZone = new Vector2(0.75f, transform.position.y);
            transform.position = emergencyZone;
        }
    }

    public void ResumeSituation()
    {
        // move back to position and resume
        if (!followingGhostCar & !isEastTrail && !isWestTrail)
        {
            avSpeed = 1.5f;
            Vector2 resumeZone = new Vector2(0.45f, transform.position.y);
            transform.position = resumeZone;
        }
    }
}
