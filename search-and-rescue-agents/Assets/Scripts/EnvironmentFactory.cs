using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentFactory {

	public static Environment createBasicEnvironment () {

		// Create ground
		int width = 50;
		int height = 50;
		GameObject ground = GameObject.CreatePrimitive (PrimitiveType.Cube);
		ground.transform.position = new Vector3 (0, 0, 1.0f);
		ground.transform.localScale = new Vector3 (height, width, 1);
		ground.transform.parent = GameObject.Find ("_Environment").transform;
		ground.name = "ground";
		ground.GetComponent<Renderer>().material.color = Color.black;

		// Create obstacles
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

		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-3)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-4)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-5)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-6)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-7)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-8)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-9)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-10)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-11)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-12)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-13)));


		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2,-10)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2,-9)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2,-8)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2,-7)));

		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2,2)));

		// Set entrances
		List<Vector2> entrances = new List<Vector2>();

		entrances.Add(new Vector2(-2,1));

		// Set humans
		List<Human> humans = new List<Human>();
		HumanFactory.spawnHumanAt(new Vector2 (0,0));


		Environment env = new Environment (height, width, new List<Human>(), entrances, obstacles);

		return env;
	}
}
