using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Initiates the search and rescue simulator by creating the environment.
 * It then waits for the user input to create the rescue agents (mouse clicks)
 * and then to start the simulation (space bar).
 */
public class Main : MonoBehaviour {

	public float tileSize; // Set in inspector
	public int height, width; // Set in inspector

	private BaseStation baseStation;
	
	void Start () {
		Debug.Log ("Starting main...");

		// Create the environment with the obstacles and the humans
		Environment env = EnvironmentFactory.createBasicEnvironment ();
		GridEnvironment gridEnv = new GridEnvironment(height, width, tileSize);

		// TODO Hardcoded at the moment. Make the agents find the entrances instead;
		baseStation = (BaseStation) GameObject.Find ("BaseStation").GetComponent(typeof(BaseStation));
		baseStation.entrances = env.entrances; // TODO Make sure entrances can be found by the agents
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
