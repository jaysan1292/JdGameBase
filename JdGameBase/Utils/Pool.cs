// Project: JdGameBase
// Filename: Pool.cs
// 
// Author: Jason Recillo

using System;
using System.Diagnostics;

using JdGameBase.Extensions;

using Microsoft.Xna.Framework;

namespace JdGameBase.Utils {
    /// <summary>
    /// A collection that maintains a set of class instances to allow for recycling 
    /// instances and minimizing the effects of garbage collection.
    /// </summary>
    /// <remarks>
    /// Sourced from: http://www.xnawiki.com/index.php/Generic_Resource_Pool
    /// </remarks>
    /// <typeparam name="T">The type of object to store in the Pool. Pools can only
    /// hold class types.</typeparam>
    public class Pool<T> where T : class {
        // the amount to enlarge the items array if New is called and there are no free items
        private const int ResizeAmount = 20;
        private readonly Func<T> _allocate;

        // whether or not the pool is allowed to resize
        private readonly bool _canResize;

        // the actual items of the pool
        private T[] _items;

        // used for checking if a given object is still valid
        private readonly Predicate<T> _validate;

        /// <summary>
        /// Creates a new pool with a specific starting size.
        /// </summary>
        /// <param name="initialSize">The initial size of the pool.</param>
        /// <param name="resizes">Whether or not the pool is allowed to increase its size as needed.</param>
        /// <param name="validateFunc">A predicate used to determine if a given object is still valid.</param>
        /// <param name="allocateFunc">A function used to allocate an instance for the pool.</param>
        public Pool(int initialSize, bool resizes, Predicate<T> validateFunc, Func<T> allocateFunc) {
            // validate some parameters
            if (initialSize < 1)
                throw new ArgumentOutOfRangeException("initialSize", "initialSize must be at least 1.");
            if (validateFunc == null)
                throw new ArgumentNullException("validateFunc");
            if (allocateFunc == null)
                throw new ArgumentNullException("allocateFunc");

            _canResize = resizes;

            // create our items array
            _items = new T[initialSize];
            InvalidCount = _items.Length;

            // store our delegates
            _validate = validateFunc;
            _allocate = allocateFunc;
        }

        // used for allocating instances of the object

        /// <summary>
        /// Gets or sets a delegate used for initializing objects before returning them from the New method.
        /// </summary>
        public Action<T> Initialize { get; set; }

        /// <summary>
        /// Gets or sets a delegate that is run when an object is moved from being valid to invalid
        /// in the CleanUp method.
        /// </summary>
        public Action<T> Deinitialize { get; set; }

        /// <summary>
        /// Gets the number of valid objects in the pool.
        /// </summary>
        public int ValidCount { get { return _items.Length - InvalidCount; } }

        /// <summary>
        /// Gets the number of invalid objects in the pool.
        /// </summary>
        public int InvalidCount { get; private set; }

        /// <summary>
        /// Returns a valid object at the given index. The index must fall in the range of [0, ValidCount].
        /// </summary>
        /// <param name="index">The index of the valid object to get</param>
        /// <returns>A valid object found at the index</returns>
        public T this[int index] {
            get {
                index += InvalidCount;

                if (index < InvalidCount || index >= _items.Length)
                    throw new IndexOutOfRangeException("The index must be less than or equal to ValidCount");

                return _items[index];
            }
        }

        /// <summary>
        /// Cleans up the pool by checking each valid object to ensure it is still actually valid.
        /// </summary>
        public void CleanUp() {
            for (var i = InvalidCount; i < _items.Length; i++) {
                T obj = _items[i];

                // if it's still valid, keep going
                if (_validate(obj)) continue;

                // otherwise if we're not at the start of the invalid objects, we have to move
                // the object to the invalid object section of the array
                if (i != InvalidCount) {
                    _items[i] = _items[InvalidCount];
                    _items[InvalidCount] = obj;
                }

                // clean the object if desired
                if (Deinitialize != null)
                    Deinitialize(obj);

                InvalidCount++;
            }
        }

        /// <summary>
        /// Returns a new object from the Pool.
        /// </summary>
        /// <returns>The next object in the pool if available, null if all instances are valid.</returns>
        public T New() {
            // if we're out of invalid instances...
            if (InvalidCount == 0) {
                // if we can't resize, then we can't give the user back any instance
                if (!_canResize) return null;

                Debug.WriteLine("Resizing pool. Old size: {0}. New size: {1}.".Fmt(_items.Length, _items.Length + ResizeAmount));

                // create a new array with some more slots and copy over the existing items
                var newItems = new T[_items.Length + ResizeAmount];
                for (int i = _items.Length - 1; i >= 0; i--) newItems[i + ResizeAmount] = _items[i];
                _items = newItems;

                // move the invalid count based on our resize amount
                InvalidCount += ResizeAmount;
            }

            // decrement the count
            InvalidCount--;

            // get the next item in the list
            T obj = _items[InvalidCount];

            // if the item is null, we need to allocate a new instance
            if (obj == null) {
                obj = _allocate();

                if (obj == null)
                    throw new InvalidOperationException("The pool's allocate method returned a null object reference.");

                _items[InvalidCount] = obj;
            }

            // initialize the object if a delegate was provided
            if (Initialize != null) Initialize(obj);

            return obj;
        }
    }
}
