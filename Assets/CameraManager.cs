using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
        //_camera.Follow = GameManager.Instance.SpawnPlayer().transform;
        //_camera.LookAt = GameManager.Instance.SpawnPlayer().transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
