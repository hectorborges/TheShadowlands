using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpgCamera : MonoBehaviour
{
    public static RpgCamera instance;
    public float horizontalSpeed;
    public float verticalSpeed;
    public Vector3 cameraOffset;
    public Transform cameraHolder;
    public float minimumClampValue;
    public float maximumClampValue;

    float yRotationValue;
    float y2RotationValue;
    float xRotationValue;
    Quaternion cameraRotation;

    Vector3 velocity;
    Transform player;
    Camera cam;
    Vector3 offset;
    float speed;

    bool initialized;

    private void Awake()
    {
        instance = this;
    }

    public void Initialize(GameObject _player)
    {
        cam = Camera.main;
        player =_player.transform;
        offset = new Vector3(player.position.x + cameraOffset.x, player.position.y + cameraOffset.y, player.position.z + cameraOffset.z);
        initialized = true;
    }

    private void LateUpdate()
    {
        if (!initialized) return;
        Look();

        if (Input.GetKey(KeyCode.Mouse2))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            speed = horizontalSpeed;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            speed = 0;
        }
    }

    public void Look()
    {
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * speed, Vector3.up) * offset;
        transform.position = player.position + offset;
        transform.LookAt(player.position);

        //yRotationValue -= Input.GetAxis("Mouse Y") * verticalSpeed * Time.deltaTime;
        //yRotationValue = ClampAngle(yRotationValue, minimumClampValue, maximumClampValue);
        //Quaternion rotation = Quaternion.Euler(yRotationValue, 0, 90);
        //pivot.transform.localRotation = rotation;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360.0f)
            angle += 360.0f;
        if (angle > 360.0f)
            angle -= 360.0f;
        return Mathf.Clamp(angle, min, max);
    }
}
