﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentFactory {

	public static Environment createBasicEnvironment () {

<<<<<<< HEAD
		// TODO
       	List<Vector2> obstacles = new List<Vector2>();

		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2,3)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-1,3)));
        obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(0,3)));
        obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(1,3)));
        obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(2,3)));
        obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(3,3)));
        obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,3)));

		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,2)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,1)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,0)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-1)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-2)));

		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(3,-2)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(2,-2)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(1,-2)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(0,-2)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-1,-2)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2,-2)));

		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2,2)));

		List<Vector2> entrances = new List<Vector2>();

		entrances.Add(new Vector2(-2,1));


		Environment env = new Environment (new List<Human>(), obstacles, entrances);
=======
		GameObject ground = GameObject.CreatePrimitive (PrimitiveType.Cube);
		ground.transform.position = new Vector3 (0, 0, 1.0f);
		ground.transform.localScale = new Vector3 (50.0f, 50.0f, 1);
		ground.transform.parent = GameObject.Find ("_Environment").transform;
		ground.name = "ground";

		Environment env = new Environment ();
>>>>>>> origin/master

		return env;
	}
}
