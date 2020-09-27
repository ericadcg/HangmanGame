using System;
using System.Collections.Generic;
using System.Text;

namespace Hangman
{
    class Letters
    {
        public char letter { get; set; }

        public bool isGuessed { get; set; }

        public Letters()
        {
            isGuessed = false;
        }


    }
}
