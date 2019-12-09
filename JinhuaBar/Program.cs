using System;
using System.Collections.Generic;
using System.Threading;

namespace JinhuaBar
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            //TestGame();
            TestGame2();
            //int Strength = (0 << 12) + (10 << 8) + (10 << 4) + 10;
            //Console.WriteLine(Strength);
            //Console.ReadKey();
        }

        public static void TestGame()
        {
            Console.WriteLine("欢迎进行本游戏，请输入游玩人数");
            Player[] players = new Player[Convert.ToInt32(Console.ReadLine())];
            for(int i = 0; i < players.Length; i++)
            {
                Console.WriteLine("请输入第{0}位玩家的昵称", i + 1);
                players[i] = new Player(Console.ReadLine());
            }
            Pokers poker = new Pokers();
            Dealer dealer = new Dealer(players);
            //给玩家注册操作事件
            for (int i = 0; i < players.Length; i++)
            {
                players[i].Call+=new Player.CallHandler(dealer.PlayerCall);
                players[i].AddBet+=new Player.AddBetHandler(dealer.PlayerAddBet);
                players[i].Open+=new Player.OpenHandler(dealer.PlayerOpen);
            }
            int count = 1;
            while (true)
            {
                Console.Write("开始第{0}轮游戏\n场上玩家筹码情况：\n", count++);
                for (int i = 0; i < players.Length; i++)
                {
                    Console.WriteLine("{0}.玩家：{1} 筹码：{2}", i + 1,players[i].Name,players[i].Chips);
                    dealer.SumBet += dealer.MinBet;
                    players[i].Chips -= dealer.MinBet;
                    players[i].MyBet += dealer.MinBet;
                }
                Console.WriteLine("按任意键继续...");
                Console.ReadKey();
                for (int i = 0; i < players.Length; i++)
                {
                    Console.WriteLine("玩家{0}下底注（{1}）...",players[i].Name,dealer.MinBet);
                    Thread.Sleep(500);
                }
                Console.WriteLine("荷官开始洗牌...");
                poker.Shuffe();
                Thread.Sleep(2000);
                Console.WriteLine("洗牌完毕，开始发牌...");
                for (int j = 0; j < 3; j++)
                {
                    for (int i = 0; i < players.Length; i++)
                    {
                        Console.WriteLine("第{0}轮发牌,给玩家{1}...", j + 1,players[i].Name);
                        dealer.Licensing(poker.Deck, players[i]);
                        Thread.Sleep(500);
                    }
                }
                Console.WriteLine("发牌完毕，开始游戏...");
                Thread.Sleep(2000);
                int step = 0;
                while (!dealer.ResetGame)
                {
                    Console.Clear();
                    #region 显示其他玩家信息
                    string line1 ="", line2 = "", line3 = "", line4 = "", line5 = "";
                    for (int i = 0; i < players.Length; i++)
                    {
                        if(step % players.Length == i)
                        {
                            continue;
                        }
                        string temp = "";
                        line1 += " ********************** ";

                        temp = "玩家:" + players[i].Name;
                        line2 += "*";
                        for (int j = 0; j < (20 - temp.Length) / 2; j++)
                            line2 += " ";
                        line2 += temp;
                        for (int j = 0; j < (20 - temp.Length) / 2; j++)
                            line2 += " ";

                        if (players[i].IsGiveUp)
                        {
                            temp = "已弃牌";
                        }
                        else
                        {
                            temp = players[i].IsSee2String;
                        }
                        line3 += "*";
                        for (int j = 0; j < (20 - temp.Length) / 2; j++)
                            line3 += " ";
                        line3 += temp;
                        for (int j = 0; j < (20 - temp.Length) / 2; j++)
                            line3 += " ";

                        temp = "注数:" + players[i].MyBet;
                        line4 += "*";
                        for (int j = 0; j < (20 - temp.Length) / 2; j++)
                            line4 += " ";
                        line4+= temp;
                        for (int j = 0; j < (20 - temp.Length) / 2; j++)
                            line4 += " ";

                        line5 += " ********************** ";
                    }
                    Console.WriteLine(line1);
                    Console.WriteLine(line2);
                    Console.WriteLine(line3);
                    Console.WriteLine(line4);
                    Console.WriteLine(line5);
                    #endregion
                    players[step++ % players.Length].Operate();
                    dealer.DealerOpen();
                }
                dealer.Rest();
                Console.WriteLine("本轮结束，按任意键继续...");
                Console.ReadKey();
            }
        }

        public static void TestGame2()
        {
            Console.WriteLine("欢迎进行本游戏，请输入你的昵称：");
            Player user = new Player(Console.ReadLine());
            AIPlayer aIPlayer1 = new AIPlayer("Robot1");
            AIPlayer aIPlayer2 = new AIPlayer("Robot2");
            AIPlayer aIPlayer3 = new AIPlayer("Robot3");
            AIPlayer aIPlayer4 = new AIPlayer("Robot4");

            Player[] players = new Player[5];
            players[0] = user;
            players[1] = aIPlayer1;
            players[2] = aIPlayer2;
            players[3] = aIPlayer3;
            players[4] = aIPlayer4;

            Pokers poker = new Pokers();
            Dealer dealer = new Dealer(players);
            foreach (Player player in players)
            {
                player.Call += new Player.CallHandler(dealer.PlayerCall);
                player.AddBet += new Player.AddBetHandler(dealer.PlayerAddBet);
                player.Open += new Player.OpenHandler(dealer.PlayerOpen);
            }
            for(int i = 1; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    if (players[i] != players[j])
                    {
                        (players[i] as AIPlayer).OtherPlayers.Add(players[j]);
                    }
                }
            }
            int count = 1;
            while (true)
            {
                Console.Write("开始第{0}轮游戏\n庄家是{1}\n场上玩家筹码情况：\n", count++, dealer.OrderedPlayers.Peek().Name);
                for (int i = 0; i < players.Length; i++)
                {
                    Console.WriteLine("{0}.玩家：{1} 筹码：{2}", i + 1, players[i].Name, players[i].Chips);
                    dealer.SumBet += dealer.MinBet;
                    players[i].Chips -= dealer.MinBet;
                    players[i].MyBet += dealer.MinBet;
                }
                Console.WriteLine("按任意键继续...");
                Console.ReadKey();
                foreach (Player player in dealer.OrderedPlayers)
                {
                    Console.WriteLine("玩家{0}下底注（{1}）...", player.Name, dealer.MinBet);
                    Thread.Sleep(200);
                }
                Console.WriteLine("荷官开始洗牌...");
                poker.Shuffe();
                Thread.Sleep(1000);
                Console.WriteLine("洗牌完毕，开始发牌...");
                for (int j = 0; j < 3; j++)
                {
                    foreach (Player player in dealer.OrderedPlayers)
                    {
                        Console.WriteLine("第{0}轮发牌,给玩家{1}...", j + 1, player.Name);
                        dealer.Licensing(poker.Deck, player);
                        Thread.Sleep(100);
                    }
                }
                Console.WriteLine("发牌完毕，开始游戏...");
                Thread.Sleep(1000);
                int step = 0;
                while (!dealer.ResetGame)
                {
                    Console.Clear();
                    Console.WriteLine("第{0}轮次", step++/5+1);
                    #region 显示其他玩家信息
                    string line1 = "", line2 = "", line3 = "", line4 = "", line5 = "";
                    for (int i = 0; i < players.Length; i++)
                    {
                        string temp = "";
                        line1 += " ********************* ";

                        temp = "玩家:" + players[i].Name;
                        line2 += "*";
                        for (int j = 0; j < (20 - temp.Length) / 2; j++)
                            line2 += " ";
                        line2 += temp;
                        for (int j = 0; j < (20 - temp.Length) / 2; j++)
                            line2 += " ";

                        if (players[i].IsGiveUp)
                        {
                            temp = "已弃牌";
                        }
                        else
                        {
                            temp = players[i].IsSee2String;
                        }
                        line3 += "*";
                        for (int j = 0; j < (20 - temp.Length) / 2; j++)
                            line3 += " ";
                        line3 += temp;
                        for (int j = 0; j < (20 - temp.Length) / 2; j++)
                            line3 += " ";

                        temp = "注数:" + players[i].MyBet;
                        line4 += "*";
                        for (int j = 0; j < (20 - temp.Length) / 2; j++)
                            line4 += " ";
                        line4 += temp;
                        for (int j = 0; j < (20 - temp.Length) / 2; j++)
                            line4 += " ";

                        line5 += " ********************* ";
                    }
                    Console.WriteLine(line1);
                    Console.WriteLine(line2);
                    Console.WriteLine(line3);
                    Console.WriteLine(line4);
                    Console.WriteLine(line5);
                    #endregion

                    dealer.OrderedPlayers.Peek().Operate();
                    dealer.OrderedPlayers.Enqueue(dealer.OrderedPlayers.Dequeue());
                    dealer.DealerOpen();
                    Thread.Sleep(500);
                }
                dealer.Rest();
                Console.WriteLine("本轮结束，按任意键继续...");
                Console.ReadKey();
            }

        }
    }
}
