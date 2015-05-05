using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseStation : MonoBehaviour {

	public int height, width;
	public List<Vector2> entrances;

	public List<Agent> agents;
	public List<Human> unrescuedHumans;

	public GridEnvironment gridEnvironment;
	
	void Start () {
		agents = new List<Agent> ();
		unrescuedHumans = new List<Human> ();
		gridEnvironment = new GridEnvironment(height, width, 0.1f);
	}

	/**
	 * Decides which agent should go and rescue a certain human that is already
	 * found by an agent
	 */
	void Update () {

		List<Human> isBeingRescued = new List<Human> ();
		foreach (Agent agent in agents) {
			if (!agent.hasTarget()) {
				continue;
			}

			Human target = agent.getCurrentTarget();
			if (!isBeingRescued.Contains(target)) {
				isBeingRescued.Add(target);
			}
		}

		foreach (Agent agent in agents) {
			
			// TODO At the moment we let the agent pick up the current target if it has one,
			// in the future, we might have action that overrides the current target if another 
			// task with a higher priority is found.
			if (agent.hasTarget()) {
				continue;
			}

			foreach (Human unrescuedHuman in unrescuedHumans) {
				if (!isBeingRescued.Contains(unrescuedHuman)) {
					Debug.Log ("Assignning new target human from base station");
					agent.assignTarget(unrescuedHuman);
					isBeingRescued.Add(unrescuedHuman);
				}
			}
		}
	}

	public void uploadSavedTaret(Human human) {

		if (!unrescuedHumans.Contains (human)) {
			Debug.Log ("ERROR: Rescued unregistered human");
			return;
		}

		unrescuedHumans.Remove (human);
	}

	public void uploadTargetLocation(Human human) {
		if (!unrescuedHumans.Contains (human)) {
			Debug.Log ("Found human at " + (Vector2) human.transform.position);
			unrescuedHumans.Add (human);
		}

		Vector2 position = (Vector2) human.transform.position; // Cast to Vector2 from Vector3
		gridEnvironment.addHuman(position);
	}

	// TODO how is this supposed to work? Should we maybe assume everything not identified is ground?
	public void uploadGroundLocation(Vector2 loc) {
		gridEnvironment.addGround(loc);
	}

	public void uploadObstacleLocation(Vector2 loc) {
		gridEnvironment.addObstacle(loc);
	}
	
	public void addAgent(Agent agent) {
		agents.Add(agent);
	}
}
