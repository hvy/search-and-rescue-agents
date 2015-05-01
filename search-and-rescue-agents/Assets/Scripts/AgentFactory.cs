using UnityEngine;
using System.Collections;

public class AgentFactory : MonoBehaviour {

	public static Transform agentPrefab;

	public static void spawnAgentAt (Vector3 pos) {
		Transform prefab = Resources.Load("Prefabs/Agent", typeof(Transform)) as Transform;
		GameObject.Instantiate (prefab, pos, Quaternion.identity);
	}
}
