using CLI.Interfaces;
using CLI.Utils;
using CLI.Utils.Exceptions;
using CLI.Utils.Extensions;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace CLI
{
    public class App : IDisposable
    {
        private const char SeparatorCharacter = '-';
        private const int MaxArgumentLength = 50;

        private string? _arg1;
        private string? _arg2;
        private bool _requestedExit;
        private IRepository<ArgumentsRecord> _repository;
        private ILogger _logger;

        public App()
        {
            _repository = new ArgumentsRepository();
            _requestedExit = false;
            _logger = new ConsoleLogger();
        }

        ~App()
        {
            Dispose();
        }

        public void Dispose()
        {
            _repository.Dispose();             
        }

        public async Task Run()
        {
            Console.WriteLine("Welcome to ATEA Technical Task program.");

            while(!_requestedExit)
            {
                try
                {
                    Console.WriteLine();
                    PrintArguments();
                    await ExecuteAction(PrintMenu().Key);
                    PrintSeparator();
                }
                catch(Exception e)
                {
                    _logger.LogError(e.Message);
                }
            }
        }

        private void PrintArguments()
        {
            if (ArgumentsAreValid())
            {
                Console.WriteLine($"Current arguments: arg1 = {_arg1}; arg2 = {_arg2}");
            }
        }

        private ConsoleKeyInfo PrintMenu()
        {
            Console.WriteLine($"({ConsoleKey.S})et arguments");
            if(ArgumentsAreValid()) Console.WriteLine($"({ConsoleKey.A})dd arguments to each other");
            Console.WriteLine($"({ConsoleKey.L})ist previous arguments (database)");
            Console.WriteLine($"({ConsoleKey.F})etch and set arguments from database by number");
            Console.WriteLine($"({ConsoleKey.D})elete arguments from database by number");
            Console.WriteLine($"({ConsoleKey.Q})uit");

            return Console.ReadKey(true);
        }

        private async Task ExecuteAction(ConsoleKey key)
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
                        PrintAdditionResult(this.AddArguments(_arg1!, _arg2!));
                        break;
                    }
                case ConsoleKey.L:
                    {
                        List<ArgumentsRecord> records = await _repository.GetAll();
                        if (records.Count == 0)
                            Console.WriteLine("\nDatabase is empty!");
                        else
                            PrintDatabaseRecords(records.ToArray());
                        break;
                    }
                case ConsoleKey.F:
                    {
                        await FetchAndSetDatabaseArgumentsByNumber();
                        break;
                    }
                case ConsoleKey.D:
                    {
                        await DeleteDatabaseArgumentsByNumber();
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
            Console.WriteLine("\nEnter two arguments separated by a whitespace character:");
            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
                throw new InvalidInputException("there should be 2 arguments separated by a whitespace character.");

            string[] inputChunks = input.Split(' ').Select(e => e.Trim()).Where(e => !string.IsNullOrWhiteSpace(e)).ToArray();

            if (inputChunks.Length != 2)
                throw new InvalidInputException("there should be 2 arguments separated by a whitespace character.");

            if(inputChunks.Any(e => e.Length > MaxArgumentLength))
                throw new InvalidInputException($"argument length shouldn't exceed the maximum length ({MaxArgumentLength})");

            _arg1 = inputChunks[0];
            _arg2 = inputChunks[1];

            _repository.Insert(new ArgumentsRecord(_arg1, _arg2));
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

        private void PrintDatabaseRecords(params ArgumentsRecord[] records)
        {
            Console.WriteLine();
            foreach (ArgumentsRecord record in records) 
            {
                Console.WriteLine($"{record.Id}) Arg1 = {record.Arg1}; Arg2 = {record.Arg2}");
            }
        }

        private async Task FetchAndSetDatabaseArgumentsByNumber()
        {
            Console.WriteLine("\nEnter arguments number:");
            string? input = Console.ReadLine();

            if(string.IsNullOrEmpty(input) || !int.TryParse(input, out int inputInt))
                throw new InvalidInputException("not a integer number");

            ArgumentsRecord record = await _repository.GetById(inputInt);
            if (string.IsNullOrEmpty(record.Arg1) || string.IsNullOrEmpty(record.Arg2))
                throw new InvalidInputException($"record with the number {input} doesn't exist in database");

            _arg1 = record.Arg1;
            _arg2 = record.Arg2;
            PrintDatabaseRecords(record);
        }

        private async Task DeleteDatabaseArgumentsByNumber()
        {
            Console.WriteLine("\nEnter arguments number:");
            string? input = Console.ReadLine();

            if(string.IsNullOrEmpty(input) || !int.TryParse(input, out int inputInt))
                throw new InvalidInputException("not a integer number");

            await _repository.Delete(new ArgumentsRecord() { Id = inputInt });

            Console.WriteLine("\nRecord was deleted");
        }

    }
}
