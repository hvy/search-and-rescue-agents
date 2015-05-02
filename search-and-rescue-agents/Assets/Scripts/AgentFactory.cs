using UnityEngine;
using System.Collections;

public class AgentFactory : MonoBehaviour {

	public static void spawnAgentAt (Vector3 pos) {
<<<<<<< HEAD
		Transform prefab = Resources.Load("Prefabs/Agent", typeof(Transform)) as Transform;
//		Transform prefab = Resources.Load("Robot Kyle/Model/Robot Kyle", typeof(Transform)) as Transform;
		Quaternion rot = Quaternion.LookRotation (Vector3.up);

		GameObject.Instantiate (prefab, pos, rot);

=======
		Transform prefab = Resources.Load("Robot Kyle/Model/Robot Kyle", typeof(Transform)) as Transform;
		Transform agent = GameObject.Instantiate (prefab, pos, Quaternion.LookRotation (Vector3.up)) as Transform;
		Debug.Log (agent);
		agent.parent = GameObject.Find ("_Agents").transform;
		agent.gameObject.name = "agent";
>>>>>>> origin/master
	}
}