using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Initiates the search and rescue simulator by creating the environment.
 * It then waits for the user input to create the rescue agents (mouse clicks)
 * and then to start the simulation (space bar).
 */
public class Main : MonoBehaviour {

	private int numAgents;
	private Environment env = null;
	private BaseStation baseStation;
	private List<Agent> agents;

	
	void Start () {
		Debug.Log ("Starting main...");

		// Create the environment with the obstacles and the humans
		env = EnvironmentFactory.createBasicEnvironment ();
		agents = new List<Agent>();
		baseStation = new BaseStation(env.entrances, env.height, env.width);
	}

	void Update () {
		if (env != null) {
			// On mouse click, create a rescue agent
			if (Input.GetButtonDown ("Fire1")) {
				Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

				Debug.Log ("Spawning rescue agent at " + clickPos);

				clickPos.z = 0;

				Agent agent = AgentFactory.spawnAgentAt (clickPos);
				agent.setBase(baseStation);
				agents.Add(agent);

			}
			
			// On Space bar click, start the rescue simulation
			if (Input.GetKeyDown (KeyCode.Space)) {
				Debug.Log("Starting simulation...");
			}
		}
	}
}
