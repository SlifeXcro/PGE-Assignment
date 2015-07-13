using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent (typeof (Grid))]
[RequireComponent (typeof (PathRequestManager))]
public class Pathfinding : MonoBehaviour {

	//public Transform seeker, target;
	PathRequestManager requestManager;
	Grid grid;

	void Awake() {
		requestManager = GetComponent<PathRequestManager>();
		grid = GetComponent<Grid>();
	}

//	void Update() {
//		FindPath(seeker.position, target.position);
//	}

	public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
		StartCoroutine(FindPath(startPos, targetPos));
	}

	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {
		Vector3[] waypoints = new Vector3[0];
		bool pathFindSuccess = false;

		Node startNode = grid.NodeFromWorldPoint(startPos);		// node AI is at
		Node targetNode = grid.NodeFromWorldPoint(targetPos);	// node to travel to

		//if(startNode.walkable && targetNode.walkable) {
			Heap<Node> openSet = new Heap<Node>(grid.MaxSize);		// set of nodes to be evaluated
			HashSet<Node> closedSet = new HashSet<Node>();			// set of nodes alr evaluated
			openSet.Add(startNode);

			// search entire open set to find node with lowest f cost
			while(openSet.Count > 0)
			{
				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				if(currentNode == targetNode)	// path found
				{
					pathFindSuccess = true;
					break;
				}

				// loop thru neighbours of current node
				foreach(Node neighbour in grid.GetNeighbours(currentNode)) {
					if(!neighbour.walkable || closedSet.Contains(neighbour))	// if neighbour is obstacle or closed
						continue;

					int newMovementCostToNeighbour = currentNode.gCost + GetDist(currentNode, neighbour);
					// if new path to neighbour shorter / neighbour is not in open
					if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDist(neighbour, targetNode);
						neighbour.parent = currentNode;

						if(!openSet.Contains(neighbour))
							openSet.Add(neighbour);
						else
							openSet.UpdateItem(neighbour);
					}
				}
			}
		//}
		yield return null;

		if(pathFindSuccess)
			waypoints = RetracePath(startNode, targetNode);

		requestManager.FinishedProcessingPath(waypoints, pathFindSuccess);
	}

	Vector3[] RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node> ();
		Node currentNode = endNode;

		while(currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}

		Vector3[] waypoints = SimplifyPath(path);
		Array.Reverse(waypoints);

		return waypoints;
	}

	// fn. to reduce waypts - adds waypts only when path chgs dir
	Vector3[] SimplifyPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3>();
		//Vector2 directionOld = Vector2.zero;

		for(int i = 1; i < path.Count; ++i) {
			//Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX,
			//                                   path[i-1].gridY - path[i].gridY);

			//if(directionNew != directionOld) 
				waypoints.Add(path[i-1].worldPos);

			//directionOld = directionNew;
		}

		return waypoints.ToArray();
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
