using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public string targetTagName;
    private void Awake()
    {      
        if (string.IsNullOrEmpty(targetTagName))
        {
            targetTagName = "Character";
        }
    }
}
