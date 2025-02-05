using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using static TextRPG.Program;

namespace TextRPG
{
    public class Program
    {
        //  플레이어 클래스
        public class Player
        {
            public int level { get; set; }
            public int exp { get; set; }
            public string chad { get; set; }
            public float attack { get; set; }
            public int attackItem { get; set; }
            public int defense { get; set; }
            public int defenseItem { get; set; }
            public int health { get; set; }
            public int gold { get; set; }
            public Item? weapon { get; set; }
            public Item? armor { get; set; }

            //  Player 클래스의 직렬화를 위한 기본 생성자
            public Player()
            {
                level = 1;
                exp = 0;
                chad = "전사";
                attack = 10;
                attackItem = 0;
                defense = 5;
                defenseItem = 0;
                health = 100;
                gold = 1500;
            }

            // Player 클래스 생성자
            public Player(string _chad, int _attack, int _defense, int _health, int _gold)
            {
                level = 1;
                exp = 0;
                chad = _chad;
                attack = _attack;
                attackItem = 0;
                defense = _defense;
                defenseItem = 0;
                health = _health;
                gold = _gold;
                weapon = new Item("없음", "무기", 0, "-", 0);
                armor = new Item("없음", "방어구", 0, "-", 0);
            }

            //  플레이어의 정보를 보여주는 함수
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

            //  Player 클래스의 데이터를 저장하는 함수
            public void Save(string fileName)
            {
                using (var stream = new FileStream(fileName, FileMode.Create))
                {
                    var XML = new XmlSerializer(typeof(Player));
                    XML.Serialize(stream, this);
                }
            }

            //  Player 클래스의 데이터를 불러오는 함수
            public static Player LoadFromFile(string fileName)
            {
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    var XML = new XmlSerializer(typeof(Player));
                    return (Player)XML.Deserialize(stream)!;
                }
            }
        }

        //  아이템 클래스
        public class Item
        {
            public string name { get; set; }
            public string type { get; set; }
            public int stat { get; set; }
            public string description { get; set; }
            public bool equipState { get; set; }
            public int price { get; set; }

            //  직렬화를 위한 Item 클래스의 기본 생성자
            public Item()
            {
                name = "";
                type = "";
                stat = 0;
                description = "";
                equipState = false;
                price = 0;
            }

            //  Item 클래스의 생성자
            public Item(string _name, string _type, int _stat, string _description, int _price)
            {
                name = _name;
                type = _type;
                stat = _stat;
                description = _description;
                price = _price;
            }

            //  아이템의 정보를 보여주는 클래스
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

            //  아이템을 장착하거나 해제할 때 사용하는 함수
            public void Equip(Player player, Item item)
            {
                if (!equipState)
                {
                    equipState = true;

                    if (type == "방어구")
                    {
                        player.armor!.equipState = false;
                        player.armor = item;
                        player.defenseItem = player.armor.stat;
                    }
                    else if (type == "무기")
                    {
                        player.weapon!.equipState = false;
                        player.weapon = item;
                        player.attackItem = player.weapon.stat;
                    }

                    Console.WriteLine($"{name}을(를) 착용 하였습니다.");
                    Console.WriteLine();
                }
                else
                {
                    equipState = false;

                    if (type == "방어구")
                    {
                        player.armor = new Item("없음", "방어구", 0, "-", 0);
                        player.defenseItem = player.armor.stat;
                    }
                    else if (type == "무기")
                    {
                        player.weapon = new Item("없음", "무기", 0, "-", 0);
                        player.attackItem = player.weapon.stat;
                    }

                    Console.WriteLine($"{name}을(를) 해제 하였습니다.");
                    Console.WriteLine();
                }
            }
        }

        //  인벤토리 클래스
        public class Inventory
        {
            public List<Item> items { get; set; }

            //  Inventory 클래스의 생성자
            public Inventory()
            {
                items = new List<Item>();
            }

