using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {
	public bool displayGrid = false;
	public LayerMask unwalkableMask;
	public Vector3 gridWorldSize;
	public float nodeRadius;			// size of 1 node
	Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;			// no. of rows/columns 

	void Awake() {
		nodeRadius *= AspectRatio.Instance.Scale;					// adjust according to aspect ratio
		nodeDiameter = nodeRadius*2; 
		gridWorldSize.x *= AspectRatio.Instance.Scale;				// adjust according to aspect ratio
		gridWorldSize.y *= AspectRatio.Instance.Scale;				// adjust according to aspect ratio
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		CreateGrid();
	}
	public int MaxSize {
		get { 
			return gridSizeX*gridSizeY;
		}
	}

	void CreateGrid() {
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBtmLeft = transform.position - Vector3.right*gridWorldSize.x/2 - Vector3.up*gridWorldSize.y/2;

		for(int x = 0; x < gridSizeX; ++x)
		{
			for(int y = 0; y < gridSizeY; ++y)
			{
				Vector3 worldPoint = worldBtmLeft + Vector3.right*(x*nodeDiameter + nodeRadius) 
									+ Vector3.up*(y*nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
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
				if(x == 0 && y == 0)						// ctr (current block is node given)
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
	
#if UNITY_EDITOR
	// grid visualization
	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position, (new Vector3(gridWorldSize.x, gridWorldSize.y, 1)));
		if(grid != null && displayGrid)
		{
			foreach(Node n in grid)
			{
				if(n.walkable)
					Gizmos.color = new Color(1,1,1,0.3f);
				else
					Gizmos.color = new Color(1,0,0,0.3f);

				//Vector2 gridOffset = new Vector2(transform.position.x, transform.position.y);
				Gizmos.DrawCube(n.worldPos/*-gridOffset*/, transform.root.localScale*(nodeDiameter-0.1f));
			}
		}
	}
#endif
}
