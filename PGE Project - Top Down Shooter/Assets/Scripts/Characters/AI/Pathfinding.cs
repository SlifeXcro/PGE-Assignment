using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour {

	public Transform seeker, target;

	Grid grid;

	void Start() {
		grid = GetComponent<Grid> ();
	}

	void Update() {
		FindPath(seeker.position, target.position);
	}


	void FindPath(Vector3 startPos, Vector3 targetPos) {
		Node startNode = grid.NodeFromWorldPoint(startPos);		// node AI is at
		Node targetNode = grid.NodeFromWorldPoint(targetPos);	// node to travel to

		List<Node> openSet = new List<Node>();					// set of nodes to be evaluated
		HashSet<Node> closedSet = new HashSet<Node>();			// set of nodes alr evaluated
		openSet.Add(startNode);

		while(openSet.Count > 0)
		{
			Node currentNode = openSet[0];
			for(int i = 1; i < openSet.Count; ++i)
			{
				if(openSet[i].fCost <= currentNode.fCost  
				&& openSet[i].hCost < currentNode.hCost) {
					currentNode = openSet[i];
				}
			}

			openSet.Remove(currentNode);
			closedSet.Add(currentNode);

			if(currentNode == targetNode)	// path found
			{
				RetracePath(startNode, targetNode);
				return;
			}

			// loop thru neighbours of current node
			foreach(Node neighbour in grid.GetNeighbours(currentNode)) {
				if(!neighbour.walkable || closedSet.Contains(neighbour)) {	// if neighbour is obstacle or closed
					continue;
				}

				int newMovementCostToNeighbour = currentNode.gCost + GetDist(currentNode, neighbour);
				// if new path to neighbour shorter / neighbour is not in open
				if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDist(neighbour, targetNode);
					neighbour.parent = currentNode;

					if(!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
	}

	void RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node> ();
		Node currentNode = endNode;

		while(currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();

		grid.path = path;
	}

	// returns dist from node a to b
	int GetDist(Node nodeA, Node nodeB) {
		int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if(distX > distY)
			return 14*distY + 10*(distX-distY);
		return 14*distX + 10*(distY-distX);
	}
}
