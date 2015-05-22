using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Initiates the search and rescue simulator by creating the environment.
 * It then waits for the user input to create the rescue agents (mouse clicks)
 * and then to start the simulation (space bar).
 */
public class MainOpenLevel : MonoBehaviour {
	
	public Vector2 environmentPosition;
	
	private int height, width;
	private BaseStation baseStation;
	
	void Start () {
		
		Debug.Log ("[INFO] Starting main...");
		
		// Find out the dimensions of the environment
		GameObject envBase = GameObject.Find ("EnvironmentBase");
		width = (int) envBase.transform.localScale.x;
		height = (int) envBase.transform.localScale.y;
		
		Debug.Log ("[INFO] Found environment");
		Debug.Log ("[INFO] Width: " + width);
		Debug.Log ("[INFO] Height: " + height);
		
		List<Vector2> entrances = new List<Vector2> ();
		entrances.Add (new Vector2(48.0f, 0.3f));
		entrances.Add (new Vector2(49.0f, 0.3f));
		entrances.Add (new Vector2(50.0f, 0.3f));
		entrances.Add (new Vector2(51.0f, 0.3f));
		
		// Find the base station game object and add the entrance information and the grid representation of the environment
		baseStation = (BaseStation) GameObject.Find ("BaseStation").GetComponent(typeof(BaseStation));
		baseStation.entrances = entrances; // TODO Make sure entrances can be found by the agents
		baseStation.setEnvironmentPos (environmentPosition);
		GridEnvironment gridEnv = new GridEnvironment(height, width);
		baseStation.setGridEnvironment (gridEnv);
	}
	
	void Update () {
		// On mouse click, create a rescue agent
		if (Input.GetButtonDown ("Fire1")) {
			Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			clickPos.z = 0;
			
			// Create the agent and add a reference to the base station
			Agent agent = AgentFactory.spawnAgentAt (clickPos);
			agent.setBase(baseStation);
			
			// Register the agent in the base station
			baseStation.addAgent(agent);
		}
	}
}
