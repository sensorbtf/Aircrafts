using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform _baseTransform;
    public Transform UnitTransform;

    private void Start()
    {
        transform.position = _baseTransform.position + offset;
    }

    void Update()
    {
        if (UnitTransform != null)
        {
            transform.position = UnitTransform.position + offset;
        }
        else
        {
            transform.position = _baseTransform.position + offset;
        }
    }
}