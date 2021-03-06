﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BaseStation : MonoBehaviour {

	public List<Vector2> entrances;
	public List<Agent> agents;

	private List<Human> assignedTargets;
	private List<Human> unassignedTargets;

	public bool performEANT = false;

	public GridEnvironment gridEnv = null;
	private Vector2 environmentPosition;
	private System.Random rand;
	private int width, height;

	private int savedTargets = 0;
	private int updateAntC = 0;
	private double time = 0;
	GameObject ui;

	void Start () {
		agents = new List<Agent> ();
		assignedTargets = new List<Human> ();
		unassignedTargets = new List<Human> ();
		rand = new System.Random();
		ui = GameObject.Find ("Hejsan");
	}

	/**
	 * Decides which agent should go and rescue a certain human that is already
	 * found by an agent
	 */
	void Update () {

		List<Agent> availableAgents = new List<Agent> ();
		foreach (Agent agent in agents) {
			if (!agent.hasTarget ()) {
				availableAgents.Add (agent);
			}
		}

		for (int i = unassignedTargets.Count - 1; i >= 0; i--) {

			Human unassignedTarget = unassignedTargets[i];

			float closestDistanceToAgent = float.MaxValue;
			Agent closestAgent = null;

			foreach (Agent agent in availableAgents) {

				float distanceToAgent = Vector2.SqrMagnitude(unassignedTarget.transform.position - agent.transform.position);

				if(distanceToAgent < closestDistanceToAgent) {
					closestAgent = agent;
					closestDistanceToAgent = distanceToAgent;
				}
			}

			// Assign the closest agent to the target where the distance is measured in the euclidean space
			if(closestAgent != null) {

				Debug.Log ("[INFO] Assignning new target human from base station at " + (Vector2) unassignedTarget.transform.position + " to agent " + (Vector2) closestAgent.transform.position);

				closestAgent.assignTarget(unassignedTarget);
				availableAgents.Remove(closestAgent);

				unassignedTargets.Remove (unassignedTarget);
				assignedTargets.Add(unassignedTarget);
			}
		}

		/* Update weights for ANT algo */
		if (agents.Count > 0)
			time += Time.deltaTime;
		if (updateAntC > 5) {
			if (performEANT)
				weightANT();

//			Text txt = (Text) ui.GetComponent(typeof(Text));
//			txt.text = "Covered: " + System.String.Format("{0:0.00}", (100 *(1 - tilesUnknown()/(gridEnv.getHeight()*gridEnv.getWidth()))) + "%" );
//			txt.text += "\nTime: " + System.String.Format("{0:0.00}", time) + "s";
//			txt.text += "\nSaved: " + savedTargets;
			Debug.Log("Covered: " + System.String.Format("{0:0.00}", (100 *(1 - tilesUnknown()/(gridEnv.getHeight()*gridEnv.getWidth()))) + "%"));
			Debug.Log("Saved: " + savedTargets);

			updateAntC = 0;
		}
		updateAntC += 1;

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

	/**
	 * Called when an agent brings a human back to the environment entrance
	 */
	public void uploadSavedTarget(Human human) {

		if (unassignedTargets.Contains (human) || !assignedTargets.Contains(human)) {
			Debug.Log ("[ERROR] Rescued unregistered human");
		} else {
			Debug.Log ("[INFO] Rescued target");
			savedTargets++;
			assignedTargets.Remove(human);
		}
	}

	/**
	 * Called when an agent find a human in the environment.
	 * If the agent has no current target, that human is assigned as the target.
	 */
	public void uploadTargetLocation(Agent agent, Human human) {
					
		gridEnv.addHuman((Vector2) human.transform.position);

		if (!unassignedTargets.Contains (human) && !assignedTargets.Contains(human)) {

			// Immediately assign the human to the agent if the agent has no current target
			if(!agent.hasTarget()) {

				Debug.Log ("[INFO] Assignning new target human from base station at " + (Vector2) human.transform.position + " to agent " + (Vector2) agent.transform.position);

				agent.assignTarget(human);
				assignedTargets.Add(human);
			} else {
				unassignedTargets.Add (human);
			}
		}
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
					float dist;
					dist = Vector2.Distance(temp, pos);
					if (dist < closestDistance) {
						closestDistance = dist;
						closest = temp;
					}
				}
				x_mod = (x_mod+1)%(gridEnv.width - 1);
			}
			y_mod = (y_mod+1)%(gridEnv.height - 1);
		}
		return closest;
	}

	/* Get edge closest to pos */
	public Vector2 getEdgeAStar(Vector2 pos) {

		int x_mod = rand.Next(gridEnv.width);
		int y_mod = rand.Next(gridEnv.height);

		Vector2 closest = new Vector2(-1, -1);
		float closestDistance = 1000000f;

		for (int y = gridEnv.height - 1; y >= 0; y--) {

			for (int x = 0; x < gridEnv.width; x++) {
				if (gridEnv.isWalkable(x_mod, y_mod) && gridEnv.isEdge(x_mod,y_mod)) {
					Vector2 temp = new Vector2(x_mod,y_mod);
					float dist;
					List<Vector2> path = AStarPathFinding.findPath(gridEnv.convertToGrid(pos), temp, gridEnv);
					if (path == null)
						dist = Vector2.Distance(temp, pos);
					else
						dist = path.Count;
					if (dist < closestDistance) {
						closestDistance = dist;
						closest = temp;
					}
				}
				x_mod = (x_mod+1)%(gridEnv.width - 1);
			}
			y_mod = (y_mod+1)%(gridEnv.height - 1);
		}
		return closest;
	}

	public Vector2 getEdge() {
		int x_mod = rand.Next(gridEnv.width);
		int y_mod = rand.Next(gridEnv.height);

		for (int y = gridEnv.height - 1; y >= 0; y--) {

			for (int x = 0; x < gridEnv.width; x++) {
				if (gridEnv.isWalkable(x_mod, y_mod) && gridEnv.isEdge(x_mod,y_mod)) {
					Vector2 temp = new Vector2(x_mod,y_mod);
					return temp;
				}
				x_mod = (x_mod+1)%(gridEnv.width - 1);
			}
		}
		return new Vector2(-1,-1);
	}

	public void incrementC(Vector2 pos) {
		gridEnv.incrementC(pos);
	}

	public void decrementC(Vector2 pos) {
		gridEnv.decrementC(pos);
	}

	public void weightANT() {
		Debug.Log("weighting ant tiles");
		for (int y = gridEnv.height - 1; y >= 0; y--) {

			for (int x = 0; x < gridEnv.width; x++) {
				if (gridEnv.isWalkable(x, y) && gridEnv.isEdge(x,y)) {
                    decrementC(new Vector2(x,y));
				}

			}
		}
	}

	public double tilesUnknown() {
		int i = 0;
		for (int y = gridEnv.height - 1; y >= 0; y--) {

			for (int x = 0; x < gridEnv.width; x++) {
				if (gridEnv.isUnknown(x, y)) {
					i++;
				}

			}
		}
		return i;
	}

	public Vector2 minVisitedANT(Vector2 pos) {

		Vector2 gridPos = gridEnv.convertToGrid(pos);

        Vector2 minPos = new Vector2(-1, -1);
        int minCount = 100000000;
        List<Vector2> positionsToChoose = new List<Vector2>();
		Vector2 position = new Vector2(gridPos.x, gridPos.y+1);
        if (gridEnv.getCount(position) <= minCount && gridEnv.getCount(position) != -1) {
			if (minCount == gridEnv.getCount(position))
							positionsToChoose.Add(position);
			minPos = position;
			minCount = gridEnv.getCount(position);
			position = new Vector2(gridPos.x, gridPos.y+2);
			if (gridEnv.getCount(position) < minCount && gridEnv.getCount(position) != -1) {
//				minPos = position;
				minCount = gridEnv.getCount(position);
				position = new Vector2(gridPos.x, gridPos.y+3);
				if (gridEnv.getCount(position) < minCount && gridEnv.getCount(position) != -1) {
//					minPos = position;
					minCount = gridEnv.getCount(position);
				}
			}
		}
		position = new Vector2(gridPos.x+1, gridPos.y);
		if (gridEnv.getCount(position) <= minCount && gridEnv.getCount(position) != -1) {
			if (minCount == gridEnv.getCount(position))
				positionsToChoose.Add(position);

			minPos = position;
			minCount = gridEnv.getCount(position);

			position = new Vector2(gridPos.x+2, gridPos.y);
			if (gridEnv.getCount(position) < minCount && gridEnv.getCount(position) != -1) {
//				minPos = position;
				minCount = gridEnv.getCount(position);
				position = new Vector2(gridPos.x+3, gridPos.y);
				if (gridEnv.getCount(position) < minCount && gridEnv.getCount(position) != -1) {
//					minPos = position;
					minCount = gridEnv.getCount(position);
				}
			}
		}
        position = new Vector2(gridPos.x-1, gridPos.y);
        if (gridEnv.getCount(position) <= minCount && gridEnv.getCount(position) != -1) {
			if (minCount == gridEnv.getCount(position))
							positionsToChoose.Add(position);
			minPos = position;
			minCount = gridEnv.getCount(position);
			position = new Vector2(gridPos.x-2, gridPos.y);
			if (gridEnv.getCount(position) < minCount && gridEnv.getCount(position) != -1) {
//				minPos = position;
				minCount = gridEnv.getCount(position);
				position = new Vector2(gridPos.x-3, gridPos.y);
				if (gridEnv.getCount(position) < minCount && gridEnv.getCount(position) != -1) {
//					minPos = position;
					minCount = gridEnv.getCount(position);
				}
			}
		}
		position = new Vector2(gridPos.x, gridPos.y-1);
		if (gridEnv.getCount(position) <= minCount && gridEnv.getCount(position) != -1) {
			if (minCount == gridEnv.getCount(position))
							positionsToChoose.Add(position);
			minPos = position;
			minCount = gridEnv.getCount(position);
			position = new Vector2(gridPos.x, gridPos.y-2);
			if (gridEnv.getCount(position) < minCount && gridEnv.getCount(position) != -1) {
//				minPos = position;
				minCount = gridEnv.getCount(position);

				position = new Vector2(gridPos.x, gridPos.y-3);
				if (gridEnv.getCount(position) < minCount && gridEnv.getCount(position) != -1) {
//					minPos = position;
					minCount = gridEnv.getCount(position);
				}
			}
		}

		position = new Vector2(gridPos.x+1, gridPos.y+1);
		if (gridEnv.getCount(position) <= minCount && gridEnv.getCount(position) != -1) {
			if (minCount == gridEnv.getCount(position))
							positionsToChoose.Add(position);
			minPos = position;
			minCount = gridEnv.getCount(position);
		}
        position = new Vector2(gridPos.x-1, gridPos.y+1);
        if (gridEnv.getCount(position) <= minCount && gridEnv.getCount(position) != -1) {
			if (minCount == gridEnv.getCount(position))
				positionsToChoose.Add(position);
			minPos = position;
			minCount = gridEnv.getCount(position);
		}
		position = new Vector2(gridPos.x-1, gridPos.y-1);
		if (gridEnv.getCount(position) <= minCount && gridEnv.getCount(position) != -1) {
			if (minCount == gridEnv.getCount(position))
							positionsToChoose.Add(position);
			minPos = position;
			minCount = gridEnv.getCount(position);
		}
		position = new Vector2(gridPos.x+1, gridPos.y-1);
		if (gridEnv.getCount(position) <= minCount && gridEnv.getCount(position) != -1) {
			if (minCount == gridEnv.getCount(position))
				positionsToChoose.Add(position);
			minPos = position;
			minCount = gridEnv.getCount(position);
		}


		int index = rand.Next(positionsToChoose.Count);
		if (index <= 0)
			return minPos;
//		Debug.Log(index);
		return positionsToChoose[index];

	}
}
