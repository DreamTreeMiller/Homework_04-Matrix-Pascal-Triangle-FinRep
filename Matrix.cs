using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_Theme_04
{

    class Matrix
    {

        #region объявления переменных

        int winHeight = 20;             // Высота экрана (для меню) 40 строк
        int winWidth = 80;              // Ширина экрана (для меню) 80 строк

        static MenuItem[] menuItems =          // Пункты меню для вывода на экран
                   {new MenuItem {itemKey = ConsoleKey.D1,    itemName = "1.   УМНОЖЕНИЕ МАТРИЦЫ НА ЧИСЛО" },
                    new MenuItem {itemKey = ConsoleKey.D2,    itemName = "2.   СЛОЖЕНИЕ МАТРИЦ" },
                    new MenuItem {itemKey = ConsoleKey.D3,    itemName = "3.   ВЫЧИТАНИЕ МАТРИЦ" },
                    new MenuItem {itemKey = ConsoleKey.D4,    itemName = "4.   УМНОЖЕНИЕ МАТРИЦ" },
                    new MenuItem {itemKey = ConsoleKey.Escape,itemName = "ESC  ВЫХОД" } };

        // Окно, в котором будем выводить меню матрицы
        CorrectInput correctInput;
        WindowOutput windowM;

        #endregion объявления переменных

        public Matrix()
        {
            correctInput = new CorrectInput();
            windowM = new WindowOutput(winWidth, winHeight);
        }

        public void matrixMenu()
        {
            ConsoleKey action;       // Переменная, в которую будет считываться нажатие клавиши
            int currItem = 1;        // Текущий пункт меню


            do                  // Считываем нажатия, пока не будет ESC
            {
                windowM.newWindow(winWidth, winHeight);
                Console.CursorVisible = false;  // Делаем курсор невидимым
                windowM.HeaderCenter("ОПЕРАЦИИ С МАТРИЦАМИ", winWidth, 2, ConsoleColor.Yellow);
                action = windowM.MenuSelect(menuItems, currItem, winWidth, 4);

                switch (action)
                {
                    case ConsoleKey.D1:
                        multiplyMatrixByNumber();  
                        currItem = 1;
                        break;

                    case ConsoleKey.D2:
                        addMatrixes();
                        currItem = 2;
                        break;

                    case ConsoleKey.D3:
                        subtractMatrixes();
                        currItem = 3;
                        break;

                    case ConsoleKey.D4:
                        multiplyMatrixes();
                        currItem = 4;
                        break;

                    case ConsoleKey.Escape:
                        Console.WriteLine("ДО СВИДАНИЯ!");
                        break;

                    default:
                        break;   // нажата любая другая клавиша - ничего не происходит
                }

            } while (action != ConsoleKey.Escape);

        }   // public void matrixMenu()

        /// <summary>
        /// Печатает матрицу в заданной области буфера экрана с заданной шириной ячейки. 
        /// Расстояние между ячейками - пробел. Выравнивание в ячейке по левому краю
        /// </summary>
        /// <param name="matrixToPrint">Матрица для вывода.</param>
        /// <param name="cellSize">Количество символов для вывода каждой ячейки матрицы</param>
        /// <param name="topLeftX">Столбец, с которого выводить матрицу</param>
        /// <param name="topLeftY">Строка, с которой выводить матрицу</param>
        public void printMatrix(int[,] matrixToPrint, int cellSize, int topLeftX, int topLeftY)
        {
            int i, j;

            for (i = 0; i < matrixToPrint.GetLength(0); i++)
            {
                Console.SetCursorPosition(topLeftX, topLeftY + i);
                Console.Write("| ");
                for (j = 0; j < matrixToPrint.GetLength(1); j++)
                {
                    Console.SetCursorPosition(topLeftX + 2 + j * (1 + cellSize), topLeftY + i);
                    Console.Write($"{matrixToPrint[i, j]}".PadLeft(cellSize));
                }
                Console.SetCursorPosition(topLeftX + 2 + j * (1 + cellSize), topLeftY + i);
                Console.Write(" |");

            }
        }
        public void multiplyMatrixByNumber()
        {
            int winHeight = 30;        // Высота экрана (для меню) 40 строк
            int winWidth = 120;         // Ширина экрана (для меню) 80 строк
            bool cursorVisibility;


            int num;             // вводимое число 
            int m, n;            // размерности матрицы : m - число строк (1 х 10), n - число столбцов (1 х 10)
            int i, j;            // вспомогательная переменная
            Random r = new Random();

            windowM.newWindow(winWidth, winHeight);
            Console.SetBufferSize(200, 40);
            windowM.HeaderCenter("УМНОЖЕНИЕ МАТРИЦЫ НА ЧИСЛО", winWidth, 2, ConsoleColor.Yellow);
            Console.WriteLine();
            cursorVisibility = Console.CursorVisible;
            Console.CursorVisible = true;

            m   = correctInput.Parse("Введите количество строк матрицы",    "Введите число", 1, 10, 0, 4);
            n   = correctInput.Parse("Введите количество столбцов матрицы", "Введите число", 1, 10, 0, 5);
            num = correctInput.Parse("Введите число, на которое хотите умножить матрицу", 
                                     "Введите число", -100, 100, 0, 6);

            Console.CursorVisible = cursorVisibility;

            int[,] matrix = new int[m, n];  // создаём матрицу m, n

            // Инициализируем матрицу случайными числами
            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                    matrix[i, j] = r.Next(-10, 11);

            // Выводим изначальную матрицу
            // 3 - ширина поля для вывода каждого элемента матрицы
            // 5 и 8  - координаты Х и У левого верхнего угла матрицы
            printMatrix(matrix, 3, 10, 8);

            // Умножаем матрицу на число. Результат храним в той же матрице
            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                    matrix[i, j] *= num;

            Console.SetCursorPosition(2, 8 + m / 2);
            Console.Write($"{num} x".PadLeft(6));

            // Выводим * num =  между матрицами
            Console.SetCursorPosition(10 + 1 + n * 4 + 2 + 5, 8 + m / 2);
            Console.Write("=");
            
            // Выводим получившуюся матрицу
            // 5 - ширина поля для вывода каждого элемента матрицы
            // 5+1+n*4+2+20 и 8  - координаты Х и У левого верхнего угла матрицы
            printMatrix(matrix, 5, 10+1+n*4+2+9, 8);

            windowM.HeaderCenter("НАЖМИТЕ ЛЮБУЮ КЛАВИШУ", winWidth, Console.CursorTop + 2, ConsoleColor.DarkYellow);
            Console.ReadKey();
        }

        public void addMatrixes()
        {
            int winHeight = 30;        // Высота экрана (для меню) 40 строк
            int winWidth = 120;         // Ширина экрана (для меню) 80 строк
            bool cursorVisibility;


            int m, n;            // размерности матрицы : m - число строк (1 х 10), n - число столбцов (1 х 10)
            int i, j;            // вспомогательная переменная
            Random r = new Random();

            windowM.newWindow(winWidth, winHeight);
            Console.SetBufferSize(200, 40);
            windowM.HeaderCenter("СЛОЖЕНИЕ МАТРИЦ", winWidth, 2, ConsoleColor.Yellow);
            cursorVisibility = Console.CursorVisible;
            Console.CursorVisible = true;

            m = correctInput.Parse("Введите количество строк матриц", "Введите число", 1, 10, 0, 4);
            n = correctInput.Parse("Введите количество столбцов матриц", "Введите число", 1, 10, 0, 5);

            Console.CursorVisible = cursorVisibility;

            int[,] matrixA = new int[m, n];  // создаём матрицу A m, n - уменьшаемое
            int[,] matrixB = new int[m, n];  // создаём матрицу B m, n - вычитаемое
            int[,] matrixC = new int[m, n];  // создаём матрицу C m, n - разность


            // Инициализируем матрицу случайными числами
            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                {
                    matrixA[i, j] = r.Next(-10, 11);
                    matrixB[i, j] = r.Next(-10, 11);
                    matrixC[i, j] = matrixA[i, j] + matrixB[i, j];
                }


            // Выводим матрицу A
            // 3 - ширина поля для вывода каждого элемента матрицы
            // 5 и 8  - координаты Х и У левого верхнего угла матрицы
            printMatrix(matrixA, 3, 5, 8);

            // Выводим знак плюс "+"  между матрицами
            Console.SetCursorPosition(5 + 1 + n * 4 + 2 + 4, 8 + m / 2);
            Console.Write("+");

            // Выводим матрицу B
            // 3 - ширина поля для вывода каждого элемента матрицы
            // 5+1+n*4+2+9 и 8  - координаты Х и У левого верхнего угла матрицы
            printMatrix(matrixB, 3, 5 + 1 + n * 4 + 2 + 9, 8);

            // Выводим знак равно "="  между матрицами
            Console.SetCursorPosition(5 + (1 + n * 4 + 2 + 9) * 2 - 5, 8 + m / 2);
            Console.Write($"=");

            // Выводим получившуюся матрицу
            // 5 - ширина поля для вывода каждого элемента матрицы
            // 5 + (1 + n * 4 + 2 + 9)*2 и 8  - координаты Х и У левого верхнего угла матрицы
            printMatrix(matrixC, 3, 5 + (1 + n * 4 + 2 + 9)*2, 8);

            windowM.HeaderCenter("НАЖМИТЕ ЛЮБУЮ КЛАВИШУ", winWidth, Console.CursorTop + 2, ConsoleColor.DarkYellow);
            Console.ReadKey();

        }

        public void subtractMatrixes()
        {
            int winHeight = 30;        // Высота экрана (для меню) 40 строк
            int winWidth = 120;         // Ширина экрана (для меню) 80 строк
            bool cursorVisibility;

            int m, n;            // размерности матрицы : m - число строк (1 х 10), n - число столбцов (1 х 10)
            int i, j;            // вспомогательная переменная
            Random r = new Random();

            windowM.newWindow(winWidth, winHeight);
            Console.SetBufferSize(200, 40);
            windowM.HeaderCenter("ВЫЧИТАНИЕ МАТРИЦ", winWidth, 2, ConsoleColor.Yellow);
            cursorVisibility = Console.CursorVisible;
            Console.CursorVisible = true;

            m = correctInput.Parse("Введите количество строк матриц",    "Введите число", 1, 10, 0, 4);
            n = correctInput.Parse("Введите количество столбцов матриц", "Введите число", 1, 10, 0, 5);

            Console.CursorVisible = cursorVisibility;

            int[,] matrixA = new int[m, n];  // создаём матрицу A m, n - уменьшаемое
            int[,] matrixB = new int[m, n];  // создаём матрицу B m, n - вычитаемое
            int[,] matrixC = new int[m, n];  // создаём матрицу C m, n - разность


            // Инициализируем матрицу случайными числами
            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                {
                    matrixA[i, j] = r.Next(-10, 11);
                    matrixB[i, j] = r.Next(-10, 11);
                    matrixC[i, j] = matrixA[i, j] - matrixB[i, j];
                }


            // Выводим матрицу A
            // 3 - ширина поля для вывода каждого элемента матрицы
            // 5 и 8  - координаты Х и У левого верхнего угла матрицы
            printMatrix(matrixA, 3, 5, 8);

            // Выводим знак минус "-"  между матрицами
            Console.SetCursorPosition(5 + 1 + n * 4 + 2 + 4, 8 + m / 2);
            Console.Write("-");

            // Выводим матрицу B
            // 5 - ширина поля для вывода каждого элемента матрицы
            // 5+1+n*4+2+9 и 8  - координаты Х и У левого верхнего угла матрицы
            printMatrix(matrixB, 3, 5 + 1 + n * 4 + 2 + 9, 8);

            // Выводим знак равно "-"  между матрицами
            Console.SetCursorPosition(5 + (1 + n * 4 + 2 + 9) * 2 - 5, 8 + m / 2);
            Console.Write($"=");

            // Выводим получившуюся матрицу
            // 5 - ширина поля для вывода каждого элемента матрицы
            // 5 + (1 + n * 4 + 2 + 9) * 2 и 8  - координаты Х и У левого верхнего угла матрицы
            printMatrix(matrixC, 3, 5 + (1 + n * 4 + 2 + 9) * 2, 8);

            windowM.HeaderCenter("НАЖМИТЕ ЛЮБУЮ КЛАВИШУ", winWidth, Console.CursorTop + 2, ConsoleColor.DarkYellow);
            Console.ReadKey();
        }

        public void multiplyMatrixes()
        {
            int winHeight = 30;        // Высота экрана (для меню) 40 строк
            int winWidth = 120;         // Ширина экрана (для меню) 80 строк
            bool cursorVisibility;

            int m, n, k;         // размерности матрицы A: m - число строк (1 х 10), n - число столбцов (1 х 10)
                                 // размерности матрицы B: n - число строк (1 х 10), k - число столбцов (1 х 10)
            int i, j;            // вспомогательная переменная
            Random r = new Random();

            windowM.newWindow(winWidth, winHeight);
            Console.SetBufferSize(200, 40);
            windowM.HeaderCenter("УМНОЖЕНИЕ МАТРИЦ", winWidth, 2, ConsoleColor.Yellow);
            Console.WriteLine();
            cursorVisibility = Console.CursorVisible;
            Console.CursorVisible = true;

            m = correctInput.Parse("Введите количество строк матрицы А",    "Введите число", 1, 10, 0, 4);
            n = correctInput.Parse("Введите количество столбцов матрицы А", "Введите число", 1, 10, 0, 5);
            Console.SetCursorPosition(0, 7);
            Console.Write($"В матрице B {n} строк");
            k = correctInput.Parse("Введите количество столбцов матрицы B",  "Введите число", 1, 10, 0, 8);

            Console.CursorVisible = cursorVisibility;

            int[,] matrixA = new int[m, n];  // создаём матрицу A m, n - первый множитель
            int[,] matrixB = new int[n, k];  // создаём матрицу B m, n - второй множитель
            int[,] matrixC = new int[m, k];  // создаём матрицу C m, n - произведение


            // Инициализируем матрицу A случайными числами
            for (i = 0; i < m; i++)
                for (j = 0; j < n; j++)
                {
                    matrixA[i, j] = r.Next(-10, 11);
                }

            // Инициализируем матрицу B случайными числами
            for (i = 0; i < n; i++)
                for (j = 0; j < k; j++)
                {
                    matrixB[i, j] = r.Next(-10, 11);
                }

            // Перемножаем матрицы
            for (i = 0; i < m; i++)
                for (j = 0; j < k; j++)
                    for (int e = 0; e < n; e++)
                        matrixC[i, j] += matrixA[i, e] * matrixB[e, j];

            int offsetA = (m >= n) ? 0 : (n - m) / 2;
            int offsetB = (m >= n) ? (m - n) / 2 : 0;

            // Выводим матрицу A
            // 3 - ширина поля для вывода каждого элемента матрицы
            // 5 и 8  - координаты Х и У левого верхнего угла матрицы
            printMatrix(matrixA, 3, 5, 10 + offsetA);

            // Выводим знак умножить "X"  между матрицами
            Console.SetCursorPosition(5 + 1 + n * 4 + 2 + 3, 
                                      10 + ((offsetA == 0)? offsetB + n/2 : offsetA + m/2));
            Console.Write("x");
             
            // Выводим матрицу B
            // 5 - ширина поля для вывода каждого элемента матрицы
            //  5 + 1 + n * 4 + 2 + 5 и 8  - координаты Х и У левого верхнего угла матрицы
            printMatrix(matrixB, 3, 5 + 1 + n * 4 + 2 + 6, 10 + offsetB);
             
            // Выводим знак равно "="  между матрицами
            Console.SetCursorPosition(5 + (1 + n * 4 + 2 + 6) + (1 + k*4 + 2) + 3, 
                                      10 + ((offsetA == 0) ? offsetB + n / 2 : offsetA + m / 2));
            Console.Write($"=");

            // Выводим получившуюся матрицу
            // 5 - ширина поля для вывода каждого элемента матрицы
            // 5+1+n*4+2+20 и 8  - координаты Х и У левого верхнего угла матрицы
            printMatrix(matrixC, 4, 5 + (1 + n * 4 + 2 + 6) + (1 + k * 4 + 2) + 6, 10 + offsetA);

            windowM.HeaderCenter("НАЖМИТЕ ЛЮБУЮ КЛАВИШУ", 
                                 winWidth, 
                                 10 + ((m > n) ? m : n) + 2, 
                                 ConsoleColor.DarkYellow);
            Console.ReadKey();
        }


    }   // class Matrix
}   // namespace Homework_Theme_04
