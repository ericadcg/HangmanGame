using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Hangman
{
    class CurrentMatch
    {
        public GuessingWord CurrentWord { get; set; }

        private List<Letters> AllLetters { get; set; }

        public int Attempts { get; set; }

        public List<char> WrongLetters {get; set;}

        public bool IsMatchWon { get; set; }

      
        public CurrentMatch(GuessingWord currentWord)
        {
            Attempts = 5;
            IsMatchWon = false;

            AllLetters = new List<Letters>();
            WrongLetters = new List<char>();

            this.CurrentWord = currentWord;

            for(int i=0; i<currentWord.NumberOfLetters(); i++)
            {
                Letters aux = new Letters();
                aux.letter = currentWord.Word[i];

                AllLetters.Add(aux);
            }
        }

        //Prints string to show player with guessed letters and _ (for missing letters)
        public void PrintWord()
        {
            string toPrint = "";

            foreach(Letters l in AllLetters)
            {
                if (l.isGuessed)
                    toPrint = toPrint + " " + l.letter;
                else
                    toPrint = toPrint + " _";
            }

            Console.WriteLine(toPrint);
            return;
        }

        //Checks if the letter is in the word
        public bool checkLetterAtempt(char letter)
        {
            bool isCorrectGuess = false;
            letter = char.ToLower(letter);

            if(WrongLetters.Contains(letter))
            {
                Console.WriteLine("You have used this letter before. Try another one.");
                return false;
            }

            foreach(Letters l in AllLetters)
            {
                if(letter == l.letter)
                {
                    l.isGuessed = true;
                    isCorrectGuess = true;
                }
            }

            if (!isCorrectGuess)
            {
                WrongLetters.Add(letter);
                Attempts--;
            }
            return isCorrectGuess;
        }

        //Checks if the word has all letters guessed
        public void checkWordGuessed()
        {
            bool isWordGuessed = true;

            foreach (Letters l in AllLetters)
            {
                if (!l.isGuessed)
                {
                   isWordGuessed = false;
                }
            }
            IsMatchWon = isWordGuessed;
        }

        //Checks if the word guessing tentative is correct
        public void checkWordAtempt(string word)
        {
            if(string.Equals(CurrentWord.Word, word.ToLower(), StringComparison.InvariantCultureIgnoreCase))
            {
                IsMatchWon = true;
            }
            else
            {
                Attempts--;
            }
            
        }

        //Prints formated string with wrong letters
        public void PrintWrongLetters()
        {
            if (WrongLetters.Any())
            {
                Console.WriteLine("Wrong guesses: " + string.Join(", ", WrongLetters));
            }
        }
    }
}
