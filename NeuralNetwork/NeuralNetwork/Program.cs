﻿using System;
using System.Collections.Generic;
using System.Linq;
using NeuralNetwork.Classes;

namespace NeuralNetwork
{
	internal class Program
	{
		#region -- Constants --
		private const int MaxEpoch = 5000;
		#endregion

		#region -- Variables --
		private static int _numInputParameters;
		private static int _numHiddenLayerNeurons;
		private static int _numOutputParameters;
		private static Network _network;
		private static List<DataSet> _dataSets; 
		#endregion

		#region -- Main --
		private static void Main()
		{
			Greet();
			SetNumInputParameters();
			SetNumNeuronsInHiddenLayer();
			SetNumOutputParameters();

			CreateNetwork();
			TrainNetwork();
			VerifyTraining();

			Console.ReadLine();
		}
		#endregion

		#region -- Network Training --
		private static void TrainNetwork()
		{
			Console.WriteLine("Now, we need some input data.");
			PrintUnderline(50);
			PrintNewLine(2);

			_dataSets = new List<DataSet>();

			for (var i = 0; i < 4; i++)
			{
				var values = GetInputData(String.Format("Data Set {0}", i + 1));
				var expectedResult = GetExpectedResult(String.Format("Expected Result for Data Set {0}:", i + 1));
				_dataSets.Add(new DataSet(values, expectedResult));
				PrintNewLine(2);
			}

			Console.WriteLine("Training...");
			PrintUnderline(50);
			PrintNewLine();

			_network.Train(_dataSets);
			PrintUnderline(50);
			PrintNewLine();
			Console.WriteLine("Training Complete!");
			PrintNewLine();
		}

		private static void VerifyTraining()
		{
			Console.WriteLine("Let's test it!");
			PrintNewLine();

			while (true)
			{
				PrintUnderline(50);
				var values = GetInputData(String.Format("Type {0} inputs: ", _numInputParameters));
				var results = _network.GetResult(values);
				PrintNewLine();

				foreach (var result in results)
				{
					Console.WriteLine("Output: {0}", result);
				}

				PrintNewLine();

				var message = String.Format("Was the result supposed to be {0}? (y/n/exit)", String.Join(" ", results.Select(x => x > 0.5 ? "1" : "0")));
				if (!GetBool(message))
				{
					PrintNewLine();
					var expectedResults = GetExpectedResult("What were the expected results?");
					_dataSets.Add(new DataSet(values, expectedResults));
					PrintNewLine();
					Console.WriteLine("Retraining Network...");
					PrintNewLine();

					_network.Train(_dataSets, false);
				}
				else
				{
					PrintNewLine();
					Console.WriteLine("Neat!");
					Console.WriteLine("Encouraging Network...");
					PrintNewLine();

					_network.Train(_dataSets, false);
				}
			}
		}
		#endregion

		#region -- Network Setup --
		private static void Greet()
		{
			Console.WriteLine("We're going to create an artificial Neural Network!");
			Console.WriteLine("The network will use back propagation to train itself.");
			PrintUnderline(50);
			PrintNewLine(2);
		}

		private static void SetNumInputParameters()
		{
			Console.WriteLine("How many input parameters will there be? (2 or more)");
			_numInputParameters = GetInput("Input Parameters: ", 2);
			PrintNewLine(2);
		}

		private static void SetNumNeuronsInHiddenLayer()
		{
			Console.WriteLine("How many neurons in the hidden layer? (2 or more)");
			_numHiddenLayerNeurons = GetInput("Neurons: ", 2);
			PrintNewLine(2);
		}

		private static void SetNumOutputParameters()
		{
			Console.WriteLine("How many output parameters will there be? (1 or more)");
			_numOutputParameters = GetInput("Output Parameters: ", 1);
			PrintNewLine(2);
		}

		private static double[] GetInputData(string message)
		{
			Console.WriteLine(message);
			var line = Console.ReadLine();

			while (line == null || line.Split(' ').Count() != _numInputParameters)
			{
				Console.WriteLine("{0} inputs are required.", _numInputParameters);
				PrintNewLine();
				Console.WriteLine(message);
				line = Console.ReadLine();
			}

			var values = new double[_numInputParameters];
			var lineNums = line.Split(' ');
			for(var i = 0; i < lineNums.Length; i++)
			{
				double num;
				if (Double.TryParse(lineNums[i], out num))
				{
					values[i] = num;
				}
				else
				{
					Console.WriteLine("You entered an invalid number.  Try again");
					PrintNewLine(2);
					return GetInputData(message);
				}
			}

			return values;
		}

		private static int[] GetExpectedResult(string message)
		{
			Console.WriteLine(message);
			var line = Console.ReadLine();

			while (line == null || line.Split(' ').Count() != _numOutputParameters)
			{
				Console.WriteLine("{0} outputs are required.", _numOutputParameters);
				PrintNewLine();
				Console.WriteLine(message);
				line = Console.ReadLine();
			}

			var values = new int[_numOutputParameters];
			var lineNums = line.Split(' ');
			for (var i = 0; i < lineNums.Length; i++)
			{
				int num;
				if (int.TryParse(lineNums[i], out num) && (num == 0 || num == 1))
				{
					values[i] = num;
				}
				else
				{
					Console.WriteLine("You must enter 1s and 0s!");
					PrintNewLine(2);
					return GetExpectedResult(message);
				}
			}

			return values;
		}

		private static void CreateNetwork()
		{
			Console.WriteLine("Creating Network...");
			_network = new Network(_numInputParameters, _numHiddenLayerNeurons, _numOutputParameters, MaxEpoch);
		}
		#endregion

		#region -- Console Helpers --
		private static int GetInput(string message, int min)
		{
			Console.Write(message);
			var num = GetNumber();

			while (num < min)
			{
				Console.Write(message);
				num = GetNumber();
			}

			return num;
		}

		private static int GetNumber()
		{
			int num;
			return int.TryParse(Console.ReadLine(), out num) ? num : 0;
		}

		private static bool GetBool(string message)
		{
			Console.WriteLine(message);
			Console.Write("Answer: ");
			var line = Console.ReadLine();

			while (line == null || (line.ToLower() != "y" && line.ToLower() != "n"))
			{
				if (line == "exit")
					Environment.Exit(0);

				Console.WriteLine(message);
				Console.Write("Answer: ");
				line = Console.ReadLine();
			}

			return line.ToLower() == "y";
		}

		private static void PrintNewLine(int numNewLines = 1)
		{
			for(var i = 0; i < numNewLines; i++)
				Console.WriteLine();
		}

		private static void PrintUnderline(int numUnderlines)
		{
			for(var i = 0; i < numUnderlines; i++)
				Console.Write('-');
			Console.WriteLine();
		}
		#endregion
	}
}