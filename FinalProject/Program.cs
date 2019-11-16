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



            do
            {
                // get user key
                key = getKeyStroke();

                //move
                move(key, ref x, ref y);

                //draw
                draw(meChar, x, y);

            } while (!gameover(key));
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



