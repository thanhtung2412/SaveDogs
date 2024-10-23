using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPath2 : MonoBehaviour
{
    public bool isPermanent;

    private List<Vector2> newVerticies = new List<Vector2>();

    public float destroyCounter;

    private bool canDestroy;

    private Vector2 centerOfMass = Vector2.zero;

    private DrawingManager managerScript;


    private void Start()
    {
        managerScript = GameObject.Find("DrawingManager").GetComponent<DrawingManager>();
        newVerticies = managerScript.newVerticies;
        destroyCounter = managerScript.lifeTime;
        isPermanent = managerScript.isPermanent;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && name.Equals("Drawing"))
        {
            foreach (Vector2 current in newVerticies)
            {
                centerOfMass += current;
            }
            centerOfMass /= (float)newVerticies.Count;
            canDestroy = true;
        }
        if (destroyCounter > 0f && !isPermanent && canDestroy)
        {
            destroyCounter -= Time.deltaTime;
            if (destroyCounter <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
