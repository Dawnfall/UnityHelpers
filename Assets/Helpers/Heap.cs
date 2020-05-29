using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dawnfall.Helper
{
    /// <summary>
    /// min heap with custom comparer class
    /// </summary>
    /// <typeparam name="T"> Iheapable type </typeparam>
    /// <typeparam name="D"> IComparer type </typeparam>
    public class Heap<T> where T : class, IHeapable, System.IComparable<T>
    {
        T[] m_items;
        int m_maxSize;
        int m_freeIndex;

        //TODO: add Replace to improve remove+add; can be used in pathfinding
        public Heap(int maxSize)
        {
            if (maxSize <= 0)
                throw new System.Exception();

            m_maxSize = maxSize;
            m_items = new T[m_maxSize];
            m_freeIndex = 0;
        }

        /// <summary>
        /// total number of items in this heap
        /// </summary>
        public int Count
        {
            get { return m_freeIndex; }
        }

        /// <summary>
        /// get heap root(min) without removing it
        /// </summary>
        /// <returns> heap root(min) </returns>
        public T Peek()
        {
            return m_items[0];
        }

        /// <summary>
        /// get heap root(min) after removing it from heap
        /// </summary>
        /// <returns></returns>
        public T Extract()
        {
            T root = m_items[0];
            RemoveRoot();
            return root;
        }

        /// <summary>
        /// adds new item to the heap, and restoring its properties
        /// </summary>
        /// <param name="newItem"></param>
        public void Add(T newItem)
        {
            newItem.HeapIndex = m_freeIndex;
            m_items[m_freeIndex] = newItem;

            SortUp(newItem);
            m_freeIndex++;
        }

        /// <summary>
        /// removes heap root(min) and restores heap properties
        /// </summary>
        public void RemoveRoot()
        {
            m_freeIndex--;
            T newFirst = m_items[0] = m_items[m_freeIndex];
            newFirst.HeapIndex = 0;

            SortDown(newFirst);
        }

        /// <summary>
        /// checks if given item is member of this heap
        /// </summary>
        /// <param name="item"> item to be checked </param>
        /// <returns> bool ; true if item is in this heap </returns>
        public bool Contains(T item)
        {
            return Equals(m_items[item.HeapIndex], item);
        }

        /// <summary>
        /// updates heap for given node if its priority changed ; only works for increased priorities!
        /// </summary>
        /// <param name="item"> item to be updated </param>
        public void Update(T item)
        {
            SortUp(item);
        }

        /// <summary>
        /// recursivly heapifies item upwards so that it satisfies heap properties
        /// </summary>
        /// <param name="item"> item to be heapified up </param>
        private void SortUp(T item)
        {
            T parent = m_items[(item.HeapIndex - 1) / 2];
            //if (item.CompareTo(parent) > 0)
            if (item.CompareTo(parent) > 0)
            {
                Swap(item, parent);
                SortUp(item);
            }
        }

        /// <summary>
        /// recursivly heapifies item downwards so that it sarisfies heap properties
        /// </summary>
        /// <param name="item"> item to be heapified down </param>
        private void SortDown(T item)
        {
            int leftChildIndex = item.HeapIndex * 2 + 1;
            int rightChildIndex = leftChildIndex++;

            if (leftChildIndex >= m_freeIndex)
                return;

            int greaterChild = leftChildIndex;

            if (rightChildIndex < m_freeIndex && m_items[rightChildIndex].CompareTo(m_items[leftChildIndex]) > 0)
                greaterChild = rightChildIndex;

            if (m_items[greaterChild].CompareTo(item) > 0)
            {
                Swap(item, m_items[greaterChild]);
                SortDown(item);
            }
        }

        /// <summary>
        /// swaps two node in a heap
        /// </summary>
        /// <param name="a"> first node to be swaped </param>
        /// <param name="b"> second node to be swaped </param>
        private void Swap(T a, T b)
        {
            m_items[a.HeapIndex] = b;
            m_items[b.HeapIndex] = a;

            int tempIndex = a.HeapIndex;
            a.HeapIndex = b.HeapIndex;
            b.HeapIndex = tempIndex;
        }
    }

    public interface IHeapable
    {
        int HeapIndex { get; set; }
    }
}