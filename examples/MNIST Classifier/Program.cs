using NeuralNetwork;
using MNIST;

class Program{
    public static NeuralNet net = new(784, 80, 10);
    public static string location = "";

    public static void Main(string[] args)
    {

        if(args.Length >0) 
            location = (args[0]);
        Console.Write("Train or Test or Read PNG (0/1/2) : ");
        string s = Console.ReadLine() + "";
        switch (s)
        {
            case ("0"):
                Train();
                break;
            case ("1"):
                Test();
                break;
            case ("2"):
                ReadPNG();
                break;
        }
        Console.ReadLine();
    }

    public static void Train()
    {
        net.setLearningRate(0.1f);
        int i = 0;
        TimeSpan timeTaken;
        Console.Write("MNIST ubyte file directory : ");
        MnistReader.SetDirectory(Console.ReadLine()+"");
        Image[] mnistData = MnistReader.ReadTrainingData().ToArray();
        Console.WriteLine(mnistData.Length);
        Console.WriteLine("Loaded MNIST");
        Console.Write("Batch size (Small 1000, Medium 5000, Large 10000) : ");
        int batchSize = int.Parse(Console.ReadLine() + "");
        DateTime t1 = DateTime.Now;
        do
        {
            for (int j = 0; j < batchSize; j++)
            {
                if (i % 1000 == 0) Console.WriteLine(net.getFitness());
                float[] oui = new float[10];
                oui[mnistData[i].Label] = 1;
                net.train(mnistData[i].Data, oui);
                i++;
            }
            Console.WriteLine("Network Fitness (Less is Good) : {0}", net.getFitness());
            Console.Write("Start new Batch (y/n) : ");

        }
        while (Console.ReadLine() != "n");
        timeTaken = DateTime.Now - t1;
        Console.WriteLine("Time Taken : {0} seconds", timeTaken.TotalSeconds);
        Console.WriteLine("Epochs : {0} with learning rate as {1}%", i, (net.learning_rate * 100).ToString("n2"));
        Console.WriteLine();
        Console.WriteLine();
        Console.Write("Do u wanna save? (y/n) : ");
        if (Console.ReadLine() != "n")
        {
            Console.Write("Name of DNA: ");
            net.Serialize(Console.ReadLine()+"");
        }

    }
    
    public static void ReadPNG()
    {
        Console.WriteLine("Disclaimer: This program may not understand your handwriting. It may fail. To see full potential of this program go to testing mode and enable 'view all images.'");
        if (location == "")
        {
            Console.Write("Location of network DNA file : ");
            location = (Console.ReadLine() + "");
        }
        net = NeuralNet.Deserialize(location);
        Console.Write("IMG location : ");
        Image img = MnistReader.ReadPNG(Console.ReadLine() + "");
        img.Print();

        Console.WriteLine("I think it is a {0}.", NeuralNet.Max(net.predict(img.Data)));
    }

    public static void Test()
    {
        if (location == "")
        {
            Console.Write("Location of network DNA file : ");
            location = (Console.ReadLine() + "");
        }
        net = NeuralNet.Deserialize(location);
        Console.Write("MNIST ubyte file directory : ");
        MnistReader.SetDirectory(Console.ReadLine() + "");
        Image[] mnistData = MnistReader.ReadTestData().ToArray();
        Console.WriteLine("Loaded MNIST Test Dataset");
        Console.Write("DO u wanna see each image (y/n):");
        bool test = Console.ReadLine() == "y";
        int su = 0;
        int tot = 0;
        foreach (var item in mnistData)
        {
            int f = NeuralNet.Max(net.predict(item.Data));
            if (f == item.Label)
                su++;
            tot++;

            if (test)
            {
                item.Print();
                Console.WriteLine("Guess {0} : Actually {1}", f, item.Label);
                Console.ReadLine();
            }
        }
        Console.WriteLine("Accuracy {0}%", (float)su / tot * 100);

    }
}