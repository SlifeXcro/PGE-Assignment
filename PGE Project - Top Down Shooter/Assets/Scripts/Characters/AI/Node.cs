using UnityEngine;
using System.Collections;

public class Node {

	public bool walkable;		// if true, grid is walkable
	public Vector2 worldPos;	// pos of grid in world

	public int gridX, gridY;	// current node index pos in grid
	public int gCost;			// dist from starting node
	public int hCost;			// dist from end(targeted) node
	public Node parent;

	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY) {
		walkable = _walkable;
		worldPos = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	}

	public int fCost {
		get {
			return gCost + hCost;
		}
	}
}