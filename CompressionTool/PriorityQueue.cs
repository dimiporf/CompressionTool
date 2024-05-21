using System;
using System.Collections.Generic;

namespace CompressionTool
{
    public class PriorityQueue<T>
    {
        private List<T> heap;
        private Comparison<T> comparison;

        public PriorityQueue(IEnumerable<T> elements, Comparison<T> comparison)
        {
            this.comparison = comparison;
            heap = new List<T>(elements);
            for (int i = heap.Count / 2 - 1; i >= 0; i--)
            {
                HeapifyDown(i);
            }
        }

        public int Count => heap.Count;

        public void Enqueue(T element)
        {
            heap.Add(element);
            HeapifyUp(heap.Count - 1);
        }

        public T Dequeue()
        {
            if (heap.Count == 0) throw new InvalidOperationException("Queue is empty");
            T min = heap[0];
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            HeapifyDown(0);
            return min;
        }

        private void HeapifyUp(int i)
        {
            while (i > 0)
            {
                int parent = (i - 1) / 2;
                if (comparison(heap[i], heap[parent]) >= 0) break;
                Swap(i, parent);
                i = parent;
            }
        }

        private void HeapifyDown(int i)
        {
            while (true)
            {
                int left = 2 * i + 1;
                int right = 2 * i + 2;
                int smallest = i;

                if (left < heap.Count && comparison(heap[left], heap[smallest]) < 0) smallest = left;
                if (right < heap.Count && comparison(heap[right], heap[smallest]) < 0) smallest = right;

                if (smallest == i) break;

                Swap(i, smallest);
                i = smallest;
            }
        }

        private void Swap(int i, int j)
        {
            T temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }
    }
}
