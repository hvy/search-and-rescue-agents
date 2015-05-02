using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Environment {

	public int width, height;
	public List<Human> humans;
	public List<Vector2> entrances;
	public List<Vector2> obstacles;

	public List<GameObject> gameObjects;

	public Environment (int height, int width, List<Human> humans, List<Vector2> entrances, List<Vector2> obstacles) {
		this.height = height;
		this.width = width;
		this.humans = humans;
		this.entrances = entrances;
		this.obstacles = obstacles;
		
	}
	
	public void delete () {
		foreach (GameObject gameObject in gameObjects) {
			GameObject.Destroy(gameObject);
		}
	}
}
