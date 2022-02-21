using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    public Button emergencyButton;
    public AutonomousVehicleSpawner aVS;

    public void SimulateEmergency()
    {
        aVS.emergencyEvent = true;
    }

    public void GoToHome()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void DisableButton()
    {
        emergencyButton.interactable = false;
    }

    public void ResumeButton()
    {
        emergencyButton.interactable = true;
    }
}
