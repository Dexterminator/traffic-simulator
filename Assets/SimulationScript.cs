using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimulationScript : MonoBehaviour {
	public GameObject car;
	public GameObject path1Object;
	public List<Transform> path1;

	// Use this for initialization
	void Start () {
		PathScript pathScript = (PathScript) path1Object.gameObject.GetComponent ("PathScript");
		path1 = pathScript.path;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
