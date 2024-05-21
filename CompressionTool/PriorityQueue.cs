using System;
using System.Collections.Generic;

namespace CompressionTool
{
    /// <summary>
    /// Represents a priority queue (min-heap) data structure.
    /// </summary>
    /// <typeparam name="T">The type of elements in the queue.</typeparam>
    public class PriorityQueue<T>
    {
        private List<T> heap;
        private Comparison<T> comparison;

        /// <summary>
        /// Initializes a new instance of the PriorityQueue class with the specified elements and comparison function.
        /// </summary>
        /// <param name="elements">The elements to initialize the queue with.</param>
        /// <param name="comparison">The comparison function used to order elements in the queue.</param>
        public PriorityQueue(IEnumerable<T> elements, Comparison<T> comparison)
        {
            this.comparison = comparison;

            // Initialize the heap with the provided elements
            heap = new List<T>(elements);

            // Build the heap by heapifying down from the middle to the root
            for (int i = heap.Count / 2 - 1; i >= 0; i--)
            {
                HeapifyDown(i);
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the queue.
        /// </summary>
        public int Count => heap.Count;

        /// <summary>
        /// Adds an element to the queue.
        /// </summary>
        /// <param name="element">The element to add to the queue.</param>
        public void Enqueue(T element)
        {
            // Add the new element to the end of the heap
            heap.Add(element);

            // Restore the heap property by heapifying up from the last element
            HeapifyUp(heap.Count - 1);
        }

        /// <summary>
        /// Removes and returns the element with the lowest priority in the queue.
        /// </summary>
        /// <returns>The element with the lowest priority.</returns>
        public T Dequeue()
        {
            if (heap.Count == 0) throw new InvalidOperationException("Queue is empty");

            // Get the element with the lowest priority (root of the heap)
            T min = heap[0];

            // Replace the root with the last element
            heap[0] = heap[heap.Count - 1];

            // Remove the last element
            heap.RemoveAt(heap.Count - 1);

            // Restore the heap property by heapifying down from the root
            HeapifyDown(0);

            return min;
        }

        private void HeapifyUp(int i)
        {
            // Heapify up until the heap property is restored
            while (i > 0)
            {
                int parent = (i - 1) / 2;

                // If the current element is not less than its parent, the heap property is restored
                if (comparison(heap[i], heap[parent]) >= 0) break;

                // Swap the current element with its parent
                Swap(i, parent);

                // Move up to the parent index
                i = parent;
            }
        }

        private void HeapifyDown(int i)
        {
            // Heapify down until the heap property is restored
            while (true)
            {
                int left = 2 * i + 1;
                int right = 2 * i + 2;
                int smallest = i;

                // Find the smallest among the current element and its children
                if (left < heap.Count && comparison(heap[left], heap[smallest]) < 0) smallest = left;
                if (right < heap.Count && comparison(heap[right], heap[smallest]) < 0) smallest = right;

                // If the current element is the smallest, the heap property is restored
                if (smallest == i) break;

                // Swap the current element with the smallest child
                Swap(i, smallest);

                // Move down to the smallest child index
                i = smallest;
            }
        }

        private void Swap(int i, int j)
        {
            // Swap elements at indices i and j
            T temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }
    }
}
