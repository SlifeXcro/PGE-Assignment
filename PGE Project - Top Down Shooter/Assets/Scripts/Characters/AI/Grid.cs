using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {
	public bool displayGrid = false;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;			// size of 1 node
	Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;			// no. of rows/columns 

	void Start() {
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		CreateGrid();
	}

	void CreateGrid() {
		grid = new Node[gridSizeX, gridSizeY];
		Vector2 worldBtmLeft = new Vector2(transform.position.x, transform.position.y)
								 - Vector2.right*gridWorldSize.x/2 - Vector2.up*gridWorldSize.y/2;


		for(int x = 0; x < gridSizeX; ++x)
		{
			for(int y = 0; y < gridSizeY; ++y)
			{
				Vector2 worldPoint = worldBtmLeft + Vector2.right*(x*nodeDiameter + nodeRadius) + Vector2.up*(y*nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(new Vector3(worldPoint.x, worldPoint.y, 0), 
				                                      nodeRadius, unwalkableMask));
				grid[x,y] = new Node(walkable, worldPoint, x, y);
			}
		}
	}

	public List<Node> GetNeighbours(Node node) {			// returns a list of neighbour nodes
		List<Node> neighbours = new List<Node> ();

		// search 3x3 block
		for(int x = -1; x <= 1; ++x)
		{
			for(int y = -1; y <= 1; ++y)
			{
				if(x == 0 && y == 0)		// ctr (current block is node given)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if(checkX >= 0 && checkX < gridSizeX 		// if node is valid
				&& checkY >= 0 && checkY < gridSizeY) {
					neighbours.Add(grid[checkX, checkY]);
				}
			}
		}

		return neighbours;
	}

	public Node NodeFromWorldPoint(Vector3 worldPos) {
		float percentX = (worldPos.x + gridWorldSize.x / 2)/gridWorldSize.x;
		float percentY = (worldPos.y + gridWorldSize.y / 2)/gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);

		return grid[x,y];
	}

	public List<Node> path;
	void OnDrawGizmos() {
		if(displayGrid)
		{
			Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

			if(grid != null)
			{
				foreach(Node n in grid)
				{
					Gizmos.color = (n.walkable)? Color.white : Color.red;
					if(path != null)
					{
						if(path.Contains(n))
							Gizmos.color = Color.black;
					}
					Gizmos.DrawCube(n.worldPos, Vector3.one*(nodeDiameter-0.1f));
				}
			}
		}
	}
}
