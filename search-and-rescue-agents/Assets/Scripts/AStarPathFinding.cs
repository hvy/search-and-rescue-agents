using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarPathFinding  {

	public static List<Vector2> findPath(Vector2 from, Vector2 to, GridEnvironment env) {

		HashSet<Vector2> openSet = new HashSet<Vector2> (); // Set of nodes to evaluate
		HashSet<Vector2> closedSet = new HashSet<Vector2> (); // Set of nodes already evaluated

		Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2> ();
		Dictionary<Vector2, int> gScore = new Dictionary<Vector2, int> ();
		Dictionary<Vector2, int> fScore = new Dictionary<Vector2, int> ();

		// Initialize
		openSet.Add (from);
		gScore [from] = 0;
		fScore [from] = gScore[from] + costEstimate(from, to); // Estimate cost using the manhattan distance

		// Find the path
		while (openSet.Count > 0) {

			Vector2 current = lowestFScoreFromOpenSet(openSet, fScore); // find the Vector2 node in openSet with the lowest fScore

//			Debug.Log (current);

			if(current == to) {
				return reconstructPath(cameFrom, to);
			}

			openSet.Remove(current);
			closedSet.Add(current);

			foreach (Vector2 neighbor in neighborNodes(current, env)) {

				if(closedSet.Contains(neighbor)) {
					continue;
				}

				int tentativeGScore = gScore[current] + distanceBetween(current, neighbor, env);

				if(!openSet.Contains(neighbor) || tentativeGScore < gScore[neighbor]) {
					cameFrom[neighbor] = current;
					gScore[neighbor] = tentativeGScore;
					fScore[neighbor] = gScore[neighbor] + costEstimate(neighbor, to);

					if(!openSet.Contains(neighbor)) {
						openSet.Add(neighbor);
					}
				}
			}
		}

		return null;
	}

	// Estimate the cost from the from node to the to node using the manhattan distance.
	private static int costEstimate(Vector2 from, Vector2 to) {
		return (int) (Mathf.Abs (to.x - from.x) + Mathf.Abs (to.y - from.y));
	}

	private static List<Vector2> reconstructPath (Dictionary<Vector2, Vector2> cameFrom, Vector2 current) {
		List<Vector2> path = new List<Vector2> (); // The path that is built and returned
		path.Add (current);
		while (cameFrom.ContainsKey(current)) {
			current = cameFrom[current];
			path.Add(current);
		}

		path.Reverse ();

		return path;
	}

	// Find all neighbors to a given node. A maximum of 4 neigors can be found
	private static List<Vector2> neighborNodes(Vector2 current, GridEnvironment env) {

		List<Vector2> neighbors = new List<Vector2> ();

		if (current.x > 0) {
			if(env.isWalkable((int) current.x - 1, (int) current.y))
				neighbors.Add(new Vector2 (current.x - 1, current.y));
		}

		if (current.x < env.width - 1) {
			if(env.isWalkable((int) current.x + 1, (int) current.y))
				neighbors.Add(new Vector2 (current.x + 1, current.y));
		}

		if (current.y < env.height - 1) {
			if(env.isWalkable((int) current.x, (int) current.y + 1))
				neighbors.Add(new Vector2 (current.x, current.y + 1));
		}

		if (current.y > 0) {
			if(env.isWalkable((int) current.x, (int) current.y - 1))
				neighbors.Add (new Vector2 (current.x, current.y - 1));
		}

		return neighbors;
	}

	// The cost between two nodes lying next to each other is currently always 1
	private static int distanceBetween(Vector2 current, Vector2 neighbor, GridEnvironment env) {
		return 1;
	}

	private static Vector2 lowestFScoreFromOpenSet(HashSet<Vector2> openSet, Dictionary<Vector2, int> fScore) {

		int minFScore = int.MaxValue;
		Vector2 minFScoreNode = new Vector2(-1, -1);

		foreach (Vector2 openNode in openSet) {
			if (fScore[openNode] < minFScore) {
				minFScore = fScore[openNode];
				minFScoreNode = openNode;
			}
		}

		return minFScoreNode;
	}
}
