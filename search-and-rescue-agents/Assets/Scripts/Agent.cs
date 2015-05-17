using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour {

	public float velocity;
	public float rotationSpeed;
	public float collisionDistance;

	public Vector2 start;

	private System.Random rand;
	private BaseStation baseStation;
    private bool carryingTarget; // Agent is carrying a target
    private Human currentTarget = null; // Target to rescue
	private long searchCount = 0;
	private Vector2 goal;
	private List<Vector2> path; // path to current goal (if exists)

	// Use this for initialization
	void Start () {
    	carryingTarget = false;
    	path = new List<Vector2>();
    	start = transform.position;
    	rand = new System.Random();
    	Display.agentCount++;
	}

	// Update is called once per fr	ame
	void Update () {

		if (carryingTarget)
			moveToEntrance ();
		else if (!isInsideEnvironment())
			moveTowardsEnvironment ();
		else if (currentTarget != null && !currentTarget.saved)
			moveToTarget ();
		else
			searchForTargets ();
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, 4.0f);
	}

    void OnTriggerEnter2D(Collider2D other) {
		sendEnvironmentData(other); // Collect information. This trigger will find humans and obstacles.
    }

	/*
    void OnTriggerStay2D(Collider2D other) {

		//if (isInsideEnvironment ()) {

			if (other.tag == "Human") {
				Human human = (Human) other.gameObject.GetComponent(typeof(Human));
				
				// Only save humans who are 
				// 1. not already saved
				// 2. is assignment to this agent by the base station
				// 3. is close enough to this agent
				if (!human.saved && currentTarget == human && Vector3.Distance(transform.position, other.transform.position) < 0.3f) {
					pickUpTarget(other);
				}
			}
    }
	*/

	/**
	 * Assign a target to this agent. This is decided by the base station.
	 */
	public void assignTarget(Human target) {
		currentTarget = target;
	}

	public void setBase(BaseStation baseStation) {
		this.baseStation = baseStation;
	}
	
	public bool isCarryingTarget() {
		return carryingTarget;
	}
	
	public bool isMovingToTarget() {
		return !isCarryingTarget() && currentTarget != null;
	}
	
	public bool hasTarget() {
		return currentTarget != null;
	}
	
	public Human getCurrentTarget() {
		return currentTarget;
	}

	private void move (Vector2 g) {
		collisionAvoidance(g);
	}

    private void searchForTargets() {

		// TODO Use flood fill algorithm
		int h = rand.Next(0, (int)baseStation.getGridEnvironment().getWidth());
		int w = rand.Next(0, (int)baseStation.getGridEnvironment().getHeight());

		if (searchCount > 50) {
			goal = new Vector2(h,w);
			searchCount = 0;
        }

		move(goal);
		searchCount++;
    }

	private void moveTowardsEnvironment() {
		goal = closestEntrance();
		move (goal);
	}

	private void tryToPickUpTarget(Human human) {

		if (human.saved) {
			Debug.Log ("[ERROR] Trying to save an already rescued target. Target: " + (Vector2) human.transform.position);
			return;
		} else if (carryingTarget) {
			Debug.Log ("[ERROR] Trying to save a target while carrying a target. Trying to pick up target: " + (Vector2) human.transform.position);
			return;
		} else if (human != currentTarget) {
			Debug.Log ("[ERROR] Trying to save a target of another agent. Target: " + human.transform.position);
			return;
		}

		// Debug
		Debug.Log ("[INFO] Picking up target at: " + (Vector2) human.transform.position); 
		gameObject.GetComponent<Renderer>().material.color = Color.white;
		human.gameObject.SetActive(false);

        currentTarget.saved = true;
		carryingTarget = true;

		// Make sure the base station is notified of the pick up and that the position of the target is now walkable
		baseStation.uploadGroundLocation ((Vector2) human.transform.position);
	}

	private void putDownTarget() {

		Debug.Log ("Saved Human!"); // Debug
		gameObject.GetComponent<Renderer>().material.color = Color.blue; // Debug

		baseStation.uploadSavedTarget (currentTarget);

		currentTarget.transform.position = transform.position;
      	currentTarget.gameObject.SetActive(true);

		// TODO Destroying the target so that it does not block the entrance for other agents
		Destroy (currentTarget.gameObject);
      	
		currentTarget = null;
		carryingTarget = false;
      	
      	Display.currentRescued++;
    }

    /* Returns true if the ray between the two points collides with an obstacle */
    private bool rayCastForObstruction(Vector2 p1, Vector2 p2) {
		int layerMask = 1 << 9;

    	RaycastHit2D hit;
    	RaycastHit2D hit2;
		RaycastHit2D hit3;
		RaycastHit2D hit4;
		RaycastHit2D hit5;
		RaycastHit2D hit6;
		RaycastHit2D hit7;

		layerMask = layerMask;
		hit = Physics2D.Raycast(p1, (p2-p1).normalized, Vector2.Distance (p1, p2), layerMask);
		hit2 = Physics2D.Raycast(p1+new Vector2(0.5f, 0), (p2-p1).normalized, Vector2.Distance (p1, p2), layerMask);
		hit3 = Physics2D.Raycast(p1+new Vector2(-0.5f, 0), (p2-p1).normalized, Vector2.Distance (p1, p2), layerMask);
		hit4 = Physics2D.Raycast(p1+new Vector2(0, 0.5f), (p2-p1).normalized, Vector2.Distance (p1, p2), layerMask);
		hit5 = Physics2D.Raycast(p1+new Vector2(0, -0.5f), (p2-p1).normalized, Vector2.Distance (p1, p2), layerMask);
		hit6 = Physics2D.Raycast(p1+new Vector2(-0.5f, -0.5f), (p2-p1).normalized, Vector2.Distance (p1, p2), layerMask);
		hit7 = Physics2D.Raycast(p1+new Vector2(0.5f, 0.5f), (p2-p1).normalized, Vector2.Distance (p1, p2), layerMask);

		if (hit.collider != null && hit.collider.tag == "Obstacle" &&
		hit2.collider != null && hit2.collider.tag == "Obstacle" &&
		hit3.collider != null && hit3.collider.tag == "Obstacle" &&
		hit4.collider != null && hit4.collider.tag == "Obstacle" &&
		hit5.collider != null && hit4.collider.tag == "Obstacle" &&
		hit6.collider != null && hit4.collider.tag == "Obstacle" &&
		hit7.collider != null && hit4.collider.tag == "Obstacle") {
			return true;
		}

		return false;
    }

	/* 
	 * Send environment data to the base station
	 */
	private void sendEnvironmentData(Collider2D other) {

		RaycastHit2D hit;
		int layerMask = 1 << 8;

		switch (other.tag) {

		case "Empty":

			// TODO Make sure that the agent can't see through walls
            if (rayCastForObstruction(transform.position, other.transform.position))
            	break;
			baseStation.uploadGroundLocation (other.transform.position);
			Destroy(other.gameObject);
			break;

		case "Obstacle":

			// TODO Make sure that the agent can't see through walls

		  	if (rayCastForObstruction(transform.position, other.transform.position))
				break;
			baseStation.uploadObstacleLocation (other.transform.position);
			other.gameObject.GetComponent<Renderer>().material.color = Color.green; // Debug
			break;

		case "Human":


			// This would cast rays only against colliders in layer 8, so we just inverse the mask.
			layerMask = ~layerMask;
			hit = Physics2D.Raycast(transform.position, (other.transform.position-transform.position).normalized, Vector2.Distance (transform.position, other.transform.position)-1f, layerMask);
			if (hit.collider != null && hit.collider.tag == "Obstacle") {
				break;
			}
			Human human = (Human) other.gameObject.GetComponent(typeof(Human));
			if (!human.saved)
				baseStation.uploadTargetLocation(human);
			other.gameObject.GetComponent<Renderer>().material.color = Color.green; // Debug
			break;

		default:
			break;
		}
	}

	private void moveToEntrance() {

		// If the agent is carrying the target and is close enought to an entrance, drop the target at the current position
		if (isTouching(closestEntrance()) && isCarryingTarget() ) {
			putDownTarget();
			path.Clear ();
			return;
		} 
		
		if (path.Count > 0 /* A path is precomputed */) {
			
			if (isTouching(path[0]) /* Remove a checkpoint from the path if it is reached*/) {
				path.RemoveAt(0);
			}
			
		} else /* No path is precomputed. Find one! */ {
			
			// Convert from world coordinate to grid coordinate
			Vector2 from = baseStation.gridEnv.convertToGrid (transform.position);
			Vector2 to = baseStation.gridEnv.convertToGrid (closestEntrance ());
			
			// DEBUG
			Debug.Log ("[INFO] Running A* from " + (Vector2) from + " to: " + (Vector2) to);
			
			path = baseStation.getPathFromTo (from, to);
			
			// DEBUG
			Debug.Log ("[INFO] Found a path of length: " + path.Count);
			for (int i = 1; i < path.Count; i++) 
				Debug.DrawLine(path[i - 1], path[i], Color.cyan, 15.0f);
		}
		
		Debug.Log ("Next goal: " + path[0]);
		
		move(path[0]);
	}

	private void moveToTarget() {
		
		if (isTouching(currentTarget.transform.position)) {
			tryToPickUpTarget(currentTarget);
			path.Clear ();
			return;
		}

//        if (path == null)
//        	return;
		
		if (path.Count > 0 /* A path is precomputed */) {
			
			if (isTouching(path[0]) /* Remove a checkpoint from the path if it is reached*/) {
				path.RemoveAt(0);
			}
			
		} else /* No path is precomputed. Find one! */ {

			// Convert from world coordinate to grid coordinate
			Vector2 from = baseStation.gridEnv.convertToGrid (transform.position);
			Vector2 to = baseStation.gridEnv.convertToGrid (currentTarget.transform.position);
				
			// DEBUG
			Debug.Log ("[INFO] Running A* from " + (Vector2) from + " to: " + (Vector2) to);
			
			path = baseStation.getPathFromTo (from, to);
			
			// DEBUG
			Debug.Log ("[INFO] Found a path of length: " + path.Count);
			for (int i = 1; i < path.Count; i++) 
				Debug.DrawLine(path[i - 1], path[i], Color.red, 15.0f);
		}
		
		Debug.Log ("Next goal: " + path[0]);
		
		move(path[0]);
	}

	private bool collisionAvoidance(Vector2 goal) {
		bool avoiding = false;

		Vector2 position = (Vector2) transform.position;
		Vector2 dir = (goal - position).normalized;

		float distToObstacle = collisionDistance;//*Vector3.Distance(position, goal)/10f;

		// Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

		Debug.DrawLine(position, transform.position+transform.forward*5f, Color.green);
        // This would cast rays only against colliders in layer 8, so we just inverse the mask.
        layerMask = ~layerMask;
		RaycastHit2D hit;

		hit = Physics2D.Raycast(position, transform.forward, distToObstacle, layerMask);
        if (hit.collider != null) {
        	Debug.DrawLine(position, hit.point, Color.red);
            dir += hit.normal * 10;
            avoiding = true;
        }

        Vector3 left = transform.position;
        Vector3 right = transform.position;

		left.x -= 0.5f;
		right.x += 0.5f;

        hit = Physics2D.Raycast(left, transform.forward, distToObstacle, layerMask);
		if (hit.collider != null) {
			Debug.DrawLine(position, hit.point, Color.red);
			dir += hit.normal * 10;
			avoiding = true;
		}

		hit = Physics2D.Raycast(right, transform.forward, distToObstacle, layerMask);
		if (hit.collider != null) {
			Debug.DrawLine(position, hit.point, Color.red);
			dir += hit.normal * 10;
			avoiding = true;
		}

        Vector3 newDir = Vector3.RotateTowards(transform.forward, dir, Time.deltaTime * rotationSpeed, 10.0f);
      	transform.rotation = Quaternion.LookRotation(newDir);

        Vector3 posDiff = transform.forward * velocity * Time.deltaTime;
        posDiff.z = 0f;
        transform.position += posDiff;

        return avoiding;
	}

	private Vector2 closestEntrance() {
		int index = 0;
		float closest = float.MaxValue;
		for (int i = 0; i < baseStation.entrances.Count; i++) {
			float dist = Vector2.Distance(baseStation.entrances[i], transform.position);
			if (dist < closest) {
				closest = dist;
				index = i;
			}
		}
		return baseStation.entrances[index];
	}

	/* 
	 * TODO At the moment this method assumes that the environment is a square. 
	 * Make sure it works for other environments.
	 */
	private bool isInsideEnvironment() {
		if (transform.position.x > 0 && 
			transform.position.x < baseStation.getGridEnvironment ().getWidth () && 
			transform.position.y > 0 && 
			transform.position.y < baseStation.getGridEnvironment ().getHeight ()) {
			return true;
		} else {
			return false;
		}
	}

	private bool isTouching(Vector2 pos) {
		return Vector2.Distance (transform.position, pos) < 0.5f;
	}
}
