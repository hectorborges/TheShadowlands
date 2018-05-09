using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour
{
    public Canvas enemyCanvas;
    Camera mainCamera;

    void Start()
    {
        mainCamera = ReferenceManager.mainCamera;
        enemyCanvas.worldCamera = mainCamera;
    }

    void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
            mainCamera.transform.rotation * Vector3.up);
    }
}
