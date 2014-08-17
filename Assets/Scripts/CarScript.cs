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
	float accidentSpeed;
	float forwardSpeed;
	public bool broken;

	float CONSTANT_ACCELERATION = 0.01f;
	float PERCENTUAL_ACCELERATION = 0.02f;
	float NOTICE_DISTANCE = 15f;

	float distance;
	float oldDistance;
	ZoneScript leftZone;
	bool leftSafe;
	ZoneScript rightZone;
	bool rightSafe;
	ZoneScript forwardZone;
	bool forwardSafe;
	AccidentZoneScript accidentZone;
	bool noAccidentClose;
	
    // Use this for initialization
    void Start()
    {
		//broken = false;
		leftZone = (ZoneScript) (transform.Find ("LeftZone").GetComponent("ZoneScript"));
		leftSafe = true;
		rightZone = (ZoneScript) (transform.Find ("RightZone").GetComponent("ZoneScript"));
		rightSafe = true;

		accidentZone = (AccidentZoneScript) (transform.Find ("AccidentZone").GetComponent("AccidentZoneScript"));
		noAccidentClose = true;

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

	public void Init(int lane, float speed, bool b)
	{
		broken = b;
		if (broken)
			Break();
		currentLaneIndex = lane;
		optimalSpeed = speed;
		forwardSpeed = optimalSpeed;
		accidentSpeed = optimalSpeed/2;
	}

    void Update()
    {
		//float lockPos = -180;
		transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
		leftSafe = leftZone.safe;
		rightSafe = rightZone.safe;
		noAccidentClose = accidentZone.safe;
		List<Transform> lane = lanes [currentLaneIndex];

        CastRays();

		float curOpSpeed;
		if(noAccidentClose) {
			curOpSpeed = optimalSpeed;
		} else {
			curOpSpeed = accidentSpeed;
		}

		if (oldDistance > distance || forwardSpeed > curOpSpeed) {
			forwardSpeed = forwardSpeed*(1.0f - PERCENTUAL_ACCELERATION) - CONSTANT_ACCELERATION;
		} else if (forwardSpeed < curOpSpeed) {
			forwardSpeed = forwardSpeed*(1.0f + PERCENTUAL_ACCELERATION) + CONSTANT_ACCELERATION;
		}

		if (forwardSpeed < 0) {
			forwardSpeed = 0;
		}

		oldDistance = distance;


		Debug.DrawLine (transform.position, currentNode);
		Vector3 delta = transform.position;
		if (!broken) {
			delta = MoveToPosition (currentNode);
		};

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
				transform.forward = -(currentNode - transform.position).normalized;
			}
			break;
		case State.SwitchLanesLeft:
			if (delta.magnitude < 3) {
				currentLaneIndex--;
				currentNode = lanes [currentLaneIndex] [currentNodeIndex].position;
				currentState = State.Normal;
				transform.forward = -(currentNode - transform.position).normalized;
			}
			break;
		default:
			Debug.Log ("Lol, no state wat");
			break;
		}



    }

	void Break() {
		foreach (Transform child in transform) {
			child.gameObject.SetActive(true);
		}
	}

	Vector3 MoveToPosition (Vector3 position)
	{
		Vector3 delta = transform.position - position;
		Vector3 movement = new Vector3(0, 0, 0);
		Vector3 xzdir = transform.forward;
		xzdir.y = 0;
		float dot = Vector3.Dot (xzdir, delta);
		if (Vector3.Angle (-xzdir, delta) > 2.0f) {
			float rotationSpeed = 2.0f;
			if (currentState == State.Normal) {
				rotationSpeed = 1.0f;
			}
			if (AngleDir (-xzdir, delta, Vector3.up) == 1)
				transform.RotateAround (transform.position, Vector3.up, -rotationSpeed);
			else
				transform.RotateAround (transform.position, Vector3.up, rotationSpeed);
		}

		movement = forwardSpeed * -xzdir;
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
			if (leftSafe && currentState == State.Normal && currentLaneIndex > 0) {
				SwitchLanesLeft();
			}
        } else {
			distance = float.MaxValue;
		}

		if (rightSafe && currentState == State.Normal && currentLaneIndex < lanes.Count - 1) {
			SwitchLanesRight();
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
		Vector3 t = lanes [currentLaneIndex] [currentNodeIndex].position - lanes [currentLaneIndex] [currentNodeIndex-1].position;
		Vector3 s = t.normalized * 20;
		Vector3 k = s + d;
		currentNode = transform.position + k;
	}

	public void SwitchLanesLeft() {
		currentState = State.SwitchLanesLeft;
		Vector3 a = lanes [currentLaneIndex] [currentNodeIndex].position;
		Vector3 b = lanes [currentLaneIndex - 1] [currentNodeIndex].position;
		Vector3 d = b - a;
		Vector3 t = lanes [currentLaneIndex] [currentNodeIndex].position - lanes [currentLaneIndex] [currentNodeIndex-1].position;
		Vector3 s = t.normalized * 20;
		Vector3 k = s + d;
		currentNode = transform.position + k;
	}

}
