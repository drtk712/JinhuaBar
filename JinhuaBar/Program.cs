using System;
using System.Collections.Generic;
using System.Threading;

namespace JinhuaBar
{
    //Console.WriteLine("┌─────┐ ┌─────┐ ┌─────┐");
    //Console.WriteLine("│3    │ │3    │ │4    │");
    //Console.WriteLine("│  ♣ │ │  ♦ ♦ │ │  ♥ │");
    //Console.WriteLine("│     │ │     │ │     │");
    //Console.WriteLine("└─────┘ └─────┘ └─────┘");
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            TestGame();
            ♦
        }

        public static void TestGame()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("欢迎进行本游戏，请输入游玩人数");
            Player[] players = new Player[Convert.ToInt32(Console.ReadLine())];
            for(int i = 0; i < players.Length; i++)
            {
                Console.WriteLine("请输入第{0}位玩家的昵称", i + 1);
                players[i] = new Player(Console.ReadLine());
            }
            Pokers poker = new Pokers();
            Dealer dealer = new Dealer();
            int count = 1;
            while (true)
            {
                Console.Write("开始第{0}轮游戏\n场上玩家筹码情况：\n", count++);
                for (int i = 0; i < players.Length; i++)
                {
                    Console.WriteLine("{0}.玩家：{1} 筹码：{2}", i + 1,players[i].Name,players[i].Chips);
                }
                Console.WriteLine("按任意键继续...");
                Console.ReadKey();
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
                        Thread.Sleep(1000);
                    }
                }
                Console.WriteLine("发牌完毕，开始游戏...");
                Thread.Sleep(2000);
                int step = 0;
                while (true)
                {
                    players[step++ % players.Length].Operate();
                }
                
            }
        }
    }
}
