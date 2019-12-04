using System;
using System.Collections.Generic;
using System.Text;

namespace JinhuaBar
{
    class Dealer
    {
        public static int minBet = 20;
        public static int stepBet = 20;
        public static int sumBet=0;
        public Dealer()
        {

        }
        public void Licensing(Stack<Card> deck,Player player)
        {
            player.Cards.Add(deck.Pop());
        }
        public static Player JudgeWinner(Player[] players)
        {
            Player winner=new Player();
            foreach(Player player in players)
            {
                if (!player.IsGiveUp)
                {
                    if (winner == null)
                    {
                        winner = player;
                        continue;
                    }
                    if (player.CardType >winner.CardType)
                    {
                        winner = player;
                    }
                    else if(player.CardType == winner.CardType)
                    {
                        if (player.CardType == CardType.Single || player.CardType == CardType.GoldeFlower)//单只or金花pk
                        {
                            if (player.Cards[2].Number > winner.Cards[2].Number)
                            {
                                winner = player;
                            }
                            else if(player.Cards[2].Number == winner.Cards[2].Number)
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
                                    else if(player.Cards[0].Number == winner.Cards[0].Number)
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
            return winner;
        }
    }
}
