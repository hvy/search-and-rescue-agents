using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour {

	public float velocity;
	public float rotationSpeed;
	public float collisionDistance;
	public Vector2 goal; // current position to move toward
	public Vector2 start;

	private System.Random rand;
	private BaseStation baseStation;
    private bool carryingTarget; // Agent is carrying a target
    private Human currentTarget = null; // Target to rescue
	private long searchCount = 0;
	private List<GNode> path; // path to current goal (if exists)
	
	// Use this for initialization
	void Start () {
    	carryingTarget = false;
    	path = new List<GNode>();
    	start = transform.position;
    	rand = new System.Random();
    	Display.agentCount++;
	}

	// Update is called once per fr	ame
	void Update () {

		if (carryingTarget)
			moveToEntrance ();
		else if (currentTarget != null && !currentTarget.saved)
			moveToTarget ();
		else if (isInsideEnvironment())
			searchForTargets ();
		else
			searchForEntrance ();
		    
		Debug.DrawLine(transform.position, goal, Color.white); // Debug
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, 4.0f);
	}

    void OnTriggerEnter2D(Collider2D other) {
    	// Collect information. This trigger will find humans and obstacles.
		sendEnvironmentData(other);
    }

    void OnTriggerStay2D(Collider2D other) {

        if (other.name == "human") {
        
			Human human = (Human) other.gameObject.GetComponent(typeof(Human));

			// Only save humans who are 
			// 1. not already saved
			// 2. is assignment to this agent by the base station
			// 3. is close enough to this agent
			if (!human.saved && currentTarget == human && Vector3.Distance(transform.position, other.transform.position) < 0.1f) {
				pickUpTarget(other);
            }
        }
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

    private void searchForTargets() {

		// TODO Use flood fill algorithm
    	
		//int h = rand.Next((int)-baseStation.width/2, (int)baseStation.width/2);
		//int w = rand.Next((int)-baseStation.height/2, (int)baseStation.height/2);
		int h = rand.Next((int)-baseStation.getGridEnvironment().getWidth()/2, (int)baseStation.getGridEnvironment().getWidth()/2);
		int w = rand.Next((int)-baseStation.getGridEnvironment().getHeight()/2, (int)baseStation.getGridEnvironment().getHeight()/2);

		if (searchCount > 100) {
			goal = new Vector2(h,w);
			searchCount = 0;
        }

		move(goal);
		searchCount++;
    }

	private void searchForEntrance() {
		// TODO 
		// 1. If agent is close to a wall, move alongside the wall to find an entrance (clockwise or anti clockwise)
		// 2. If agent is not close to a wall, move randomly, or move towards the environment if that information is available

		move(closestEntrance());


	}

	private void pickUpTarget(Collider2D human) {

		Debug.Log ("Picking up target at " + (Vector2) human.transform.position); // Debug
		gameObject.GetComponent<Renderer>().material.color = Color.white; // Debug

		human.gameObject.SetActive(false);

        currentTarget.saved = true;
		carryingTarget = true;
	}

	private void putDownTarget() {

		Debug.Log ("Saved Human!"); // Debug
		gameObject.GetComponent<Renderer>().material.color = Color.blue; // Debug

		baseStation.uploadSavedTarget (currentTarget);

		currentTarget.transform.position = transform.position;
      	currentTarget.gameObject.SetActive(true);
      	
		currentTarget = null;
		carryingTarget = false;
      	
      	Display.currentRescued++;
    }

	/* 
	 * Send environment data to the base station
	 */
	private void sendEnvironmentData(Collider2D other) {

		switch (other.name) {

		case "obstacle":

			baseStation.uploadObstacleLocation (other.transform.position);
			other.gameObject.GetComponent<Renderer>().material.color = Color.green; // Debug
			break;

		case "human":

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
		if (path.Count > 0 && Vector2.Distance(goal, transform.position) < 0.5) {
            goal = path[0].getPos();
            path.RemoveAt(0);
		} else {
        	goal = closestEntrance();
		}
		move(goal);

		if (Vector2.Distance(transform.position, closestEntrance()) < 0.5f) {
			putDownTarget();
		}
	}

	private void moveToTarget() {
		if (path.Count > 0 && Vector2.Distance(goal, transform.position) < 0.5) {
			goal = path[0].getPos();
			path.RemoveAt(0);
		} else {
			goal = currentTarget.transform.position;
		}
		move(goal);
	}

	private bool collisionAvoidance(Vector2 goal) {
		bool avoiding = false;

		Vector2 position = (Vector2) transform.position;
		Vector2 dir = (goal - position).normalized;

		float distToObstacle = collisionDistance*Vector3.Distance(position, goal)/10f;

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
		float closest = 1000000f;
		for (int i = 0; i < baseStation.entrances.Count; i++) {
			float dist = Vector2.Distance(baseStation.entrances[i], transform.position);
			if (dist < closest) {
				closest = dist;
				index = i;
			}
		}
		return baseStation.entrances[index];
	}

	private bool isInsideEnvironment() {
		if (transform.position.x >= -baseStation.getGridEnvironment().getWidth()/2-0.3f && 
		    transform.position.x <= baseStation.getGridEnvironment().getWidth()/2+0.3f && 
		    transform.position.y <= baseStation.getGridEnvironment().getHeight()/2+0.3f && 
		    transform.position.y >= -baseStation.getGridEnvironment().getHeight()/2-0.3f)
			return true;
		return false;
	}
}
