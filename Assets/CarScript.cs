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
	
    void Update()
    {
        CastRays();
        Vector3 movement = new Vector3(0, 0, 0);
        Vector3 delta = transform.position - path [currentPathObject].position;

        if (delta.magnitude < 5)
        {
            currentPathObject++;
        } 

        if (currentPathObject > path.Count - 1)
        {
            currentPathObject = 0;
        }

        float dot = Vector3.Dot(transform.forward, delta);

        if (Vector3.Angle(-transform.forward, delta) > 2.0f)
        {
            if (AngleDir(-transform.forward, delta, Vector3.up) == 1)
                transform.RotateAround(transform.position, Vector3.up, -3.0f);
            else
                transform.RotateAround(transform.position, Vector3.up, 3.0f);
        } 

        forwardSpeed = 0.2f;
		
        movement = forwardSpeed * -transform.forward;
        transform.position += movement;
    }

    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);
        
        if (dir > 0f)
        {
            return 1f;
        } else if (dir < 0f)
        {
            return -1f;
        } else
        {
            return 0f;
        }
    }

    void CastRays()
    {
        RaycastHit hit;
        Vector3 heightBoost = new Vector3(0, 0.7f, 0);
        if (Physics.Raycast(transform.position + heightBoost, -transform.forward, out hit, 100.0f))
        {
            Debug.Log("Hit something in front of car");
            Debug.DrawLine(transform.position, hit.point, Color.white);
        }

        if (Physics.Raycast(transform.position, transform.right, out hit, 100.0f))
        {
            Debug.Log("Hit something to the left of the car");
            Debug.DrawLine(transform.position, hit.point, Color.red);
        }

        if (Physics.Raycast(transform.position, -transform.right, out hit, 100.0f))
        {
            Debug.Log("Hit something to the right of the car");
            Debug.DrawLine(transform.position, hit.point, Color.blue);
        }

        if (Physics.Raycast(transform.position + heightBoost, transform.forward, out hit, 100.0f))
        {
            Debug.Log("Hit something behind the car");
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
        }

    }

}
