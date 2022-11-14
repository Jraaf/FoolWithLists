using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoolWithLists
{    class Card
    {
        public int suit { get; set; }
        public int val { get; set; }
        public bool ifTrump { get; set; }

        public Card(int suit, int power, bool ifTrump)
        {
            this.suit = suit;
            this.val = power;
            this.ifTrump = ifTrump;
        }
        public Card() { }

        public void ShowCard()
        {
            ConsoleColor color = Console.BackgroundColor;
            ConsoleColor color1 = Console.ForegroundColor;

            if (ifTrump)
            {
                Console.ForegroundColor = color;
                Console.BackgroundColor = color1;
            }
            Console.Write($" {(Suits)suit} {(Value)val}");
            Console.ForegroundColor = color1;
            Console.BackgroundColor = color;
        }

        public enum Suits { clubs, diamonds, hearts, spades }
        public enum Value { _6, _7, _8, _9, _10, J, Q, K, A }
    }
}