            //  인벤토리에 아이템을 넣는 함수
            public void AddItem(Item item)
            {
                items.Add(item);
            }

            //  Inventory 클래스의 데이터를 저장하기 위한 함수
            public void Save(string fileName)
            {
                using (var stream = new FileStream(fileName, FileMode.Create))
                {
                    var XML = new XmlSerializer(typeof(Inventory));
                    XML.Serialize(stream, this);
                }
            }

            //  Inventory 클래스의 데이터를 불러오기 위한 함수
            public static Inventory LoadFromFile(string fileName)
            {
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    var XML = new XmlSerializer(typeof(Inventory));
                    return (Inventory)XML.Deserialize(stream)!;
                }
            }
        }

        //  상점 클래스
        public class Shop
        {
            public List<Item> shopItems { get; set; }
            public List<Item> buyItems { get; set; }

            //  Shop 클래스의 생성자
            public Shop()
            {
                shopItems = new List<Item>();
                buyItems = new List<Item>();
            }

            //  상점에 물품을 추가하는 함수
            public void AddShopItem(Item item)
            {
                shopItems.Add(item);
            }

            //  상점에 있는 아이템을 살 때 호출하는 함수
            public void BuyItem(Player player, Inventory inventory, Item item)
            {
                inventory.AddItem(item);
                buyItems.Add(item);
                player.gold -= item.price;
            }

            //  상점에 있는 아이템의 목록을 보여주는 함수
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

            //  Shop 클래스의 데이터를 저장하는 함수
            public void Save(string fileName)
            {
                using (var stream = new FileStream(fileName, FileMode.Create))
                {
                    var XML = new XmlSerializer(typeof(Shop));
                    XML.Serialize(stream, this);
                }
            }

            //  Shop 클래스의 데이터를 불러오는 함수
            public static Shop LoadFromFile(string fileName)
            {
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    var XML = new XmlSerializer(typeof(Shop));
                    return (Shop)XML.Deserialize(stream)!;
                }
            }
        }

        //  던전 클래스
        public class Dungeon
        {
            public string name { get; set; }
            public int level { get; set; }
            public int dungeonForce { get; set; }
            public int reward { get; set; }
            public bool isClear { get; set; }

            //  Dungeon 클래스의 생성자
            public Dungeon(int _level)
            {
                level = _level;
                isClear = false;

                if (level == 1)
                {
                    name = "쉬운 던전";
                    dungeonForce = 5;
                    reward = 1000;
                }
                else if (level == 2)
                {
                    name = "일반 던전";
                    dungeonForce = 11;
                    reward = 1700;
                }
                else if (level == 3)
                {
                    name = "어려운 던전";
                    dungeonForce = 17;
                    reward = 2500;
                }
                else
                {
                    name = "-";
                    dungeonForce = 0;
                    reward = 0;
                }
            }

            //  Dungeon이 시작되고 클리어를 했는지 못했는지 체크해주는 함수
            public void DungeonPlay(Player player)
            {
                if (player.defense + player.defenseItem >= dungeonForce)
                {
                    isClear = true;
                }
                else
                {
                    if (new Random().Next(0, 10) < 4)
                    {
                        isClear = false;
                    }
                    else
                    {
                        isClear = true;
                    }
                }
            }
        }

        //  시작 마을 장면 함수
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
            Console.WriteLine("4. 던전입장");
            Console.WriteLine("5. 휴식하기");
            Console.WriteLine("6. 저장하기");
            Console.WriteLine("7. 불러오기");

            Console.WriteLine();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                int cmd = -1;

                try
                {
                    cmd = int.Parse(Console.ReadLine()!);

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
                        case 4:
                            StartDungeonScene(player, inventory, shop);
                            break;
                        case 5:
                            StartRestScene(player, inventory, shop);
                            break;
                        case 6:
                            StartSaveScene(player, inventory, shop);
                            break;
                        case 7:
                            StartLoadScene(player, inventory, shop);
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            Console.WriteLine();
                            break;

                    }
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine();
                }
            }
        }

        //  상태 보기 장면 함수
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
                    cmd = int.Parse(Console.ReadLine()!);

                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");

                    switch (cmd)
                    {
                        case 0:
                            StartVilageScene(player, inventory, shop);
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            Console.WriteLine();
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine();
                }
            }
        }

        //  인벤토리 장면 함수
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
                //  아이템이 장착된 상태이면 "[E]"를 추가해준다.
                if (inventory.items[i].equipState)
                {
                    Console.Write("[E] ");
                }
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
                    cmd = int.Parse(Console.ReadLine()!);

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
                            Console.WriteLine();
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine();
                }
            }
        }

        //  장착 관리 장면 함수
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
                //  아이템이 장착되어 있으면 "[E]"를 추가
                if (inventory.items[i].equipState)
                {
                    Console.Write("[E] ");
                }
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

                Console.WriteLine();
                Console.WriteLine("----------------------------------------------------------");

                try
                {
                    cmd = int.Parse(Console.ReadLine()!);

                    if (cmd == 0)
                    {
                        StartInventoryScene(player, inventory, shop);
                    }
                    else if (cmd > 0 && cmd <= inventory.items.Count)
                    {
                        //  플레이어가 선택한 장비를 장착한다.
                        inventory.items[cmd - 1].Equip(player, inventory.items[cmd - 1]);
                        StartEquipManagementScene(player, inventory, shop);
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.WriteLine();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine();
                }
            }
        }

        //  상점 장면 함수
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
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                int cmd = -1;

                try
                {
                    cmd = int.Parse(Console.ReadLine()!);

                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");

                    switch (cmd)
                    {
                        case 1:
                            StartBuyItemScene(player, inventory, shop);
                            break;
                        case 2:
                            StartSaleItemScene(player, inventory, shop);
                            break;
                        case 0:
                            StartVilageScene(player, inventory, shop);
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            Console.WriteLine();
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine();
                }
            }
        }

        //  아이템 구매 장면 함수
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
                    cmd = int.Parse(Console.ReadLine()!);

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
                            //  Shop 클래스의 구매한 아이템을 관리하는 리스트인 buyItems에 사려는 아이템이 존재하면,
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
                            //  Shop 클래스의 구매한 아이템을 관리하는 리스트인 buyItems에 사려는 아이템이 존재하면,
                            if (shop.buyItems.FindIndex(Item => Item.name.Equals(shop.shopItems[cmd - 1].name)) >= 0)
                            {
                                Console.WriteLine("이미 구매한 아이템입니다.");
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine("Gold가 부족합니다.");
                                Console.WriteLine();
                            }
                        }

                        StartBuyItemScene(player, inventory, shop);
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.WriteLine();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine();
                }
            }
        }

        //  아이템 판매 장면 함수
        public static void StartSaleItemScene(Player player, Inventory inventory, Shop shop)
        {
            Console.WriteLine("상점 - 아이템 판매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.gold} G");
            Console.WriteLine();

            for (int i = 0; i < inventory.items.Count; i++)
            {
                Console.Write($"- {i + 1}. ");
                inventory.items[i].ItemInfo();
                Console.Write($"| {inventory.items[i].price} G |");
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
                    cmd = int.Parse(Console.ReadLine()!);

                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");

                    if (cmd == 0)
                    {
                        StartShopScene(player, inventory, shop);
                    }
                    else if (cmd > 0 && cmd <= inventory.items.Count)
                    {
                        //  shop.buyItems(구매한 아이템 목록)과 inventory.items(인벤토리에 존재하는 아이템)에서 판매하려는 아이템의 주소를 저장한다.
                        int saleItemIndex = shop.buyItems.FindIndex(Item => Item.name.Equals(inventory.items[cmd - 1].name));
                        int inventoryItemIndex = inventory.items.FindIndex(Item => Item.name.Equals(inventory.items[cmd - 1].name));

                        //  만약 판매하려는 아이템이 장착 상태이면, 장착을 해제한다.
                        if (inventory.items[inventoryItemIndex].equipState)
                        {
                            inventory.items[inventoryItemIndex].equipState = false;

                            if (inventory.items[inventoryItemIndex].type == "방어구")
                            {
                                player.defenseItem -= inventory.items[inventoryItemIndex].stat;
                            }
                            else if (inventory.items[inventoryItemIndex].type == "무기")
                            {
                                player.attackItem -= inventory.items[inventoryItemIndex].stat;
                            }
                        }

                        //  판매한 아이템 가격의 85%만큼 gold를 획득한다.
                        player.gold += (inventory.items[inventoryItemIndex].price) * 85 / 100;

                        //  구매한 아이템 목록과 인벤토리에 존재하는 아이템 리스트에서 판매한 아이템을 삭제한다.
                        shop.buyItems.RemoveAt(saleItemIndex);
                        inventory.items.RemoveAt(inventoryItemIndex);


                        StartSaleItemScene(player, inventory, shop);
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.WriteLine();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine();
                }
            }
        }

        //  던전 장면 함수
        public static void StartDungeonScene(Player player, Inventory inventory, Shop shop)
        {
            Console.WriteLine("던전입장");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");

            Console.WriteLine();

            Console.WriteLine("1. 쉬운 던전      | 방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전      | 방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전    | 방어력 17 이상 권장");
            Console.WriteLine("0. 나가기");

            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                int cmd = -1;

                try
                {
                    cmd = int.Parse(Console.ReadLine()!);

                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");

                    switch (cmd)
                    {
                        case 1:
                            Dungeon dungeon1 = new Dungeon(1);
                            dungeon1.DungeonPlay(player);

                            if (dungeon1.isClear)
                            {
                                StartDungeonClearScene(player, inventory, shop, dungeon1);
                            }
                            else
                            {
                                StartDungeonFailScene(player, inventory, shop, dungeon1);
                            }
                            break;
                        case 2:
                            Dungeon dungeon2 = new Dungeon(2);
                            dungeon2.DungeonPlay(player);

                            if (dungeon2.isClear)
                            {
                                StartDungeonClearScene(player, inventory, shop, dungeon2);
                            }
                            else
                            {
                                StartDungeonFailScene(player, inventory, shop, dungeon2);
                            }
                            break;
                        case 3:
                            Dungeon dungeon3 = new Dungeon(2);
                            dungeon3.DungeonPlay(player);

                            if (dungeon3.isClear)
                            {
                                StartDungeonClearScene(player, inventory, shop, dungeon3);
                            }
                            else
                            {
                                StartDungeonFailScene(player, inventory, shop, dungeon3);
                            }
                            break;
                        case 0:
                            StartVilageScene(player, inventory, shop);
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            Console.WriteLine();
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine();
                }
            }
        }

        //  던전 클리어 장면 함수
        public static void StartDungeonClearScene(Player player, Inventory inventory, Shop shop, Dungeon dungeon)
        {

            int playerHP = player.health - new Random().Next(20, 36) + player.defense + player.defenseItem - dungeon.dungeonForce;

            if(playerHP > 100)
            {
                playerHP = 100;
            }

            if (playerHP < 0)
            {
                Console.WriteLine("**********");
                Console.WriteLine("GameOver!");
                Console.WriteLine("**********");
                Console.WriteLine();
                Console.WriteLine("체력이 전부 닳았습니다!!");

                Environment.Exit(0);
            }

            Console.WriteLine("던전 클리어");
            Console.WriteLine("축하합니다!!");
            Console.WriteLine($"{dungeon.name}을 클리어 하였습니다.");

            Console.WriteLine();

            Console.WriteLine("[탐험 결과]");
            Console.Write($"체력 {player.health} -> ");
            player.health = playerHP;
            Console.WriteLine(player.health);

            Console.Write($"Gold {player.gold} G -> ");
            player.gold = player.gold + (dungeon.reward * new Random().Next((int)player.attack + player.attackItem, ((int)player.attack + player.attackItem)* 2 + 1) / 100);
            Console.WriteLine($"{player.gold} G");

            Console.Write($"EXP {player.exp} -> ");
            player.exp++;
            Console.WriteLine($"EXP {player.exp}");

            if (player.level == player.exp)
            {
                Console.WriteLine();
                Console.WriteLine("[레벨업!]");
                Console.Write($"LV {player.level} -> ");
                player.level++;
                Console.WriteLine($"LV {player.level}");

                Console.Write($"EXP {player.exp} -> ");
                player.exp = 0;
                Console.WriteLine(player.exp);

                Console.Write($"기본공격력 {player.attack} -> ");
                player.attack += 0.5f;
                Console.WriteLine(player.attack);

                Console.Write($"기본방어력 {player.defense} -> ");
                player.defense += 1;
                Console.WriteLine(player.defense);
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
                    cmd = int.Parse(Console.ReadLine()!);

                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");

                    switch (cmd)
                    {
                        case 0:
                            StartVilageScene(player, inventory, shop);
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            Console.WriteLine();
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine();
                }
            }
        }

        //  던전 실패 장면 함수
        public static void StartDungeonFailScene(Player player, Inventory inventory, Shop shop, Dungeon dungeon)
        {
            int playerHP = player.health / 2;

            if (playerHP < 0)
            {
                Console.WriteLine("**********");
                Console.WriteLine("GameOver!");
                Console.WriteLine("**********");
                Console.WriteLine();
                Console.WriteLine("체력이 전부 닳았습니다!!");

                Environment.Exit(0);
            }

            Console.WriteLine("던전 실패");
            Console.WriteLine($"{dungeon.name}을 클리어하지 못했습니다.");

            Console.WriteLine();

            Console.WriteLine("[탐험 결과]");
            Console.Write($"체력 {player.health} -> ");
            player.health = playerHP;
            Console.WriteLine($"{player.health}");

            Console.WriteLine($"Gold {player.gold} G -> {player.gold} G");

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
                    cmd = int.Parse(Console.ReadLine()!);

                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");

                    switch (cmd)
                    {
                        case 0:
                            StartVilageScene(player, inventory, shop);
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            Console.WriteLine();
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine();
                }
            }
        }

        //  휴식하기 장면 함수
        public static void StartRestScene(Player player, Inventory inventory, Shop shop)
        {
            Console.WriteLine("휴식하기");
            Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.gold}) G");

            Console.WriteLine();

            Console.WriteLine("1. 휴식하기");
            Console.WriteLine("0. 나가기");

            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                int cmd = -1;

                try
                {
                    cmd = int.Parse(Console.ReadLine()!);

                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");

                    switch (cmd)
                    {
                        case 1:
                            if (player.gold >= 500)
                            {
                                player.gold -= 500;
                                player.health = 100;
                                Console.WriteLine("휴식을 완료했습니다.");
                                Console.WriteLine();
                                StartVilageScene(player, inventory, shop);
                            }
                            else
                            {
                                Console.WriteLine("Gold가 부족합니다.");
                                Console.WriteLine();
                            }
                            break;
                        case 0:
                            StartVilageScene(player, inventory, shop);
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            Console.WriteLine();
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine();
                }
            }
        }

        //  저장하기 장면 함수
        public static void StartSaveScene(Player player, Inventory inventory, Shop shop)
        {
            Console.WriteLine("저장하기");
            Console.WriteLine($"현재까지 플레이한 데이터를 저장할 수 있습니다.");

            Console.WriteLine();

            Console.WriteLine("1. 저장하기");
            Console.WriteLine("0. 나가기");

            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                int cmd = -1;

                try
                {
                    cmd = int.Parse(Console.ReadLine()!);

                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");

                    switch (cmd)
                    {
                        case 1:
                            DataSave(player, inventory, shop);
                            Console.WriteLine("데이터를 저장했습니다.");
                            Console.WriteLine();
                            StartVilageScene(player, inventory, shop);
                            break;
                        case 0:
                            StartVilageScene(player, inventory, shop);
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            Console.WriteLine();
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine();
                }
            }
        }

        //  현재 게임의 데이터를 저장하는 함수
        public static void DataSave(Player player, Inventory inventory, Shop shop)
        {
            player.Save("Save/player.xml");
            inventory.Save("Save/inventory.xml");
            shop.Save("Save/shop.xml");
        }

        //  불러오기 장면 함수
        public static void StartLoadScene(Player player, Inventory inventory, Shop shop)
        {
            Console.WriteLine("불러오기");
            Console.WriteLine($"이전에 저장했던 데이터를 불러올 수 있습니다.");

            Console.WriteLine();

            Console.WriteLine("1. 불러오기");
            Console.WriteLine("0. 나가기");

            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                int cmd = -1;

                try
                {
                    cmd = int.Parse(Console.ReadLine()!);

                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");

                    switch (cmd)
                    {
                        case 1:
                            DataLoad(ref player, ref inventory, ref shop);
                            Console.WriteLine("데이터를 불러왔습니다.");
                            Console.WriteLine();
                            StartVilageScene(player, inventory, shop);
                            break;
                        case 0:
                            StartVilageScene(player, inventory, shop);
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            Console.WriteLine();
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine();
                }
            }
        }

        //  이전에 저장했던 게임의 데이터를 불러오는 함수
        public static void DataLoad(ref Player player, ref Inventory inventory, ref Shop shop)
        {
            player = Player.LoadFromFile("Save/player.xml");
            inventory = Inventory.LoadFromFile("Save/inventory.xml");
            shop = Shop.LoadFromFile("Save/shop.xml");
        }
        static void Main(string[] args)
        {
            //  클래스 생성
            Player player = new Player("전사", 10, 5, 100, 1500);
            Inventory inventory = new Inventory();
            Shop shop = new Shop();

            //  아이템 목록
            Item noviceArmor = new Item("수련자갑옷", "방어구", 5, "수련에 도움을 주는 갑옷입니다.", 1000);
            Item ironArmor = new Item("무쇠갑옷", "방어구", 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 1800);
            Item spartaArmor = new Item("스파르타의 갑옷", "방어구", 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500);
            Item oldSword = new Item("낡은 검", "무기", 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600);
            Item bronzeAxe = new Item("청동 도끼", "무기", 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500);
            Item spartaSpear = new Item("스파르타의 창", "무기", 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2700);

            //  자작 아이템
            Item gmSword = new Item("운영자의 검", "무기", 999, "운영자용 테스트 검", 99999);
            Item gmHat = new Item("운영자의 모자", "방어구", 999, "운영자용 테스트 모자", 99999);

            //  초기 플레이어 아이템
            inventory.AddItem(gmSword);
            shop.buyItems.Add(gmSword);
            inventory.AddItem(gmHat);
            shop.buyItems.Add(gmHat);
            inventory.AddItem(spartaSpear);
            shop.buyItems.Add(ironArmor);
            inventory.AddItem(ironArmor);
            shop.buyItems.Add(spartaSpear);

            //  상점 아이템
            shop.AddShopItem(noviceArmor);
            shop.AddShopItem(ironArmor);
            shop.AddShopItem(spartaArmor);
            shop.AddShopItem(oldSword);
            shop.AddShopItem(bronzeAxe);
            shop.AddShopItem(spartaSpear);

            //  마을부터 게임시작
            StartVilageScene(player, inventory, shop);
        }
    }
}