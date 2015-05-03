using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour {

	public float velocity;
	public float rotationSpeed;
	public float collisionDistance;

    private bool carryingTarget; // Agent is carrying a target
    private Human currentTarget; // Target to rescue
    public Vector2 goal; // current position to move toward
   	public Vector2 start;


    private List<GNode> path; // path to current goal (if exists)

	// Use this for initialization
	void Start () {
    	carryingTarget = false;
    	path = new List<GNode>();
    	start = transform.position;
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
            if (Vector3.Distance(transform.position, other.transform.position) < 0.1f && !((Human)other.gameObject.GetComponent(typeof(Human))).saved) {
				pickUpTarget(other);
            }
        }

    }

    private void move (Vector2 g) {
    	collisionAvoidance(g);
	}

    private void search() {
    	// FIXME
    	move(new Vector2(0,0));
    }

	private void pickUpTarget(Collider2D human) {
		gameObject.GetComponent<Renderer>().material.color = Color.white;
		human.gameObject.SetActive(false);
        currentTarget = (Human) human.gameObject.GetComponent(typeof(Human));
        currentTarget.saved = true;
		carryingTarget = true;
	}

	private void putDownTarget() {
		gameObject.GetComponent<Renderer>().material.color = Color.blue;
		currentTarget.transform.position = transform.position;
      	currentTarget.gameObject.SetActive(true);
      	carryingTarget = false;
      	currentTarget = null;
    }

	private void sendEnvironmentData(Collider2D other) {
		Vector2 pos = other.transform.position;
		string name = other.name;
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
		// FIXME call BaseStation to get cloest entrance
		return start;
	}

	// Assign a target to this agent. Should be decided by the Base.
	public void assignTarget(Human target) {
		//currentTarget = human;
	}

}
