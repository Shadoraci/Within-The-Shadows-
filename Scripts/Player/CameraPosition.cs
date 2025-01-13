using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Transform CameraPositionTrans;
    // Update is called once per frame
    void Update()
    {
        transform.position = CameraPositionTrans.position;
    }
}
