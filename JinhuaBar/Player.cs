using System;
using System.Collections.Generic;
using System.Text;

namespace JinhuaBar
{
    class Player
    {
        public Player(string name)
        {
            this.name = name;
        }
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int chips=10000;
        public int Chips
        {
            get { return chips; }
            set { chips = value; }
        }
        private int myBet = 0;
        public int MyBet
        {
            get { return myBet; }
            set { myBet = value; }
        }
        private List<Card> cards=new List<Card>();
        public List<Card> Cards
        {
            get{ return cards; }
            set
            {
                cards = value;
            }
        }
        private CardType cardType;
        public CardType CardType
        {
            get { return cardType; }
            set { cardType = value; }
        }
        private bool isSee = false;
        public bool IsSee
        {
            get { return isSee; }
            set { isSee = value; }
        }
        public string IsSee2String
        {
            get
            {
                if (IsSee)
                    return "已看牌";
                else
                    return "未看牌";
            }
        }
        private bool isGiveUp=false;
        public bool IsGiveUp
        {
            get { return isGiveUp; }
            set { isGiveUp = value; }
        }
        public virtual void Operate()
        {
            if (!isGiveUp)
            {
                Console.WriteLine("----------玩家{0}，请选择你的操作----------", name);
                See();
                Console.WriteLine("----------1.跟注(剩余筹码{0}，已投注{1})", chips, myBet);
                Console.WriteLine("----------2.加注");
                Console.WriteLine("----------3.看牌({0})-----------", IsSee2String);
                Console.WriteLine("----------4.弃牌-----------");
                Console.WriteLine("----------5.开！-----------");
                string input;
                //输入判断
                while (true)
                {
                    input = Console.ReadLine();
                    if(input=="1")
                    {
                        if (OnCall())
                        {
                            break;
                        }
                    }
                    else if (input == "2")
                    {
                        if (OnAddBet())
                        {
                            break;
                        }
                    }
                    else if (input == "3")
                    {
                        isSee = true;
                        See();
                        Console.WriteLine();
                        Console.WriteLine("请继续操作");
                    }
                    else if (input == "4")
                    {
                        GiveUp();
                        break;
                    }
                    else if (input == "5")
                    {
                        OnOpen();
                        break;
                    }
                    else
                    {
                        Console.Write("请输入1~4的数字，并按下回车");
                    }
                }
            }
            else
            {
                Console.WriteLine("玩家{0}已弃牌，跳过操作",name);
            }
        }
        public delegate bool CallHandler(Player player);
        public event CallHandler Call;
        public bool OnCall()
        {
            return Call(this);
        }
        public delegate bool AddBetHandler(Player player);
        public event AddBetHandler AddBet;
        public bool OnAddBet()
        {
            return AddBet(this);
        }
        public void GiveUp()
        {
            isGiveUp = true;
            Console.WriteLine("玩家{0}弃牌成功",name);
        }
        public void See()
        {
            if (isSee)
            {
                Console.WriteLine("┌─────┐ ┌─────┐ ┌─────┐");
                Console.WriteLine("│ {0}   │ │ {1}   │ │ {2}   │", cards[0].Number2String, cards[1].Number2String, cards[2].Number2String);
                Console.WriteLine("│  {0} │ │  {1} │ │  {2} │", cards[0].Suit2Sharp, cards[1].Suit2Sharp, cards[2].Suit2Sharp);
                Console.WriteLine("│     │ │     │ │     │");
                Console.WriteLine("└─────┘ └─────┘ └─────┘");
            }
            else
            {
                Console.WriteLine("┌─────┐ ┌─────┐ ┌─────┐");
                Console.WriteLine("│     │ │     │ │     │");
                Console.WriteLine("│  ?  │ │  ?  │ │  ?  │");
                Console.WriteLine("│     │ │     │ │     │");
                Console.WriteLine("└─────┘ └─────┘ └─────┘");
            }
        }
        public delegate void OpenHandler(Player player);
        public event OpenHandler Open;
        public void OnOpen()
        {
            Open(this);
        }
        public void Rest()
        {
            cards.Clear();
            isGiveUp = false;
            isSee = false;
            myBet = 0;
        }
        public void JudgeCards()
        {
            bool isLeopard = false;
            bool isGoldeFlower = false;
            bool isJunko = false;
            bool isPair = false;
            //是否为豹子
            if (cards[0].Number==cards[1].Number && cards[0].Number == cards[2].Number)
            {
                isLeopard = true;
            }
            //是否为对子
            if (!isLeopard &&(cards[0].Number == cards[1].Number || cards[0].Number == cards[2].Number || cards[1].Number == cards[2].Number))
            {
                isPair = true;
            }
            if (isLeopard)
            {
                cardType = CardType.Leopard;
                return;
            }
            else if (isPair)
            {
                cardType = CardType.Pair;
                return;
            }
            //是否为金花
            if (cards[0].Suit == cards[1].Suit && cards[0].Suit == cards[2].Suit)
            {
                isGoldeFlower = true;
            }
            //是否为顺子
            if((cards[2].Number-cards[1].Number)==1&& (cards[1].Number - cards[0].Number) == 1)
            {
                isJunko = true;
            }
            if(isGoldeFlower && isJunko)
            {
                cardType = CardType.Flush;
            }
            else if (isGoldeFlower)
            {
                cardType = CardType.GoldeFlower;
            }else if (isJunko)
            {
                cardType = CardType.Junko;
            }
            else
            {
                cardType = CardType.Single;
            }
        }
    }
}
