using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
public class CameraControl : MonoBehaviour
{
    [Header("事件监听")]
    public VoidEventSO afterSceneLoadedEvent;

    private CinemachineConfiner2D confiner2D;

    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
        
    }
    private void OnEnable()
    {
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoaded;
    }

    
    private void OnDisable()
    {
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoaded;
    }

    private void OnAfterSceneLoaded()
    {
        GetNewCameraBounds();
    }

    //private void Start()
    //{
        
    //    GetNewCameraBounds();

    //}

    private void GetNewCameraBounds()
    {
        //Debug.Log("get");
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
        {
            return;
        }
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache();
    }

    

}
