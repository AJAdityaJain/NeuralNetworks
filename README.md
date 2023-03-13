# NeuralNetworks

NeuralNetworks is a C# library for neural networks, AI and machine learning written from **Scratch**

It also doubles as a Matrix library for all your matrix needs

*"What you know you can't explain, but you feel it. You've felt it your entire life" - Morpheus*

## Limitations

This only supports a single hidden layer. I am working on implementing a system which can handle an indefinite amount of hidden layers.

Uses the Sigmoid fuction as the activation as I was too lazy to implement any others.

## Installation

Put the folder NeuralNetworks folder in your project and import the namspace
```csharp
using NeuralNetworks

```
In the 'examples' directory exists an implementation which can handle the MNIST dataset with 94% accuracy

## Usage NeuralNet
```csharp
using NeuralNetworks

//x represents inputs. y represents hiddens. z represents outputs
NeuralNet net = new NeuralNet(x, y, z);

//x represents a floating point between 1.0f and 0.0f
//People usually keep learning rate at 0.01f but I found 0.1f works better
net.setLearningRate(x);

//This function should be used with the Training dataset
//Input array is a float[]
//Target array is (also a float[]) the targetted output for that input
net.train(input_array, target_array);
//Meant to be used in a for loop with randomly selected inputs and corresponding targets

//This function should be used with the Testing dataset
//It returns a set of guesses in the form of a float[]
float[] guess_array = net.predict(input_array);
//Meant to be used in a for loop with randomly selected inputs from the testing dataset

//Returns the fitness value of the Neural network
float fitness = net.getFitness();

//Clones the network
NeuralNet clone = net.copy()

//Exports the entirety of the Neural network as a .dna file to the location specified
net.Serialize(location)

//STATIC method. Returns deserialized neural network from the specified file location
NeuralNet deserializedNetwork = NeuralNet.Deserialize(location)

//STATIC returns the largest value from the given float array
float maxValue = NeuralNet.Max(float_array)



public float myMutater(float value, int x, int y){
    float mutated_value;
    //Blah Blah Blah
    return mutated_value;
}
//This function mutates every weight and bias
//Takes in a Func<float,int,int,float>
//That means the function in the parameter has 3 parameters
//Param 1 : float value which has the value of the particular weight or bias
//Param 2 : int x is the x coordinate in a matrix
//Param 3 : int y is the y coordinate in a matrix
mutate(myMutater)
//This one is a bit weird

```
## Usage Matrix
```csharp
using NeuralNetworks
//4 rows 8 cols
Matrix mat = new Matrix(4,8)

//There are a ton of functions. Quite intuitive to understand
```


## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

## License

[MIT](https://choosealicense.com/licenses/mit/) License
