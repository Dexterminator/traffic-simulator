using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimulationScript : MonoBehaviour {
	public GameObject carPrefab;
	public GameObject path1Object;
	List<Transform> path1;

	// Use this for initialization
	void Start () {
		PathScript pathScript = (PathScript) path1Object.gameObject.GetComponent ("PathScript");
		path1 = pathScript.path;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.Log("lolfi");
			GameObject carInstance;
			carInstance = Instantiate(carPrefab, path1[0].transform.position, carPrefab.transform.rotation) as GameObject;
			((CarScript) carInstance.gameObject.GetComponent("CarScript")).pathGroup = path1Object; 
		}

	}
}
