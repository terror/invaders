using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace FinalProject
{
    class Object
    {
        public int x;
        public int y;
        public char onscreen;
    }

    class Program
    {
        //DEFAULT SETTINGS//
        static double startSpeed = 100.0;
        static double speed = startSpeed;
        static int points = 0;
        static int livesCount = 5;
        static double maxspeed = 500.0;
        static double acceleration = 0.5;
        static int livesObjectChance = 10;
        static int pointsObjectChance = 20;
        static int playfieldWidth = 10;
        //CHANGEABLE IN SETTINGS TAB//

        static Object newObject = new Object();
        static Object newBullet = new Object();
        static Random value = new Random();
        static bool hit;
        static bool bulletHit;

        static void Main(string[] args)
        {
            // set window size on windows machine
            // Console.BufferHeight = Console.WindowHeight = 20;
            // Console.BufferWidth = Console.WindowWidth = 30;

            //Creation of the player object
            Object player = new Object();
            player.x = 2;
            player.y = Console.WindowHeight - 1;
            player.onscreen = 'X';

            List<Object> objects = new List<Object>();
            List<Object> bullets = new List<Object>();
            int choice;

            do
            {
                string[] homescreen = { "Enter a choice!", "1 - New Game", "2 - Instructions", "3 - Config Settings", "4 - Exit Game" };

                for (int i = 0; i < homescreen.Length; i++)
                {
                    Console.SetCursorPosition((Console.WindowWidth - homescreen[i].Length) / 2, Console.CursorTop);
                    Console.WriteLine(homescreen[i]);
                }
                int.TryParse(Console.ReadLine(), out choice);

                switch (choice)
                {
                    //NEW GAME -- LOOP
                    case 1:
                        while (true)
                        {
                            speed += acceleration;
                            speedCapCheck();
                            hit = false;

                            { // Object creation based on chance
                                int chance = value.Next(0, 100);
                                chanceCheck(chance, livesObjectChance, pointsObjectChance, objects);
                            }


                            // key press check
                            while (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo pressedKey = getKeyStroke();
                                move(pressedKey, ref player.x);

                                if (pressedKey.Key == ConsoleKey.Escape)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Lives remaining: " + livesCount + "\nPoints accumulated: " + points + "\n");
                                    Console.WriteLine("Game Over. Press any key to exit.");
                                    Console.ReadKey();
                                    Environment.Exit(0);
                                }

                                createBullet(pressedKey, bullets, player.x, player.y);
                            }
                            // bullet movement and collision check
                            List<Object> newListBullet = new List<Object>();
                            for (int i = 0; i < bullets.Count; i++)
                            {
                                newBullet = new Object();
                                moveBullet(bullets, newBullet, i);
                                //bulletCollisionCheck(newObject.x, newBullet.x, newObject.y, newBullet.y);


                                if (newBullet.y < Console.WindowHeight)
                                {
                                    newListBullet.Add(newBullet);
                                }
                            }
                            //object movement and collision check
                            List<Object> newList = new List<Object>();
                            for (int i = 0; i < objects.Count; i++)
                            {
                                newObject = new Object();
                                moveDownConsole(objects, newObject, i);
                                collisionCheck(player.x, player.y);

                                if (newObject.y < Console.WindowHeight)
                                {
                                    newList.Add(newObject);
                                }
                            }
                            //refreshing y positions & clearing to draw again
                            objects = newList;
                            bullets = newListBullet;

                            // checks if bullet is going to leave the console window
                            if (newBullet.y == 0)
                            {
                                newListBullet.Remove(newBullet);
                            }
                            Console.Clear();

                            // checks if hit by '#'
                            bulletHitCheck(objects, bullets);
                            enemyHitCheck(objects, player.onscreen, player.x, player.y);

                            foreach (Object newBullet in bullets)
                            {
                                draw(newBullet.onscreen, ref newBullet.x, ref newBullet.y);
                            }

                            foreach (Object newObject in objects)
                            {
                                draw(newObject.onscreen, ref newObject.x, ref newObject.y);
                            }

                            // Prints setting data to console window
                            drawString(playfieldWidth + 2, 4, "Lives: " + livesCount);
                            drawString(playfieldWidth + 2, 5, "Points: " + points);
                            drawString(playfieldWidth + 2, 6, "Current Speed: " + speed);
                            drawString(playfieldWidth + 2, 7, "Acceleration: " + acceleration);
                            drawString(playfieldWidth + 2, 8, "Max Speed: " + maxspeed);
                            drawString(playfieldWidth + 2, 9, "Playfield Width: " + playfieldWidth);
                            drawString(playfieldWidth + 2, 10, "Ys : " + newBullet.y + " " + newObject.y + " " + " Xs : " + newBullet.x + " " + newObject.x);
                            // ---------------------------------------- //


                            // ticker
                            Thread.Sleep((int)(maxspeed - speed));
                        }

                    // GAME INSTRUCTIONS 
                    case 2:
                        Console.WriteLine("You are spawned in the game as player 'X' and are trying to avoid all of the enemies on screen, also known as '#'. " +
                            "The object of the game is to collect as many points ('*') as you can while surviving as long as you can. Use the UP ARROW KEY to shoot bullets. " +
                            "You may only shoot one bullet at a time and if the bullet collides with an enemy it clears the screen. " +
                            "The bullets are meant to be used as a tool to clear the screen if overwhelmed." +
                            "You may change different settings under the options tab. To move you use the left and right arrow keys and to quit at any time press the escape key. Enjoy!\n");

                        break;
                    // CONFIG SETTINGS
                    case 3:
                        int dchoice;
                        do
                        {
                            Console.WriteLine("Choose what settings you want to change\n");
                            Console.WriteLine("1 - Speed\n" +
                                "2 - Number of Lives\n" +
                                "3 - Acceleration\n" +
                                "4 - Lives object chance of spawning\n" +
                                "5 - Points object chance of spawning\n" +
                                "6 - Playfield width\n" +
                                "7 - Max speed\n" +
                                "8 - Back to main menu\n");


                            int.TryParse(Console.ReadLine(), out dchoice);

                            switch (dchoice)
                            {
                                case 1:
                                    Console.WriteLine("Enter the new value you want for the starting speed. Current value: " + startSpeed);
                                    speed = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Value changed to " + speed);
                                    continue;
                                case 2:
                                    Console.WriteLine("Enter the new value you want for the number of lives. Current value: " + livesCount);
                                    livesCount = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Value changed to " + livesCount);
                                    continue;
                                case 3:
                                    Console.WriteLine("Enter the new value you want for acceleration. Current value: " + acceleration);
                                    acceleration = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Value changed to " + acceleration);
                                    continue;
                                case 4:
                                    Console.WriteLine("Enter the new value you want for the chance value for the number of lives objects spawning. Current value: " + livesObjectChance + "%");
                                    livesObjectChance = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Value changed to " + livesObjectChance);
                                    continue;
                                case 5:
                                    Console.WriteLine("Enter the new value you want for chance value for the number of points objects spawning. Current value: " + pointsObjectChance);
                                    pointsObjectChance = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Value changed to " + pointsObjectChance);
                                    continue;
                                case 6:
                                    Console.WriteLine("Enter the new value you want for the playfield width. Current value: " + playfieldWidth);
                                    playfieldWidth = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Value changed to " + playfieldWidth);
                                    continue;
                                case 7:
                                    Console.WriteLine("Enter the new value you want for the max speed. Current value: " + maxspeed);
                                    maxspeed = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Value changed to " + maxspeed);
                                    continue;
                                case 8:
                                    break;
                            }
                            break;

                        } while (dchoice <= 4 || dchoice > 0);

                        break;


                    case 4:
                        Environment.Exit(0);
                        break;
                }

            } while (choice > 0 && choice <= 3);
        }

        // reading the key pressed by user, no issues.
        static ConsoleKeyInfo getKeyStroke()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            return key;
        }

        //changing the position based on user key (left, right)
        static void move(ConsoleKeyInfo dKey, ref int dx)
        {
            int xspeed = 1;

            if (dKey.Key == ConsoleKey.LeftArrow)
            {
                if (dx - xspeed >= 0)
                {
                    dx -= xspeed;
                }
            }
            else if (dKey.Key == ConsoleKey.RightArrow)
            {
                if (dx + 1 < playfieldWidth)
                {
                    dx += xspeed;
                }
            }
        }

        // checks the random chance value generated and makes new object based on that chance
        static void chanceCheck(int dchance, int dlivesObjectChance, int dpointsObjectChance, List<Object> objects)
        {
            if (dchance < dlivesObjectChance)
            {
                newObject = new Object
                {
                    onscreen = '+', // lives object
                    x = value.Next(0, playfieldWidth),
                    y = 0
                };
                objects.Add(newObject);
            }
            else if (dchance < dpointsObjectChance)
            {
                newObject = new Object
                {
                    onscreen = '*', //points object
                    x = value.Next(0, playfieldWidth),
                    y = 0
                };
                objects.Add(newObject);
            }
            else
            {
                newObject = new Object
                {
                    x = value.Next(0, playfieldWidth),
                    y = 0,
                    onscreen = '#' //enemy object
                };
                objects.Add(newObject);
            }
        }

        public static void moveDownConsole(List<Object> objects, Object newObject, int index)
        {
            Object oldObject = objects[index];
            newObject.x = oldObject.x;
            newObject.y = oldObject.y + 1;
            newObject.onscreen = oldObject.onscreen;
        }

        // Checks if there is a collision between the object and the player then changes settings accordingly
        static void collisionCheck(int playerX, int playerY)
        {
            if (newObject.onscreen == '#' && newObject.y == newBullet.y && newObject.x == newBullet.x)
            {
                bulletHit = true;
            }


            if (newObject.onscreen == '*' && newObject.y == playerY && newObject.x == playerX)
            {

                if (points >= 10)
                {
                    speed += 20; // making the * object no longer impact the speed of the player, therfore will increase regardless whether they accumulate points (challenge)
                }
                else
                {
                    speed -= 20;
                }

                points++;
            }
            if (newObject.onscreen == '+' && newObject.y == playerY && newObject.x == playerX)
            {
                livesCount++;
            }
            if (newObject.onscreen == '#' && newObject.y == playerY && newObject.x == playerX)
            {
                livesCount--;
                hit = true;
                speed += 0.20 * maxspeed; //increase by 20% of maxspeed
                speedCapCheck();

                if (gameover(livesCount))
                {
                    Console.Clear();
                    Console.WriteLine("Lives remaining: " + livesCount + "\nPoints accumulated: " + points + "\n");
                    Console.WriteLine("Game Over. Press any key to exit.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
        }

        static void bulletHitCheck(List<Object> objects, List<Object> bullets)
        {
            if (bulletHit)
            {
                objects.Clear();
                bullets.Remove(newBullet);
                bulletHit = false;
            }
        }

        static void enemyHitCheck(List<Object> objects, char playerOnscreen, int playerX, int playerY)
        {
            if (hit)
            {
                objects.Clear();
                speed = startSpeed; // if hit sets speed back to original
                draw(playerOnscreen, ref playerX, ref playerY);
            }
            else
            {
                draw(playerOnscreen, ref playerX, ref playerY);
            }
        }

        public static void createBullet(ConsoleKeyInfo key, List<Object> bullets, int playerX, int playerY)
        {
            if (key.Key == ConsoleKey.UpArrow)
            {
                Object bullet = new Object();
                bullet.x = playerX;
                bullet.y = playerY;
                bullet.onscreen = '^';
                bullets.Add(bullet);
            }
        }

        static void moveBullet(List<Object> bullets, Object bullet, int index)
        {
            Object oldBullet = bullets[index];
            bullet.x = oldBullet.x;
            bullet.y = oldBullet.y - 1;
            bullet.onscreen = oldBullet.onscreen;
        }

        static void speedCapCheck()
        {
            if (speed > maxspeed)
            {
                speed = maxspeed; // capping speed at maxspeed
            }
        }
        //draws character on screen referring to it's x and y
        static void draw(char dplayerchar, ref int dx, ref int dy)
        {
            Console.SetCursorPosition(dx, dy);
            Console.Write(dplayerchar);
        }

        //draws any string on screen given coords 
        public static void drawString(int x, int y, string toPrint)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(toPrint);
        }

        // game over function solely based on number of lives
        public static bool gameover(int dlives)
        {
            if (dlives <= 0)
            {
                return true;
            }
            return false;
        }


    }
}



