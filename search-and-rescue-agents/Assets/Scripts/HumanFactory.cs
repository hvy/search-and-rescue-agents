using UnityEngine;
using System.Collections;

public class HumanFactory : MonoBehaviour {

	public static Human spawnHumanAt (Vector2 pos) {

		Transform prefab = Resources.Load("Prefabs/Human", typeof(Transform)) as Transform;
		Transform human = GameObject.Instantiate (prefab, pos, Quaternion.LookRotation (Vector3.up)) as Transform;

		human.parent = GameObject.Find ("_Humans").transform;
		human.gameObject.name = "human";
		human.gameObject.GetComponent<Renderer>().material.color = Color.yellow;

		return (Human) human.gameObject.GetComponent("Human");
	}
}