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
    public int x ,y; // number of tiles in x and y axis

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
        grid[(int)pos.x, (int)pos.y].type = Tile.Type.OBSTACLE;
    }

    public void addGround(Vector2 pos) {

        pos = convertToGrid(pos);
        grid[(int)pos.x, (int)pos.y].type = Tile.Type.GROUND;
    }

    public Vector2 convertToGrid(Vector2 pos) {
        int x = Mathf.RoundToInt(pos.x);
		int y = Mathf.RoundToInt(pos.y);
        return new Vector2(x, y);
    }

	public int getHeight() {
		return height;
	}

	public int getWidth() {
		return width;
	}

	public bool isWalkable(int x, int y) {
		if (grid[x, y].type == Tile.Type.GROUND) {
			return true;
		} else {
			return false;
		}
	}
}
