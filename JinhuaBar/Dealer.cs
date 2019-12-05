using System;
using System.Collections.Generic;
using System.Text;

namespace JinhuaBar
{
    class Dealer
    {
        public Dealer(Player[] players)
        {
            this.players = players;
        }
        private Player[] players;
        public Player[] Players
        {
            get { return players; }
            set { players = value; }
        }
        private int minBet = 20;
        public int MinBet
        {
            get { return minBet; }
            set { minBet = value; }
        }
        private int stepBet = 20;
        public int StepBet
        {
            get { return stepBet; }
            set { stepBet = value; }
        }
        private int sumBet = 0;
        public int SumBet
        {
            get { return sumBet; }
            set { sumBet = value; }
        }
        private int maxBet = 1000;
        public int MaxBet
        {
            get { return maxBet; }
            set { maxBet = value; }
        }
        private bool resetGame = false;
        public bool ResetGame
        {
            get { return resetGame; }
            set { resetGame = value; }
        }
        public void Licensing(Stack<Card> deck,Player player)
        {
            player.Cards.Add(deck.Pop());
            if (player.Cards.Count == 3)
                player.Cards.Sort();
        }
        public bool PlayerCall(Player player)
        {
            if (player.IsSee)
            {
                if (player.Chips < stepBet*2)
                {
                    Console.WriteLine("对不起，你没有足够的筹码，只能弃牌或者全推");
                    return false;
                }
                else
                {
                    sumBet += stepBet * 2;
                    player.Chips -= stepBet * 2;
                    player.MyBet += stepBet * 2;
                    Console.WriteLine("跟注成功！");
                    return true;
                }
            }
            else
            {
                if (player.Chips < stepBet)
                {
                    Console.WriteLine("对不起，你没有足够的筹码，只能弃牌或者全推");
                    return false;
                }
                else
                {
                    sumBet += stepBet;
                    player.Chips -= stepBet;
                    player.MyBet += stepBet;
                    Console.WriteLine("跟注成功！");
                    return true;
                }
            }
        }
        public bool PlayerAddBet(Player player)
        {
            if (player.IsSee)
            {
                if (player.Chips < (stepBet + 20) * 2)
                {
                    Console.WriteLine("对不起，你没有足够的筹码加注");
                    return false;
                }
                else
                {
                    stepBet += 20;
                    sumBet += stepBet * 2;
                    player.Chips -= stepBet * 2;
                    player.MyBet += stepBet * 2;
                    Console.WriteLine("加注成功！");
                    return true;
                }
            }
            else
            {
                if (player.Chips < (stepBet + 20))
                {
                    Console.WriteLine("对不起，你没有足够的筹码加注");
                    return false;
                }
                else
                {
                    stepBet += 20;
                    sumBet += stepBet;
                    player.Chips -= stepBet;
                    player.MyBet += stepBet;
                    Console.WriteLine("加注成功！");
                    return true;
                }
            }
        }
        public void JudgeWinner()
        {
            Player winner = null;
            foreach (Player player in players)
            {
                if (!player.IsGiveUp)
                {
                    if (winner == null)
                    {
                        winner = player;
                        continue;
                    }
                    if (player.CardType > winner.CardType)
                    {
                        winner = player;
                    }
                    else if (player.CardType == winner.CardType)
                    {
                        if (player.CardType == CardType.Single || player.CardType == CardType.GoldeFlower)//单只or金花pk
                        {
                            if (player.Cards[2].Number > winner.Cards[2].Number)
                            {
                                winner = player;
                            }
                            else if (player.Cards[2].Number == winner.Cards[2].Number)
                            {
                                if (player.Cards[1].Number > winner.Cards[1].Number)
                                {
                                    winner = player;
                                }
                                else if (player.Cards[1].Number == winner.Cards[1].Number)
                                {
                                    if (player.Cards[0].Number > winner.Cards[0].Number)
                                    {
                                        winner = player;
                                    }
                                    else if (player.Cards[0].Number == winner.Cards[0].Number)
                                    {
                                        //平局
                                    }
                                }
                            }
                        }
                        else if (player.CardType == CardType.Pair)//对子pk
                        {
                            if (player.Cards[2].Number > winner.Cards[2].Number)
                            {
                                winner = player;
                            }
                            else if (player.Cards[2].Number == winner.Cards[2].Number)
                            {
                                if (player.Cards[0].Number > winner.Cards[0].Number)
                                {
                                    winner = player;
                                }
                                else if (player.Cards[0].Number == winner.Cards[0].Number)
                                {
                                    //平局
                                }
                            }
                        }
                        else if (player.CardType == CardType.Junko || player.CardType == CardType.Flush || player.CardType == CardType.Leopard)//顺子or同花顺or豹子pk
                        {
                            if (player.Cards[0].Number > winner.Cards[0].Number)
                            {
                                winner = player;
                            }
                            else if (player.Cards[0].Number == winner.Cards[0].Number)
                            {
                                //平局
                            }
                        }
                    }
                }
            }
            resetGame = true;
            winner.Chips += sumBet;
            Console.WriteLine("--------------赢家是{0}--------------", winner.Name);
            Console.WriteLine("┌─────┐ ┌─────┐ ┌─────┐");
            Console.WriteLine("│ {0}   │ │ {1}   │ │ {2}   │", winner.Cards[0].Number2String, winner.Cards[1].Number2String, winner.Cards[2].Number2String);
            Console.WriteLine("│  {0} │ │  {1} │ │  {2} │", winner.Cards[0].Suit2Sharp, winner.Cards[1].Suit2Sharp, winner.Cards[2].Suit2Sharp);
            Console.WriteLine("│     │ │     │ │     │");
            Console.WriteLine("└─────┘ └─────┘ └─────┘");
            Console.WriteLine("-------------------------------------");
            foreach(Player player in players)
            {
                Console.WriteLine("--------------玩家{0}手牌--------------", player.Name);
                Console.WriteLine("┌─────┐ ┌─────┐ ┌─────┐");
                Console.WriteLine("│ {0}   │ │ {1}   │ │ {2}   │", player.Cards[0].Number2String, player.Cards[1].Number2String, player.Cards[2].Number2String);
                Console.WriteLine("│  {0} │ │  {1} │ │  {2} │", player.Cards[0].Suit2Sharp, player.Cards[1].Suit2Sharp, player.Cards[2].Suit2Sharp);
                Console.WriteLine("│     │ │     │ │     │");
                Console.WriteLine("└─────┘ └─────┘ └─────┘");
                Console.WriteLine("-------------------------------------");
            }
        }
        public void Rest()
        {
            resetGame = false;
            stepBet = 20;
            sumBet = 0;
            foreach(Player player in players)
            {
                player.Cards.Clear();
            }
        }
    }
}
