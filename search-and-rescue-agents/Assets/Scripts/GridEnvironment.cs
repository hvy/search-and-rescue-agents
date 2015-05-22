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
        public int C; // counter for ANT

        public Tile(int x, int y) {
			this.x = x;
			this.y = y;
			type = Tile.Type.UNKNOWN;
			C = 0;
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

		if ((int)pos.x >= width)
			pos.x = width-1;
		if ((int)pos.y >= height)
			pos.y = height-1;
		
		grid[Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y)].type = Tile.Type.HUMAN;
		grid[(int)pos.x, (int)pos.y].C++;
    }

    public void addObstacle(Vector2 pos) {
        pos = convertToGrid(pos);

        if ((int)pos.x >= width)
            pos.x = width-1;
        if ((int)pos.y >= height)
            pos.y = height-1;

        grid[Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y)].type = Tile.Type.OBSTACLE;
        grid[(int)pos.x, (int)pos.y].C = -1;
    }

    public void addGround(Vector2 pos) {
        pos = convertToGrid(pos);

        if ((int)pos.x >= width)
            pos.x = width-1;
        if ((int)pos.y >= height)
            pos.y = height-1;

        grid[(int)pos.x, (int)pos.y].type = Tile.Type.GROUND;
        grid[(int)pos.x, (int)pos.y].C++;

    }

    public void antChange(Vector2 pos, int i) {
//        pos = convertToGrid(pos);
//
//        if ((int)pos.x >= width)
//            pos.x = width-1;
//        if ((int)pos.y >= height)
//            pos.y = height-1;
//
//        grid[(int)pos.x, (int)pos.y].type = Tile.Type.GROUND;
//        if (grid[(int)pos.x, (int)pos.y].C == 0)
//            grid[(int)pos.x, (int)pos.y].C++;
//        else
    }

    public Vector2 convertToGrid(Vector2 pos) {
		float flooredX = Mathf.Floor (pos.x);
		float flooredY = Mathf.Floor (pos.y);
		int x = Mathf.Abs (pos.x - flooredX) >= 0.5f ? Mathf.CeilToInt (pos.x) : Mathf.FloorToInt (pos.x);
		int y = Mathf.Abs (pos.y - flooredY) >= 0.5f ? Mathf.CeilToInt (pos.y) : Mathf.FloorToInt (pos.y);

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
