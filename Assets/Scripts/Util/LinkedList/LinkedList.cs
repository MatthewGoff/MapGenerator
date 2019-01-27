using System.Collections.Generic;
using System.Collections;
using System;

namespace Util
{
    public class LinkedList<T> : IEnumerable<T>
    {
        public int Length { get; private set; }

        public LinkedListNode<T> First { get; private set; }
        public LinkedListNode<T> Last { get; private set; }

        public void AddFirst(T data)
        {
            LinkedListNode<T> node = new LinkedListNode<T>(data);
            if (First == null)
            {
                First = node;
                Last = node;
            }
            else
            {
                node.Next = First;
                First = node;
            }
            Length++;
        }

        public void AddLast(T data)
        {
            LinkedListNode<T> node = new LinkedListNode<T>(data);
            if (First == null)
            {
                First = node;
                Last = node;
            }
            else
            {
                Last.Next = node;
                Last = node;
            }
            Length++;
        }

        public void Append(LinkedList<T> data)
        {
            if (First == null)
            {
                First = data.First;
                Last = data.Last;
            }
            else
            {
                Last.Next = data.First;
                Last = data.Last;
            }
            Length += data.Length;
        }

        public void Append(T[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                AddLast(data[i]);
            }
        }

        public void Prepend(T[] data)
        {
            for (int i = data.Length - 1; i >= 0; i--)
            {
                AddFirst(data[i]);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new LinkedListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}