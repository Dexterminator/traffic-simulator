using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimulationScript : MonoBehaviour {
	public GameObject carPrefab;
	public GameObject path1Object;
	List<Transform> path1;
	GameObject currentCar;

	// Use this for initialization
	void Start () {
		PathScript pathScript = (PathScript) path1Object.gameObject.GetComponent ("PathScript");
		path1 = pathScript.path;
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
			Debug.Log("lolfi");
			GameObject carInstance;
			carInstance = Instantiate(carPrefab, path1[0].transform.position, carPrefab.transform.rotation) as GameObject;
			currentCar = carInstance;
//			CarScript carScript = ((CarScript) carInstance.gameObject.GetComponent("CarScript"));
		}
	}
}
