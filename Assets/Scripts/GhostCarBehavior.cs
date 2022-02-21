using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCarBehavior : MonoBehaviour
{
    public SimulationManager sM;
    private Rigidbody2D rb;

    public bool isNorth;
    public bool isEast;
    public bool isWest;
    public bool isSouth;

    public int slowdownExitTimes;

    public bool occupied;

    private void Awake()
    {
        sM = FindObjectOfType<SimulationManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isNorth)
        {
            rb.velocity = sM.ghostNorthCarVelocity;
        }
        else if (isEast)
        {
            rb.velocity = sM.ghostEastCarVelocity;
        }
        else if (isWest)
        {
            rb.velocity = sM.ghostWestCarVelocity;
        }
        else if (isSouth)
        {
            rb.velocity = sM.ghostSouthCarVelocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Slowdown Intersection"))
        {
            if (slowdownExitTimes < 1)
            {
                slowdownExitTimes += 1;
            }

            else
            {
                Destroy(gameObject);
            }
        }

    }
}
