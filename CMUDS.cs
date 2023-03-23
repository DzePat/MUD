using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace D8_porting_WPF
{
    public class CMUDS
    {
        public static List<CMUD> Rooms = new List<CMUD>();
        public static Player player = new Player();
        public static List<string> keystrings = new List<string>();
        public static Player.Inventory Inventory = new Player.Inventory(keystrings, 0);
        public static List<CMUD.Objects[]> objects = new List<CMUD.Objects[]>();
        public static int searchcount = 1;
        public static int skeletoncount = 1;
        public static string Error = "";
        public static string wizardanswer = "";
        public static string test = "";

        public class CMUD
        {
            private string name, presentation, keytype;
            private int number, north, east, south, west;
            private bool doorstatus;
            private CMUD.Objects[] items;

            public string keyType
            {
                get { return keytype; }
                set { keytype = value; }
            }
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            public string Presentation
            {
                get { return presentation; }
                set { presentation = value; }
            }
            public int North
            {
                get { return north; }
                set { north = value; }
            }
            public int East
            {
                get { return east; }
                set { east = value; }
            }
            public int South
            {
                get { return south; }
                set { south = value; }
            }
            public int West
            {
                get { return west; }
                set { west = value; }
            }
            public int Number
            {
                get { return number; }
                set { number = value; }
            }
            public CMUD.Objects[] Items
            {
                get { return items; }
                set { items = value; }
            }
            public bool DoorStatus
            {
                get { return doorstatus; }
                set { doorstatus = value; }
            }
            public class Objects
            {
                string place, item;

                public string Place
                {
                    get { return place; }
                    set { place = value; }
                }
                public string Item
                {
                    get { return item; }
                    set { item = value; }
                }
                public Objects(string pl, string it)
                {
                    place = pl;
                    item = it;
                }
            }
            public CMUD(int numb, string na, string pres, int nor, int eas, int sou, int wes, CMUD.Objects[] ite, bool ds, string keyType)
            {
                Number = numb;
                Name = na;
                Presentation = pres;
                North = nor;
                East = eas;
                South = sou;
                West = wes;
                items = ite;
                doorstatus = ds;
                this.keyType = keyType;
            }
        }
        public class Player
        {
            private int playernumber, previousnumber;
            public int PlayerNumber
            {
                get { return playernumber; }
                set { playernumber = value; }
            }
            public int Previousnumber
            {
                get { return previousnumber; }
                set { previousnumber = value; }
            }
            public class Inventory
            {
                private List<string> keys;
                private int coins;

                public List<string> Keys
                {
                    get { return keys; }
                    set { keys = value; }
                }
                public int Coins
                {
                    get { return coins; }
                    set { coins = value; }
                }
                public Inventory(List<string> key, int coin)
                {
                    keys = key;
                    coins = coin;
                }
            }
        }
        //returns the length of the string
        public static bool comlength(string[] test)
        {
            return test.Length > 1;
        }

        //prints the presentation
        public static void printpresentation()
        {
            Console.Out.WriteLine($"{Rooms[player.PlayerNumber].Name}\n{Rooms[player.PlayerNumber].Presentation}");
        }
        //places the player in the first room of the file
        public static void start()
        {
            try
            {
                player.PlayerNumber = Rooms[0].Number;
                printpresentation();
            }
            catch { Console.WriteLine("please load a file first"); }

        }
        //search option
        public static void Search(string loc)
        {

            try
            {
                foreach (CMUD.Objects a in objects[player.PlayerNumber])
                {
                    Console.WriteLine(a.Place);
                }
                string location = loc;
                Console.WriteLine("searching the object");
                int checker = 0;
                foreach (CMUD.Objects obj in objects[player.PlayerNumber])
                {
                    if (location == obj.Place)
                    {
                        if (obj.Place == "wizard")
                        {
                            
                                if (true == wizardquiz(wizardanswer))
                                {
                                    getitem(obj);
                                    
                                }
                                else {
                                    Error = "you got punished";
                                    Console.WriteLine("you got punished"); }
                        }
                        else if (obj.Place == "Pile of trash")
                        {
                            if (searchcount == 0)
                            {
                                getitem(obj);
                            }
                            else
                            {
                                Console.WriteLine("you remove the big objects from the pile but find nothing");
                                searchcount = searchcount - 1;
                            }
                        }
                        else if (obj.Place == "skeleton")
                        {
                            if (true == skeletons())
                            {
                                getitem(obj);
                            }
                        }
                        else
                        {
                            getitem(obj);
                        }
                        checker = 1;
                    }
                }
                if (checker == 0)
                {
                    Console.WriteLine("could not find the item");
                }
            }
            catch (System.ArgumentOutOfRangeException) { Console.WriteLine("type 'start' to start the game"); }
        }

        private static void getitem(CMUD.Objects obj)
        {
            int checker;
            string item = obj.Item;
            test = item;
            if (item == null)
            {
                Error = "You found nothing";
                Console.WriteLine("You found nothing");

            }
            else if (int.TryParse(item, out int value))
            {
                Error = $"you found {int.Parse(item)} coins";
                Console.WriteLine($"you found {int.Parse(item)} coins");
                Inventory.Coins = Inventory.Coins + int.Parse(item);
                obj.Item = null;
            }
            else
            {
                Error = $"you got a {item}!!";
                Console.WriteLine($"you got a {item}!!");
                Inventory.Keys.Add(item);
                obj.Item = null;
            }
        }

        //go north
        public static void n()
        {
            if (player.PlayerNumber != 4)
            {
                Move(1, 0, 0, 0);
            }
            else { Console.WriteLine("you went into the a wall"); }
        }
        //go east
        public static void e()
        {
            Move(0, 1, 0, 0);
        }
        //go south
        public static void s()
        {
            Move(0, 0, 1, 0);
        }
        //go west
        public static void w()
        {
            Move(0, 0, 0, 1);
        }

        //moves to a direction sellected above
        private static void Move(int a, int b, int c, int d)
        {
            player.Previousnumber = player.PlayerNumber;
            getrum(player.PlayerNumber, a, b, c, d);
            if (player.PlayerNumber != player.Previousnumber)
            {
                printpresentation();
            }
        }

        // go west
        //equalizes the south and north value and same with the east and west then searches the coordinates if they exist if they do places a player into that room.
        public static void getrum(int CR, int north, int east, int south, int west)
        {
            CMUD a = Rooms[CR];
            int checker = 0;
            int check2 = 0;
            int[] newdir = equalize(a.North + north, a.East + east, a.South + south, a.West + west);
            foreach (CMUD cmud in Rooms)
            {
                if (cmud.North == newdir[0] && cmud.East == newdir[1] && cmud.South == newdir[2] && cmud.West == newdir[3])
                {
                    if (cmud.DoorStatus == true)
                    {
                        int checker1 = 0;
                        foreach (string key in Inventory.Keys)
                        {
                            if (key == cmud.keyType)
                            {
                                checker1 = 1;
                            }
                        }
                        if (checker1 == 1)
                        {
                            Error = $"You unlocked the door";
                            a.DoorStatus = false;
                            Console.WriteLine($"You unlocked the door with a {cmud.keyType}");
                            player.PlayerNumber = cmud.Number;
                            checker++;
                        }
                        else
                        {
                            check2 = 1; Error = $"Door is locked.";
                            Console.WriteLine(Error);
                        }

                    }
                    else
                    {
                        Error = "";
                        player.PlayerNumber = cmud.Number;
                        checker++;
                    }
                }
            }
            if (checker == 0 && check2 != 1)
            {
                Error = "You went into the wall";
                Console.WriteLine("You went into the wall");
            }
        }

        //adds an object to the list
        public static CMUD.Objects addobject(string place, string item)
        {
            return new CMUD.Objects(place, item);
        }

        //prints players inventory
        public static void Printinventory()
        {
            foreach (string key in Inventory.Keys)
            {

                Console.WriteLine($"Key: {key}");
            }
            Console.WriteLine($"Coins: {Inventory.Coins}");
        }

        //loads a file into an array of CMUD objects.
        public static void loadFile(string file)

        {
            Rooms.Clear();
            string path = "C:\\Users\\dzeda\\source\\repos\\D3OVN12";
            string filename = file;
            string main = path + "\\" + filename;
            
            if (File.Exists(main))
            {
                string lines = File.ReadAllText(main);
                string[] parts = lines.Split(';');
                LoadingObjects();
                int i = 6;
                string[] keylocations = { null, "Tree", null, null, null, null, "search twice", "kill to skeletons", "gold chest", null };
                string[] keytypes = { null, "deadmans key", null, null, null, null, "skeleton key", "lost key", "tomb key", "wizards key" };
                int ki = 0;
                bool DS;
                while (i < parts.Length)
                {
                    if (keylocations[ki] != null)
                    {
                        DS = true;
                    }
                    else { DS = false; }
                    int number = int.Parse(parts[i - 6]);
                    string name = parts[i - 5];
                    string presentation = parts[i - 4];
                    int north = int.Parse(parts[i - 3]);
                    int east = int.Parse(parts[i - 2]);
                    int south = int.Parse(parts[i - 1]);
                    int west = int.Parse(parts[i]);
                    CMUD room = new CMUD(number, name, presentation, north, east, south, west, objects[ki], DS, keytypes[ki]);
                    //Console.WriteLine($"number: {room.Number}  name: {room.Name} n: {room.North} e: {room.East} s: {room.South} w: {room.West}");
                    Rooms.Add(room);
                    i = i + 7;
                    ki = ki + 1;
                }
                Console.WriteLine($"Loaded {Rooms.Count} rooms from file.");
            }
        }

        //loads the objects and items into the rooms
        private static void LoadingObjects()
        {
            objects.Add(getObject("tree", "deadmans key", "corpse", "20"));
            objects.Add(getObject("pillar", null, "shiny", "20"));
            objects.Add(getObject("armor", null, "chest", "30"));
            objects.Add(getObject("fountain", null, "Flower Patch", "40"));
            objects.Add(getObject("pile off snow", null, "Snowman", "lost key"));
            objects.Add(getObject("wizard", "wizards key", "bench", null));
            objects.Add(getObject("Pile of trash", "tomb key", "cleaning supplies", "15"));
            objects.Add(getObject("skeleton", "skeleton key", "tombstone", "50"));
            objects.Add(getObject("golden chest", "5000", "shiny pile", null));
            objects.Add(getObject("lantern", null, "old statue", "20"));
            //Objects.Add(getObject(""));

        }
        //returns true if you answer correctly else false
        public static bool wizardquiz(string input)
        {
            Console.WriteLine("abra?");
            if (input == "kadabra")
            {
                return true;
            }
            else { return false; }

        }
        //returns true if you beat all the skeletons
        public static bool skeletons()
        {
            if (skeletoncount == 0)
            {
                Console.WriteLine("The last skeleton has been slayed it looks like it dropped something");
                return true;
            }
            else
            {
                Console.WriteLine("you have defeated a skeleton with the mighty blow");
                skeletoncount = 0;
                return false;
            }
        }
        public static CMUD.Objects[] getObject(string a, string b, string c, string d)
        {
            CMUD.Objects[] object1 = { addobject(a, b), addobject(c, d) };
            return object1;
        }

        //save a room the file
        public static async Task savefile(string file)
        {
            string path = "C:\\Users\\dzeda\\source\\repos\\D3OVN12";
            string filename = file;
            string main = path + "\\" + filename;



            foreach (CMUD mud in Rooms)
            {
                string text = $"{mud.Number};{mud.Name};{mud.Presentation};{mud.North};{mud.East};{mud.South};{mud.West};";
                using StreamWriter filew = new(path + "\\" + "test.txt", append: true);
                await filew.WriteLineAsync(text);

            }
        }

        //equalizes the coordinates north and south , west and east from the main room 0,0,0,0.
        public static int[] equalize(int a, int b, int c, int d)
        {
            int[] anb = new int[4];
            if (a >= c)
            {
                anb[0] = a - c;
                anb[2] = 0;
                if (b >= d)
                {
                    anb[1] = b - d;
                    anb[3] = 0;
                }
                else if (d > b)
                {
                    anb[1] = 0;
                    anb[3] = d - b;
                }

            }
            else if (c > a)
            {
                anb[0] = 0;
                anb[2] = c - a;
                if (b >= d)
                {
                    anb[1] = b - d;
                    anb[3] = 0;
                }
                else if (d > b)
                {
                    anb[1] = 0;
                    anb[3] = d - b;
                }
            }
            return anb;
        }

        //prints a welcome message
        public static void PrintWelcome()
        {
            Console.WriteLine("Hello and welcome to CMUD");
            Console.WriteLine("enter 'start' to start the game.");
            Console.WriteLine("Write 'help' for help!");
        }

        //prints out all available commands
        public static void WriteTheHelp()
        {
            string[] hstr = {
                "help  - display this help",
                "load  - load all links from a file",
                "start  - go to room 0",
                "save - save to a test file",
                "inventory - shows what you got in inventory",
                "n - go north",
                "e - go east",
                "s - go south",
                "w - go west",
                "quit  - quit the program"
            };
            foreach (string h in hstr) Console.WriteLine(h);
        }

    }
}
