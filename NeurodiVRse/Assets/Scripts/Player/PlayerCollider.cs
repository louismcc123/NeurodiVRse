using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public Transform camera;

    void Update()
    {
        if (camera != null)
        {
            transform.position = camera.position;
        }
    }
}
