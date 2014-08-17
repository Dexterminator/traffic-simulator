using UnityEngine;
using System.Collections;

public class ZoneScript : MonoBehaviour {
	public bool safe;
	private int carsInside;

	// Use this for initialization
	void Start () {
		safe = true;
		carsInside = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (carsInside);
	}

	void OnTriggerEnter(Collider other) {
		carsInside++;
		safe = carsInside == 0 ? true : false;
	}

	void OnTriggerExit(Collider other) {
		carsInside--;
		safe = carsInside == 0 ? true : false;
	}
}
