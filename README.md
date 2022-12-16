# ATEA Technical task

This is a .NET Core console application which consists of 4 projects:

 - Command line interface "CLI.csproj". It's responsible for console input/output;
 - Data access layer "DataAccsess.csproj". It's essentially a repository wrapper aroung SQLite.
 - Unit tests project "ATEA_TechnicalTask.Tests.csproj".
 - Class library "ATEA_TechnicalTask.Shared.csproj". It contains commonly used utility classes and interfaces.

## CLI

![Alt text](/readme_screenshot.jpg)

CLI works by offering to choose one of the available actions from the menu and executing action depending on the user's input. The process repeats itself in a loop until the **"Quit"** action is chosen.  

The program supports two arguments, which can be set via command line arguments at the start of the application, or through the corresponding action in app menu.  

To add arguments together user must enter arguments first. After that the **"Add arguments to each other"** menu options will appear. Every arguments input will result in a database insert of those arguments. List of all previously used arguments can be viewed by selecting **"List previous arguments"** menu option.  

To set one of the previously used set of arguments as the current user must select the **"Fetch and set arguments from database by id"** option.  

Sets of arguments can be deleted from the database by selecting the **"Delete arguments from database by id"** option.  

**Input error handling** is implemented through try/catch, custom exception class (InvalidInputException) and custom Logger class.  

Most of the application methods are helper methods, designed to help with the console printing.  

**Arguments addition** is implemented through an extension method to the main App class. The method tries to parse string arguments first as ints, then as floats. If neither of the parsing steps worked method simply concatenates string arguments and returns the result.   

**Unit tests** of the addition method are located in the "ATEA_TechnicalTask.Tests" project.

## DataAccess

DataAccess is the project responsible for database interactions. It was built by implementing the **"Repository" pattern**, which is reflected in generic IRepository interface and ArgumentsRepository class, which implements the interface for the corresponding model: ArgumentsRecord class.  

For the SQL database **ADO + SQLite .NET Core** implementation was used (via nuget packages), because of it's simplicity and convenience.  

The application creates a new database file with table (if it doesn't already exist) right next to it's executable. The single added **table** has three columns: id -> int, arg1 -> nvarchar(50), arg2 -> nvarchar(50).  

**Unit tests** for the repository implementation are located in the "ATEA_TechnicalTask.Tests" project.

## ATEA_TechnicalTask.Tests
The project contains unit tests for the App class extension method and ArgumentsRepository repository implementation class.  

Repository tests are supplemented with a fixture class, which is responsible for separate test database creation at the the start of the test run.

## ATEA_TechnicalTask.Shared
The project was introduced to avoid the cyclic project reference issue. It consists of classes and intefaces commonly used among other projects: ILogger interface, ConsoleLogger implementation and Utility class.  

ILogger interface was added to abstract out the logging process from logger implementation details.