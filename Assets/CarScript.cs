﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarScript : MonoBehaviour
{
    public float forwardSpeed;
    public float maxForwardSpeed;
    public float backingSpeed;
    public float maxBackingSpeed;
    public GameObject pathGroup;
    private List<Transform> path;
    int currentPathObject;
	
    // Use this for initialization
    void Start()
    {
        forwardSpeed = 0f;
        maxForwardSpeed = 1.0f;
        backingSpeed = 0f;
        maxBackingSpeed = 0.5f;
        currentPathObject = 1;
        Component[] pathObjects = pathGroup.GetComponentsInChildren<Transform>();
        path = new List<Transform>();
        foreach (Transform pathChild in pathObjects)
        {
            if (pathChild != pathGroup.transform)
                path.Add(pathChild);
        }
    }
	
    void Update()
    {
        Vector3 movement = new Vector3(0, 0, 0);
        Vector3 delta = transform.position - path [currentPathObject].position;
        Debug.Log(delta);
        Debug.Log(delta.magnitude);
        Debug.Log(currentPathObject);
        Debug.Log("Transform forward: " + transform.forward);
        Debug.Log("Direction of next path object: " + delta);

        if (delta.magnitude < 12)
            currentPathObject++;

        float dot = Vector3.Dot(transform.forward, delta);

        if (Vector3.Angle(transform.forward, delta) > 15.0f)
        {
            transform.RotateAround(transform.position, Vector3.up, 2.0f);
        } 

        forwardSpeed = 0.2f;
		
        movement = forwardSpeed * -transform.forward;
        transform.position += movement;
    }
}
