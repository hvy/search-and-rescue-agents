using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour {

	public float velocity;
	public float rotationSpeed;
	public float collisionDistance;

    private bool carryingTarget; // Agent is carrying a target
    private bool insideEnvironment;
    private Human currentTarget; // Target to rescue
    public Vector2 goal; // current position to move toward
   	public Vector2 start;

   	private BaseStation baseStation;
    private List<GNode> path; // path to current goal (if exists)

    private System.Random rand;
    private long searchCount = 0;


	// Use this for initialization
	void Start () {
    	carryingTarget = false;
    	insideEnvironment = false;
    	path = new List<GNode>();
    	start = transform.position;
    	rand = new System.Random();
    	Display.agentCount++;
	}

	// Update is called once per fr	ame
	void Update () {
		// FIXME
		insideEnvironment = isInsideEnvironment();

		if (carryingTarget)
		    moveToEntrance();
		else if (currentTarget != null && !currentTarget.saved)
		    moveToTarget();
		else
		    search();
		Debug.DrawLine(transform.position, goal, Color.white);

	}
	void OnDrawGizmos() {
		Gizmos.color = Color.white;
//        Gizmos.DrawWireSphere(transform.position, 4.0f);
	}

    void OnTriggerEnter2D(Collider2D other) {
    	// TODO, collect information. This trigger will find humans and obstacles.
		sendEnvironmentData(other);

		// Found human
		if (other.name == "human") {
        	other.gameObject.GetComponent<Renderer>().material.color = Color.green;
		}

    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.name == "human") {
        	// Only save humans who are not already saved and close.
        	Human human = (Human) other.gameObject.GetComponent(typeof(Human));
        	if (!carryingTarget)
        		currentTarget = human;
            if (Vector3.Distance(transform.position, other.transform.position) < 0.1f && !human.saved) {
				pickUpTarget(other);
            }
        }

        if (other.name == "obstacle") {
        	other.gameObject.GetComponent<Renderer>().material.color = Color.green;
        }

    }

    private void move (Vector2 g) {
    	collisionAvoidance(g);
	}

    private void search() {
    	if (!insideEnvironment)
    		move(closestEntrance());
    	else {
    		int h = rand.Next((int)-baseStation.width/2, (int)baseStation.width/2);
    		int w = rand.Next((int)-baseStation.height/2, (int)baseStation.height/2);
            if (searchCount > 100) {
    			goal = new Vector2(h,w);
    			searchCount = 0;
            }
    		move(goal);
    		searchCount++;
    	}
    }

	private void pickUpTarget(Collider2D human) {
		gameObject.GetComponent<Renderer>().material.color = Color.white;
		human.gameObject.SetActive(false);

        currentTarget.saved = true;
		carryingTarget = true;
	}

	private void putDownTarget() {
		gameObject.GetComponent<Renderer>().material.color = Color.blue;
		currentTarget.transform.position = transform.position;
      	currentTarget.gameObject.SetActive(true);
      	carryingTarget = false;
      	currentTarget = null;
      	Display.currentRescued++;
    }

	private void sendEnvironmentData(Collider2D other) {
		Vector2 pos = other.transform.position;
		string name = other.name;
		if (name == "obstacle")
			baseStation.uploadObstacleLocation(pos);
		else if (name == "human")
			baseStation.uploadTargetLocation(pos);
//		else if (name == "human")
//        	baseStation.uploadTargetLocation(pos);
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

	// Assign a target to this agent. Should be decided by the Base.
	public void assignTarget(Human target) {
		//currentTarget = human;
	}

	public void setBase(BaseStation baseStation) {
		this.baseStation = baseStation;
	}

	private bool isInsideEnvironment() {
		if (transform.position.x >= -baseStation.width/2-0.3f && transform.position.x <= baseStation.width/2+0.3f && transform.position.y <= baseStation.height/2+0.3f && transform.position.y >= -baseStation.height/2-0.3f)
			return true;
		return false;
	}

}
