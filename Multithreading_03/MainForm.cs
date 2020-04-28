using System;
using System.Collections.Generic;

using System.Windows.Forms;

namespace Multithreading_03
{
    public partial class MainForm : Form
    {
        //Use Dictionary to allow a more dynamic and easy identification of each object
        private readonly Dictionary<string, Producer> myProducers;
        private readonly Dictionary<string, Consumer> myConsumers;

        //Create storage buffer which is shared between producers and consumers
        private readonly StorageBuffer myStorageBuffer;

        //List the producers access to generate a random food item
        private List<FoodItem> myFoodItems;

        //Simplification to consistently access right producer and consumer without letter mismatch error
        private string myProducerName1;
        private string myProducerName2;
        private string myProducerName3;

        private string myConsumerName1;
        private string myConsumerName2;
        private string myConsumerName3;

        public static MainForm Form;

        public MainForm()
        {
            InitializeComponent();

            Form = this;

            myProducers = new Dictionary<string, Producer>();
            myConsumers = new Dictionary<string, Consumer>();
            
            InitializeFoodItems();

            myStorageBuffer = new StorageBuffer();

            //Name identifying each object in dictionary
            myProducerName1 = "Scan";
            myProducerName2 = "Arla";
            myProducerName3 = "AxFood";

            myConsumerName1 = "ICA";
            myConsumerName2 = "COOP";
            myConsumerName3 = "CITY GROSS";

            //Create each producer and add to dictionary
            myProducers.Add(myProducerName1, new Producer(myStorageBuffer, ProducerStatus1, 1500));
            myProducers.Add(myProducerName2, new Producer(myStorageBuffer, ProducerStatus2, 1200));
            myProducers.Add(myProducerName3, new Producer(myStorageBuffer, ProducerStatus3, 2500));

            //Create each consumer and add to dictionary
            myConsumers.Add(myConsumerName1, new Consumer(
                myStorageBuffer, ConsumerList1, ConsumerStatus1, 
                1000, 15, 27.00f, 12.50f));
            myConsumers.Add(myConsumerName2, new Consumer(
                myStorageBuffer, ConsumerList2, ConsumerStatus2, 
                500, 12, 31.50f, 10.20f));
            myConsumers.Add(myConsumerName3, new Consumer(
                myStorageBuffer, ConsumerList3, ConsumerStatus3,
                600, 19, 22.30f, 17.00f));

            //Update GUI after new values given to objects
            StorageCapacity.Text = "0 / " + myStorageBuffer.MaxSize.ToString();

            ConsumerItems1.Text = myConsumers[myConsumerName1].MaxItems.ToString();
            ConsumerWeight1.Text = myConsumers[myConsumerName1].MaxWeight.ToString();
            ConsumerVolume1.Text = myConsumers[myConsumerName1].MaxVolume.ToString();

            ConsumerItems2.Text = myConsumers[myConsumerName2].MaxItems.ToString();
            ConsumerWeight2.Text = myConsumers[myConsumerName2].MaxWeight.ToString();
            ConsumerVolume2.Text = myConsumers[myConsumerName2].MaxVolume.ToString();

            ConsumerItems3.Text = myConsumers[myConsumerName3].MaxItems.ToString();
            ConsumerWeight3.Text = myConsumers[myConsumerName3].MaxWeight.ToString();
            ConsumerVolume3.Text = myConsumers[myConsumerName3].MaxVolume.ToString();
        }

        private void InitializeFoodItems()
        {
            myFoodItems = new List<FoodItem>();

            myFoodItems.Add(new FoodItem("Milk", 1.1f, 0.5f));
            myFoodItems.Add(new FoodItem("Cream", 0.6f, 0.1f));
            myFoodItems.Add(new FoodItem("Yoghurt", 1.1f, 0.5f));
            myFoodItems.Add(new FoodItem("Butter", 2.24f, 0.66f));
            myFoodItems.Add(new FoodItem("Flower", 3.4f, 1.2f));
            myFoodItems.Add(new FoodItem("Sugar", 3.7f, 1.8f));
            myFoodItems.Add(new FoodItem("Salt", 1.55f, 0.27f));
            myFoodItems.Add(new FoodItem("Almonds", 0.6f, 0.19f));
            myFoodItems.Add(new FoodItem("Bread", 1.98f, 0.75f));
            myFoodItems.Add(new FoodItem("Donuts", 1.4f, 0.5f));
            myFoodItems.Add(new FoodItem("Jam", 1.2f, 1.5f));
            myFoodItems.Add(new FoodItem("Ham", 4.1f, 2.5f));
            myFoodItems.Add(new FoodItem("Chicken", 6.8f, 3.9f));
            myFoodItems.Add(new FoodItem("Salad", 0.87f, 0.55f));
            myFoodItems.Add(new FoodItem("Orange", 2.46f, 0.29f));
            myFoodItems.Add(new FoodItem("Apple", 2.44f, 0.4f));
            myFoodItems.Add(new FoodItem("Pear", 1.3f, 0.77f));
            myFoodItems.Add(new FoodItem("Soda", 2.98f, 2.0f));
            myFoodItems.Add(new FoodItem("Beer", 3.74f, 1.5f));
            myFoodItems.Add(new FoodItem("Hotdogs", 2.0f, 1.38f));
        }

