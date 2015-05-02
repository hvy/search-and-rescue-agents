using UnityEngine;
using System.Collections;

public class AgentFactory : MonoBehaviour {

	public static void spawnAgentAt (Vector3 pos) {
		Transform prefab = Resources.Load("Robot Kyle/Model/Robot Kyle", typeof(Transform)) as Transform;
		Transform agent = GameObject.Instantiate (prefab, pos, Quaternion.LookRotation (Vector3.up)) as Transform;
		Debug.Log (agent);
		agent.parent = GameObject.Find ("_Agents").transform;
		agent.gameObject.name = "agent";
	}
}