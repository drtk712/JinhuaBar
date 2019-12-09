using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace JinhuaBar
{
    class Dealer
    {
        public Dealer(Player[] players)
        {
            foreach (Player player in players)
            {
                orderedPlayers.Enqueue(player);
            }
        }
        private Queue<Player> orderedPlayers=new Queue<Player>();
        public Queue<Player> OrderedPlayers
        {
            get
            {
                return orderedPlayers;
            }
            set
            {
                orderedPlayers = value;
            }
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
            {
                player.Cards.Sort();
                player.JudgeCards();
            }

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
                    Console.WriteLine("玩家{0}跟注成功！",player.Name);
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
                    Console.WriteLine("玩家{0}跟注成功！", player.Name);
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
                    Console.WriteLine("玩家{0}加注成功！", player.Name);
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
                    Console.WriteLine("玩家{0}加注成功！", player.Name);
                    return true;
                }
            }
        }
        public void PlayerOpen(Player player)
        {
            Console.WriteLine("玩家{0}选择开牌", player.Name);
            Thread.Sleep(1000);
            if (player.Chips < stepBet * 2)
            {
                sumBet += player.Chips;
                player.MyBet += player.Chips;
                player.Chips = 0;
                Console.WriteLine("全推！");
            }
            else
            {
                sumBet += stepBet * 2;
                player.Chips -= stepBet * 2;
                player.MyBet += stepBet * 2;
                Console.WriteLine("开牌成功！");
            }
            PKWinner(player);
        }
        public void DealerOpen()
        {
            int count = 0;
            foreach(Player player in OrderedPlayers)
            {
                if (!player.IsGiveUp)
                {
                    count++;
                }
            }
            if (count == 1)
            {
                Console.WriteLine("场上仅剩一名玩家未弃牌，该玩家获胜");
                Thread.Sleep(2000);
                JudgeWinner();
            }
        }
        public void PKWinner(Player playerPK)
        {
            while(true)
            {
                if (orderedPlayers.Peek() != playerPK)
                {
                    orderedPlayers.Enqueue(orderedPlayers.Dequeue());
                }
                else
                {
                    foreach (Player player in orderedPlayers)
                    {
                        if (playerPK == player)
                        {
                            continue;
                        }
                        if (!player.IsGiveUp)
                        {
                            Console.WriteLine("玩家{0}开始与玩家{1} PK", playerPK.Name, player.Name);
                            Thread.Sleep(1000);
                            Player winner = null;
                            if (player.CardType > playerPK.CardType)//TODO:修改成个人PK
                            {
                                winner = player;
                            }
                            else if (player.CardType == playerPK.CardType)
                            {
                                if (player.CardType == CardType.Single || player.CardType == CardType.GoldeFlower)//单只or金花pk
                                {
                                    if (player.Cards[2].Number > playerPK.Cards[2].Number)
                                    {
                                        winner = player;
                                    }
                                    else if (player.Cards[2].Number == playerPK.Cards[2].Number)
                                    {
                                        if (player.Cards[1].Number > playerPK.Cards[1].Number)
                                        {
                                            winner = player;
                                        }
                                        else if (player.Cards[1].Number == playerPK.Cards[1].Number)
                                        {
                                            if (player.Cards[0].Number > playerPK.Cards[0].Number)
                                            {
                                                winner = player;
                                            }
                                            else if (player.Cards[0].Number == playerPK.Cards[0].Number)
                                            {
                                                //平局
                                            }
                                        }
                                    }
                                }
                                else if (player.CardType == CardType.Pair)//对子pk
                                {
                                    if (player.Cards[2].Number > playerPK.Cards[2].Number)
                                    {
                                        winner = player;
                                    }
                                    else if (player.Cards[2].Number == playerPK.Cards[2].Number)
                                    {
                                        if (player.Cards[0].Number > playerPK.Cards[0].Number)
                                        {
                                            winner = player;
                                        }
                                        else if (player.Cards[0].Number == playerPK.Cards[0].Number)
                                        {
                                            //平局
                                        }
                                    }
                                }
                                else if (player.CardType == CardType.Junko || player.CardType == CardType.Flush || player.CardType == CardType.Leopard)//顺子or同花顺or豹子pk
                                {
                                    if (player.Cards[0].Number > playerPK.Cards[0].Number)
                                    {
                                        winner = player;
                                    }
                                    else if (player.Cards[0].Number == playerPK.Cards[0].Number)
                                    {
                                        //平局
                                    }
                                }
                            }
                            if (winner == player)
                            {
                                Console.WriteLine("玩家{0}与玩家{1} PK失败", playerPK.Name, player.Name);
                                playerPK.IsGiveUp = true;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("玩家{0}与玩家{1} PK胜利", playerPK.Name, player.Name);
                                player.IsGiveUp = true;
                            }
                        }
                        Thread.Sleep(2000);
                    }
                    break;
                }
            }
        }
        public void JudgeWinner()
        {
            while (true)
            {
                if (!orderedPlayers.Peek().IsGiveUp)
                {
                    resetGame = true;
                    orderedPlayers.Peek().Chips += sumBet;
                    Console.WriteLine("--------------赢家是{0}--------------", orderedPlayers.Peek().Name);
                    Console.WriteLine("┌─────┐ ┌─────┐ ┌─────┐");
                    Console.WriteLine("│ {0}   │ │ {1}   │ │ {2}   │", orderedPlayers.Peek().Cards[0].Number2String, orderedPlayers.Peek().Cards[1].Number2String, orderedPlayers.Peek().Cards[2].Number2String);
                    Console.WriteLine("│  {0} │ │  {1} │ │  {2} │", orderedPlayers.Peek().Cards[0].Suit2Sharp, orderedPlayers.Peek().Cards[1].Suit2Sharp, orderedPlayers.Peek().Cards[2].Suit2Sharp);
                    Console.WriteLine("│     │ │     │ │     │");
                    Console.WriteLine("└─────┘ └─────┘ └─────┘");
                    Console.WriteLine("-------------------------------------");
                    Thread.Sleep(2000);

                    ShowResult();
                    break;
                }
                else
                {
                    orderedPlayers.Enqueue(orderedPlayers.Dequeue());
                }
            }

        }

        public void ShowResult()
        {
            foreach (Player player in orderedPlayers)
            {
                Console.WriteLine("--------------玩家{0}手牌--------------", player.Name);
                Console.WriteLine("┌─────┐ ┌─────┐ ┌─────┐");
                Console.WriteLine("│ {0}   │ │ {1}   │ │ {2}   │", player.Cards[0].Number2String, player.Cards[1].Number2String, player.Cards[2].Number2String);
                Console.WriteLine("│  {0} │ │  {1} │ │  {2} │", player.Cards[0].Suit2Sharp, player.Cards[1].Suit2Sharp, player.Cards[2].Suit2Sharp);
                Console.WriteLine("│     │ │     │ │     │");
                Console.WriteLine("└─────┘ └─────┘ └─────┘");
                Console.WriteLine("-------------------------------------");
                Thread.Sleep(2000);
            }
        }
        public void Rest()
        {
            resetGame = false;
            stepBet = 20;
            sumBet = 0;
            foreach(Player player in orderedPlayers)
            {
                player.Rest();
                if((player as AIPlayer) != null)
                {
                    (player as AIPlayer).OtherPlayersStrength = null;
                }
            }
        }
    }
}
