using System;
using System.Collections.Generic;

namespace SeraphRandom
{
    public class ShuffleBag<T>
    {
        private Random _random = new Random();
        private List<T> _data;

        private T _currentItem;
        private int _currentPosition = -1;

        public int Capacity { get { { return _data.Capacity; } } }
        public int Size { get { { return _data.Count; } } }
        
        public bool IsInfinite { get; private set; }
        
        public ShuffleBag(int initCapacity)
        {
            _data = new List<T>(initCapacity);
        }

        public ShuffleBag(List<T> data)
        {
            _data = data;
            _currentPosition = Size - 1;
        }

        /// <summary>
        /// Adds an item to the bag.
        /// </summary>
        /// <param name="item"> The item being added to the bag</param>
        /// <param name="amount"> The number of times the item is added, default 1</param>
        public void Add(T item, int amount = 1)
        {
            for(int  i = 0; i < amount; i++)
                _data.Add(item);

            _currentPosition = Size - 1;
        }

        public T Pick()
        {
            // If last item, pull and reset bag.
            if(_currentPosition < 1)
            {
                _currentPosition = Size - 1;
                _currentItem = _data[0];

                return _currentItem;
            }

            // Find a random position between the start and the current position
            int pos = _random.Next(_currentPosition);

            // replace the items.
            _currentItem = _data[pos];
            _data[pos] = _data[_currentPosition];
            _data[_currentPosition] = _currentItem;
            _currentPosition--;

            // give the item found.
            return _currentItem;
        }

        public List<T> GetCurrentBag()
        {
            List<T> val = new List<T>();

            for(int i = 0; i <= _currentPosition; i++)
            {
                val.Add(_data[i]);
            }

            return val;
        }

    }
}

