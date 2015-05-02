using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Environment {

	public List<Human> humans;
<<<<<<< HEAD
	public List<Vector2> obstacles;
    public List<Vector2> entrances;

	public Environment (List<Human> humans, List<Vector2> obstacles, List<Vector2> entrances) {
        this.humans = humans;
        this.obstacles = obstacles;
        this.entrances = entrances;
=======
	public List<GameObject> gameObjects;

	public Environment () {
		
	}
	
	public void delete () {
		foreach (GameObject gameObject in gameObjects) {
			GameObject.Destroy(gameObject);
		}
>>>>>>> origin/master
	}
}
