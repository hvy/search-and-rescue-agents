using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseStation : MonoBehaviour {

	public List<Vector2> entrances;
	public List<Agent> agents;
	public List<Human> unrescuedHumans;
	public GridEnvironment gridEnv = null;
	private Vector2 environmentPosition;
	private System.Random rand;
	private int width, height;

	void Start () {
		agents = new List<Agent> ();
		unrescuedHumans = new List<Human> ();
		rand = new System.Random();
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

	public bool isEdge(Vector2 pos) {
		Vector2 gridPos = gridEnv.convertToGrid(pos);

		return gridEnv.isEdge((int) gridPos.x, (int) gridPos.y);
	}

	public bool isUnknownLocation(Vector2 pos) {
		Vector2 gridPos = gridEnv.convertToGrid(pos);

		return gridEnv.isUnknown((int)gridPos.x, (int)gridPos.y);
	}


	/* Get edge closest to pos */
	public Vector2 getEdge(Vector2 pos) {

		int x_mod = rand.Next(gridEnv.width);
		int y_mod = rand.Next(gridEnv.height);

		Vector2 closest = new Vector2(-1, -1);
		float closestDistance = 1000000f;

		for (int y = gridEnv.height - 1; y >= 0; y--) {

			for (int x = 0; x < gridEnv.width; x++) {
				if (gridEnv.isWalkable(x_mod, y_mod) && gridEnv.isEdge(x_mod,y_mod)) {
					Vector2 temp = new Vector2(x_mod,y_mod);
					float dist = Vector2.Distance(temp, pos);
					if (dist < closestDistance) {
						closestDistance = dist;
						closest = temp;
					}
					//return new Vector2(x_mod,y_mod);
				}
				x_mod = (x_mod+1)%(gridEnv.width - 1);
			}
			y_mod = (y_mod+1)%(gridEnv.height - 1);
		}
//		return new Vector2(-1,-1);
		return closest;
	}
}
