using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; 
    [SerializeField] private Vector3 offset; 

    void Update()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + offset;
        }
    }
}