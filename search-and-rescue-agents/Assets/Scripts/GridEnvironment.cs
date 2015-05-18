using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * One instance maintained by the BaseStation
 */
public class GridEnvironment {

    public int height, width;
    public Tile[,] grid;
    public float tileSize;

    // UNKNOWN as default
    public class Tile {

		public enum Type {
			UNKNOWN,
			OBSTACLE,
			HUMAN,
			GROUND
		}

		int x, y;
        public Type type;

        public Tile(int x, int y) {
			this.x = x;
			this.y = y;
			type = Tile.Type.UNKNOWN;
        }

        public bool isInTile(Vector2 pos) {
			if (pos.x >= x - 0.5f && pos.x < x + 0.5f && pos.y >= y - 0.5f && pos.y < y + 0.5f)
                return true;
            return false;
        }
    }

    public GridEnvironment(int height, int width) {
        
		this.height = height;
        this.width = width;

        grid = new Tile[width, height];

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                grid[i,j] = new Tile(i, j);
            }
        }
    }

    public void addHuman(Vector2 pos) {
        pos = convertToGrid(pos);
        grid[(int)pos.x, (int)pos.y].type = Tile.Type.HUMAN;
    }

    public void addObstacle(Vector2 pos) {
        pos = convertToGrid(pos);

//		Debug.Log ("======================================");
//		Debug.Log ((int)pos.x);
//		Debug.Log ((int)pos.y);

        grid[(int)pos.x, (int)pos.y].type = Tile.Type.OBSTACLE;
    }

    public void addGround(Vector2 pos) {
        pos = convertToGrid(pos);
        grid[(int)pos.x, (int)pos.y].type = Tile.Type.GROUND;
    }

    public Vector2 convertToGrid(Vector2 pos) {
		float flooredX = Mathf.Floor (pos.x);
		float flooredY = Mathf.Floor (pos.y);
		int x = Mathf.Abs (pos.x - flooredX) >= 0.5f ? Mathf.CeilToInt (pos.x) : Mathf.FloorToInt (pos.x);
		int y = Mathf.Abs (pos.y - flooredY) >= 0.5f ? Mathf.CeilToInt (pos.y) : Mathf.FloorToInt (pos.y);

        //int x = Mathf.RoundToInt(pos.x);
		//int y = Mathf.RoundToInt(pos.y);

		// Since Mathf.RoundToInt may round up or down on .5 depending on if the number is even or odd
		// x can potentially equal width and y can potentially qual y. 
		//if (x >= width) x = width - 1;
		//if (y >= height) x = height - 1;

        return new Vector2(x, y);
    }

	public int getHeight() {
		return height;
	}

	public int getWidth() {
		return width;
	}

	public bool isUnknown(int x, int y) {
	    if (grid[x, y].type == Tile.Type.UNKNOWN)
	        return true;
	    return false;
	}

	public bool isEdge(int x, int y) {
	    if (this.width-1 <= x || this.height-1 <= y || x <= 0 || y <= 0)
	        return false;


	    if (grid[x+1,y].type == Tile.Type.UNKNOWN || grid[x,y+1].type == Tile.Type.UNKNOWN || grid[x-1,y].type == Tile.Type.UNKNOWN || grid[x,y-1].type == Tile.Type.UNKNOWN)
	        return true;

	    return false;

	}

	public bool isWalkable(int x, int y) {
		if (grid[x, y].type == Tile.Type.GROUND || grid[x, y].type == Tile.Type.HUMAN) {
			return true;
		} else {
			return false;
		}
	}
}
