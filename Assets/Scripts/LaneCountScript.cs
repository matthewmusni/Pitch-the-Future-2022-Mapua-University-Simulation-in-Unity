using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneCountScript : MonoBehaviour
{
    public AutonomousVehicleSpawner aVS;
    public int aVCount;

    // Update is called once per frame
    void Update()
    {
        CheckCount();
    }

    private void CheckCount()
    {
        if (aVCount >= 4)
        {
            aVS.limitReached = true;
        }
        else
        {
            aVS.limitReached = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AV"))
        {
            aVCount += 1;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("AV"))
        {
            aVCount -= 1;
        }
    }
}
