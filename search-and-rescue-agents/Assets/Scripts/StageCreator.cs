using UnityEngine;
using System.Collections;

public class StageCreator : MonoBehaviour {

	private int width, height;

	void Start () {
		scanEnvironment ();
		createEmptyTileColliders ();
	}
	

	void Update () {
	
	}

	private void scanEnvironment() {
		// Find the width and the height of the environment. Assume that its bottom left corner is positioned at (0, 0).
		GameObject envBase = GameObject.Find ("EnvironmentBase");
		width = (int) envBase.transform.localScale.x;
		height = (int) envBase.transform.localScale.y;
	}

	private void createEmptyTileColliders() {

		Transform prefab = Resources.Load("Prefabs/Empty", typeof(Transform)) as Transform;
		Transform tileContainer = GameObject.Find ("_EmptyTiles").transform;

		bool[,] occupiedTile = new bool[width, height];
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < width; y++) {
				occupiedTile[x, y] = false;	
			}
		}

		GameObject[] obstacles = GameObject.FindGameObjectsWithTag ("Obstacle");
		GameObject[] humans = GameObject.FindGameObjectsWithTag ("Human");

		foreach (GameObject o in obstacles) {
			occupiedTile[(int) o.transform.position.x, (int) o.transform.position.y] = true;
		}

		foreach (GameObject h in humans) {
			occupiedTile[(int) h.transform.position.x, (int) h.transform.position.y] = true;
		}

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < width; y++) {
				if(!occupiedTile[x, y]) {					
					Transform emptyTile = GameObject.Instantiate (prefab, new Vector2(x, y), Quaternion.identity) as Transform;
					emptyTile.SetParent(tileContainer);
					emptyTile.gameObject.name = "empty";
				}
			}
		}
	}
}
