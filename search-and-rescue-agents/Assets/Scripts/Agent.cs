using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour {

	public enum ExplorationStrategy { FloodFillRandom, FloodFillNearest, FloodFillNearestAStar, Brownian, TeSLiSMA };

	public ExplorationStrategy explorationStrategy;
	public float velocity;
	public float rotationSpeed;
	public float collisionDistance;
	public Vector2 start;

	// Brownian walk
	private int browninanSegmentLength; // measured in frames
	private int brownianSegmentCount = 0; // measured in frames
	private int brownianDirection = -1;

	private System.Random rand;
	private BaseStation baseStation;
    private bool carryingTarget; // Agent is carrying a target
    private Human currentTarget = null; // Target to rescue
	private long searchCount = 0;
	private Vector2 goal;
	private List<Vector2> path; // path to current goal (if exists)

	private long c; // counter

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


    void OnTriggerStay2D(Collider2D other) {
		sendEnvironmentData(other);          // TODO maybe shouldn't be called EVERY time for performance reasons?
    }


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

	/**
	 * Explores the environment using the specified exploration strategy
	 */
    private void searchForTargets() {

		switch (explorationStrategy) {
			case ExplorationStrategy.FloodFillNearest:
				performFloodFillNearest();
				break;
		    case ExplorationStrategy.FloodFillNearestAStar:
				performFloodFillNearestAStar();
				break;
			case ExplorationStrategy.FloodFillRandom:
				performFloodFillRandom();
				break;
			case ExplorationStrategy.Brownian:
				performRandom();
				break;
			case ExplorationStrategy.TeSLiSMA:
				// TODO
				break;
			default:
				Debug.Log("[ERROR] Exploration strategy not found");
				break;
		}

		searchCount++;
    }

	/* Brownian walk */
    private void performRandom() {
		
		if (brownianDirection < 0 || brownianSegmentCount >= browninanSegmentLength) {
			// Not yet defined or walked in this direction for long enough, re-initialize the walk
			brownianDirection = rand.Next (4);
			brownianSegmentCount = 0;
			browninanSegmentLength = rand.Next(1, 200);
		} else {

			Vector2 nextPos = new Vector2 (transform.position.x, transform.position.y);

			if (brownianDirection == 0)
				nextPos += Vector2.up;
			else if (brownianDirection == 1)
				nextPos += Vector2.right;
			else if (brownianDirection == 2)
				nextPos -= Vector2.up;
			else if (brownianDirection == 3)
				nextPos -= Vector2.right;

			Vector2 gridCoordNextPos = baseStation.gridEnv.convertToGrid (nextPos);

			if (baseStation.gridEnv.isWalkable ((int)gridCoordNextPos.x, (int)gridCoordNextPos.y)) {
				brownianSegmentCount++;
				move (nextPos);
			} else {
				// Blocked, re-initialize the walk
				brownianDirection = rand.Next (4);
				brownianSegmentCount = 0;
				browninanSegmentLength = rand.Next(1, 200);
			}
		}

		/*
		Vector2 pos = new Vector2(-1,-1);

        int x = rand.Next(baseStation.gridEnv.getWidth());
        int y = rand.Next(baseStation.gridEnv.getHeight());
        pos.x = x;
        pos.y = y;
        Vector2 from = baseStation.gridEnv.convertToGrid (transform.position);
        Vector2 to = baseStation.gridEnv.convertToGrid (pos);

        if (path != null && path.Count != 0 && !baseStation.isEdge(path[path.Count-1])) {
			path = null;
		}

		if (path == null || path.Count == 0)
			path = baseStation.getPathFromTo (from, to);

		if (path == null) {
			move(pos);
			path = new List<Vector2>();
			return;
		}

		*/
		/*Remove a checkpoint from the path if it is reached*/
		/*
		if (path != null && isTouching(path[0])) {
			path.RemoveAt(0);
		}
		*/
		/*
        move(pos);

		if (searchCount > 50) {
			goal = pos;
			searchCount = 0;
		}

		if (path == null || path.Count == 0)
			move(goal);
		else
			move(path[0]);
		*/
    }

	/* Flood fill, but doesnt move toward nearest edge */
    private void performFloodFillRandom() {
  		Vector2 pos = new Vector2(-1,-1);
		pos = baseStation.getEdge();
		Vector2 from = baseStation.gridEnv.convertToGrid (transform.position);
		Vector2 to = baseStation.gridEnv.convertToGrid (pos);

		if (to.x == -1 && to.y == -1) {
			moveToEntrance();
			return;
		}

		if (path != null && path.Count != 0 && !baseStation.isEdge(path[path.Count-1])) {
			path = null;
		}

		if (path == null || path.Count == 0)
			path = baseStation.getPathFromTo (from, to);

		if (path != null && isTouching(path[0]) /* Remove a checkpoint from the path if it is reached*/) {
			path.RemoveAt(0);
		}

		if (searchCount > 50) {
			goal = pos;
			searchCount = 0;
		}

		if (path == null || path.Count == 0)
			move(goal);
		else
			move(path[0]);
    }

	/* Flood fill, moves to nearest edge */
    private void performFloodFillNearest() {
		Vector2 pos = new Vector2(-1,-1);
    	pos = baseStation.getEdge(transform.position);
		Vector2 from = baseStation.gridEnv.convertToGrid (transform.position);
		Vector2 to = baseStation.gridEnv.convertToGrid (pos);

		if (to.x == -1 && to.y == -1) {
			moveToEntrance();
			return;
		}

		if (path != null && path.Count != 0 && !baseStation.isEdge(path[path.Count-1])) {
			path = null;
		}

		if (path == null || path.Count == 0)
			path = baseStation.getPathFromTo (from, to);

		if (path != null && isTouching(path[0]) /* Remove a checkpoint from the path if it is reached*/) {
			path.RemoveAt(0);
		}

		if (searchCount > 50) {
			goal = pos;
			searchCount = 0;
		}

		if (path == null || path.Count == 0)
			move(goal);
		else
			move(path[0]);
    }

	/* Flood fill, moves to nearest edge in A star distance */
    private void performFloodFillNearestAStar() {
   		Vector2 pos = new Vector2(-1,-1);
		pos = baseStation.getEdgeAStar(transform.position);
		Vector2 from = baseStation.gridEnv.convertToGrid (transform.position);
		Vector2 to = baseStation.gridEnv.convertToGrid (pos);

		if (to.x == -1 && to.y == -1) {
			moveToEntrance();
			return;
		}

		if (path != null && path.Count != 0 && !baseStation.isEdge(path[path.Count-1])) {
			path = null;
		}

		if (path == null || path.Count == 0)
			path = baseStation.getPathFromTo (from, to);

		if (path != null && isTouching(path[0]) /* Remove a checkpoint from the path if it is reached*/) {
			path.RemoveAt(0);
		}

		if (searchCount > 50) {
			goal = pos;
			searchCount = 0;
		}

		if (path == null || path.Count == 0)
			move(goal);
		else
			move(path[0]);
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
		int layerMask = 1 << 8;

    	RaycastHit2D hit;
    	RaycastHit2D hit2;
		RaycastHit2D hit3;
		RaycastHit2D hit4;
		RaycastHit2D hit5;
		RaycastHit2D hit6;
		RaycastHit2D hit7;
		RaycastHit2D hit8;
		RaycastHit2D hit9;
		RaycastHit2D hit10;
		RaycastHit2D hit11;

		layerMask = ~layerMask;
		float distOffset = 0.0f;

		Vector2 direction = (p2-p1);


		hit = Physics2D.Raycast(p1, direction, Vector2.Distance (p1, p2)-distOffset, layerMask);
		hit2 = Physics2D.Raycast(p1+new Vector2(0.5f, 0), direction, Vector2.Distance (p1, p2)-distOffset, layerMask);
		hit3 = Physics2D.Raycast(p1+new Vector2(-0.5f, 0), direction, Vector2.Distance (p1, p2)-distOffset, layerMask);
		hit4 = Physics2D.Raycast(p1+new Vector2(0, 0.5f), direction, Vector2.Distance (p1, p2)-distOffset, layerMask);
		hit5 = Physics2D.Raycast(p1+new Vector2(0, -0.5f), direction, Vector2.Distance (p1, p2)-distOffset, layerMask);
		hit6 = Physics2D.Raycast(p1+new Vector2(-0.5f, -0.5f), direction, Vector2.Distance (p1, p2)-distOffset, layerMask);
		hit7 = Physics2D.Raycast(p1+new Vector2(0.5f, 0.5f), direction, Vector2.Distance (p1, p2)-distOffset, layerMask);

		if (hit.collider != null && hit.collider.tag == "Obstacle" &&
		hit2.collider != null && hit2.collider.tag == "Obstacle" &&
		hit3.collider != null && hit3.collider.tag == "Obstacle" &&
		hit4.collider != null && hit4.collider.tag == "Obstacle" &&
		hit5.collider != null && hit5.collider.tag == "Obstacle" &&
		hit6.collider != null && hit6.collider.tag == "Obstacle" &&
		hit7.collider != null && hit7.collider.tag == "Obstacle") {
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

			// TODO fix bug. Reason: it is only called when objects come INTO the radius. Needs to be called more often
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
				baseStation.uploadTargetLocation(this, human);
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

		if (path == null)
			return;
		
		if (path.Count > 0 /* A path is precomputed */) {
			
			if (isTouching(path[0]) /* Remove a checkpoint from the path if it is reached*/) {
				path.RemoveAt(0);
			}
			
		} else /* No path is precomputed. Find one! */ {
			
			// Convert from world coordinate to grid coordinate
			Vector2 from = baseStation.gridEnv.convertToGrid (transform.position);
			Vector2 to = baseStation.gridEnv.convertToGrid (closestEntrance ());

			path = baseStation.getPathFromTo (from, to);

			for (int i = 1; i < path.Count; i++) 
				Debug.DrawLine(path[i - 1], path[i], Color.cyan, 15.0f);
		}

		if (path.Count != 0)
			move(path[0]);
	}

	private void moveToTarget() {
		
		if (isTouching(currentTarget.transform.position)) {
			tryToPickUpTarget(currentTarget);
			path.Clear ();
			return;
		}

        if (path == null) {
        	Debug.Log("vafan");
        	return;
        }

		if (path.Count > 0 /* A path is precomputed */) {
			
			if (isTouching(path[0]) /* Remove a checkpoint from the path if it is reached*/) {
				path.RemoveAt(0);
			}
			
		} else /* No path is precomputed. Find one! */ {

			// Convert from world coordinate to grid coordinate
			Vector2 from = baseStation.gridEnv.convertToGrid (transform.position);
			Vector2 to = baseStation.gridEnv.convertToGrid (currentTarget.transform.position);
			
			path = baseStation.getPathFromTo (from, to);
			if (path == null)
				return;

			for (int i = 1; i < path.Count; i++) 
				Debug.DrawLine(path[i - 1], path[i], Color.red, 15.0f);
		}

		if (path.Count != 0)
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
