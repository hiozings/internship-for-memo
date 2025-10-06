using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public Vector3 endPos;
    public Vector3 startPos;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.left * Time.fixedDeltaTime * 0.5f);
        if (transform.position.x <= endPos.x)
        {
            transform.position = startPos;
        }
    }
}
