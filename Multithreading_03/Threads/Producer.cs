using System.Windows.Forms;
using System.Threading;

namespace Multithreading_03
{
    class Producer : ThreadObject
    {
        private readonly StorageBuffer mySharedBuffer;
        private readonly Label myStatusLabel;
        private readonly int myProduceWait;

        public Producer(StorageBuffer sharedBuffer, Label statusLabel, int produceWait)
        {
            this.mySharedBuffer = sharedBuffer;
            this.myStatusLabel = statusLabel;
            this.myProduceWait = produceWait;
        }

        public override void Update()
        {
            while (IsRunning)
            {
                myStatusLabel.InvokeIfRequired(() => { myStatusLabel.Text = "Producing"; });

                Thread.Sleep(myProduceWait);

                myStatusLabel.InvokeIfRequired(() => { myStatusLabel.Text = "Waiting"; });

                FoodItem itemProduced = MainForm.Form.ReturnRandomFood();
                mySharedBuffer.EmptySemaphore.Wait();
                mySharedBuffer.Add(itemProduced);
                mySharedBuffer.FullSemaphore.Release();
            }

            myStatusLabel.InvokeIfRequired(() => { myStatusLabel.Text = "Stopped"; });
        }
    }
}
