using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
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
            int numOfPlayers = 2;
            while (true)
            {
                try
                {
                    Console.Write("Enter number of cards (24/36/52): ");
                    NumOfCards = int.Parse(Console.ReadLine());
                    if (NumOfCards == 36 || NumOfCards == 52 || NumOfCards == 24) break;
                    else throw new Exception();
                }
                catch (Exception) { }
            }
            while(true)
            {                 
                try
                {
                    Console.Write("Enter number of players (2/4): ");
                    numOfPlayers = int.Parse(Console.ReadLine());
                    if (numOfPlayers == 2 || numOfPlayers == 4) break;
                    else throw new Exception();
                }
                catch (Exception) { }
            }
            Card actions = new Card();

            //Card[,] deck = new Card[4, NumOfCards / 4];
            List<Card> Deck = new List<Card>();
            Random rand = new Random();
            int trump = rand.Next(0, 3);//визначення козиря
            int pow = NumOfCards == 36 ? 4 : NumOfCards == 52 ? 0 : 7;
            for (int suit = 0; suit < 4; suit++)
            {
                for (int i = 0; i < NumOfCards/4; i++)
                {
                    Deck.Add(new Card(suit, pow+i, suit == trump));
                    //Console.WriteLine();
                }// виведення колоди
            }
            
            Shuffle(rand, ref Deck);
            foreach (Card c in Deck)
            {
                c.ShowCard();
                Console.Write("\t");
            }

            List<Card> P1 =new List<Card>();
            List<Card> P2 = new List<Card>();
            List<Card> P3 = new List<Card>();
            List<Card> P4 = new List<Card>();

            int cardNum = 0;
            List<Card> ToBeat = new List<Card>();

            bool turner = true;
            while (true)
            {
                if (numOfPlayers == 2)
                {
                    Turn(Deck, ref P1, ref P2, ref cardNum, ref turner);
                }
                else if (numOfPlayers == 4)
                {
                    Turn(Deck,ref ToBeat, ref P1, ref P2, ref P3, ref P4, ref cardNum);
                }

            }

            Console.ReadLine();
        }
        public void Turn(List<Card> Tdeck, ref List<Card> ToBeat, ref List<Card> P1, ref List<Card> P2, ref List<Card> P3, ref List<Card> P4,ref int cardNum)
        {
            FillMe(ref P1, Tdeck, ref cardNum);
            FillMe(ref P2, Tdeck, ref cardNum);
            FillMe(ref P3, Tdeck, ref cardNum);
            FillMe(ref P4, Tdeck, ref cardNum);

            Console.WriteLine("Turn Started");
            Console.WriteLine("Your cards:\n");
            foreach (var item in P1) item.ShowCard();

            Console.WriteLine();

            int act1 = Action(P1)-1;
            while (act1 < 0)
            {
                Console.WriteLine("you can't do that");
                act1 = Action(P1) - 1;
            }

            ToBeat.Add(P1[act1]);
            P1.RemoveAt(act1);

            Console.Clear();

            int shift = 0;//who is the last who beats the card
            int temp2 = P2.Count;
            Beating(ref ToBeat, ref P2);
            shift++;
            if (P2.Count > temp2) shift++;
            if (ToBeat.Count > 0)
            {
                int temp3 = P3.Count;
                Beating(ref ToBeat, ref P3);
                shift++;
                if (P3.Count > temp3) shift++;
                if (ToBeat.Count > 0)
                {
                    int temp4 = P4.Count;
                    Beating(ref ToBeat, ref P4);
                    shift++;
                    if (P4.Count > temp4) shift++;
                    if (ToBeat.Count>0)
                    {
                        int temp = P1.Count;
                        Beating(ref ToBeat, ref P1);
                        shift++;
                        if (P1.Count > temp) shift++;
                    }
                }
            }
            for (int i = 0; i < shift; i++)
            {
                List<Card> temp = P1;
                P1 = P2;
                P2 = P3;
                P3 = P4;
                P4 = temp;
            }
            if (P1.Count==0|| P2.Count == 0 || P3.Count == 0 || P4.Count == 0)
            {
                Console.WriteLine("Game Ended");
                Console.ReadLine();
                Environment.Exit(1);
            }
        }
        public void FillMe(ref List<Card> P, List<Card> Tdeck, ref int cardNum) 
        {
            while (P.Count < 6) if (!(1 + cardNum > Tdeck.Count - 1)) P.Add(Tdeck[cardNum++]); else break;
        }


        public void Turn(List<Card> Tdeck, ref List<Card> P1, ref List<Card> P2, ref int cardNum, ref bool turner)
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
            List<Card> ToBeat = new List<Card>();
            ToBeat.Add(c1);


            //Console.Clear();

            P1.RemoveAt(act);

            Console.WriteLine();



            //int act1 = Action(P2) - 1;
            bool beaten = false;

            while (!beaten)
            {
                List<Card> ToAdd = new List<Card>();
                Beating(ref ToBeat,ref P2);
                if (ToAdd.Count==0)
                {
                    while (P1.Count < 6)
                    {
                        if (!(1 + cardNum > Tdeck.Count - 1)) P1.Add(Tdeck[++cardNum]); else break;
                    }
                    while (P2.Count < 6)
                    {
                        if (!(1 + cardNum > Tdeck.Count - 1)) P2.Add(Tdeck[++cardNum]); else break;
                    }
                    beaten = true;
                }
                else
                {
                    foreach ( Card card in ToAdd)
                        card.ShowCard();
                    Console.WriteLine("\nAnything to add?");
                    foreach (Card card in P1)                    
                        card.ShowCard();
                    Console.WriteLine();

                    while (true)
                    {
                        int act1 = Action(P1) - 1;
                        if (act1 == -1)
                        {
                            beaten = true;
                            turner = !turner;
                            break;
                        }
                        else
                        {
                            int count = ToBeat.Count;
                            foreach (Card c in ToAdd)
                            {
                                if (P1[act1].val == c.val)
                                {
                                    ToBeat.Add(P1[act1]);
                                    break;
                                }
                            }
                            P1.RemoveAt(act1);
                            if (ToBeat.Count != count)
                            {
                                Console.WriteLine("WRONG CARD");
                                continue;
                            }
                            break;
                        }
                    }
                }
                              
            }
            if (a)
            {
                List<Card> temp = P1;
                P1 = P2;
                P2 = temp;
            }
            if (cardNum == Tdeck.Count - 1)
            {
                List<Card> Temp1 = new List<Card>();
                List<Card> Temp2 = new List<Card>();
                int newTrump = new Random().Next(0, 3);
                foreach (var c in P1)
                    Temp1.Add(new Card(c.suit, c.val, newTrump == c.suit));
                foreach (var c in P2)
                    Temp2.Add(new Card(c.suit, c.val, newTrump == c.suit));

                P1 = Temp1;
                P2 = Temp2;
                cardNum++;
            }
        }
        
        public void Beating(ref List<Card>ToBeat, ref List<Card> P)
        {            
            bool beat=false;
            bool tobreak = false;
            int num= ToBeat.Count;

            Console.WriteLine();

            while (ToBeat.Count > 0)
            {

                while (!beat)
                {
                    Console.WriteLine("Here're cards you have to beat:");
                    foreach (var item in ToBeat) item.ShowCard();
                    Console.Write("\nchoose card to beat ");
                    ToBeat[0].ShowCard();
                    Console.WriteLine();
                    foreach (Card c in P)
                        c.ShowCard();
                    Console.WriteLine();
                    int act1 = Action(P) - 1;
                    if (act1 == -1)
                    {//taking card
                        beat = true;
                        tobreak = true;
                        P.AddRange(ToBeat);
                        ToBeat.Clear();
                    }
                    else if (P[act1].suit == ToBeat[0].suit && P[act1].val > ToBeat[0].val || P[act1].ifTrump && !ToBeat[0].ifTrump)
                    {//beating card
                        ToBeat.RemoveAt(0);
                        P.RemoveAt(act1);
                        while (ToBeat.Count > 0)
                        {
                            Beating(ref ToBeat, ref P);
                        }
                        Console.WriteLine("Card beaten");
                        tobreak = true;
                        beat = true;
                    }
                    else if (P[act1].val == ToBeat[0].val)
                    {//passing card
                        ToBeat.Add(P[act1]);
                        P.RemoveAt(act1);
                        beat = true;
                        tobreak = true;
                    }
                    else
                    {
                        Console.WriteLine("WRONG CARD");
                        act1 = Action(P) - 1;
                    }
                }
                if (tobreak) break;
            }
            Thread.Sleep(500);
        }
        public int Action(List<Card> p)
        {//choosing action (picking a card or taking a card)
            Console.WriteLine($"choose option: 1...{p.Count}; 0");
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

        public void Shuffle(Random rng, ref List<Card> Deck)
        {
            int n = Deck.Count;
            while (n > 1)
            {
                int k = rng.Next(n--);
                Card temp = Deck[n];
                Deck[n] = Deck[k];
                Deck[k] = temp;
            }
        }
    }
}

