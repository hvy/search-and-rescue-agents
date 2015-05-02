using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Environment {

	public List<Human> humans;
	public List<GameObject> gameObjects;

	public Environment () {
		
	}
	
	public void delete () {
		foreach (GameObject gameObject in gameObjects) {
			GameObject.Destroy(gameObject);
		}
	}
}
