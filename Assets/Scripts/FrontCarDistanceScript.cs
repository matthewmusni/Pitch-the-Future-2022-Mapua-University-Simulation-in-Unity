using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCarDistanceScript : MonoBehaviour
{
    private GameObject parent;

    private void Awake()
    {
        parent = transform.parent.gameObject;
    }

    private void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("AV") || other.CompareTag("EV"))
        {
            if (other.gameObject != parent)
            {
                this.transform.parent.GetComponent<AutonomousVehicleBehavior>().avSpeed = 0f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("AV") || other.CompareTag("EV"))
        {
            this.transform.parent.GetComponent<AutonomousVehicleBehavior>().avSpeed = 1.5f;
        }
    }
}
