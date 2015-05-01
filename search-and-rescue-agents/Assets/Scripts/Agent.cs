using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour {

    private bool carryingTarget; // Agent is carrying a target
    private Human currentTarget; // Target to rescue

    private List<GNode> path; //

	// Use this for initialization
	void Start () {
    	carryingTarget = false;
    	//path = new ArrayList<GNode>();
	}

	// Update is called once per frame
	void Update () {
		// FIXME
		scan();
		sendEnvironmentData();

		if (carryingTarget)
		    moveToEntrance();
		else if (currentTarget != null)
		    moveToTarget();
		else
		    //search();

		collisionAvoidance();

	}

	void OnCollisionEnter2D(Collision2D coll){
       // FIXME
    }

	// Scan with CircleCollision2D, collect objects.
    private void scan() {
    	// FIXME
    }

	private void pickUpTarget() {
		// FIXME
	}

	private void putDownTarget() {
    	// FIXME
    }

	private void sendEnvironmentData() {
		// FIXME send data to Base
	}

	private void moveToEntrance() {
       // FIXME
	}

	private void moveToTarget() {
    	// FIXME
	}

	private void collisionAvoidance() {
       // FIXME collision avoidance AI
	}

	// Assign a target to this agent. Should be decided by the Base.
	public void assignTarget(Human target) {
		//currentTarget = human;
	}

}
