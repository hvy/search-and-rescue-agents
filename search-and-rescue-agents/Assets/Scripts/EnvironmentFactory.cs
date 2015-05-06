using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentFactory {

	public static Environment createBasicEnvironment () {

		// Create ground
		int width = 20;
		int height = 30;
		GameObject ground = GameObject.CreatePrimitive (PrimitiveType.Cube);
		ground.transform.position = new Vector3 (0, 0, 1.0f);
		ground.transform.localScale = new Vector3 (width, height, 1);
		ground.transform.parent = GameObject.Find ("_Environment").transform;
		ground.name = "ground";
		ground.GetComponent<Renderer>().material.color = new Color(0.43f, 0.54f, 0.56f, 1f);

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
//		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-9)));
//		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-10)));
//		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-11)));
//		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-12)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-13)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-14)));


		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2,2)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2,-7)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2,-8)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2,-9)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2,-10)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2,-11)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2,-12)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2,-13)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2,-14)));



		// Outer lower wall
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-9,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-8,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-7,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-6,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-5,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-4,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-3,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-1,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(0,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(1,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(2,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(3,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(5,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(6,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(7,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(8,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(9,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10,-15)));

		// Outer left wall
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10,-15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10,-14)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10,-13)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10,-12)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10,-11)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10,-10)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10,-9)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10,-8)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10,-7)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10,-6)));
//		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10,-5)));
//		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10,-4)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10,-3)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10,-2)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10,-1)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10, 0)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10, 1)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10, 2)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10, 3)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10, 4)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10, 5)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10, 6)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10, 7)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10, 8)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10, 9)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10, 10)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10, 11)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10, 12)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10, 13)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10, 14)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-10, 15)));

		// Outer upper wall
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-9, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-8, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-7, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-6, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-5, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-4, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-3, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-2, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(-1, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(0, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(1, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(2, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(3, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(4, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(5, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(6, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(7, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(8, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(9, 15)));

		// Outer right wall

		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, 15)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, 14)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, 13)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, 12)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, 11)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, 10)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, 9)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, 8)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, 7)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, 6)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, 5)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, 4)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, 3)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, 2)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, 1)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, 0)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, -1)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, -2)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, -3)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, -4)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, -5)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, -6)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, -7)));
//		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, -8)));

		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, -10)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, -11)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, -12)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, -13)));
		obstacles.Add(ObstacleFactory.spawnObstacleAt(new Vector2(10, -14)));

		/*
		// Set entrances
		List<Vector2> entrances = new List<Vector2>();

		entrances.Add(new Vector2(4,50));
		entrances.Add(new Vector2(5,50));

		entrances.Add(new Vector2(50,-2));
		entrances.Add(new Vector2(50,-3));

		entrances.Add(new Vector2(-4,-50));
		entrances.Add(new Vector2(-5,-50));

		// Set humans
		List<Human> humans = new List<Human>();
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (-4,0)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (0,-8)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (4,10)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (7,2)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (-2,10)));

		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (26,0)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (26,27)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (2,27)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (2,7)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (-23,7)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (-23,-27)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (-2,-27)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (25,-27)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (35,-27)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (35,-44)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (-23,-44)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (-23,30)));

		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (-37,21)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (-39,42)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (-39,-10)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (-39,-42)));
		*/

		List<Vector2> entrances = new List<Vector2>();

		entrances.Add(new Vector2(-10,-9));
		entrances.Add(new Vector2(-10,-8));
		entrances.Add(new Vector2(-10,-7));
		
		entrances.Add(new Vector2(10,-1));
		entrances.Add(new Vector2(10,-2));
		entrances.Add(new Vector2(10,-3));
		entrances.Add(new Vector2(10,-4));
		entrances.Add(new Vector2(10,-5));
		entrances.Add(new Vector2(10,-6));
		entrances.Add(new Vector2(10,-7));
		entrances.Add(new Vector2(10,-8));
		
		// Set humans
		List<Human> humans = new List<Human>();
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (-4,0)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (0,-8)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (4,10)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (7,2)));
		humans.Add(HumanFactory.spawnHumanAt(new Vector2 (-2,10)));


		Environment env = new Environment (height, width, humans, entrances, obstacles);

		return env;
	}
}
