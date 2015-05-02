using UnityEngine;
using System.Collections;

public class ObstacleFactory : MonoBehaviour {

	public static Vector2 spawnObstacleAt (Vector2 pos) {
		Transform prefab = Resources.Load("Prefabs/Obstacle", typeof(Transform)) as Transform;
		Quaternion rot = Quaternion.LookRotation (Vector3.up);

		GameObject.Instantiate (prefab, pos, rot);
        return pos;
	}
}
