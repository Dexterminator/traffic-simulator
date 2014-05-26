using UnityEngine;
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
	
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(0, 0, 0);
        Vector3 forward = transform.forward;
//        Vector3 positionXZ = new Vector3(transform.position.x, 0, transform.p);
        Vector3 delta = transform.position - path [currentPathObject].position;
        Debug.Log(delta);
        Debug.Log(delta.magnitude);
        Debug.Log(currentPathObject);
        Vector3 dir = delta.normalized;
        Debug.Log("Transform forward: " + transform.forward);
        Debug.Log("Direction of next path object: " + dir);

        if (delta.magnitude < 3)
            currentPathObject++;

//        if (Input.GetKey(KeyCode.UpArrow))
//        {
//            if (forwardSpeed <= maxForwardSpeed)
//                forwardSpeed += 0.01f;
//        } else
//        {
//            if (forwardSpeed > 0)
//                forwardSpeed -= 0.01f;
//        }
//        if (Input.GetKey(KeyCode.DownArrow))
//        {
//            if (backingSpeed <= maxBackingSpeed)
//                backingSpeed += 0.01f;
//        } else
//        {
//            if (backingSpeed > 0)
//                backingSpeed -= 0.01f;
//        }
//				
        if (Vector3.Angle(transform.forward, dir) > 15.0f)
            transform.RotateAround(transform.position, Vector3.up, 2.0f);
//        if (Input.GetKey(KeyCode.RightArrow))
//            transform.RotateAround(transform.position, Vector3.up, 0.9f);
        forwardSpeed = 0.1f;
		
        movement = forwardSpeed * -transform.forward;
        transform.position += movement;
    }
}
