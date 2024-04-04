#nullable enable
using System.Collections.Generic;

namespace Grid
{
    public class Recorder<T>
    {
        private readonly LinkedList<T> _elements = new();

        public void Add(T element)
        {
            this._elements.AddFirst(element);
        }

        public T RemoveLast()
        {
            T element = _elements.Last.Value; 
            _elements.RemoveLast();
            return element;
        }
        
        public T HeadFirst()
        {
            return _elements.First.Value;
        }

        public T RemoveFirst()
        {
            T element = _elements.First.Value; 
            _elements.RemoveFirst();
            return element;
        }
        
        public bool IsEmpty()
        {
            return _elements.Count < 1;
        }

        public int Size()
        {
            return _elements.Count; 
        }

        public void Reset()
        {
            while (!IsEmpty()) 
                RemoveLast();
        }
    }
}
