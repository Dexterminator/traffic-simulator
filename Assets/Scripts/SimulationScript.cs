using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimulationScript : MonoBehaviour {
	public GameObject carPrefab;
	public GameObject path0Object;
	public GameObject path1Object;
	public GameObject path2Object;
	public GameObject path3Object;
	List<Transform> path0;
	List<Transform> path1;
	List<Transform> path2;
	List<Transform> path3;
	GameObject currentCar;
	float timer;
	//List<float> laneTimers;
	float[] laneSpawnTimes;
	float[] laneTimers;
	float spawnTime;

	const float INTENSITY = 1.0f;
	const float SPAWNING_OFFSET = 0.3f;
	const float AVG_SPEED = 0.5f;

	// Use this for initialization
	void Start () {

		//laneTimers = new List<float> (4);
		laneTimers = new float[4];
		laneSpawnTimes = new float[4];
		for (int i = 0; i < laneTimers.Length; i++)
			laneSpawnTimes[i] = ExponentialTime ();
		PathScript pathScript = (PathScript) path0Object.gameObject.GetComponent ("PathScript");
		path0 = pathScript.path;
		pathScript = (PathScript) path1Object.gameObject.GetComponent ("PathScript");
		path1 = pathScript.path;
		pathScript = (PathScript) path2Object.gameObject.GetComponent ("PathScript");
		path2 = pathScript.path;
		pathScript = (PathScript) path3Object.gameObject.GetComponent ("PathScript");
		path3 = pathScript.path;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			CarScript carScript = ((CarScript) currentCar.gameObject.GetComponent("CarScript"));
			carScript.SwitchLanesRight();
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			CarScript carScript = ((CarScript) currentCar.gameObject.GetComponent("CarScript"));
			carScript.SwitchLanesLeft();
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			GenerateCar (1);
		}

		CarPoissonProcess (INTENSITY);
	}

	static float ExponentialTime ()
	{
		return SPAWNING_OFFSET + Mathf.Log (1 - Random.value) / (-INTENSITY);
		//return 0.3f;
	}

	void CarPoissonProcess (float intensity) 
	{
		for (int i = 0; i < laneTimers.Length; i++)
		{
			laneTimers[i] += Time.deltaTime;
			if (laneTimers[i] > laneSpawnTimes[i])
			{	
				if (i == 0)
					Debug.Log ("Generating car at: " + laneTimers[0]);
				GenerateCar (i);
				laneTimers[i] -= laneSpawnTimes[i];
				laneSpawnTimes [i] = ExponentialTime ();

			}
			//if (laneSpawnTimes[0] < 1.0f)
			//	Debug.Log ("i=" + 0 + ":  " + laneSpawnTimes[i]);
		}
	}


	void GenerateCar (int lane)
	{
		List<Transform> path = path0;
		switch (lane)
		{
		case 0:
			path = path0; break;
		case 1:
			path = path1; break;
		case 2:
			path = path2; break;
		case 3:
			path = path3; break;
		default:
			path = path0; break;
		}

		//Debug.Log ("lolfi");
		GameObject carInstance;
		carInstance = Instantiate (carPrefab, path [0].transform.position, carPrefab.transform.rotation) as GameObject;
		((CarScript) carInstance.gameObject.GetComponent("CarScript")).Init (lane, AVG_SPEED);
		currentCar = carInstance;
	}
	//			CarScript carScript = ((CarScript) carInstance.gameObject.GetComponent("CarScript"));
}
