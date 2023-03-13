using NeuralNetworks;

namespace NeuralNetwork
{
    public class NeuralNet
    {
        public int input_nodes;
        public int hidden_nodes;
        public int output_nodes;
        public int Total;

        public float Fitness;
        public float learning_rate;

        public Matrix weights_ih;
        public Matrix weights_ho;

        public Matrix bias_h;
        public Matrix bias_o;

        public NeuralNet()
        {
            input_nodes = 0;
            hidden_nodes = 0;
            output_nodes = 0;

            weights_ih = new Matrix(0, 0);
            weights_ho = new Matrix(0, 0);

            bias_h = new Matrix(0, 0);
            bias_o = new Matrix(0, 0);
        }
        public NeuralNet(NeuralNet a)
        {
            input_nodes = a.input_nodes;
            hidden_nodes = a.hidden_nodes;
            output_nodes = a.output_nodes;

            weights_ih = a.weights_ih.copy();
            weights_ho = a.weights_ho.copy();

            bias_h = a.bias_h.copy();
            bias_o = a.bias_o.copy();
        }
        public NeuralNet(int in_nodes, int hid_nodes, int out_nodes)
        {
            input_nodes = in_nodes;
            hidden_nodes = hid_nodes;
            output_nodes = out_nodes;

            weights_ih = new Matrix(hidden_nodes, input_nodes);
            weights_ho = new Matrix(output_nodes, hidden_nodes);
            weights_ih.randomize();
            weights_ho.randomize();

            bias_h = new Matrix(hidden_nodes, 1);
            bias_o = new Matrix(output_nodes, 1);
            bias_h.randomize();
            bias_o.randomize();

            setLearningRate();
        }

        public static float activation(float x, int _, int __)
        {
            //return (float)Math.Tanh(x);
            //return Math.Max(x, 0);
            return (float)(1 / (1 + Math.Exp(-x)));
        }

        public static float dactivation(float x, int _, int __)
        {
            //return (float)(1 - Math.Pow(x,2));
            //return x>=0? 1 : 0;
            return x * (1 - x);
        }

        public float[] predict(float[] input_array)
        {

            // Generating the Hidden Outputs
            Matrix inputs = Matrix.fromArray(input_array);
            Matrix hidden = Matrix.multiply(weights_ih, inputs);
            hidden.add(bias_h);
            // activation function!
            hidden.map(activation);

            // Generating the output's output!
            Matrix output = Matrix.multiply(weights_ho, hidden);
            output.add(bias_o);
            output.map(activation);

            // Sending back to the caller!
            return output.toArray();
        }

        public void setLearningRate(float learning_rate = 0.1f)
        {
            learning_rate = learning_rate;
        }

        public void train(float[] input_array, float[] target_array)
        {
            // Generating the Hidden Outputs
            Matrix inputs = Matrix.fromArray(input_array);
            Matrix hidden = Matrix.multiply(weights_ih, inputs);
            hidden.add(bias_h);
            // activation function!
            hidden.map(activation);

            // Generating the output's output!
            Matrix outputs = Matrix.multiply(weights_ho, hidden);
            outputs.add(bias_o);
            outputs.map(activation);

            // Convert array to matrix object
            Matrix targets = Matrix.fromArray(target_array);

            // Calculate the error
            // ERROR = TARGETS - OUTPUTS
            Matrix output_errors = Matrix.subtract(targets, outputs);
            foreach (var item in output_errors.data)
            {
                Fitness += Math.Abs(item);
                Total++;
            }
            // let gradient = outputs * (1 - outputs) * err * aplha;
            // Calculate gradient
            Matrix gradients = Matrix.map(outputs, dactivation);
            gradients.multiply(output_errors);
            gradients.multiply(learning_rate);


            // Calculate deltas
            Matrix hidden_T = Matrix.transpose(hidden);
            Matrix weight_ho_deltas = Matrix.multiply(gradients, hidden_T);

            // Adjust the weights by deltas
            weights_ho.add(weight_ho_deltas);
            // Adjust the bias by its deltas (which is just the gradients)
            bias_o.add(gradients);

            // Calculate the hidden layer errors
            Matrix who_t = Matrix.transpose(weights_ho);
            Matrix hidden_errors = Matrix.multiply(who_t, output_errors);

            // Calculate hidden gradient
            Matrix hidden_gradient = Matrix.map(hidden, dactivation);
            hidden_gradient.multiply(hidden_errors);
            hidden_gradient.multiply(learning_rate);

            // Calcuate input->hidden deltas
            Matrix inputs_T = Matrix.transpose(inputs);
            Matrix weight_ih_deltas = Matrix.multiply(hidden_gradient, inputs_T);

            weights_ih.add(weight_ih_deltas);
            // Adjust the bias by its deltas (which is just the gradients)
            bias_h.add(hidden_gradient);

            // outputs.print();
            // targets.print();
            // error.print();
        }

        public float getFitness()
        {
            return Fitness / Total;
        }

        public static NeuralNet Deserialize(string loc)
        {
            System.Xml.Serialization.XmlSerializer reader = new(typeof(NeuralNet));
            StreamReader file = new(loc);
            NeuralNet? nn = (NeuralNet?)reader.Deserialize(file);
            file.Close();
            return nn;
        }
        public void Serialize(string loc)
        {
            System.Xml.Serialization.XmlSerializer writer = new(typeof(NeuralNet));
            string name = loc;
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"//" + name + ".dna";
            FileStream file = File.Create(path);

            writer.Serialize(file, this);
            file.Close();

        }

        // Adding function for neuro-evolution
        public NeuralNet copy()
        {
            return new NeuralNet(this);
        }

        public static int Max(float[] arr)
        {
            float x = 0;
            int j = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] > x)
                {
                    x = arr[i];
                    j = i;
                }
            }
            return j;
        }
        // Accept an arbitrary function for mutation
        public void mutate(Func<float, int, int, float> func)
        {
            weights_ih.map(func);
            weights_ho.map(func);
            bias_h.map(func);
            bias_o.map(func);
        }
    }

}
