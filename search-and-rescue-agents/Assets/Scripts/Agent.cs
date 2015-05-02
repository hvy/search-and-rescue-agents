using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour {

	public float velocity;

    private bool carryingTarget; // Agent is carrying a target
    private Human currentTarget; // Target to rescue
    public Vector2 goal; // current position to move toward
    private Vector2 avoidanceDirection;

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

    private void move (Vector2 g) {
    	if (avoidanceDirection.x == 0 && avoidanceDirection.y == 0)
        	g = goal;
        else
        	g = avoidanceDirection;

       	float speed = 100f;
		Vector3 targetDir = (Vector3) g - transform.position;
	   	float step = speed * Time.deltaTime;
	   	Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
	   	transform.rotation = Quaternion.LookRotation(newDir);

		float distance = Vector2.Distance (GetComponent<Rigidbody2D>().position, g);
		GetComponent<Rigidbody2D>().transform.position = (Vector2.Lerp (GetComponent<Rigidbody2D>().transform.position, g, velocity * Time.deltaTime / distance));
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
//    	Debug.Log("Sending data " + name + " at " + pos);
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
		goal = currentTarget.transform.position;
	}
    	// FIXME
	}

	private void collisionAvoidance() {


		// Avoid obstacles
		Vector2 position = (Vector2) transform.position;

		//new avoidance

		Vector2 dir = (goal - position).normalized;
		RaycastHit2D hit;
		float distToObstacle = 10f;

		Vector2 movementDirection = (dir + position);

		Vector2 velocityDirection = ((movementDirection)-position).normalized;

		Vector3 moveDir = new Vector3(movementDirection.x, 0.0f, movementDirection.y);
		Debug.DrawLine(position, movementDirection, Color.green);

		// Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8, so we just inverse the mask.
        layerMask = ~layerMask;
//		hit = Physics2D.Raycast(position, transform.forward, distToObstacle, layerMask);
//        if (hit.collider != null) {
//
//				Debug.Log(hit.transform.name);
//				Debug.DrawLine(position, hit.point, Color.red);
//				dir = new Vector2(position.x-2, movementDirection.y);
//				Debug.Log("MOVE TOWARD " + hit.point);
//				avoidanceDirection = dir;
//        } else {
//        	avoidanceDirection = new Vector2(0,0);
//        }

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
