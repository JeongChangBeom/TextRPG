using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using static TextRPG.Program;

namespace TextRPG
{
    internal class Program
    {
        public class Player
        {
            public int level { get; set; }
            public string chad { get; set; }
            public int attack { get; set; }
            public int attackItem { get; set; }
            public int defense { get; set; }
            public int defenseItem { get; set; }
            public int health { get; set; }
            public int gold { get; set; }

            public Player(string _chad, int _attack, int _defense, int _health, int _gold)
            {
                level = 1;
                chad = _chad;
                attack = _attack;
                defense = _defense;
                health = _health;
                gold = _gold;
            }

            public void PlayerInfo()
            {
                Console.WriteLine("상태보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.");

                Console.WriteLine();

                Console.WriteLine($"Lv. {level}");
                Console.WriteLine($"Chad ( {chad} )");
                if (attackItem > 0)
                {
                    Console.WriteLine($"공격력 : {attack + attackItem} (+{attackItem})");
                }
                else
                {
                    Console.WriteLine($"공격력 : {attack}");
                }

                if (defenseItem > 0)
                {
                    Console.WriteLine($"방어력 : {defense + defenseItem} (+{defenseItem})");
                }
                else
                {
                    Console.WriteLine($"방어력 : {defense}");
                }
                Console.WriteLine($"체 력 : {health}");
                Console.WriteLine($"Gold : {gold} G");
            }
        }

        public class Item
        {
            public string name { get; set; }
            public string type { get; set; }
            public int stat { get; set; }
            public string description { get; set; }
            public bool equipState { get; set; }
            public int price { get; set; }


            public Item(string _name, string _type, int _stat, string _description, int _price)
            {
                name = _name;
                type = _type;
                stat = _stat;
                description = _description;
                price = _price;
            }

            public void ItemInfo()
            {
                int spaceLength = 12;

                Console.Write($"{name}");

                for (int j = 0; j < spaceLength - name.Length; j++)
                {
                    Console.Write("  ");
                }

                if (type == "방어구")
                {
                    Console.Write($"| 방어력 +{stat} | {description} ");
                }
                else if (type == "무기")
                {
                    Console.Write($"| 공격력 +{stat} | {description} ");
                }
            }

            public void Equip(Player player)
            {
                StringBuilder sb = new StringBuilder();

                if (!equipState)
                {
                    equipState = true;

                    if (type == "방어구")
                    {
                        player.defenseItem += stat;
                    }
                    else if (type == "무기")
                    {
                        player.attackItem += stat;
                    }

                    Console.WriteLine($"{name}을(를) 착용 하였습니다.");

                    sb.Append("[E]" + name);

                    name = sb.ToString();

                    Console.WriteLine();
                }
                else
                {
                    equipState = false;

                    if (type == "방어구")
                    {
                        player.defenseItem -= stat;
                    }
                    else if (type == "무기")
                    {
                        player.attackItem -= stat;
                    }

                    sb.Append(name);
                    sb.Remove(0, 3);

                    name = sb.ToString();

                    Console.WriteLine($"{name}을(를) 해제 하였습니다.");
                    Console.WriteLine();
                }
            }
        }

        public class Inventory
        {
            public List<Item> items { get; set; }

            public Inventory()
            {
                items = new List<Item>();
            }

            public void AddItem(Item item)
            {
                items.Add(item);
            }
        }

        public class Shop
        {
            public List<Item> shopItems { get; set; }
            public List<Item> buyItems { get; set; }

            public Shop()
            {
                shopItems = new List<Item>();
                buyItems = new List<Item>();
            }

            public void AddShopItem(Item item)
            {
                shopItems.Add(item);
            }

            public void BuyItem(Player player, Inventory inventory, Item item)
            {
                inventory.AddItem(item);
                buyItems.Add(item);
                player.gold -= item.price;
            }

            public void ShopItemListView()
            {
                Console.WriteLine("[아이템 목록]");
                Console.WriteLine();

                for (int i = 0; i < shopItems.Count; i++)
                {
                    Console.Write($"- {i + 1}. ");
                    shopItems[i].ItemInfo();

                    if (buyItems.FindIndex(item => item.name.Equals(shopItems[i].name)) == -1)
                    {
                        Console.Write($"| {shopItems[i].price} G |");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.Write($"| 구매완료 |");
                        Console.WriteLine();
                    }
                }
            }
        }

        public static void StartVilageScene(Player player, Inventory inventory, Shop shop)
        {
            Console.WriteLine("****************************************************");
            Console.WriteLine();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("****************************************************");

            Console.WriteLine();

            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");

            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                int cmd = -1;

                try
                {
                    cmd = int.Parse(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }

                Console.WriteLine();
                Console.WriteLine("----------------------------------------------------------");

                switch (cmd)
                {
                    case 1:
                        StartStateScene(player, inventory, shop);
                        break;
                    case 2:
                        StartInventoryScene(player, inventory, shop);
                        break;
                    case 3:
                        StartShopScene(player, inventory, shop);
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }

                if (cmd == 1 || cmd == 2 || cmd == 3)
                {
                    break;
                }
            }

            Console.WriteLine();

            Console.WriteLine("----------------------------------------------------------");
        }

