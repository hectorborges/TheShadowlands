using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    public Camera _mainCamera;
    public GameObject _player;

    public static Camera mainCamera;
    public static GameObject player;

    void Awake()
    {
        mainCamera = _mainCamera;
        player = _player;
    }
}
