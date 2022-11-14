using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoolWithLists
{
    class Game
    {
        public Game() { }

        public void Start()
        {
            Random random = new Random();
            int NumOfCards = 36;
            Card actions = new Card();

            Card[,] deck = new Card[4, NumOfCards / 4];
            Random rand = new Random();
            int trump = rand.Next(0, 3);//визначення козиря
            for (int i = 0; i < deck.GetLength(0); i++)
            {
                int suitS = deck.GetLength(1) - 1;
                for (int j = 0; j < deck.GetLength(1); j++)
                {
                    deck[i, j] = new Card(i, j, i == trump);
                    //deck[i, j].ShowCard();
                }
                //Console.WriteLine();
            }// виведення колоди
            Card[] Tdeck = ToOneD(deck);
            Shuffle(rand, Tdeck);
            foreach (Card c in Tdeck)
            {
                c.ShowCard();
                Console.Write("\t");
            }
            //Card[] P1 = new Card[6];
            //Card[] P2 = new Card[6];
            List<Card> P1 =new List<Card>();
            List<Card> P2 = new List<Card>();
            
            /*
            actions.FillDeck(deck, P1P2);
            for (int i = 0; i < P1.GetLength(0); i++)
            {
                P1[i] = P1P2[i];
                P2[i] = P1P2[i + 6];
            }*/


            int cardNum = 0;
            for (int i = 0; i < 6; i++)
            {
                P1.Add(Tdeck[cardNum++]);
                P2.Add(Tdeck[cardNum++]);
            }

            Console.WriteLine("\nplayer 1:");
            foreach (Card c in P1)
                c.ShowCard();
            Console.WriteLine();

            Console.WriteLine("\nplayer 2:");
            foreach (Card c in P2)
                c.ShowCard();
            Console.WriteLine();

            bool turner = true;
            while (true)
            {
                Console.Clear();
                Turn(Tdeck, ref P1, ref P2, ref cardNum, ref turner);
                if (P1.Count == 0 || P2.Count == 0)
                {
                    Console.WriteLine(P1.Count == 0 ? "Player1 won" : P2.Count == 0 ? "Player2 won" : "");
                    break;
                }
            }

            Console.ReadLine();
        }
        public void Turn(Card[] Tdeck, ref List<Card> P1, ref List<Card> P2, ref int cardNum, ref bool turner)
        {
            bool a = false;
            if (!turner)
            {
                List<Card> temp = P1;
                P1 = P2;
                P2 = temp;
                a = true;
            }
            Console.WriteLine("it's player" + (turner ? 1 : 2) + "'s turn\n here's your cards:");
            foreach (Card c in P1)
                c.ShowCard();
            Console.WriteLine();

            int act = Action(P1) - 1;
            while (act < 0)
            {
                Console.WriteLine("you can't do that");
                act = Action(P1) - 1;
            }            
            Card c1 = P1[act];


            Console.Clear();

            Console.Write($"it's player" + (turner ? 2 : 1) + "'s turn\n You have to beat");
            P1[act].ShowCard();
            Console.WriteLine(" here's your cards:");
            foreach (Card c in P2)
                c.ShowCard();
            Console.WriteLine();

            P1.RemoveAt(act);

            int act1 = Action(P2) - 1;
            bool beaten = false;

            while (!beaten)
            {
                if (act1 == -1)
                {
                    P2.Add(c1);  
                    if (!(1 + cardNum > 35)) P1.Add(Tdeck[++cardNum]);
                    //P1.RemoveAt(act);
                    /*else                    
                        P1.Add(Tdeck[++cardNum]); */
                    
                    beaten = true;
                }//забирання карти
                else if (c1.suit == P2[act1].suit && P2[act1].val > c1.val || !c1.ifTrump && P2[act1].ifTrump)
                {
                    if (cardNum + 1 > Tdeck.Length - 1)
                    {
                        P1.RemoveAt(act);
                        P2.RemoveAt(act1); 
                    }
                    else if (P2.Count > 6)
                    {
                        P2.RemoveAt(act1);
                    }
                    else
                    {
                        P1.Add(Tdeck[++cardNum]);
                        P2.Add(Tdeck[++cardNum]);
                    }
                    Console.WriteLine("Card beaten");
                    turner = !turner;
                    beaten = true;
                }
                else
                {
                    Console.WriteLine("WRONG CARD");
                    act1 = Action(P1) - 1;
                }
            }//побиття карти
            if (a)
            {
                List<Card> temp = P1;
                P1 = P2;
                P2 = temp;
            }
        }
        public void LoseCard(ref Card[] P, int act)
        {
            Card[] temp = new Card[P.Length - 1];
            int j = 0, nulls = 0;
            for (int i = 0; i < P.Length - 1; i++)
            {
                if (i == act) continue;
                temp[j] = P[i];
                //if (temp[j] == null) nulls++;
                j++;
            }
            /*if (nulls != 0)
            {
                Card[] NoNulls = new Card[temp.Length - nulls];
                int k = 0;
                for (int i = 0; i < temp.Length - nulls; i++)
                {
                    if (temp[i] == null) continue;
                    NoNulls[k++] = temp[i];
                }
                temp = NoNulls;
            }*/
            P = temp;
        }
        public int Action(List<Card> p)
        {
            Console.WriteLine($"choose option: 1...{p.Count}; 0 to take cards");
            int a;
            try
            {
                a = Convert.ToInt32(Console.ReadLine());
                if (a > p.Count | a < 0)
                    throw new Exception();
            }
            catch (Exception)
            {
                Console.WriteLine("INAPROPPRIATE INPUT");
                a = Action(p);
            }
            return a;
        }

        public Card[] Shuffle(Random rng, Card[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                Card temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
            return array;
        }
        public Card[] ToOneD(Card[,] deck)
        {
            Card[] temp = new Card[deck.Length];
            int k = 0;
            for (int i = 0; i < deck.GetLength(0); i++)
            {
                for (int j = 0; j < deck.GetLength(1); j++)
                {
                    temp[k] = deck[i, j];
                    k++;
                }
            }
            return temp;
        }
    }
}

