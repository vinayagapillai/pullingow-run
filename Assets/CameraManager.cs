using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [HideInInspector]
    public CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        GameObject vCamObject = GameObject.FindWithTag("VirtualCamera");

        _virtualCamera = vCamObject.GetComponent<CinemachineVirtualCamera>();
    }

}
