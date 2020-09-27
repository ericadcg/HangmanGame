using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Hangman
{
    class GuessingWord
    {
        public string Category { get; set; }

        public string Word { get; set; }


        public GuessingWord(string category ,string word)
        {
            this.Category = category;
            this.Word = word;
        }

        public int NumberOfLetters()
        {
            return this.Word.Length;
        }

    }
}
