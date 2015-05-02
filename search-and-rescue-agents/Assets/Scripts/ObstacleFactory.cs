using UnityEngine;
using System.Collections;

public class ObstacleFactory : MonoBehaviour {

	private static int count = 0;

	public static Vector2 spawnObstacleAt (Vector2 pos) {
		Transform prefab = Resources.Load("Prefabs/Obstacle", typeof(Transform)) as Transform;
		Quaternion rot = Quaternion.LookRotation (Vector3.up);

		Transform obstacle = GameObject.Instantiate (prefab, pos, rot) as Transform;

		obstacle.parent = GameObject.Find ("_Environment").transform;
		obstacle.gameObject.name = "obstacle";

        return pos;
	}
}
