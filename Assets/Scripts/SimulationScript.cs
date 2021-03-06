﻿using UnityEngine;
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

	float SPEED = 0.5f;

	readonly float[] INTENSITY = new float[4] {0f, 0f, 0f, 7f};
	readonly float[] SPAWNING_OFFSET = new float[4] {0.5f, 0.5f, 0.5f, 0.3f};
	readonly float[] AVG_SPEED = new float[4] {0.5f, 0.5f, 0.5f, 0.4f};

	float NORM_DEV = 0.06f;

	// Use this for initialization
	void Start () {


		//laneTimers = new List<float> (4);
		laneTimers = new float[4];
		laneSpawnTimes = new float[4];
		for (int i = 0; i < laneTimers.Length; i++)
			laneSpawnTimes[i] = ExponentialTime (INTENSITY[i]);
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
			Vector3 accidentPosition = new Vector3(-56,0.6393032f,198);
			GameObject carInstance = Instantiate (carPrefab, accidentPosition, carPrefab.transform.rotation) as GameObject;
			((CarScript) carInstance.gameObject.GetComponent("CarScript")).Init (0, 0, true);
		}

		CarPoissonProcess ();
	}

	static float ExponentialTime (float intensity)
	{
		return Mathf.Log (1 - Random.value) / (-intensity);
		//return SPAWNING_OFFSET;
	}

	void CarPoissonProcess () 
	{
		for (int i = 0; i < laneTimers.Length; i++)
		{
			if (AVG_SPEED[i] > 0) {
				laneTimers[i] += Time.deltaTime;
				if (laneTimers[i] > laneSpawnTimes[i])
				{	
					GenerateCar (i);
					laneTimers[i] -= laneSpawnTimes[i];
					laneSpawnTimes [i] = SPAWNING_OFFSET[i] + ExponentialTime (INTENSITY[i]);
				}
				
			}
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
		//GameObject carInstance;
		GameObject carInstance = Instantiate (carPrefab, path [0].transform.position, carPrefab.transform.rotation) as GameObject;
		((CarScript) carInstance.gameObject.GetComponent("CarScript")).Init (lane, NormDist(AVG_SPEED[lane], NORM_DEV), false);
		currentCar = carInstance;
	}
	//			CarScript carScript = ((CarScript) carInstance.gameObject.GetComponent("CarScript"));

	float NormDist(float mean, float stdDev)
	{
		Random rand = new Random();
		float u1 = Random.value;
		float u2 = Random.value;
		float randStdNormal = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Sin(2f * Mathf.PI * u2);
		float randNormal = mean + stdDev * randStdNormal;
		return randNormal;
	}
}
