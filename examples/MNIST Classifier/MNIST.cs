using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MNIST
{
    public static class Extensions
    {
        public static int ReadBigInt32(this BinaryReader br)
        {
            var bytes = br.ReadBytes(sizeof(Int32));
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static void ForEach<T>(this T[,] source, Action<int, int> action)
        {
            for (int w = 0; w < source.GetLength(0); w++)
            {
                for (int h = 0; h < source.GetLength(1); h++)
                {
                    action(w, h);
                }
            }
        }
    }
    public class Image
    {
        public static readonly string ASCIIGrad = ".:-=+*#%O@";

        public byte Label { get; set; }
        public float[] Data { get; set; }

        public void Print()
        {
            int i = 0;
            for (int y = 0; y < 28; y++)
            {
                for (int x = 0; x < 28; x++)
                {
                    int a = (int)(Data[i] * (ASCIIGrad.Length - 1)/2);
                    Console.Write("{0}{0}", ASCIIGrad[a]);
                    //Console.Write("{0} ", Data[i].ToString("n2"));
                    i++;
                }
                Console.WriteLine();
            }
        }
    }

    public static class MnistReader
    {
        private static string TrainImages = @"\train-images.idx3-ubyte";
        private static string TrainLabels = @"\train-labels.idx1-ubyte";
        private static string TestImages = @"\t10k-images.idx3-ubyte";
        private static string TestLabels = @"\t10k-labels.idx1-ubyte";

        public static void SetDirectory(string dir)
        {
            TrainImages = dir + TrainImages;
            TrainLabels = dir + TrainLabels;
            TestImages = dir + TestImages;
            TestLabels = dir + TestLabels;
        }

        private static Bitmap resizeImage(Bitmap imgToResize, Size size)
        {

            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //New Width  
            int destWidth = (int)(sourceWidth * nPercent);
            //New Height  
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height  
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (Bitmap)b;
        }

        public static Image ReadPNG(string name)
        {
            Image img = new()
            {
                Data = new float[28 * 28],
                Label = 255
            };

            if (OperatingSystem.IsWindows())
            {
                Bitmap bmp = new(name);
                bmp = resizeImage(bmp, new Size(28, 28));

                int i = 0;
                for (int y = 0; y < 28; y++)
                {
                    for (int x = 0; x < 28; x++)
                    {
                        img.Data[i] = 2*bmp.GetPixel(x, y).A/255.0f;
                        i++;
                    }
                }
            }

            return img;
        }

        public static IEnumerable<Image> ReadTrainingData()
        {
            foreach (var item in Read(TrainImages, TrainLabels))
            {
                yield return item;
            }
        }

        public static IEnumerable<Image> ReadTestData()
        {
            foreach (var item in Read(TestImages, TestLabels))
            {
                yield return item;
            }
        }

        private static IEnumerable<Image> Read(string imagesPath, string labelsPath)
        {

            BinaryReader labels = new (new FileStream(labelsPath, FileMode.Open));
            BinaryReader images = new (new FileStream(imagesPath, FileMode.Open));
            int magicNumber = images.ReadBigInt32();
            int numberOfImages = images.ReadBigInt32();
            int width = images.ReadBigInt32();
            int height = images.ReadBigInt32();

            int magicLabel = labels.ReadBigInt32();
            int numberOfLabels = labels.ReadBigInt32();

            for (int i = 0; i < numberOfImages; i++)
            {
                var bytes = images.ReadBytes(width * height);
                var arr = new float[height, width];

                arr.ForEach((j, k) => arr[j, k] = bytes[j * height + k]/128.0f);

                yield return new Image()
                {
                    Data = arr.Cast<float>().ToArray(),
                    Label = labels.ReadByte()
                };
            }
        }
    }
}
