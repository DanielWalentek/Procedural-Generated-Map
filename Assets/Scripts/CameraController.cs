using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    GameObject cameraM;
    [SerializeField]
    GridCreator gridCreator;
    
    public void Camera()
    {
        cameraM.transform.position = new Vector3(gridCreator.cellArray.GetLength(0) / 2, 20, -5);
    }
}
