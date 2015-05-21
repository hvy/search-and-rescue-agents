using UnityEngine;
using System.Collections;

public class AgentFactory : MonoBehaviour {

	public static Agent spawnAgentAt (Vector3 pos) {

//		Transform prefab = Resources.Load("Robot Kyle/Model/Robot Kyle", typeof(Transform)) as Transform;
		Transform prefab = Resources.Load("Prefabs/Agent", typeof(Transform)) as Transform;
		Transform agent = GameObject.Instantiate (prefab, pos, Quaternion.LookRotation (Vector3.up)) as Transform;
//		Debug.Log (agent);
		agent.SetParent (GameObject.Find ("_Agents").transform);
		agent.gameObject.name = "agent";
		agent.gameObject.GetComponent<Renderer>().material.color = Color.blue;

		return (Agent) agent.gameObject.GetComponent("Agent");
	}
}