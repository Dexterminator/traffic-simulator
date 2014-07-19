using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarScript : MonoBehaviour
{
    public List<GameObject> laneGroups;
	public List<List<Transform>> lanes;
    int currentNodeIndex;
	int currentLaneIndex;
	float laneChangeSpeed;
	enum State {Normal, SwitchLanesRight, SwitchLanesLeft};
	private State currentState;
	Vector3 currentNode;
	float optimalSpeed;
	float forwardSpeed;

	float CONSTANT_ACCELERATION = 0.005f;
	float PERCENTUAL_ACCELERATION = 0.1f;
	float NOTICE_DISTANCE = 15f;

	float distance;
	float oldDistance;
	
    // Use this for initialization
    void Start()
    {
		distance = float.MaxValue; 
		oldDistance = float.MaxValue;
		laneGroups = new List<GameObject> ();
		lanes = new List<List<Transform>> ();
		laneChangeSpeed = 10f;
        currentNodeIndex = 1; //Because of reasons
		currentState = State.Normal;
		laneGroups.Add(GameObject.Find("Lane0"));
		laneGroups.Add(GameObject.Find("Lane1"));
		laneGroups.Add(GameObject.Find("Lane2"));
		laneGroups.Add(GameObject.Find("Lane3"));
		foreach (GameObject laneGroup in laneGroups) {
			Component[] laneObjects = laneGroup.GetComponentsInChildren<Transform> ();
			List<Transform> lane = new List<Transform> ();
			foreach (Transform node in laneObjects) {
				if (node != laneGroup.transform)
					lane.Add (node);
			}
			lanes.Add (lane);
		}
		currentNode = lanes [currentLaneIndex] [currentNodeIndex].position;
    }

	public void Init(int lane, float speed)
	{
		currentLaneIndex = lane;
		optimalSpeed = speed;
		forwardSpeed = optimalSpeed;
	}

    void Update()
    {
		List<Transform> lane = lanes [currentLaneIndex];

        CastRays();

		if (oldDistance > distance) {
			forwardSpeed = forwardSpeed*(1.0f - PERCENTUAL_ACCELERATION) - CONSTANT_ACCELERATION;
		} else if (forwardSpeed < optimalSpeed) {
			forwardSpeed = forwardSpeed*(1.0f + PERCENTUAL_ACCELERATION) + CONSTANT_ACCELERATION;
		}
		oldDistance = distance;

		Debug.DrawLine (transform.position, currentNode);
		Vector3 delta = MoveToPosition (currentNode);

		switch (currentState)
		{
		case State.Normal:
			if (delta.magnitude < 5) {
				currentNodeIndex++;
				if (currentNodeIndex == lane.Count)
					Destroy (gameObject);
				else
					currentNode = lane [currentNodeIndex].position;
			}
			break;
		case State.SwitchLanesRight:
			if (delta.magnitude < 3) {
				currentLaneIndex++;
				currentNode = lanes [currentLaneIndex] [currentNodeIndex].position;
				currentState = State.Normal;
			}
			break;
		case State.SwitchLanesLeft:
			if (delta.magnitude < 3) {
				currentLaneIndex--;
				currentNode = lanes [currentLaneIndex] [currentNodeIndex].position;
				currentState = State.Normal;
			}
			break;
		default:
			Debug.Log ("Lol, no state wat");
			break;
		}



    }

	Vector3 MoveToPosition (Vector3 position)
	{
		Vector3 delta = transform.position - position;
		Vector3 movement = new Vector3(0, 0, 0);
		float dot = Vector3.Dot (transform.forward, delta);
		if (Vector3.Angle (-transform.forward, delta) > 2.0f) {
			if (AngleDir (-transform.forward, delta, Vector3.up) == 1)
				transform.RotateAround (transform.position, Vector3.up, -1.0f);
			else
				transform.RotateAround (transform.position, Vector3.up, 1.0f);
		}

		movement = forwardSpeed * -transform.forward;
		transform.position += movement;
		return delta;
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
        if (Physics.Raycast(transform.position + heightBoost, -transform.forward, out hit, NOTICE_DISTANCE))
        {
            //Debug.Log("Hit something in front of car");
            Debug.DrawLine(transform.position, hit.point, Color.red);
			distance = hit.distance;
        } else {
			distance = float.MaxValue;
		}

        /*if (Physics.Raycast(transform.position, transform.right, out hit, 100.0f))
        {
            //Debug.Log("Hit something to the left of the car");
            Debug.DrawLine(transform.position, hit.point, Color.red);
        }

        if (Physics.Raycast(transform.position, -transform.right, out hit, 100.0f))
        {
            //Debug.Log("Hit something to the right of the car");
            Debug.DrawLine(transform.position, hit.point, Color.blue);
        }

        if (Physics.Raycast(transform.position + heightBoost, transform.forward, out hit, 100.0f))
        {
            //Debug.Log("Hit something behind the car");
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
        }*/

    }

	public void SwitchLanesRight() {
		currentState = State.SwitchLanesRight;
		Vector3 a = lanes [currentLaneIndex] [currentNodeIndex].position;
		Vector3 b = lanes [currentLaneIndex + 1] [currentNodeIndex].position;
		Vector3 d = b - a;
		Vector3 s = -transform.forward * 20;
		Vector3 k = s + d;
		currentNode = transform.position + k;
	}

	public void SwitchLanesLeft() {
		currentState = State.SwitchLanesLeft;
		Vector3 a = lanes [currentLaneIndex] [currentNodeIndex].position;
		Vector3 b = lanes [currentLaneIndex - 1] [currentNodeIndex].position;
		Vector3 d = b - a;
		Vector3 s = -transform.forward * 20;
		Vector3 k = s + d;
		currentNode = transform.position + k;
	}

}
