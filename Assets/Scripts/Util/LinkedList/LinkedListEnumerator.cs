using System.Collections.Generic;
using System.Collections;
using System;

namespace Util
{
    public class LinkedListEnumerator<T> : IEnumerator<T>
    {
        public T Current
        {
            get
            {
                return CurrentNode.Data;
            }
        }
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        private LinkedList<T> LinkedList;
        private LinkedListNode<T> CurrentNode;

        public LinkedListEnumerator(LinkedList<T> linkedList)
        {
            LinkedList = linkedList;
        }

        public bool MoveNext()
        {
            if (CurrentNode == null)
            {
                CurrentNode = LinkedList.First;
                return true;
            }
            else if (CurrentNode.Next != null)
            {
                CurrentNode = CurrentNode.Next;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            CurrentNode = null;
        }

        void IDisposable.Dispose() { }
    }
}