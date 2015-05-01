using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {

    private bool carryingTarget; // Agent is carrying a target
    private Human currentTarget; // Target to rescue

	// Use this for initialization
	void Start () {
    	carryingTarget = false;
	}
	
	// Update is called once per frame
	void Update () {
		// FIXME
		sendEnvironmentData();

	}

	void OnCollisionEnter2D(Collision2D coll){
       // FIXME
    }

	private void pickUpTarget() {
		// FIXME
	}

	private void putDownTarget() {
    	// FIXME
    }

	private void sendEnvironmentData() {
		// FIXME
	}

	private void moveToExit() {

	}

	private void moveToTarget() {
    	// FIXME
	}

}
