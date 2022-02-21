using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private float zoomStep, minCamSize, maxCamsize;

    private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    private Vector3 dragOrigin;

    public float ghostCarDistance = 3.5f;

    public float ghostCarVelocity;

    public Vector2 ghostNorthCarVelocity;
    public Vector2 ghostEastCarVelocity;
    public Vector2 ghostWestCarVelocity;
    public Vector2 ghostSouthCarVelocity;

    public Transform northSpawner;
    public Transform eastSpawner;

    public bool stopSpawn;

    public int vehicleCount;
    public Text aVCountText;

    // Start is called before the first frame update

    public void Awake()
    {
        mapMinX = -7.5f;
        mapMaxX = 37.5f;
        mapMinY = -5f;
        mapMaxY = 25f;

        ghostNorthCarVelocity = new Vector2(0f,ghostCarVelocity);
        ghostEastCarVelocity = new Vector2(ghostCarVelocity,0f);
        ghostWestCarVelocity = new Vector2(-ghostCarVelocity,0f);
        ghostSouthCarVelocity = new Vector2(0f, -ghostCarVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        aVCountText.text = vehicleCount.ToString();

        PanCamera();
    }

    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            cam.transform.position = ClampCamera(cam.transform.position + difference);
        }
    }

    public void ZoomIn()
    {
        float newSize = cam.orthographicSize - zoomStep;

        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamsize);
        cam.transform.position = ClampCamera(cam.transform.position);
    }

    public void ZoomOut()
    {
        float newSize = cam.orthographicSize + zoomStep;

        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamsize);

        cam.transform.position = ClampCamera(cam.transform.position);
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
}
