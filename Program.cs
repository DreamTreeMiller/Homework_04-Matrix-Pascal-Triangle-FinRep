using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_Theme_04
{
    public struct MenuItem
    {
        public ConsoleKey itemKey;     // Клавиша, которой соответствует пункт меню
        public string itemName;        // Текст пункта меню
    }
    class Program
    {

        static void Main(string[] args)
        {
            #region Задание 1.
            // Заказчик просит вас написать приложение по учёту финансов
            // и продемонстрировать его работу.
            // Суть задачи в следующем: 
            // Руководство фирмы по 12 месяцам ведет учет расходов и поступлений средств. 
            // За год получены два массива – расходов и поступлений.
            // Определить прибыли по месяцам
            // Количество месяцев с положительной прибылью.
            // Добавить возможность вывода трех худших показателей по месяцам, с худшей прибылью, 
            // если есть несколько месяцев, в которых худшая прибыль совпала - вывести их все.
            // Организовать дружелюбный интерфейс взаимодействия и вывода данных на экран

            // Пример
            //       
            // Месяц      Доход, тыс. руб.  Расход, тыс. руб.     Прибыль, тыс. руб.
            //     1              100 000             80 000                 20 000
            //     2              120 000             90 000                 30 000
            //     3               80 000             70 000                 10 000
            //     4               70 000             70 000                      0
            //     5              100 000             80 000                 20 000
            //     6              200 000            120 000                 80 000
            //     7              130 000            140 000                -10 000
            //     8              150 000             65 000                 85 000
            //     9              190 000             90 000                100 000
            //    10              110 000             70 000                 40 000
            //    11              150 000            120 000                 30 000
            //    12              100 000             80 000                 20 000
            // 
            // Худшая прибыль в месяцах: 7, 4, 1, 5, 12
            // Месяцев с положительной прибылью: 10
            #endregion

            #region * Задание 2
            // Заказчику требуется приложение строящее первых N строк треугольника паскаля. N < 25
            // 
            // При N = 9. Треугольник выглядит следующим образом:
            //                                 1
            //                             1       1
            //                         1       2       1
            //                     1       3       3       1
            //                 1       4       6       4       1
            //             1       5      10      10       5       1
            //         1       6      15      20      15       6       1
            //     1       7      21      35      35       21      7       1
            //                                                              
            //                                                              
            // Простое решение:                                                             
            // 1
            // 1       
            // 1       2       
            // 1       3       
            // 1       4       6     
            // 1       5      10     
            // 1       6      15      20      15       6       1
            // 1       7      21      35      35       21      7       1
            // 
            // Справка: https://ru.wikipedia.org/wiki/Треугольник_Паскаля
            #endregion

            #region * Задание 3.1
            // Заказчику требуется приложение позволяющщее умножать математическую матрицу на число
            // Справка https://ru.wikipedia.org/wiki/Матрица_(математика)
            // Справка https://ru.wikipedia.org/wiki/Матрица_(математика)#Умножение_матрицы_на_число
            // Добавить возможность ввода количество строк и столцов матрицы и число,
            // на которое будет производиться умножение.
            // Матрицы заполняются автоматически. 
            // Если по введённым пользователем данным действие произвести невозможно - сообщить об этом
            //
            // Пример
            //
            //      |  1  3  5  |   |  5  15  25  |
            //  5 х |  4  5  7  | = | 20  25  35  |
            //      |  5  3  1  |   | 25  15   5  |
            //
            //
            // ** Задание 3.2
            // Заказчику требуется приложение позволяющщее складывать и вычитать математические матрицы
            // Справка https://ru.wikipedia.org/wiki/Матрица_(математика)
            // Справка https://ru.wikipedia.org/wiki/Матрица_(математика)#Сложение_матриц
            // Добавить возможность ввода количество строк и столцов матрицы.
            // Матрицы заполняются автоматически
            // Если по введённым пользователем данным действие произвести невозможно - сообщить об этом
            //
            // Пример
            //  |  1  3  5  |   |  1  3  4  |   |  2   6   9  |
            //  |  4  5  7  | + |  2  5  6  | = |  6  10  13  |
            //  |  5  3  1  |   |  3  6  7  |   |  8   9   8  |
            //  
            //  
            //  |  1  3  5  |   |  1  3  4  |   |  0   0   1  |
            //  |  4  5  7  | - |  2  5  6  | = |  2   0   1  |
            //  |  5  3  1  |   |  3  6  7  |   |  2  -3  -6  |
            //
            // *** Задание 3.3
            // Заказчику требуется приложение позволяющщее перемножать математические матрицы
            // Справка https://ru.wikipedia.org/wiki/Матрица_(математика)
            // Справка https://ru.wikipedia.org/wiki/Матрица_(математика)#Умножение_матриц
            // Добавить возможность ввода количество строк и столцов матрицы.
            // Матрицы заполняются автоматически
            // Если по введённым пользователем данным действие произвести нельзя - сообщить об этом
            //  
            //  |  1  3  5  |   |  1  3  4  |   | 22  48  57  |
            //  |  4  5  7  | х |  2  5  6  | = | 35  79  95  |
            //  |  5  3  1  |   |  3  6  7  |   | 14  36  45  |
            //
            //  
            //                  | 4 |   
            //  |  1  2  3  | х | 5 | = | 32 |
            //                  | 6 |  
            #endregion
            
            ConsoleKey action;       // Переменная, в которую будет считываться нажатие клавиши
            #region example of key reading
            //do
            //{
            //    Console.WriteLine("Press a key, together with Alt, Ctrl, or Shift.");
            //    Console.WriteLine("Press Esc to exit.");
            //    action = Console.ReadKey(true);

            //    StringBuilder output = new StringBuilder(
            //                  String.Format("You pressed {0}", action.Key.ToString()));
            //    bool modifiers = false;

            //    if ((action.Modifiers & ConsoleModifiers.Alt) == ConsoleModifiers.Alt)
            //    {
            //        output.Append(", together with " + ConsoleModifiers.Alt.ToString());
            //        modifiers = true;
            //    }
            //    if ((action.Modifiers & ConsoleModifiers.Control) == ConsoleModifiers.Control)
            //    {
            //        if (modifiers)
            //        {
            //            output.Append(" and ");
            //        }
            //        else
            //        {
            //            output.Append(", together with ");
            //            modifiers = true;
            //        }
            //        output.Append(ConsoleModifiers.Control.ToString());
            //    }
            //    if ((action.Modifiers & ConsoleModifiers.Shift) == ConsoleModifiers.Shift)
            //    {
            //        if (modifiers)
            //        {
            //            output.Append(" and ");
            //        }
            //        else
            //        {
            //            output.Append(", together with ");
            //            modifiers = true;
            //        }
            //        output.Append(ConsoleModifiers.Shift.ToString());
            //    }
            //    output.Append(".");
            //    Console.WriteLine(output.ToString());
            //    Console.WriteLine();
            //} while (action.Key != ConsoleKey.Escape);
            #endregion example of key reading

            Console.CursorVisible = false;  // Делаем курсор невидимым
            var winHeight = 20;             // Высота экрана (для меню) 20 строк
            var winWidth = 80;              // Ширина экрана (для меню) 80 строк
            int currItem = 1;               // текущая позиция меню

            WindowOutput window;
            FinancialReport financialReport;
            PascalTriangle pascalTriangle;
            Matrix matrix;

            MenuItem[] menuItems =          // Пункты меню для вывода на экран
                       {new MenuItem {itemKey = ConsoleKey.D1,    itemName = "1.   ФИНАНСОВЫЙ УЧЁТ В КОМПАНИИ" },
                        new MenuItem {itemKey = ConsoleKey.D2,    itemName = "2.   ТРЕУГОЛЬНИК ПАСКАЛЯ" },
                        new MenuItem {itemKey = ConsoleKey.D3,    itemName = "3.   ОПЕРАЦИИ С МАТРИЦАМИ" },
                        new MenuItem {itemKey = ConsoleKey.Escape,itemName = "ESC  ВЫХОД" } };

            window = new WindowOutput(winWidth, winHeight);
            financialReport = new FinancialReport();
            pascalTriangle = new PascalTriangle();
            matrix = new Matrix();

            do                  // Считываем нажатия, пока не будет ESC
            {
                window.newWindow(winWidth, winHeight);
                window.HeaderCenter("Домашняя работа  №4", winWidth, 2, ConsoleColor.Yellow);
                window.HeaderCenter("Дмитрий Мельников", winWidth, 3, ConsoleColor.Yellow);
                
                action = window.MenuSelect(menuItems, currItem, winWidth, 5);
                
                switch (action)
                {
                    case ConsoleKey.D1:
                        financialReport.financialReportMenu();
                        currItem = 1;
                        break;

                    case ConsoleKey.D2:
                        pascalTriangle.printPascalTriangle();
                        currItem = 2;
                        break;

                    case ConsoleKey.D3:
                        matrix.matrixMenu();
                        currItem = 3;
                        break;

                    case ConsoleKey.Escape:
                        break;

                    default:
                        break;   // нажата любая другая клавиша - ничего не происходит
                }
               
            } while (action != ConsoleKey.Escape);
        }
    }
}
