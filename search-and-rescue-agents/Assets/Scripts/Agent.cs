using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {

    private bool carryingTarget;
    private Human currentTarget;

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
}
