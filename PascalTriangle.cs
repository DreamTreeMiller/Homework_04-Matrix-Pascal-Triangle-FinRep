using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_Theme_04
{
    class PascalTriangle
    {
        CorrectInput correctInput;
        int winHeight = 40;             // Высота экрана (для меню) 40 строк
        int winWidth = 80;              // Ширина экрана (для меню) 80 строк
        WindowOutput windowPT;

        int[][] pT;             // Треугольник Паскаля
                                // Сделаем его в 2 раза меньше, т.к. он зеркальный
        int level;              // количество уровней ТП. Задаётся пользователем
        int line, pos;          // уровень и позиция в уровне в ТП

        int longest;            // длина в символах самого большого числа ТП

        string[] pTstring;      // строки для красивого вывода ТП

        public PascalTriangle()
        {
            correctInput = new CorrectInput();
            windowPT = new WindowOutput(winWidth, winHeight);
            // надо сделать буфер экрана болье размера окна консоли, т.к. может не влезть
            Console.SetBufferSize(300, 100);

        }

        public void printPascalTriangle()
        {
            int i, j;                  // вспомогательная переменная
            bool cursorVisibility;

            windowPT.newWindow(winWidth, winHeight);
            windowPT.HeaderCenter("ТРЕУГОЛЬНИК ПАСКАЛЯ", winWidth, 2, ConsoleColor.Yellow);
            Console.WriteLine(); 
            cursorVisibility = Console.CursorVisible;
            Console.CursorVisible = true;

            level = correctInput.Parse("Введите количество уровней треугольника Паскаля", "Введите число", 1, 25, 0, 4);
            Console.WriteLine();
            Console.CursorVisible = cursorVisibility;

            // Создаём зубчатый массив для ТП
            // Создаём массив уровней. Каждый элемент - это массив
            // для экономии памяти сделаем
            // количество элементов в каждом уровне ТП
            // в два раза меньше, чем номер уровня (если считать с 1)

            pT       = new int[level][];
            pTstring = new string[level];

            #region заполнение треугольника Паскаля

            pT[0] = new int[1];
            pT[0][0] = 1;               // инициализируем элемент на 1-м уровне

            if (level >= 2)
            {
                pT[1] = new int[1];
                pT[1][0] = 1;           // инициализируем элемент на 2-м уровне
            }

            j = 0;                      // потребуется дальше, если не войдём по условию ниже.

            if (level > 2)
            {
                for (i = 2; i < level; i++)
                {
                    // Создаём уровень
                    // количество элементов в каждом уровне ТП
                    // в два раза меньше, чем номер уровня (если считать с 1)

                    // pos равен количеству элементов в массиве для очередного уровня
                    // причём pos одинаковое для нечётного (если считать с 1) 
                    // и для следующего чётного уровня.
                    // Выделяем соответствующий объём памяти
                    pos = ((i + 1) / 2 + (i + 1) % 2);
                    pT[i] = new int[pos];
                    pT[i][0] = 1;                   // инициализируем первый элемент уровня

                    // Теперь надо инициализировать все остальные элементы, начиная со 2-ого по счёту
                    // если уровень нечётный, то pos указывает на средний элемент (если считать с 1)
                    // если чётный, то на первый самый большой элемент.
                    // Поэтому мы не можем делать цикл ниже до pos, надо до предпоследнего элемента
                    // 

                    for (j = 1; j < (pos - 1); j++)
                        pT[i][j] = pT[i - 1][j - 1] + pT[i - 1][j];
                    // окончили цикл j == pos - 1
                    // это либо средний элемент (если в уровне нечётное кол-во элементов)
                    // либо последний элемен, а следующий такой же (если в уровне чётное кол-во элементов)
                    // но для 3 и 4 уровней цикл выше не работает, когда всего два элемента в массиве,  
                    // т.к. pos равен 2, pos - 1 равен 1, а j уже 1. 

                    if ((i + 1) % 2 == 1)    // если нечётный уровень, когда считаем уровни с 1,
                    {                        // то элемента  pT[i - 1][pos] не существует
                        pT[i][j] = pT[i - 1][j - 1] * 2;
                    }
                    else // если чётная, когда считаем уровни с 1, то существует, 
                    {    // и мы как бы делаем последнюю итерацию цикла выше, но уже с j равным pos - 1 
                        pT[i][j] = pT[i - 1][j - 1] + pT[i - 1][j];
                    }

                }   // создание и заполнение массива for (i = 0; i < level; i++)
            }
            #endregion заполнение треугольника Паскаля

            #region вывод массива на экран

            // Чтобы вывести ТП, найдём длину самого длинного числа в ТП.
            // Это самое большое число, оно же самое последнее.
            longest = $"{pT[level - 1][j]}".Length;

            // если длина самого большого числа чётная, тогда добавляем 1
            longest = ((longest % 2) == 0) ? ++longest : longest;
            
            int spaceLeft;          // количество пробелов слева, чтобы число выводилось по центру поля
            string currString;      // строковое представление текущего числа

            int half = longest / 2;                 // половина longest
            int s = 1;                              // расстояние между полями для вывода чисел

            int first;                              // координата первого числа уровня
            int next;                               // координаты начала поля следующего числа
            int offset;                             // сдвиг, чтобы ТП показывался по центру экрана      
            int Y = 7;                              // координата первой строки

            // Сделаем количество символов в самом большом числе длиной поля 
            // для вывод всех остальных чисел, кроме кайних 1.
            // Если longest чётное, то длина поля на 1 больше. 
            // Будем помещать каждое число в центр такого поля. 
            // Например, поле длиной 5 символов. Число 3 символа    ".YYY."
            // Поле длиной 5 символов, число 2 символа ".YY..", если стоит слева от центральной вертикальной линии
            // или центральное число. И  "..YY." если справа от центальной линии.
            // Ширина поля равна (half + 1 + half)

            // Расстояние между полями S сделаем 1 символ. Допустим, длина half это 3 символа.
            // Обозначим half как ... . Тогда формат вывода ТП будет таким
            // Не крайние единицы '1' - это центры полей для вывода соответствующих чисел ТП
            // 0                 1
            // 1             1...S...1
            // 2         1...S...1...S...1
            // 3     1...S...1...S...1...S...1
            // 4 1...S...1...S...1...S...1...S...1

            // тогда координата First на экране первого символа в N-ой строке (N считаемт от 0)
            // равна (1+half)*(level - N - 1)
            // координата единицы в 0-й строке равна (1+half)*(level - 1)
            // координата единицы в последней строке (1+half)* 0 = 0
            // Найдём длину половинок слева и справа от центр

            // тогда координаты начала поля вывода любого следующих j-ого числа
            // Frist + (1 + half +S) + ((half + 1 + half) + S)* (j-1) 
            // Формула учитывает, что координаты Х первого символа равны 0 (ноль)
            // Что индекс j начинается с 0, т.е. pT[...][0] = 1 

            offset = (((half + 1 + half + s)*(level - 1) + 1) < winWidth) 
                     ? (winWidth - ((half + 1 + half + s) * (level - 1) + 1)) / 2 
                     : 5;
            
            if (level == 1)
            {
                Console.SetCursorPosition(offset, Y);
                Console.Write(pT[0][0]);
            }

            if (level >= 2)
            {
                i = 0;  j = 0;
                first = (1 + half) * (level - i - 1);
                Console.SetCursorPosition(offset + first, Y + i);
                Console.Write(pT[i][j]);


                i = 1; j = 0;
                first = (1 + half) * (level - i - 1);
                Console.SetCursorPosition(offset + first, Y + i);
                Console.Write(pT[i][j]);
                       j = 1;
                next = offset + first + (1 + half + s) + ((half + 1 + half) + s) * (j - 1);

                currString = $"{pT[i][j-1]}";
                spaceLeft = (longest - currString.Length) / 2;
                Console.SetCursorPosition(next + spaceLeft, Y + i);
                Console.Write(pT[i][j-1]);
            }

            if (level > 2)
            {
                for (i = 2; i < level; i++)
                {
                    j = 0;
                    first = (1 + half) * (level - i - 1);
                    Console.SetCursorPosition(offset + first, Y + i);
                    Console.Write(pT[i][j]);
 
                    pos = ((i + 1) / 2 + (i + 1) % 2);

                    for (j = 1; j < (pos - 1); j++)
                    {
                        next = offset + first + (1 + half + s) + ((half + 1 + half) + s) * (j - 1);

                        currString = $"{pT[i][j]}";
                        spaceLeft = (longest - currString.Length) / 2;
                        
                        Console.SetCursorPosition(next + spaceLeft, Y + i);
                        Console.Write(pT[i][j]);
                    }
    
                    // окончили цикл j == pos - 1
                    // это либо средний элемент (если в уровне нечётное кол-во элементов)
                    // либо последний элемен, а следующий такой же (если в уровне чётное кол-во элементов)
                    // но для 3 и 4 уровней цикл выше не работает, когда всего два элемента в массиве,  
                    // т.к. pos равен 2, pos - 1 равен 1, а j уже 1. 

                    // выводим центральный элемент (если уровень нечётный)
                    // или 2 однаковых центральных элемента (если уровнь чётный)

                    if ((i + 1) % 2 == 1)    // если нечётный уровень, когда считаем уровни с 1,
                    {                        // то элемента  pT[i - 1][pos] не существует
                        next = offset + first + (1 + half + s) + ((half + 1 + half) + s) * (j - 1);
                        currString = $"{pT[i][j]}";
                        spaceLeft = (longest - currString.Length) / 2;

                        Console.SetCursorPosition(next + spaceLeft, Y + i);
                        Console.Write(pT[i][j]);
                    }
                    else // если чётная, когда считаем уровни с 1, то существует, 
                    {    // и мы как бы делаем последнюю итерацию цикла выше, но уже с j равным pos - 1 
                        next = offset + first + (1 + half + s) + ((half + 1 + half) + s) * (j - 1);
                        currString = $"{pT[i][j]}";
                        spaceLeft = (longest - currString.Length) / 2;

                        Console.SetCursorPosition(next + spaceLeft, Y + i);
                        Console.Write(pT[i][j]);
                        
                        j++;
                        
                        next = offset + first + (1 + half + s) + ((half + 1 + half) + s) * (j - 1);
                        currString = $"{pT[i][j-1]}";
                        spaceLeft = (longest - currString.Length) / 2;
                        spaceLeft += (currString.Length % 2 == 0) ? 1 : 0;

                        Console.SetCursorPosition(next + spaceLeft, Y + i);
                        Console.Write(pT[i][j - 1]);
                    }
                    j++;

                    for (int k = (pos - 1 -1); k >= 0; k--)
                    {
                        next = offset + first + (1 + half + s) + ((half + 1 + half) + s) * (j - 1);
                        currString = $"{pT[i][k]}";
                        spaceLeft = (longest - currString.Length) / 2;
                        spaceLeft += (currString.Length % 2 == 0) ? 1: 0;

                        Console.SetCursorPosition(next + spaceLeft, Y + i);
                        Console.Write(pT[i][k]);
                        j++;
                    }   // выводим правое крыло


                }   // for i
            }
            Console.WriteLine();

            #endregion вывод массива на экран

            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу");
            Console.ReadKey();

        }

    }
}
