using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Hangman
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Guess the Word! Please wait while the game is being set.\n");

            Game hangmanGame = new Game(); 
            //Initializes the game

            while (true)
            {
                // display
                hangmanGame.Display();
                // update
                hangmanGame.Update(); 
            }

        } //End of main

    }

}
