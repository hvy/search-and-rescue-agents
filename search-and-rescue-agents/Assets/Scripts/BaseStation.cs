using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseStation : MonoBehaviour {

	public List<Vector2> entrances;
	public List<Agent> agents;
	public List<Human> unrescuedHumans;
	public GridEnvironment gridEnv = null;
	private Vector2 environmentPosition;
	
	void Start () {
		agents = new List<Agent> ();
		unrescuedHumans = new List<Human> ();
	}

	/**
	 * Decides which agent should go and rescue a certain human that is already
	 * found by an agent
	 */
	void Update () {

		List<Human> humansBeingRescued = new List<Human> ();

		foreach (Agent agent in agents) {
			if (agent.isMovingToTarget() || agent.isCarryingTarget()) {
				Human target = agent.getCurrentTarget();
				if (!humansBeingRescued.Contains(target)) {
					humansBeingRescued.Add(target);
				}
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
				if (!humansBeingRescued.Contains(unrescuedHuman)) {

					Debug.Log ("Assignning new target human from base station at " + (Vector2) unrescuedHuman.transform.position);

					agent.assignTarget(unrescuedHuman);
					humansBeingRescued.Add(unrescuedHuman);

					break;
				}
			}
		}


		
		// DEBUG Prints the explored grid
		/*
		Debug.Log ("===================================");
		for (int y = gridEnv.height - 1; y >= 0; y--) { 
			string gridRow = "";
			for (int x = 0; x < gridEnv.width; x++) {
				if (gridEnv.isWalkable(x, y)) {
					gridRow += "O";
				} else {
					gridRow += "X";
				}
			}
			Debug.Log(gridRow);
		}*/
	}

	public void setGridEnvironment(GridEnvironment gridEnv) {
		this.gridEnv = gridEnv;
	}

	public GridEnvironment getGridEnvironment() {
		return gridEnv;
	}

	public void setEnvironmentPos(Vector2 environmentPosition) {
		this.environmentPosition = environmentPosition;
	}

	public Vector2 getEnvironmentPos() {
		return environmentPosition;
	}

	public void uploadSavedTarget(Human human) {

		if (!unrescuedHumans.Contains (human)) {
			Debug.Log ("ERROR! Rescued unregistered human");
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
		gridEnv.addHuman(position);
	}

	public void uploadGroundLocation(Vector2 loc) {
		gridEnv.addGround(loc);
	}

	public void uploadObstacleLocation(Vector2 loc) {
		gridEnv.addObstacle(loc);
	}
	
	public void addAgent(Agent agent) {
		agents.Add(agent);
	}

	public List<Vector2> getPathFromTo(Vector2 from, Vector2 to) {
		return AStarPathFinding.findPath (from, to, gridEnv);
	}
}
