

namespace Util
{
    public class LinkedListNode<T>
    {
        public LinkedListNode<T> Next;
        public T Data;

        public LinkedListNode(T data)
        {
            Data = data;
        }
    }
}