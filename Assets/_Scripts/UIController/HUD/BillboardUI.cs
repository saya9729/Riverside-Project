using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + _cam.transform.forward);
    }
}
