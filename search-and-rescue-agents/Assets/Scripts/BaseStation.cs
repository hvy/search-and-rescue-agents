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
    	gridEnvironment = new GridEnvironment(height, width, 0.1f);

    	Transform prefab = Resources.Load("Prefabs/Base", typeof(Transform)) as Transform;
    	GameObject.Instantiate (prefab, new Vector3(0,0,0), Quaternion.LookRotation (Vector3.up));
	}

	// Use this for initialization
	void Start () {
	Debug.Log("START");

	}

	// Update is called once per frame
	void Update () {

	}

	public void uploadTargetLocation(Vector2 loc) {
        gridEnvironment.addHuman(loc);
	}

	// TODO how is this supposed to work? Should we maybe assume everything not identified is ground?
	public void uploadGroundLocation(Vector2 loc) {
		gridEnvironment.addGround(loc);
	}

	public void uploadObstacleLocation(Vector2 loc) {
		gridEnvironment.addObstacle(loc);
	}

}
