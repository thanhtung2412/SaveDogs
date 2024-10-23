using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviour
{
    public float side;
    void Start()
    {    
        transform.position = new Vector3(side * (Camera.main.orthographicSize * Camera.main.aspect + 0.5f), 0.0F, transform.position.z);
    }
}
