using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class Program
    {
        static void Main(string[] args)
        {
            char meChar = 'X';
            int x = 20;
            int y = 20;
            ConsoleKey key;

            int choice;

            do
            {
                Console.WriteLine("Enter a choice!\n" +
                    "1 - New Game\n" +
                    "2 - Options\n" +
                    "3 - Exit\n");
                int.TryParse(Console.ReadLine(), out choice);

                switch (choice)
                {
                    case 1:
                        do
                        {
                            key = getKeyStroke(); // get key pressed by user
                            move(key, ref x, ref y); // move based on key pressed by user
                            draw(meChar, x, y); // draw player character 'X'

                        } while (!gameover(key));
                        break;

                    case 2:
                        break;

                    case 3:
                        break;
                }

            } while (choice > 0 && choice <= 3);


        }

        static ConsoleKey getKeyStroke() // reading the key pressed by user, no issues.
        {
            ConsoleKey key = Console.ReadKey(true).Key;
            return key;

        }

        static void move(ConsoleKey dKey, ref int dx, ref int dy) //changing the position based on user key (up,down,left,right)
        {
            if (dKey == ConsoleKey.LeftArrow)
            {
                dx--;
                Console.SetCursorPosition(dx, dy);
            }
            else if (dKey == ConsoleKey.RightArrow)
            {
                dx++;
                Console.SetCursorPosition(dx, dy);
            }
            else if (dKey == ConsoleKey.UpArrow)
            {
                dy--;
                Console.SetCursorPosition(dx, dy);
            }
            else if (dKey == ConsoleKey.DownArrow)
            {
                dy++;
                Console.SetCursorPosition(dx, dy);
            }
        }

        //draw func
        static void draw(char dmeChar, int dx = 0, int dy = 0)
        {
            if (dx >= 0 && dy >= 0)
            {
                Console.Clear();
                Console.SetCursorPosition(dx, dy);
                Console.Write(dmeChar);
            }
        }

        static bool gameover(ConsoleKey dKey)
        {
            if (dKey == ConsoleKey.Escape)
            {
                Console.WriteLine("Game Over");
                Console.ReadKey();
                return true;
            }
            return false;
        }


    }
}



