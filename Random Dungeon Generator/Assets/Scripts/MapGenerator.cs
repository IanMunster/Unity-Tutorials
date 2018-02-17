using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

	public int height;
	public int width;

	public int smooth = 5;
	public int timeSmooth = 5;
	public Vector2 randomSmoothTime = 5;
	//
	public string seed;
	public bool useRandomSeed;
	//FillPercentage
	[Range(0,100)]
	public int randomFillPercent;
	// 1 is filled 0 not filled
	int[,] map;


	private void Start () {
		GenerateMap ();
	}


	void GenerateMap () {
		// Create new Map Array
		map = new int[width, height];
		RandomFillMap ();

		for (int i = 0; i < smooth; i++) {
			SmoothMap ();
		}

	}


	void RandomFillMap () {
		if (useRandomSeed) {
			seed = Time.time.ToString();
		}

		System.Random pseudoRandomGenerator = new System.Random (seed.GetHashCode());

		// Go through all Map Tiles
		for (int x = 0; x < width; x++) {
			
			for (int y = 0; y < height; y++) {

				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					map [x, y] = 1;
				} else {
					map[x,y] = (pseudoRandomGenerator.Next(0,100) < randomFillPercent) ? 1 : 0;
				}
			}
		}
	}


	void SmoothMap () {
		// Go through all Map Tiles
		for (int x = 0; x < width; x++) {

			for (int y = 0; y < height; y++) {

				int neighbourWallTiles = GetNeighbourWall (x,y);

				if (neighbourWallTiles > randomSmoothTimes.x) {
					map [x,y] = 1;
				} else if (neighbourWallTiles < randomSmoothTimes.y) {
					map[x,y] = 0;
				}
			}
		}
	}


	int GetNeighbourWall (int gridX, int gridY) {
		int wallCount = 0;
		for (int neightbourX = gridX - 1; neightbourX <= gridX + 1; neightbourX++) {
			for (int neightbourY = gridY - 1; neightbourY <= gridY + 1; neightbourY++) {
				if (neightbourX >= 0 && neightbourX < width && neightbourY >= 0 && neightbourY < height) {
					if (neightbourX != gridX || neightbourY != gridY) {
						wallCount += map [neightbourX, neightbourY];
					}
				} else {
					wallCount++;
				}
			}
		}
		return wallCount;
	}


	void OnDrawGizmos () {
		if (map != null) {
			for (int x = 0; x < width; x++) {

				for (int y = 0; y < height; y++) {

					Gizmos.color = (map[x,y] == 1) ? Color.black : Color.white;
					Vector3 position = new Vector3 (-width*0.5f+x+0.5f, 0f, -height*0.5f+y+0.5f);
					Gizmos.DrawCube (position, Vector3.one);
				}
			}
		}
	}

}