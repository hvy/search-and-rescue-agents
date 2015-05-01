using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PathFinding
{
	public static List<GNode> currentPath {get;set;}
	
		public class Path : IEnumerable {
				public GNode LastStep { get; private set; }
				public Path PreviousSteps { get; private set; }
				public double TotalCost { get; private set; }
				public int Length { get; private set; }
				private Path (GNode lastStep, Path previousSteps, double totalCost, int length)
				{
						LastStep = lastStep;
						PreviousSteps = previousSteps;
						TotalCost = totalCost;
						Length = length;
				}
				public Path (GNode start) : this(start, null, 0, 0)
				{
				}
				public Path AddStep (GNode step, double stepCost)
				{
                        
                        // TODO A* with space-time should also add time steps in total cost here, if Pause is executed.
				if (GameState.Instance.customers.ContainsKey(step.getPos()))
							return new Path (step, this, TotalCost + stepCost + 0.0f, Length + 1); // heuristics, costs more going through waypoints.
						return new Path (step, this, TotalCost + stepCost, Length + 1);
				}
				public IEnumerator GetEnumerator ()
				{
						for (Path p = this; p != null; p = p.PreviousSteps)
								yield return p.LastStep;
				}
				IEnumerator IEnumerable.GetEnumerator ()
				{
						return this.GetEnumerator ();
				}
		}

		public class PriorityQueue<P, V> {
				private SortedDictionary<P, Queue<V>> list = new SortedDictionary<P, Queue<V>> ();
				public void Enqueue (P priority, V value) {
						Queue<V> q;
						if (!list.TryGetValue (priority, out q)) {
								q = new Queue<V> ();
								list.Add (priority, q);
						}
						q.Enqueue (value);
				}
				public V Dequeue () {
						var pair = list.First ();
						var v = pair.Value.Dequeue ();
						if (pair.Value.Count == 0)
								list.Remove (pair.Key);
						return v;
				}
				public bool IsEmpty {
						get { return !list.Any (); }
				}
		}

		static public double calculateDistance (List<GNode> completedPath) {
				double distance = 0;
				for (int i = 0; i < completedPath.Count-1; i++) {
						distance += Vector3.Distance (completedPath [i].getPos (), completedPath [i + 1].getPos ());
				}
				return distance;
		}

		// A-STAR
		// TODO write estimate function. Just make it the distance from the point to the goal.
		static public List<GNode> aStarPath (
		GNode start, 
		GNode goal, 
		Func<GNode, GNode, double> distance)
		{
				HashSet<GNode> closed = new HashSet<GNode> (); // closed set
				PriorityQueue<double, Path> open = new PriorityQueue<double, Path> (); // open set

				open.Enqueue (0, new Path (start));
				while (!open.IsEmpty) {
						Path path = open.Dequeue ();
						if (closed.Contains (path.LastStep))
								continue;

						if (path.LastStep.Equals (goal)) {
								List<GNode> completePath = new List<GNode> ();
								foreach (GNode node in path)
										completePath.Add (node);
								currentPath = completePath;
								return completePath;
						}
						closed.Add (path.LastStep);
						foreach (GNode n in path.LastStep.getNeighbors()) {
								double d = distance (path.LastStep, n);
								Path newPath = path.AddStep (n, d);
				open.Enqueue (newPath.TotalCost + Vector2.Distance (n.getPos (), goal.getPos ()), newPath);
						}
				}
				return null;
		}

		public static void draw (List<GNode> p)
		{
				GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera");
				Renderer renderer;
		
				if (!(renderer = camera.GetComponent<Renderer> ()))
						renderer = camera.AddComponent<Renderer> ();
				if (renderer.paths == null)
					renderer.paths = new List<List<GNode>>();

				if (renderer.colors == null)
					renderer.colors = new List<Color>();

				renderer.paths.Add(p);
				float r = UnityEngine.Random.Range(0.0f, 1f);
				float b = UnityEngine.Random.Range(0.0f, 1f);
				float g = UnityEngine.Random.Range(0.0f, 1f);
				Color col = new Color (r, g, b, 1.0f);
				renderer.colors.Add(col);
		}

	public static void clearDrawnPaths() {
		GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera");
		Renderer renderer;
		
		if (!(renderer = camera.GetComponent<Renderer> ()))
			renderer = camera.AddComponent<Renderer> ();

		renderer.clear();


	}

	public static bool isInObstacle (Vector2 pos, List<Vector2[]> polygons)
	{
		foreach (Vector2[] vertices in polygons) {
			if (containsPoint (vertices, pos))
				return true;
		}
		
		return false;
		
	}
	
	private static bool containsPoint (Vector2[] polyPoints, Vector2 p)
	{ 
		int j = polyPoints.Length - 1; 
		bool inside = false; 
		for (int i = 0; i < polyPoints.Length; j = i++) { 
			if (((polyPoints [i].y <= p.y && p.y < polyPoints [j].y) || (polyPoints [j].y <= p.y && p.y < polyPoints [i].y)) && 
			    (p.x < (polyPoints [j].x - polyPoints [i].x) * (p.y - polyPoints [i].y) / (polyPoints [j].y - polyPoints [i].y) + polyPoints [i].x)) 
				inside = !inside; 
			
			if (DistancePointLine (p, polyPoints [i], polyPoints [(i + 1) % polyPoints.Length]) < 0f) {
				return true;
			}
		} 
		return inside; 
	}

	private static float DistancePointLine (Vector3 point, Vector3 lineStart, Vector3 lineEnd)
	{
		return Vector3.Magnitude (ProjectPointLine (point, lineStart, lineEnd) - point);
	}

	private static Vector3 ProjectPointLine (Vector3 point, Vector3 lineStart, Vector3 lineEnd)
	{
		Vector3 rhs = point - lineStart;
		Vector3 vector2 = lineEnd - lineStart;
		float magnitude = vector2.magnitude;
		Vector3 lhs = vector2;
		if (magnitude > 1E-06f) {
			lhs = (Vector3)(lhs / magnitude);
		}
		float num2 = Mathf.Clamp (Vector3.Dot (lhs, rhs), 0f, magnitude);
		return (lineStart + ((Vector3)(lhs * num2)));
	}
	
	public static void draw (List<GNode> p, Color color)
	{
		GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera");
		Renderer renderer;
		
		if (!(renderer = camera.GetComponent<Renderer> ()))
			renderer = camera.AddComponent<Renderer> ();
		if (renderer.paths == null)
			renderer.paths = new List<List<GNode>>();
		
		if (renderer.colors == null)
			renderer.colors = new List<Color>();
		
		renderer.paths.Add(p);
		renderer.colors.Add(color);
	}
	
}