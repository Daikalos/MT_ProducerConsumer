using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace Multithreading_03
{
    class Consumer : ThreadObject
    {
        private readonly StorageBuffer mySharedBuffer;
        private readonly ListBox myStockList;
        private readonly Label myStatusLabel;
        private readonly int myLoadWait;

        private List<FoodItem> myFoodStock;

        private bool myContinueLoading;
        private bool myLimitReached;
        private int myContinueLoadWait;

        public bool ContinueLoad { set => myContinueLoading = value; }
        public int MaxItems { get; private set; }
        public float MaxWeight { get; private set; }
        public float MaxVolume { get; private set; }

        public Consumer(StorageBuffer sharedBuffer, ListBox listBox, Label statusLabel, int loadWait, int maxItems, float maxWeight, float maxVolume)
        {
            this.mySharedBuffer = sharedBuffer;
            this.myStockList = listBox;
            this.myStatusLabel = statusLabel;
            this.myLoadWait = loadWait;

            this.MaxItems = maxItems;
            this.MaxWeight = maxWeight;
            this.MaxVolume = maxVolume;

            myFoodStock = new List<FoodItem>();
            myContinueLoading = false;
            myLimitReached = false;
            myContinueLoadWait = 5000;
        }

        public override void Update()
        {
            while (IsRunning)
            {
                //As long as limit isn't reached, continue loading items into stock
                if (!myLimitReached)
                {
                    myStatusLabel.InvokeIfRequired(() => { myStatusLabel.Text = "Loading"; });

                    Thread.Sleep(myLoadWait);

                    myStatusLabel.InvokeIfRequired(() => { myStatusLabel.Text = "Waiting"; });

                    mySharedBuffer.FullSemaphore.Wait();
                    FoodItem item = mySharedBuffer.GetFirst();
                    mySharedBuffer.EmptySemaphore.Release();
                    mySharedBuffer.Consume(item);

                    myFoodStock.Add(item);
                    myStockList.InvokeIfRequired(() => { myStockList.Items.Add(item.Name); });

                    float totalItems = myFoodStock.Count;
                    float totalWeight = myFoodStock.Sum(f => f.Weight);
                    float totalVolume = myFoodStock.Sum(f => f.Volume);

                    if (totalItems + 1 > MaxItems || totalWeight + item.Weight > MaxWeight || totalVolume + item.Volume > MaxVolume)
                    {
                        //If stock exceeds any limit, check if to empty stock or stop
                        myLimitReached = true;
                    }
                }
                else
                {
                    if (myContinueLoading)
                    {
                        myStatusLabel.InvokeIfRequired(() => { myStatusLabel.Text = "Emptying..."; });

                        Thread.Sleep(myContinueLoadWait);

                        //Clear and reset
                        myLimitReached = false;
                        myFoodStock.Clear();
                        myStockList.InvokeIfRequired(() => { myStockList.Items.Clear(); });
                    }
                    else
                    {
                        //If consumer has not been marked to continue loading, stop loading
                        IsRunning = false;
                    }
                }
            }

            myStatusLabel.InvokeIfRequired(() => { myStatusLabel.Text = "Stopped"; });
        }
    }
}
