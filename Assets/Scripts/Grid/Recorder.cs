using System.Collections.Generic;

namespace Grid
{
    public class Recorder<T>
    {
        private LinkedList<T> _element = new();

        public void AddCell(T element)
        {
            this._element.AddFirst(element);
        }

        public T RemoveLast()
        {
            T element = _element.Last.Value; 
            _element.RemoveLast();
            return element;
        }
        
        public bool IsEmpty()
        {
            return _element.Count < 1;
        }

        public int Size()
        {
            return _element.Count; 
        }

        public void Reset()
        {
            while (!IsEmpty()) 
                RemoveLast();
        }
    }
}