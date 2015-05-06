using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Initiates the search and rescue simulator by creating the environment.
 * It then waits for the user input to create the rescue agents (mouse clicks)
 * and then to start the simulation (space bar).
 */
public class Main : MonoBehaviour {

	public Vector2 environmentPosition;
	public float tileSize; // Set in inspector
	public int height, width; // Set in inspector

	private BaseStation baseStation;
	
	void Start () {

		Debug.Log ("Starting main...");

		//Environment env = EnvironmentFactory.createBasicEnvironment ();

		// TODO Hardcoded entrances
		List<Vector2> entrances = new List<Vector2> ();
		entrances.Add (new Vector2(0, 10));
		entrances.Add (new Vector2(1, 10));
		entrances.Add (new Vector2(-2, -10));
		entrances.Add (new Vector2(-1, -10));

		baseStation = (BaseStation) GameObject.Find ("BaseStation").GetComponent(typeof(BaseStation));

		baseStation.entrances = entrances; // TODO Make sure entrances can be found by the agents

		baseStation.setEnvironmentPos (environmentPosition);

		GridEnvironment gridEnv = new GridEnvironment(height, width, tileSize);

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
