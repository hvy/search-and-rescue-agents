using UnityEngine;
using System.Collections;

public class AgentFactory : MonoBehaviour {

	public static void spawnAgentAt (Vector3 pos) {
		Transform prefab = Resources.Load("Prefabs/Agent", typeof(Transform)) as Transform;
//		Transform prefab = Resources.Load("Robot Kyle/Model/Robot Kyle", typeof(Transform)) as Transform;
		Quaternion rot = Quaternion.LookRotation (Vector3.up);

		GameObject.Instantiate (prefab, pos, rot);

	}
}
