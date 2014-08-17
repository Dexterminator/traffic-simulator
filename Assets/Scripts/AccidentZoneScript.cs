using UnityEngine;
using System.Collections;

public class AccidentZoneScript : MonoBehaviour {
	public bool safe;
	private int accidentsInside;
	
	// Use this for initialization
	void Start () {
		safe = true;
		accidentsInside = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (carsInside);
	}
	
	void OnTriggerEnter(Collider other) {
		CarScript car = (CarScript) other.transform.GetComponent("CarScript");
		if(car.broken) {
			accidentsInside++;
			safe = accidentsInside == 0 ? true : false;
		}
	}
	
	void OnTriggerExit(Collider other) {
		CarScript car = (CarScript) other.transform.GetComponent("CarScript");
		if(car.broken) {
			accidentsInside--;
			safe = accidentsInside == 0 ? true : false;
		}
	}
}
