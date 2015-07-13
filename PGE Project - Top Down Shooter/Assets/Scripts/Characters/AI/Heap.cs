using UnityEngine;
using System.Collections;
using System;

public class Heap<T> where T : IHeapItem<T> {
	T[] items;
	int currentItemCount;

	public Heap(int maxHeapSize) {
		items = new T[maxHeapSize];
	}

	public void Add(T _item) {
		_item.HeapIndex = currentItemCount;
		items[currentItemCount] = _item;
		SortUp(_item);
		++currentItemCount;
	}

	public T RemoveFirst() {
		T firstItem = items[0];
		--currentItemCount;
		items[0] = items[currentItemCount];
		items[0].HeapIndex = 0;
		SortDown(items[0]);

		return firstItem;
	}
	
	public void UpdateItem(T _item) {
		SortUp(_item);
	}
	
	public bool Contains(T _item) {
		return Equals(items[_item.HeapIndex], _item);
	}

	public int Count {
		get {
			return currentItemCount;
		}
	}


	void SortDown(T _item) {
		while(true) {
			int childIndexL = _item.HeapIndex*2 + 1;
			int childIndexR = childIndexL + 1;
			int swapIndex = 0;

			if(childIndexL < currentItemCount) {
				swapIndex = childIndexL;

				if(childIndexR < currentItemCount) {
					if(items[childIndexL].CompareTo(items[childIndexR]) < 0) 
						swapIndex = childIndexR;
				}

				if(_item.CompareTo(items[swapIndex]) < 0) 
					Swap(_item, items[swapIndex]);
				else 
					return;
			}
			else 
				return;
		}
	}

	void SortUp(T _item) {
		int parentIndex = (_item.HeapIndex-1)/2;

		while(true){
			T parentItem = items[parentIndex];

			if(_item.CompareTo(parentItem) > 0)
				Swap(_item, parentItem);
			else
				break;

			parentIndex = (_item.HeapIndex-1)/2;
		}
	}

	void Swap(T itemA, T itemB) {
		items[itemA.HeapIndex] = itemB;
		items[itemB.HeapIndex] = itemA;

		int itemAIndex = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = itemAIndex;
	}
}

public interface IHeapItem<T> : IComparable<T> {
	int HeapIndex {
		get;
		set;
	}
}