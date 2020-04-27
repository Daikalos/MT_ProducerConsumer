using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Multithreading_03
{
    class StorageBuffer
    {
        private readonly List<FoodItem> myStorageBuffer;
        private readonly SemaphoreSlim mySemaphoreFull;
        private readonly SemaphoreSlim mySemaphoreEmpty;

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

        public FoodItem GetLast()
        {
            return myStorageBuffer.First();
        }

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
