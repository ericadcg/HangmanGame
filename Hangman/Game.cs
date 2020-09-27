using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Hangman
{
    class Game
    {
        //List of possible game states
        internal enum States
        {
            Menu,
            ChooseLetters,
            ChooseCategory,
            PlayGame
        }

        List<GuessingWord> AllWords { get; }
        CurrentMatch currMatch = null;
        //Defines game state variable. Start as Menu
        private States gameState = States.Menu;

        //Constructer gets words from file when game is inittialized
        public Game()
        {
            AllWords = new List<GuessingWord>();
            using StreamReader file = new StreamReader(@"AllWords.txt");
            int count = 0;
            string line;

            while ((line = file.ReadLine()) != null)
            {
                count++;
                string[] str = line.Split(" ");
                GuessingWord aux = new GuessingWord(str[0], str[1]);
                AllWords.Add(aux);
            }

            file.Close();
        }

        //Handles what to show at each phase of the game
        public void Display()
        {
            switch (gameState)
            {
                case States.Menu:
                    ShowMenu();
                    break;
                case States.ChooseLetters:
                    Console.WriteLine("Please insert the number of letters ("+ GetMinLengthWords()+ " to "+ GetMaxLengthWords()+ " ) the word to guess will have.");
                    break;
                case States.ChooseCategory:
                    Console.WriteLine("Please enter one of the following categories to get a word to guess:");
                    ReturnAllCategories().ForEach(Console.WriteLine);
                    break;
                case States.PlayGame:
                    Console.WriteLine("\nPlease enter a letter to guess if it belongs to the word.");
                    Console.WriteLine("You have " + currMatch.Attempts + " wrong attempts left.");
                    currMatch.PrintWrongLetters();
                    currMatch.PrintWord();
                    break;
                default:
                    break;
            }
        }

        //Handles game states
        public void Update()
        {
            string auxInput;
            try
            {
                auxInput = GetInput();
            }
            catch(ArgumentException a)
            {
                Console.WriteLine(a.Message);
                return;
            }

            switch (gameState)
            {
                case States.Menu:
                    switch (auxInput)
                    {
                        case "1":
                            gameState = States.ChooseLetters;
                            break;
                        case "2":
                            gameState = States.ChooseCategory;
                            break;
                        case "3":
                            Environment.Exit(0);
                            break;
                        default:
                            break;
                    }
                    break;
                case States.ChooseLetters:
                    int numberOfLetters = Int32.Parse(auxInput);
                    currMatch = new CurrentMatch(GetWordWithSize(numberOfLetters));
                    gameState = States.PlayGame ;
                    break;
                case States.ChooseCategory:
                    currMatch = new CurrentMatch(GetWordFromCategory(auxInput));
                    gameState = States.PlayGame;
                    break;
                case States.PlayGame:
                    char attempt = Char.Parse(auxInput);
                    currMatch.checkLetterAtempt(attempt);
                    currMatch.checkWordGuessed();
                    if (currMatch.IsMatchWon)
                    {
                        Console.WriteLine("----------- Congratulations! You've won!! ----------");
                        gameState = States.Menu;
                    }
                    else if (currMatch.Attempts < 1)
                    {
                        Console.WriteLine("----------- You've Lost... The word was " + currMatch.CurrentWord.Word + ". Better luck next time! -----------");
                        gameState = States.Menu;
                    }
                    break;
                default:
                    break;
            }
        }

        //Handles inputs and throw erros for each game phase
        public string GetInput()
        {
            string auxInput = Console.ReadLine();

            switch (gameState)
            {
                case States.Menu:
                    if (!Int32.TryParse(auxInput, out _))
                        throw new ArgumentException("Please input a digit.");
                    if (Int32.Parse(auxInput) < 1 || Int32.Parse(auxInput) > 3)
                        throw new ArgumentException("The number must be 1 or 2 or 3.");
                    break;
                case States.ChooseLetters:
                    if (!Int32.TryParse(auxInput, out _))
                        throw new ArgumentException("Please input a valid number.");
                    int min = GetMinLengthWords();
                    int max = GetMaxLengthWords();
                    if (Int32.Parse(auxInput) < min || Int32.Parse(auxInput) > max)
                        throw new ArgumentException("Please input a number between " + min + " and " + max );
                    break;
                case States.ChooseCategory:
                    auxInput = auxInput.ToLower();
                    if (!ReturnAllCategories().Contains(auxInput))
                        throw new ArgumentException("Please insert a valid category.");
                    break;
                case States.PlayGame:
                    if (!char.TryParse(auxInput, out _))
                        throw new ArgumentException("Please input only one letter.");
                    break;
                default:
                    break;
            }

            return auxInput;
        }

        //Return maximum size of letters in all words
        public int GetMaxLengthWords()
        {
            int max;
            max = AllWords.Select(w => w.NumberOfLetters()).Max();
            return max;
        }

        //Return minimun size of letters in all words
        public int GetMinLengthWords()
        {
            int min;
            min = AllWords.Select(w => w.NumberOfLetters()).Min();
            return min;
        }


        //Returns a word of a given size
        public GuessingWord GetWordWithSize(int size)
        {
            List<GuessingWord> sizeList = new List<GuessingWord>();

            foreach (GuessingWord w in AllWords)
            {
                if (w.NumberOfLetters() == size)
                {
                    sizeList.Add(w);
                }
            }

            var rand = new Random();

            return sizeList[rand.Next(sizeList.Count)];
        }

        //Returns a word that belongs to a category
        public GuessingWord GetWordFromCategory(string cat)
        {
            List<GuessingWord> catList = new List<GuessingWord>();

            foreach (GuessingWord w in AllWords)
            {
                if (string.Equals(w.Category, cat))
                {
                    catList.Add(w);
                }
            }

            var rand = new Random();

            return catList[rand.Next(catList.Count)];
        }

        //Returns list of all categoies 
        public List<string> ReturnAllCategories()
        {
            List<string> cat = new List<string>();

            cat = AllWords.Select(w => w.Category).Distinct().ToList();

            return cat;
        }


        static public void ShowMenu()
        {
            Console.WriteLine("Please enter a number to choose from the following options:\n");
            Console.WriteLine(" 1 - Choose number of letters and play the game;\n 2 - Choose category and play the game\n 3 - Exit the game;");
        }

    }
}
