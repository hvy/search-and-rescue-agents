using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentFactory {

	public static Environment createBasicEnvironment () {

		GameObject ground = GameObject.CreatePrimitive (PrimitiveType.Cube);
		ground.transform.position = new Vector3 (0, 0, 1.0f);
		ground.transform.localScale = new Vector3 (50.0f, 50.0f, 1);
		ground.transform.parent = GameObject.Find ("_Environment").transform;
		ground.name = "ground";

		Environment env = new Environment ();

		return env;
	}
}
