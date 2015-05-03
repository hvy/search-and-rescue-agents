using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseStation : MonoBehaviour {

	public GridEnvironment gridEnvironment;
	public int height, width;
    public List<Vector2> entrances;

	public BaseStation (List<Vector2> entrances, int height, int width) {
    	this.entrances = entrances;
    	this.height = height;
    	this.width = width;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
