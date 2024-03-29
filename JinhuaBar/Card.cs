﻿using System;
using System.Collections.Generic;
using System.Text;

namespace JinhuaBar
{
    enum Suit
    {
        Spades=9824,
        Plumblossom = 9827,
        Hearts = 9829,
        Cube = 9830
    }
    enum CardType
    {
        Single,
        Pair,
        Junko,
        GoldeFlower,
        Flush,
        Leopard
    }
    class Card:IComparable<Card>
    {
        public Card(Suit suit,int number)
        {
            this.suit = suit;
            this.number = number;
        }
        private Suit suit;
        public Suit Suit
        {
            get { return suit; }
            set { suit = value; }
        }
        public string Suit2Sharp
        {
            get
            {
                if (suit == Suit.Cube)
                {
                    return ((char)suit + " ").ToString();
                }
                else
                {
                    return ((char)suit).ToString();
                }
            }
        }
        private int number;
        public int Number
        {
            get { return number; }
            set { number = value; }
        }
        public string Number2String
        {
            get
            {
                if (number >= 2 && number <= 9)
                {
                    return number.ToString();
                }
                else if (number == 10)
                {
                    return "0";
                }
                else if (number == 11)
                {
                    return "J";
                }
                else if (number == 12)
                {
                    return "Q";
                }
                else if (number == 13)
                {
                    return "K";
                }
                else if (number == 14)
                {
                    return "A";
                }
                else
                {
                    return "?";
                }
            }
        }
        public int CompareTo(Card other)
        {
            if (other == null)
                return 1;
            return this.number - other.number;
        }
    }
}