        /// <summary>
        /// Return random food item from list
        /// </summary>
        public FoodItem ReturnRandomFood()
        {
            return myFoodItems[StaticRandom.RandomNumber(0, myFoodItems.Count)];
        }

        public void UpdateProgress()
        {
            this.InvokeIfRequired(() =>
            {
                StorageCapacity.Text = myStorageBuffer.StorageCount.ToString() + " / " + myStorageBuffer.MaxSize.ToString();
                ProgressItems.Value = (int)(((float)myStorageBuffer.StorageCount / (float)myStorageBuffer.MaxSize) * ProgressItems.Maximum); 
            });
        }

        //Producer 1 (Scan)
        private void ProducerStart1_Click(object sender, EventArgs e)
        {
            if (!myProducers[myProducerName1].IsRunning)
            {
                myProducers[myProducerName1].StartThread();
            }
        }

        private void ProducerStop1_Click(object sender, EventArgs e)
        {
            myProducers[myProducerName1].IsRunning = false;
        }

        //Producer 2 (Arla)
        private void ProducerStart2_Click(object sender, EventArgs e)
        {
            if (!myProducers[myProducerName2].IsRunning)
            {
                myProducers[myProducerName2].StartThread();
            }
        }

        private void ProducerStop2_Click(object sender, EventArgs e)
        {
            myProducers[myProducerName2].IsRunning = false;
        }

        //Producer 3 (AxFood)
        private void ProducerStart3_Click(object sender, EventArgs e)
        {
            if (!myProducers[myProducerName3].IsRunning)
            {
                myProducers[myProducerName3].StartThread();
            }
        }

        private void ProducerStop3_Click(object sender, EventArgs e)
        {
            myProducers[myProducerName3].IsRunning = false;
        }

        //Consumer 1 (ICA)
        private void ConsumerStart1_Click(object sender, EventArgs e)
        {
            if (!myConsumers[myConsumerName1].IsRunning)
            {
                myConsumers[myConsumerName1].StartThread();
            }
        }

        private void ConsumerStop1_Click(object sender, EventArgs e)
        {
            myConsumers[myConsumerName1].IsRunning = false;
        }

        private void ConsumerContinue1_CheckedChanged(object sender, EventArgs e)
        {
            myConsumers[myConsumerName1].ContinueLoad = ConsumerContinue1.Checked;
        }

        //Consumer 2 (COOP)
        private void ConsumerStart2_Click(object sender, EventArgs e)
        {
            if (!myConsumers[myConsumerName2].IsRunning)
            {
                myConsumers[myConsumerName2].StartThread();
            }
        }

        private void ConsumerStop2_Click(object sender, EventArgs e)
        {
            myConsumers[myConsumerName2].IsRunning = false;
        }

        private void ConsumerContinue2_CheckedChanged(object sender, EventArgs e)
        {
            myConsumers[myConsumerName2].ContinueLoad = ConsumerContinue2.Checked;
        }

        //Consumer 3 (CITY GROSS)
        private void ConsumerStart3_Click(object sender, EventArgs e)
        {
            if (!myConsumers[myConsumerName3].IsRunning)
            {
                myConsumers[myConsumerName3].StartThread();
            }
        }

        private void ConsumerStop3_Click(object sender, EventArgs e)
        {
            myConsumers[myConsumerName3].IsRunning = false;
        }

        private void ConsumerContinue3_CheckedChanged(object sender, EventArgs e)
        {
            myConsumers[myConsumerName3].ContinueLoad = ConsumerContinue3.Checked;
        }
    }
}
