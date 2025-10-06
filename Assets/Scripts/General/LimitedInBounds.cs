using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedInBounds : MonoBehaviour
{
    [Header("事件监听")]
    public VoidEventSO afterSceneLoadedEvent;

    public Collider2D bounds;

    public bool isLimited = true;
    public float leftBoundary;
    public float rightBoundary;
    public float topBoundary;
    public float bottomBoundary;
    public float Offset;

    //private void Start()
    //{
    //    GetNewBounds();
    //}

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
        GetNewBounds();
    }

    private void Update()
    {
        if(isLimited && bounds != null)
        {
            Vector3 pos = transform.position;
        leftBoundary = bounds.bounds.min.x;
        rightBoundary = bounds.bounds.max.x;
        bottomBoundary = bounds.bounds.min.y;
        topBoundary = bounds.bounds.max.y;
        if (pos.x > rightBoundary)
            pos.x = leftBoundary;
        else if (pos.x < leftBoundary)
            pos.x = rightBoundary;

        if (pos.y > topBoundary)
            pos.y = bottomBoundary;
        else if (pos.y < bottomBoundary)
            pos.y = topBoundary + Offset;

            transform.position = pos;
        }
    }

    private void GetNewBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if(obj == null)
        {
            return;
        }
        bounds = obj.GetComponent<Collider2D>();
    }
}