        public static void StartStateScene(Player player, Inventory inventory, Shop shop)
        {
            player.PlayerInfo();

            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                int cmd = -1;

                try
                {
                    cmd = int.Parse(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }

                Console.WriteLine();
                Console.WriteLine("----------------------------------------------------------");

                switch (cmd)
                {
                    case 0:
                        StartVilageScene(player, inventory, shop);
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }

                if (cmd == 0)
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------------");
        }

        public static void StartInventoryScene(Player player, Inventory inventory, Shop shop)
        {
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();


                Console.WriteLine("[아이템 목록]");
                Console.WriteLine();

                for (int i = 0; i < inventory.items.Count; i++)
                {
                    Console.Write("- ");
                    inventory.items[i].ItemInfo();
                    Console.WriteLine();
                }


            Console.WriteLine();
            Console.WriteLine("1. 장착관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                int cmd = -1;

                try
                {
                    cmd = int.Parse(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }

                Console.WriteLine();
                Console.WriteLine("----------------------------------------------------------");

                switch (cmd)
                {
                    case 0:
                        StartVilageScene(player, inventory, shop);
                        break;
                    case 1:
                        StartEquipManagementScene(player, inventory, shop);
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
                if (cmd == 0 || cmd == 1)
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------------");
        }

        public static void StartEquipManagementScene(Player player, Inventory inventory, Shop shop)
        {
            Console.WriteLine("인벤토리 - 장착관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();

            Console.WriteLine("[아이템 목록]");
            Console.WriteLine();

            for (int i = 0; i < inventory.items.Count; i++)
            {
                Console.Write($"- {i + 1}. ");
                inventory.items[i].ItemInfo();
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                int cmd = -1;

                try
                {
                    cmd = int.Parse(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }

                Console.WriteLine();
                Console.WriteLine("----------------------------------------------------------");

                if (cmd == 0)
                {
                    StartInventoryScene(player, inventory, shop);
                }
                else if (cmd > 0 && cmd <= inventory.items.Count)
                {
                    inventory.items[cmd - 1].Equip(player);
                    StartEquipManagementScene(player, inventory, shop);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }

                if (cmd == 0 || (cmd > 0 && cmd <= inventory.items.Count))
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------------");
        }

        public static void StartShopScene(Player player, Inventory inventory, Shop shop)
        {
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.gold} G");
            Console.WriteLine();

            shop.ShopItemListView();

            Console.WriteLine();
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                int cmd = -1;

                try
                {
                    cmd = int.Parse(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }

                Console.WriteLine();
                Console.WriteLine("----------------------------------------------------------");

                switch (cmd)
                {
                    case 1:
                        StartBuyItemScene(player, inventory, shop);
                        break;
                    case 0:
                        StartVilageScene(player, inventory, shop);
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }

                if (cmd == 0 || (cmd > 0 && cmd <= inventory.items.Count))
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------------");
        }


        public static void StartBuyItemScene(Player player, Inventory inventory, Shop shop)
        {
            Console.WriteLine("상점 - 아이템 구매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.gold} G");
            Console.WriteLine();

            shop.ShopItemListView();

            Console.WriteLine();

            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                int cmd = -1;

                try
                {
                    cmd = int.Parse(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }

                Console.WriteLine();
                Console.WriteLine("----------------------------------------------------------");

                if (cmd == 0)
                {
                    StartShopScene(player, inventory, shop);
                }
                else if (cmd > 0 && cmd <= shop.shopItems.Count)
                {
                    if (player.gold >= shop.shopItems[cmd - 1].price)
                    {
                        if (shop.buyItems.FindIndex(Item => Item.name.Equals(shop.shopItems[cmd - 1].name)) >= 0)
                        {
                            Console.WriteLine("이미 구매한 아이템입니다.");
                            Console.WriteLine();
                        }
                        else
                        {
                            shop.BuyItem(player, inventory, shop.shopItems[cmd - 1]);
                            Console.WriteLine("구매를 완료했습니다.");
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Gold가 부족합니다.");
                        Console.WriteLine();
                    }

                    StartBuyItemScene(player, inventory, shop);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }

                if (cmd == 0 || (cmd > 0 && cmd <= inventory.items.Count))
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------------");
        }


        static void Main(string[] args)
        {
            Player player = new Player("전사", 10, 5, 100, 1500);
            Inventory inventory = new Inventory();
            Shop shop = new Shop(); ;

            Item noviceArmor = new Item("수련자갑옷", "방어구", 5, "수련에 도움을 주는 갑옷입니다.", 1000);
            Item ironArmor = new Item("무쇠갑옷", "방어구", 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 1800);
            Item spartaArmor = new Item("스파르타의 갑옷", "방어구", 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500);
            Item oldSword = new Item("낡은 검", "무기", 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600);
            Item bronzeAxe = new Item("청동 도끼", "무기", 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500);
            Item spartaSpear = new Item("스파르타의 창", "무기", 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2700);

            inventory.AddItem(spartaSpear);
            shop.buyItems.Add(ironArmor);
            inventory.AddItem(ironArmor);
            shop.buyItems.Add(spartaSpear);
            
            shop.AddShopItem(noviceArmor);
            shop.AddShopItem(ironArmor);
            shop.AddShopItem(spartaArmor);
            shop.AddShopItem(oldSword);
            shop.AddShopItem(bronzeAxe);
            shop.AddShopItem(spartaSpear);

            StartVilageScene(player, inventory, shop);
        }
    }
}