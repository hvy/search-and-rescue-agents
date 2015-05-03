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

        public float xStart, xEnd;
        public float yStart, yEnd;
        public Type type;

        public enum Type {
                UNKNOWN,
                OBSTACLE,
                HUMAN,
                GROUND
        }

        public Tile() {

        }

        public Tile(float xStart, float xEnd, float yStart, float yEnd) {
            this.xStart = xStart;
            this.xEnd = xEnd;
            this.yStart = yStart;
            this.yEnd = yEnd;
        }

        public bool isInTile(Vector2 pos) {
            if (pos.x >= xStart && pos.x < xEnd && pos.y >= yStart && pos.y < yEnd)
                return true;
            return false;
        }

        public void print() {
            Debug.Log("(" + xStart + "-" + xEnd + ", " + yStart + "-" + yEnd + ") "+ type);
        }

        public Vector2 getRealCoordinates(float tileSize, int width, int height) {
            float xReal = xStart*tileSize - width/2;
            float yReal = yStart*tileSize - height/2;
            return new Vector2(xReal, yReal);
        }

    }

    public GridEnvironment(int height, int width, float tileSize) {
        this.height = height;
        this.width = width;
        this.tileSize = tileSize;

        float x = width/tileSize;
        float y = height/tileSize;

        grid = new Tile[(int)x, (int)y];

        Debug.Log(x + " " + y);

        for (int i = 0; i < x; i++) {
            for (int j = 0; j < y; j++) {
                grid[i,j] = new Tile(i, i+1, j, j+1);
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

    private Vector2 convertToGrid(Vector2 pos) {
        // subtract by a little to prevent rounding up
        pos.x = pos.x+width/2 -0.001f;
        pos.y = pos.y+height/2 - 0.001f;
        return new Vector2((int)pos.x/tileSize, (int)pos.y/tileSize);
    }


}
