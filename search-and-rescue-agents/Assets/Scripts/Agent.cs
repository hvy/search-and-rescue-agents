using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour {

	public float velocity;

    private bool carryingTarget; // Agent is carrying a target
    private Human currentTarget; // Target to rescue
    private Vector2 goal; // current position to move toward

    private List<GNode> path; // path to current goal (if exists)

	// Use this for initialization
	void Start () {
    	carryingTarget = false;
    	path = new List<GNode>();
	}

	// Update is called once per frame
	void Update () {
		// FIXME

		if (carryingTarget)
		    moveToEntrance();
		else if (currentTarget != null)
		    moveToTarget();
		else
		    search();

		collisionAvoidance();

	}

    void OnTriggerEnter2D(Collider2D other) {
    	// TODO, collect information. This trigger will find humans and obstacles.
		sendEnvironmentData(other);
    }

    private void move (Vector2 goal) {
		float distance = Vector2.Distance (GetComponent<Rigidbody2D>().position, goal);
		GetComponent<Rigidbody2D>().transform.position = (Vector2.Lerp (GetComponent<Rigidbody2D>().transform.position, goal, velocity * Time.deltaTime / distance));
	}


    private void search() {
    	// FIXME
    	move(new Vector2(0,0));
    }

	private void pickUpTarget() {
		// FIXME
	}

	private void putDownTarget() {
    	// FIXME
    }

	private void sendEnvironmentData(Collider2D other) {
		Vector2 pos = other.transform.position;
		string name = other.name;
    	Debug.Log("Sending data " + name + " at " + pos);
		// FIXME send data to Base

	}

	private void moveToEntrance() {
		if (path.Count > 0 && Vector2.Distance(goal, transform.position) < 0.5) {
            goal = path[0].getPos();
            path.RemoveAt(0);
		} else {
        	goal = closestEntrance();
		}
		move(goal);

	}

	private void moveToTarget() {
	if (path.Count > 0 && Vector2.Distance(goal, transform.position) < 0.5) {
		goal = path[0].getPos();
		path.RemoveAt(0);
	} else {
		goal = currentTarget.getPos();
	}
    	// FIXME
	}

	private void collisionAvoidance() {
       // FIXME collision avoidance AI
	}

	private Vector2 closestEntrance() {
		// FIXME call BaseStation to get cloest entrance
		return new Vector2(1,1);
	}

	// Assign a target to this agent. Should be decided by the Base.
	public void assignTarget(Human target) {
		//currentTarget = human;
	}

}
