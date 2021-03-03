using JokeGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {

        static string[] categories = new string[50];
        static ConsolePrinter printer = new ConsolePrinter();
        const int max_jokes = 9;

        static async Task Main(string[] args)
        {

            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            printer.printConsole("Welcome to Joke Generator Application.");
            printer.printConsole("Press ? to get instructions or any other key to exit the application.");
            char key = GetEnteredKey(Console.ReadKey());
            if (key == '?')
            {
                while (true)

                {
                    Person person = new Person();
                    Console.WriteLine();
                    printer.printConsole("Press c to get categories");
                    printer.printConsole("Press r to get random jokes");
                    printer.printConsole("Press Esc to exit");
                    key = GetEnteredKey(Console.ReadKey());
                    //get categories
                    if (key == 'c')
                    {
                        if (categories.All(item => item == null))
                        {
                            await getCategories();
                        }
                        PrintResult(categories, "categories");
                    }
                    //for random jokes
                    else if (key == 'r')
                    {
                        //get random name  if user wants
                        printer.printConsole("Want to use a random name? Press y for Yes or n for No.");
                        key = GetEnteredKey(Console.ReadKey());
                        //TODO: if unicode characters in name/other language then ? printed, can change UTF encoding bt not recommended
                        if (key == 'y')
                            person = await GetNames();

                        //get category if user wants
                        string category = await GetCategoryIfWanted();

                        //get number of jokes
                        int n = getJokeNumber();

                        //get jokes only if number more than 0
                        if (n > 0)
                        {
                            string[] jokes = await GetRandomJokes(category, n, person);
                            PrintResult(jokes, "jokes");
                        }
                    }

                    //Invalid input
                    else
                    {
                        printer.printConsole("ERROR: Wrong key pressed.");
                    }



                }
            }
            else
            {
                Console.WriteLine("Exiting the application.");

            }

        }
        //print result to console
        private static void PrintResult(string[] result, string resultType)
        {
            Console.WriteLine("Below are the " + resultType + ":");
            for (int i = 0; i < result.Length; i++)
            {
                printer.printConsole((i + 1) + ". " + result[i]);
            }

        }



        private static char GetEnteredKey(ConsoleKeyInfo consoleKeyInfo)

        {
            char key = '0';
            if (consoleKeyInfo.Key == ConsoleKey.Escape)
            {
                System.Environment.Exit(0);
            }
            else
            {
                key = consoleKeyInfo.KeyChar;
            }

            Console.WriteLine();
            return key;

        }
        //for any unhandled exception
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("ERROR: An error has occured with below details:");
            Console.WriteLine(e.ExceptionObject.ToString());
            Console.WriteLine("Press Enter to exit the application");
            Console.ReadLine();
            Environment.Exit(1);
        }
        private static async Task<string[]> GetRandomJokes(string category, int number, Person person)
        {
            new RandomJokeService();
            //create batch if the jokes  he can ask increase drastically : for better performance
            List<Task<String>> tasks = new List<Task<String>>();
            for (int i = 0; i < number; i++)
            {
                tasks.Add(RandomJokeService.GetRandomJokes(person, category));
            }
            var multipleJokes = await Task.WhenAll(tasks);


            return multipleJokes;

        }

        private static async Task getCategories()
        {

            categories = await RandomJokeService.GetCategories();
        }


        private static async Task<Person> GetNames()
        {
            Person result = await RandomNameService.Getnames();

            return result;
        }
        //return category name
        private static async Task<string> GetCategoryIfWanted()
        {
            string input_category = null;
            printer.printConsole("Want to specify a category?Press y for Yes or n for No.");
            char key = GetEnteredKey(Console.ReadKey());
            if (key == 'y')
            {
                printer.printConsole("Enter the number corresponding to the category you want.");
                //get categories only if empty
                if (categories.All(item => item == null))
                {
                    await getCategories();
                }
                PrintResult(categories, "categories");

                //assign category only if correct category number is supplied
                if (Int32.TryParse(Console.ReadLine(), out int number) && number <= categories.Length)
                {
                    input_category = categories[number - 1];

                }
                else
                {
                    printer.printConsole("WARNING: Wrong category number entered.No catergory Taken.");

                }


            }
            return input_category;
        }

        //return number of jokes wanted
        private static int getJokeNumber()
        {
            int n = 0;

            printer.printConsole("How many jokes do you want? Enter a number from 1 to " + max_jokes + ".");
            if (!int.TryParse(Console.ReadLine(), out n))
            {
                printer.printConsole("ERROR: Value entered is not in form of number.");
            }
            else if (n > max_jokes)
            {
                printer.printConsole("ERROR: Number given exceeds the maximum limit");
                n = 0;

            }
            else if (n <= 0)
            {
                printer.printConsole("ERROR: Number given is less than the minimum limit");
            }

            return n;

        }
    }
}
