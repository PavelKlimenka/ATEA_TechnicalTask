﻿using CLI.Extensions;

namespace CLI
{
    internal class App
    {
        private const char SeparatorCharacter = '-';
        private const int MaxArgumentLength = 50;

        private string? _arg1;
        private string? _arg2;
        private bool _requestedExit = false;


        public void Run()
        {
            while(!_requestedExit)
            {
                Console.WriteLine("Welcome to ATEA Technical Task program.");
                PrintArguments();
                ExecuteAction(PrintMenu().Key);
                PrintSeparator();
            }
        }

        private void PrintArguments()
        {
            if (ArgumentsAreValid())
            {
                Console.WriteLine($"Current arguments: 1) '{_arg1}' 2) '{_arg2}'");
            }
        }

        private ConsoleKeyInfo PrintMenu()
        {
            Console.WriteLine("(S)et arguments");
            if(ArgumentsAreValid())
            {
                Console.WriteLine("(A)dd arguments");
            }
            Console.WriteLine("(Q)uit");

            return Console.ReadKey(true);
        }

        private void ExecuteAction(ConsoleKey key)
        {
            switch(key)
            {
                case ConsoleKey.S:
                    {
                        SetArguments();
                        break;
                    }
                case ConsoleKey.A:
                    {
                        if (!ArgumentsAreValid()) goto default;
                        PrintAdditionResult(this.AddArguments(_arg1, _arg2));
                        break;
                    }
                case ConsoleKey.Q:
                    {
                        _requestedExit = true;
                        Console.WriteLine("\nGoodbye!");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("\nWrong input. Try again");
                        break;
                    }
            }
        }

        private void SetArguments()
        {
            Console.WriteLine("Enter two arguments separated by a whitespace character:");
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                PrintInvalidInputMessage("there should be 2 arguments separated by a whitespace character.");
                return;
            }

            string[] inputChunks = input.Split(' ').Select(e => e.Trim()).ToArray();

            if (inputChunks.Length != 2)
            {
                PrintInvalidInputMessage("there should be 2 arguments separated by a whitespace character.");
                return;
            }

            if(inputChunks.Any(e => e.Length > MaxArgumentLength))
            {
                PrintInvalidInputMessage($"argument length shouldn't exceed the maximum length ({MaxArgumentLength})");
                return;
            }

            _arg1 = inputChunks[0];
            _arg2 = inputChunks[1];
        }

        private void PrintSeparator(int length = 50)
        {
            Console.WriteLine("\n" + new string(SeparatorCharacter, length) + "\n");
        }

        private bool ArgumentsAreValid()
        {
            return _arg1 != null && _arg2 != null;
        }

        private void PrintAdditionResult(string result)
        {
            Console.WriteLine($"\nResult: {result}");
        }

        private void PrintInvalidInputMessage(string message)
        {
            Console.WriteLine($"\nInvalid input: {message}");
        }

    }
}
