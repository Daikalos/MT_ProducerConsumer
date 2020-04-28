using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Multithreading_03
{
    class StorageBuffer
    {
        //Queue the items are placed into and removed out of
        private readonly List<FoodItem> myStorageBuffer;

        //Simple semaphore
        private readonly SemaphoreSlim mySemaphoreFull;
        private readonly SemaphoreSlim mySemaphoreEmpty;

        //Lock methods where multiple threads will enter a critical section
        private readonly object myLockObject1 = new object();
        private readonly object myLockObject2 = new object();

        private int myMaxSize;

        public SemaphoreSlim FullSemaphore => mySemaphoreFull;
        public SemaphoreSlim EmptySemaphore => mySemaphoreEmpty;
        public int StorageCount => myStorageBuffer.Count;
        public int MaxSize => myMaxSize;

        public StorageBuffer()
        {
            myStorageBuffer = new List<FoodItem>();

            myMaxSize = 30;

            mySemaphoreFull = new SemaphoreSlim(0, myMaxSize);
            mySemaphoreEmpty = new SemaphoreSlim(myMaxSize, myMaxSize);   
        }

        /// <summary>
        /// Add an item to the buffer
        /// </summary>
        public void Add(FoodItem item)
        {
            lock (myLockObject1)
            {
                if (myStorageBuffer.Count < myMaxSize)
                {
                    myStorageBuffer.Add(item);
                }
                MainForm.Form.UpdateProgress();
            }
        }

        /// <summary>
        /// Get the oldest placed item in the list
        /// </summary>
        public FoodItem GetFirst()
        {
            return myStorageBuffer.First();
        }

        /// <summary>
        /// Remove the item from the buffer
        /// </summary>
        public void Consume(FoodItem item)
        {
            lock (myLockObject2)
            {
                myStorageBuffer.Remove(item);
                MainForm.Form.UpdateProgress();
            }
        }
    }
}
