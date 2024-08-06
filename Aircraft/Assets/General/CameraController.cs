using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform _baseTransform;
    public Transform PlayerTransform;

    private void Start()
    {
        transform.position = _baseTransform.position + offset;
    }

    void Update()
    {
        if (PlayerTransform != null)
        {
            transform.position = PlayerTransform.position + offset;
        }
    }
}