using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Activation;
using System.Text;
using System.Threading.Tasks;

namespace Homework_Theme_04
{
    class FinancialReport
    {
        /*
         * сгенерировать таблицу отчётности
         * создать массив 12 элементов для худших месяцев
         * найти лучшие три и худшие три показателя, 
         * пометить в массиве эти показатели
         * вывести на экран разные варианты 
         * предложить сгенерировать новую таблицу
         * или конец
         */

        #region объявления переменных

        int winHeight = 40;             // Высота экрана (для меню) 40 строк
        int winWidth = 90;              // Ширина экрана (для меню) 80 строк

        static MenuItem[] menuItems =          // Пункты меню для вывода на экран
                   {new MenuItem {itemKey = ConsoleKey.D1,    itemName = "1.   ПОКАЗАТЬ ДАННЫЕ / СНЯТЬ ВЫДЕЛЕНИЕ" },
                    new MenuItem {itemKey = ConsoleKey.D2,    itemName = "2.   МЕСЯЦЫ С ПОЛОЖИТЕЛЬНОЙ ПРИБЫЛЬЮ" },
                    new MenuItem {itemKey = ConsoleKey.D3,    itemName = "3.   ЛУЧШИЙ МЕСЯЦ(Ы) ПО ПРИБЫЛИ" },
                    new MenuItem {itemKey = ConsoleKey.D4,    itemName = "4.   МЕСЯЦЫ ТРЁХ ЛУЧШИХ ПОКАЗАТЕЛЕЙ ПО ПРИБЫЛИ" },
                    new MenuItem {itemKey = ConsoleKey.D5,    itemName = "5.   ХУДШИЙ МЕСЯЦ(Ы) ПО ПРИБЫЛИ" },
                    new MenuItem {itemKey = ConsoleKey.D6,    itemName = "6.   МЕСЯЦЫ ТРЁХ ХУДШИХ ПОКАЗАТЕЛЕЙ ПО ПРИБЫЛИ" },
                    new MenuItem {itemKey = ConsoleKey.D7,    itemName = "7.   СГЕНЕРИРОВАТЬ И ПОКАЗАТЬ НОВЫЕ ДАННЫЕ" },
                    new MenuItem {itemKey = ConsoleKey.Escape,itemName = "ESC  ВЫХОД" } };

        int bestMonth, secondBest, thirdBest;    // Три наилучших показателя по прибыли: B, 2B, 3B
        int thirdWorst, secondWorst, worstMonth; // Три наихудших показателя по прибыли: 3W, 2W, W
                                                 // worstMonth - W - худший 
                                                 // secondWorst - 2W - второй с конца
                                                 // thirdWorst - 3W третий с конца

        string bestMonthS, secondBestS, thirdBestS;    // Номера месяцев, удовлетворяющих B, 2B, 3B
        string thirdWorstS, secondWorstS, worstMonthS; // Номера месяцев, удовлетворяющих 3W, 2W, W
                                                       // в формате строки для вывода 

        // устанавливаем номера строк, в которых будут печататься номера соответствующих месяцев
        // строка заголовка таблицы "доходы расходы прибыль" на позиции 0 
        const int positiveProfitOffset   = 15;     // номер строки МЕСЯЦЫ С ПОЛОЖИТЕЛЬНОЙ ПРИБЫЛЬЮ
        const int bestMonthsOffset       = 16;     // номер строки ЛУЧШИЙ МЕСЯЦ(Ы) ПО ПРИБЫЛИ
        const int threeBestMonthsOffset  = 17;     // номер строки МЕСЯЦЫ ТРЁХ ЛУЧШИХ ПОКАЗАТЕЛЕЙ ПО ПРИБЫЛИ
        const int worstMonthsOffset      = 18;     // номер строки ХУДШИЙ МЕСЯЦ(Ы) ПО ПРИБЫЛИ
        const int threeWorstMonthsOffset = 19;     // номер строки МЕСЯЦЫ ТРЁХ ХУДШИХ ПОКАЗАТЕЛЕЙ ПО ПРИБЫЛИ

        int[,] finReport = new int[13, 4];       // массив финансовой отчётности
                                                 // [i, 0] - код i-ого элемента
                                                 // [i, 1] - доход в (i+1)-й месяц
                                                 // [i, 0] - расход в (i+1)-й месяц
                                                 // [i, 0] - прибыль в (i+1)-й месяц
                                                 // т.к. при разных исходных данных в таблице может оказаться больше одного значения
                                                 // и лучшего месяца, и худшего, и на втором, третьем местах может быть несколько месяцев,
                                                 // то лучше сделать один прогон в начале, выявить все эти месяцы, пометить их
                                                 // а при выводе на экран надо будет только считывать эти пометки и выводить соответствующим образом
                                                
        // [i, 0] Код i-ого элемента. Возьмём первые 6 бит 
        const int bestMonthMask   = 0b_0000_0001;      // бит 0 - лучший месяц по прибыли
        const int secondBestMask  = 0b_0000_0010;      // бит 1 - второй лучший месяц
        const int thirdBestMask   = 0b_0000_0100;      // бит 2 - третий лучший месяц
        const int thirdWorstMask  = 0b_0000_1000;      // бит 3 - третий худший месяц (третий с конца)
        const int secondWorstMask = 0b_0001_0000;      // бит 4 - второй худший месяц (второй с конца)
        const int worstMonthMask  = 0b_0010_0000;      // бит 5 - худший месяц (первый с конца)

        /* 
         * крайний, практически невероятный вариант - прибыль во всех месяцах одинаковая.
         * тогда все месяцы будут одновременно и лучшими и худшими, вторых и третьих не будет в обеих категориях
         * второй, тоже очень маловероятный вариант, за 12 месяцев всего два или три разных значения прибыли
         * тогда они будут и лучшими, и худшими одновременно, только в разном порядке
         * Тем не менее, надо учесть и эти варианты. Для этого все эти биты я и ввёл
         * Чтобы проверить, как все эти варианты работают, можно генерировать, например,
         * лишь одно значение дохода и одно расхода, тогда вся прибыль будет одинаковая
         * два значения дохода и одно расхода
         * три значения дохода и одно расхода
         * Это в методе public void newFinancialReport()
         * строчки с finReport[i, 1] = r.Next(50, 301) * 1_000 и
         * finReport[i, 2] = r.Next(0, 201) * 1_000
         * Я проверил. Программа во всех крайних случаях работает корректно
         */

        int numberOfdiffProfitValues;       // Количество РАЗНЫХ лучших значений прибыли
                                            // Это число равно количеству РАЗНЫХ худших значений прибыли

        // заголовок таблицы финансовой отчётности
        static string header = $"{"Месяц",6}" +
                               $"{"Доход, тыс. руб.",22}" +
                               $"{"Расход, тыс. руб.",19}" +
                               $"{"Прибыль, тыс. руб.",23}";

        // Окно, в котором будем выводить фин. отчёт
        WindowOutput windowFR;

        #endregion объявления переменных

        public FinancialReport()
        {
            newFinancialReport();
        }
 
        /// <summary>
        /// Создаём массив отчётности и заполняем его данными
        /// </summary>
        public void newFinancialReport()
        {
            int i;
            #region    Первый проход. Генерируем массив дохода, расхода, прибыли 

            Random r = new Random();   // доход, расход
            finReport[12, 1] = 0;      // обнуляем значение итогового дохода
            finReport[12, 2] = 0;      // обнуляем значение итогового расхода
            
            finReport[0, 0] = 0;                        // обнуляем поле индикатора
            finReport[0, 1] = r.Next(50, 301) * 1_000;  // доход. Делаем заведомо >= 50, чтобы не было странных
                                                        // цифр. 
            finReport[12, 1] += finReport[0, 1];        // накапливаем итог доходов
            finReport[0, 2] = r.Next(0, 201) * 1_000;   // расход
            finReport[12, 2] += finReport[0, 2];        // накапливаем итог расходов
            finReport[0, 3] = finReport[0, 1] - finReport[0, 2]; // прибыль

            bestMonth  = finReport[0, 3];   // значение прибыли первого месяца делаем лучшим
            worstMonth = bestMonth;         // и худшим одновременно
            numberOfdiffProfitValues = 1;   // количество разных значений прибыли пока равно 1

            for (i = 1; i < 12; i++)
            {
                finReport[i, 0] = 0;                        // обнуляем поле индикатора
                finReport[i, 1] = r.Next(50, 301) * 1_000;  // доход (50 - 301)
                finReport[12, 1] += finReport[i, 1];        // накапливаем итог доходов
                finReport[i, 2] = r.Next(0, 201) * 1_000;   // расход (0 - 201)
                finReport[12, 2] += finReport[i, 2];        // накапливаем итог расходов
                finReport[i, 3] = finReport[i, 1] - finReport[i, 2]; // прибыль

                // Находим одно, два, или три ЛУЧШИХ ЗНАЧЕНИЯ прибыли, НЕ месяца
                // И три ХУДШИХ ЗНАЧЕНИЯ прибыли
                switch (numberOfdiffProfitValues)
                {
                    case 1:
                        if (finReport[i, 3] > bestMonth)   // нашли значение больше лучшего
                        {                                  // т.к. сейчас всего одно, значит
                            numberOfdiffProfitValues = 2;  // нашли 2 разных значения
                            secondBest = bestMonth;        // копируем во второе значение лучшего
                            bestMonth = finReport[i, 3];   // текущее зн. прибыли становится лучшим
                            secondWorst = finReport[i, 3]; // текущее зн. прибыли становится вторым с конца
                                                           // значение худшего месяца остаётся неизменным
                        }
                        else if (finReport[i, 3] < bestMonth)    // нашли значение меньше лучшего
                        {                                        // т.к. сейчас всего одно, значит
                            numberOfdiffProfitValues = 2;        // нашли 2 разных значения
                                                                 // лучшее остаётся неизменным
                            secondBest = finReport[i, 3];        // копируем во второе лучшее текущее зн. прибыли
                            secondWorst = worstMonth;            // копируем во второе худшее значение
                            worstMonth = finReport[i, 3];        // текущее зн. прибыли становится худшим
                        }
                        // Если не выполняются оба условия, значит 
                        // текущее зн. прибыли равно лучшему и худшему значению, finReport[i, 3] == bestMonth
                        // количество разных ЗНАЧЕНИЙ не увеличиваем, по-прежнему 1
                        break;

                    case 2:                 
                        // B > 2B > i   ==>                    3B = i
                        // 2W > W > i   ==>   3W = 2W; 2W = W;  W = i
                        if (finReport[i, 3] < secondBest)   // нашли значение меньше второго
                        {                                   // т.к. уже есть два разных значения, значит
                            numberOfdiffProfitValues = 3;   // увеличиваем кол-во разных значений до 3.
                            
                            thirdBest = finReport[i, 3];    // 3B = i

                            thirdWorst = secondWorst;       // 3W = 2W
                            secondWorst = worstMonth;       // 2W = W
                            worstMonth = finReport[i, 3];   // W = i

                        } 
                        // i > 2B ?
                        // i >  W ?
                        else if (finReport[i, 3] > secondBest)  // текущее лучшего второго
                        {                                       
                            // B > i > 2B    ==>    3B = 2B; 2B = i
                            // 2W > i > W    ==>    3W = 2W; 2W = i
                            if (finReport[i, 3] < bestMonth)    // текущее зн. прибыли между первым и вторым
                            {                                   // т.к. уже два значения, значит
                                numberOfdiffProfitValues = 3;   // увеличиваем количество до 3
                                
                                thirdBest = secondBest;         // 3B = 2B
                                secondBest = finReport[i, 3];   // 2B = i

                                thirdWorst = secondWorst;       // 3W = 2W
                                secondWorst = finReport[i, 3];  // 2W = i
                            }

                            // i >  B > 2B   ==>    3B = 2B; 2B = B; B = i
                            // i > 2W >  W   ==>    3W = i
                            else if (finReport[i, 3] > bestMonth)   // текущее лучшего первого
                            {                                       // т.к. сейчас всего два, значит
                                numberOfdiffProfitValues = 3;       // увеличиваем количество до 3
 
                                thirdBest = secondBest;             // 3B = 2B
                                secondBest = bestMonth;             // 2B = B
                                bestMonth = finReport[i, 3];        // B = i

                                thirdWorst = finReport[i, 3];       // 3W = i
                            }
                            // Если не выполняются оба условия, значит 
                            // i == B == 2W
                            // количество разных ЗНАЧЕНИЙ не увеличиваем, по-прежнему 2
                        }
                        // Если не выполняются оба условия, значит 
                        // i == 2B == W
                        // количество разных ЗНАЧЕНИЙ не увеличиваем, по-прежнему 2
                        break;

                    case 3:                 
                        // B > 2B > 3B >= i
                        if (finReport[i, 3] <= thirdBest)    // текущее меньше или равно третьему
                        {                                    
                            // do nothing
                            // идём проверять не больше ли оно третьего худшего значения
                        }
                        // значит i > 3B.
                        // Проверяем 2B > i > 3B
                        else if (finReport[i, 3] < secondBest)  // текущее зн. прибыли между третьим и вторым?
                        {                                   
                            thirdBest = finReport[i, 3];        // 3B = i
                        }
                        // проверяем i > 2B ?
                             else if (finReport[i, 3] > secondBest)  // текущее лучшего второго
                             {                                       
                                // B > i > 2B > 3B  ==>   3B = 2B; 2B = i
                                if (finReport[i, 3] < bestMonth)    // текущее зн. прибыли между первым и вторым
                                {                                   
                                   thirdBest = secondBest;         // 3B = 2B
                                  secondBest = finReport[i, 3];   // 2B = i
                                }
                                // i > B > 2B > 3B  ==>   3B = 2B; 2B = B; B = i
                                else if (finReport[i, 3] > bestMonth)   // текущее лучшего первого
                                     {                                       
                                        thirdBest = secondBest;             // 3B = 2B
                                        secondBest = bestMonth;             // 2B = B
                                        bestMonth = finReport[i, 3];        // B = i
                                     }
                                     // Если не выполняются оба условия, значит 
                                     // i == B
                             }
                             // Если не выполняются оба условия, значит 
                             // i == 2B
 
                        // i > 3W > 2W > W
                        if (finReport[i, 3] >= thirdWorst)   // текущее больше или равно третьему
                        {
                            // do nothing
                        }
                        else if (finReport[i, 3] > secondWorst)  // текущее зн. прибыли между третьим и вторым
                        {
                            thirdWorst = finReport[i, 3];        // текущее зн. прибыли становится третьим
                        }
                        else if (finReport[i, 3] < secondWorst)  // текущее хуже второго
                        {                                        // сравниваем с наименьшим значением
                            if (finReport[i, 3] > worstMonth)    // текущее зн. прибыли между худшим и вторым
                            {
                                thirdWorst = secondWorst;       // В третье копируем второе
                                secondWorst = finReport[i, 3];  // Во второе копируем текущее зн. прибыли
                                                                // Наименьшее остаётся неизменным
                            }
                            else if (finReport[i, 3] < worstMonth)  // текущее меньше наименьшего
                            {
                                thirdWorst = secondWorst;           // В третье копируем второе
                                secondWorst = worstMonth;           // Во второе копируем первое
                                worstMonth = finReport[i, 3];       // текущее зн. прибыли становится наихудшим
                            }
                            // Если не выполняются оба условия, значит 
                            // текущее зн. прибыли равно худшему значению, finReport[i, 3] == worstMonth
                        }
                        // Если не выполняются оба условия, значит 
                        // текущее зн. прибыли равно второму значению, finReport[i, 3] == secondWorst

                        break;

                    default: break;
                } // switch (numberOfbestMonths)

            }
            finReport[12, 3] = finReport[12, 1] - finReport[12,2];   // прибыль за период

            #endregion    Первый проход. Генерируем массив дохода, расхода, прибыли

            #region    Второй проход. Помечаем лучшие и худшие месяцы
            // Формируем строки с номерами месяцев трёх лучших и трёх худших показателей
            // Устанавливаем соответствующий бит в finReport [i, 0] в 1
            // Крайние случаи. Рассмотрим для лучших месяцев. Для худших всё то же самое.
            // 1. Все значения прибыли одинаковые (теоретически такое возможно) - это значит, что
            // numberOfbestMonths == numberOfworstMonths ==  1;
            // Тогда устанавливаем в каждом месяце биты bestMonthMask и worstMonthMask
            // 2. Только два разных значения прибыли среди всех месяцев
            // 3. Три разных значения, но они перекрываются, т.е. всего 3, 4 или 5 разных значений
            // 4. Три разных значения, и они все разные

            // Инициализируем значения строк, в которых будут содержаться номера соответствующих месяцев
            bestMonthS = ""; secondBestS = ""; thirdBestS = "";    // Номера месяцев, удовлетворяющих B, 2B, 3B
            thirdWorstS = ""; secondWorstS = ""; worstMonthS = ""; // Номера месяцев, удовлетворяющих 3W, 2W, W


            for (i = 0; i < 12; i++)
            { 
                switch (numberOfdiffProfitValues)
                {
                    case 1:     // Все значения прибыли равны. Это одновременно лучшие и худшие месяцы
                        finReport[i, 0] = bestMonthMask | worstMonthMask;
                        bestMonthS += (bestMonthS == "") ? "1" : $", {i + 1}";
                        break;

                    case 2:     // Всего два значения прибыли
                        finReport[i, 0] = (finReport[i, 3] == bestMonth) 
                                          ? bestMonthMask  | secondWorstMask    // лучший, значит второй с конца
                                          : secondBestMask | worstMonthMask;    // второй, значит худший
                        if (finReport[i, 3] == bestMonth)
                        {
                            finReport[i, 0] = bestMonthMask | secondWorstMask;  // i == B == 2W

                            // формируем строку, содержащую номера лучших месяцев (B)
                            // добавляем запятую, если это уже не первый месяц
                            bestMonthS += (bestMonthS == "") ? $"{i+1}" : $", {i + 1}";

                            // формируем строку, содержащую номера вторых с конца месяцев (2W)
                            // добавляем запятую, если это уже не первый месяц
                            secondWorstS += (secondWorstS == "") ? $"{i + 1}" : $", {i + 1}";
                        }
                        else
                        {
                            finReport[i, 0] = secondBestMask | worstMonthMask;  // i == 2B == W

                            // формируем строку, содержащую номера вторых месяцев (2B)
                            // добавляем запятую, если это уже не первый месяц
                            secondBestS += (secondBestS == "") ? $"{i + 1}" : $", {i + 1}";

                            // формируем строку, содержащую номера худших месяцев (W)
                            // добавляем запятую, если это уже не первый месяц
                            worstMonthS += (worstMonthS == "") ? $"{i + 1}" : $", {i + 1}";

                        }
                        break;

                    case 3:                        
                        if (finReport[i, 3] == bestMonth)       // Проверяем i == B ?
                        {
                            finReport[i, 0] |= bestMonthMask;
                            // формируем строку, содержащую номера лучших месяцев (B)
                            // добавляем запятую, если это уже не первый месяц
                            bestMonthS += (bestMonthS == "") ? $"{i + 1}" : $", {i + 1}";
                        }

                        if (finReport[i, 3] == secondBest)       // Проверяем i == 2B ?
                        {
                            finReport[i, 0] |= secondBestMask;
                            // формируем строку, содержащую номера вторых месяцев (2B)
                            // добавляем запятую, если это уже не первый месяц
                            secondBestS += (secondBestS == "") ? $"{i + 1}" : $", {i + 1}";
                        }

                        if (finReport[i, 3] == thirdBest)       // Проверяем i == 3B ?
                        {
                            finReport[i, 0] |= thirdBestMask;
                            // формируем строку, содержащую номера третьих месяцев (3B)
                            // добавляем запятую, если это уже не первый месяц
                            thirdBestS += (thirdBestS == "") ? $"{i + 1}" : $", {i + 1}";
                        }

                        if (finReport[i, 3] == thirdWorst)       // Проверяем i == 3W ?
                        {
                            finReport[i, 0] |= thirdWorstMask;
                            // формируем строку, содержащую номера третьих месяцев (3W)
                            // добавляем запятую, если это уже не первый месяц
                            thirdWorstS += (thirdWorstS == "") ? $"{i + 1}" : $", {i + 1}";
                        }

                        if (finReport[i, 3] == secondWorst)       // Проверяем i == 2W ?
                        {
                            finReport[i, 0] |= secondWorstMask;
                            // формируем строку, содержащую номера третьих месяцев (2W)
                            // добавляем запятую, если это уже не первый месяц
                            secondWorstS += (secondWorstS == "") ? $"{i + 1}" : $", {i + 1}";
                        }

                        if (finReport[i, 3] == worstMonth)       // Проверяем i == W ?
                        {
                            finReport[i, 0] |= worstMonthMask;
                            // формируем строку, содержащую номера третьих месяцев (W)
                            // добавляем запятую, если это уже не первый месяц
                            worstMonthS += (worstMonthS == "") ? $"{i + 1}" : $", {i + 1}";
                        }
                        break;
                }

            }

            if (numberOfdiffProfitValues == 1) worstMonthS = bestMonthS;

            #endregion    Второй проход. Помечаем лучшие и худшие месяцы

        }

        /// <summary>
        /// Организация пунктов меню
        /// </summary>
        public void financialReportMenu()
        {
            ConsoleKey action;       // Переменная, в которую будет считываться нажатие клавиши
            int currItem = 1;        // Текущий пункт меню

            Console.CursorVisible = false;  // Делаем курсор невидимым
            windowFR = new WindowOutput(winWidth, winHeight);
            windowFR.HeaderCenter("ФИНАНСОВЫЙ УЧЁТ В КОМПАНИИ", winWidth, 2, ConsoleColor.Yellow);

            do                  // Считываем нажатия, пока не будет ESC
            {
                action = windowFR.MenuSelect(menuItems, currItem, winWidth, 4);

                switch (action)
                {
                    case ConsoleKey.D1:
                        printReport(5 + menuItems.Length + 5 - 1);  // 5 - main header number of lines
                                                                    // menuItems.Lenght - number of menu items
                                                                    // 5 - space and info lines after menu
                                                                    // (-1) - offset starts from zero (0)
                        currItem = 1;
                        break;

                    case ConsoleKey.D2:
                        printPositiveProfit(5 + menuItems.Length + 5 - 1); 
                        currItem = 2;
                        break;

                    case ConsoleKey.D3:
                        printBestMonth(5 + menuItems.Length + 5 - 1);
                        currItem = 3;
                        break;

                    case ConsoleKey.D4:
                        printThreeBestMonths(5 + menuItems.Length + 5 - 1);
                        currItem = 4;
                        break;

                    case ConsoleKey.D5:
                        printWorstMonth(5 + menuItems.Length + 5 - 1);
                        currItem = 5;
                        break;

                    case ConsoleKey.D6:
                        printThreeWorstMonths(5 + menuItems.Length + 5 - 1);
                        currItem = 6;
                        break;

                    case ConsoleKey.D7:
                        newFinancialReport();
                        printReport(5 + menuItems.Length + 5 - 1);
                        currItem = 7;
                        break;

                    case ConsoleKey.Escape:
                        Console.WriteLine("ДО СВИДАНИЯ!");
                        break;

                    default:
                        break;   // нажата любая другая клавиша - ничего не происходит
                }

            } while (action != ConsoleKey.Escape);

        }  //  public void financialReportMenu

        /// <summary>
        /// Вывод на экран отчётности с заголовком столбцов и итогами
        /// </summary>
        /// <param name="vert">Строка, с которой начинать вывод таблицы</param>
        public void printReport(int vert)   // Первый пункт меню
        {
            int i;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            windowFR.HeaderCenter(header, winWidth, vert, ConsoleColor.DarkYellow);
            vert++;
            for (i = 0; i < 12; i++)
            {
                windowFR.HeaderCenter($"{i + 1,5}" +                   // month number
                              $"{finReport[i, 1],21:### ##0}" +        // income  
                              $"{finReport[i, 2],19:### ##0}" +        // expenses  
                              $"{finReport[i, 3],23:### ##0}",         // profit  
                              winWidth, vert + i, ConsoleColor.Gray);
            }
            windowFR.HeaderCenter($"ИТОГО" +                   // month number
                          $"{finReport[i, 1],21:### ### ##0}" +        // income  
                          $"{finReport[i, 2],19:### ### ##0}" +        // expenses  
                          $"{finReport[i, 3],23:### ### ##0}",         // profit  
                          winWidth, vert + i, ConsoleColor.Yellow);
            i += 2;

            // Очищаем 5 строк под таблицей в которых были значения
            for (int j = 0; j < 5; j++)
            {
                windowFR.CleanLine(winWidth, vert + i, ConsoleColor.Black);
                i++;
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        
        }   //        printReport(int vert) Первый пункт меню

        /// <summary>
        /// Высвечивает на экране строки с положительной отчётностью
        /// </summary>
        /// <param name="vert">Строка, с которой начинать вывод таблицы</param>
        public void printPositiveProfit(int vert)   // Второй пункт меню МЕСЯЦЫ С ПОЛОЖИТЕЛЬНОЙ ПРИБЫЛЬЮ
        {
            int i;
            int currLine;                   // Текущая строка
            int positiveMonths = 0;         // счётчик месяцев с положительной прибылью.
            ConsoleColor currentColor;      // храним текущий цвет

            currLine = vert;
            // Выводим заголовок на всяк случай, если кнопку нажали первой (строка № 1 таблицы)
            windowFR.HeaderCenter(header, winWidth, currLine, ConsoleColor.DarkYellow);

            Console.BackgroundColor = ConsoleColor.Black;

            currLine++;             // Переходим на следующую строку
            
            // Печатаем таблицу, выделяя тёмно-жёлтым месяцы с положительной прибылью
            for (i = 0; i < 12; i++)
            {
                // Если в данном месяце прибыль положительная 
                if (finReport[i, 3] > 0)
                {
                    positiveMonths++;                           // добавляем 1 к счётчику месяцев с пол. приб
                    currentColor = ConsoleColor.Cyan;
                }
                else
                {
                    currentColor = ConsoleColor.Gray;
                }

                windowFR.HeaderCenter($"{i + 1,5}" +                   // month number
                              $"{finReport[i, 1],21:### ##0}" +        // income  
                              $"{finReport[i, 2],19:### ##0}" +        // expenses  
                              $"{finReport[i, 3],23:### ##0}",         // profit  
                              winWidth, currLine + i, currentColor);
            }

            Console.ForegroundColor = ConsoleColor.Gray;

            // Печатаем итоговую строку (это уже строка № 14 таблицы)
            windowFR.HeaderCenter($"ИТОГО" +                   // month number
                          $"{finReport[i, 1],21:### ### ##0}" +        // income  
                          $"{finReport[i, 2],19:### ### ##0}" +        // expenses  
                          $"{finReport[i, 3],23:### ### ##0}",         // profit  
                          winWidth, currLine + i, ConsoleColor.Yellow);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;

            // Очищаем строку под таблицей - чёрный цвет фона
            windowFR.CleanLine(winWidth, vert + positiveProfitOffset, ConsoleColor.Black);

            // Выводим количество месяцев с положительной прибылью
            if (positiveMonths > 0)         // Вдруг таких нет
            {
                windowFR.HeaderCenter($"Месяцев с положительной прибылью: {positiveMonths,3}", 
                                      winWidth, vert + positiveProfitOffset, ConsoleColor.Cyan);
            }
            else
            {
                windowFR.HeaderCenter($"В отчётном периоде нет месяцев с положительной прибылью!",
                      winWidth, vert + positiveProfitOffset, ConsoleColor.Cyan);
            }
            Console.ForegroundColor = ConsoleColor.Gray;

        }   //        printPositiveProfit Второй пункт меню

        /// <summary>
        /// Высвечивает на экране строки с лучшим месяцем (и)
        /// </summary>
        /// <param name="vert">Строка, с которой начинать вывод таблицы</param>
        public void printBestMonth(int vert)   // Третий пункт меню ЛУЧШИЙ МЕСЯЦ ПО ПРИБЫЛИ
        {
            int i;
            int currLine;                   // Текущая строка
            currLine = vert;
            currLine++;

            ConsoleColor currentColor;

            // Выводим заголовок на всяк случай, если кнопку нажали первой
            windowFR.HeaderCenter(header, winWidth, vert, ConsoleColor.DarkYellow);

            Console.BackgroundColor = ConsoleColor.Black;

            // Выводим всю таблицу, и помечаем месяц с лучшей прибылью
            // или месяцы, если в двух или более месяцах прибыль была одинаковой
            // Можно было бы выводить только один месяц с прибылью, НО
            // в самом начале экран может быть пуст,
            // и таких месяцев может быть несколько, поэтому надо пройтись по всему массиву

            for (i = 0; i < 12; i++)
            {
                if (finReport[i, 3] == bestMonth)
                {
                    currentColor = ConsoleColor.Green;
                }
                else
                {
                    currentColor = ConsoleColor.Gray;
                }
                windowFR.HeaderCenter($"{i + 1,5}" +                   // month number
                              $"{finReport[i, 1],21:### ##0}" +        // income  
                              $"{finReport[i, 2],19:### ##0}" +        // expenses  
                              $"{finReport[i, 3],23:### ##0}",         // profit  
                              winWidth, currLine + i, currentColor);
            }

            Console.ForegroundColor = ConsoleColor.Gray;

            windowFR.HeaderCenter($"ИТОГО" +                   // month number
                          $"{finReport[i, 1],21:### ### ##0}" +        // income  
                          $"{finReport[i, 2],19:### ### ##0}" +        // expenses  
                          $"{finReport[i, 3],23:### ### ##0}",         // profit  
                          winWidth, currLine + i, ConsoleColor.Yellow);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            
            // Очищаем строку под таблицей - чёрный цвет фона
            windowFR.CleanLine(winWidth, vert + bestMonthsOffset, ConsoleColor.Black);

            // Выводим наилучший месяц (ы)
            windowFR.HeaderCenter("Месяц(ы) с наилучшей прибылью: " + bestMonthS,
                                      winWidth, vert + bestMonthsOffset, ConsoleColor.Green);
            Console.ForegroundColor = ConsoleColor.Gray;

        }   //        printBestMonth Третий пункт меню

        /// <summary>
        /// Высвечивает все месяцы, значения прибыли которых входит в тройку лучших.
        /// Печатает внизу строку с этими месяцами
        /// </summary>
        /// <param name="vert">Строка, с которой начинать вывод таблицы</param>
        public void printThreeBestMonths(int vert)  // Четвёртый пункт меню МЕСЯЦЫ ТРЁХ ЛУЧШИХ ПОКАЗАТЕЛЕЙ ПО ПРИБЫЛИ
        {
            int i;
            int currLine;                   // Текущая строка
            currLine = vert;
            currLine++;

            ConsoleColor currentColor;      // Текущий цвет, которым выводится значение
                                            // меняется в зависимости от значения

            // Выводим заголовок на всяк случай, если кнопку нажали первой
            windowFR.HeaderCenter(header, winWidth, vert, ConsoleColor.DarkYellow);

            Console.BackgroundColor = ConsoleColor.Black;

            // Выводим всю таблицу, и помечаем месяц с прибылью, попавшей в тройку лучших
            // или месяцы, если в двух или более месяцах прибыль была одинаковой

            for (i = 0; i < 12; i++)
            {
                switch ((finReport[i, 0] & bestMonthMask) | 
                        (finReport[i, 0] & secondBestMask) |
                        (finReport[i, 0] & thirdBestMask))
                {
                    case bestMonthMask:
                        currentColor = ConsoleColor.Green;
                        break;

                    case secondBestMask:
                        currentColor = ConsoleColor.Cyan;
                        break;

                    case thirdBestMask:
                        currentColor = ConsoleColor.Blue;
                        break;

                    default:
                        currentColor = ConsoleColor.Gray;
                        break;
                }
                windowFR.HeaderCenter($"{i + 1,5}" +                   // month number
                              $"{finReport[i, 1],21:### ##0}" +        // income  
                              $"{finReport[i, 2],19:### ##0}" +        // expenses  
                              $"{finReport[i, 3],23:### ##0}",         // profit  
                              winWidth, currLine + i, currentColor);
            }

            Console.ForegroundColor = ConsoleColor.Gray;

            windowFR.HeaderCenter($"ИТОГО" +                   // month number
                          $"{finReport[i, 1],21:### ### ##0}" +        // income  
                          $"{finReport[i, 2],19:### ### ##0}" +        // expenses  
                          $"{finReport[i, 3],23:### ### ##0}",         // profit  
                          winWidth, currLine + i, ConsoleColor.Yellow);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;

            // Очищаем строку под таблицей - чёрный цвет фона
            windowFR.CleanLine(winWidth, vert + threeBestMonthsOffset, ConsoleColor.Black);

            // Выводим номера трёх наилучший месяцев
            windowFR.HeaderCenter("МЕСЯЦЫ ТРЁХ ЛУЧШИХ ПОКАЗАТЕЛЕЙ ПО ПРИБЫЛИ: " + 
                                   bestMonthS + 
                                   ((secondBestS != "") ? $", {secondBestS}" : "") + 
                                   ((thirdBestS != "") ? $", {thirdBestS}": ""),
                                      winWidth, vert + threeBestMonthsOffset, ConsoleColor.DarkYellow);
            Console.ForegroundColor = ConsoleColor.Gray;

        } // printThreeBestMonths(int vert)

        /// <summary>
        /// Высвечивает на экране строку(и) с худшим месяцем (и)
        /// </summary>
        /// <param name="vert">Строка, с которой начинать вывод таблицы</param>
        public void printWorstMonth(int vert)   // Пятый пункт меню ХУДШИЙ МЕСЯЦ ПО ПРИБЫЛИ
        {
            int i;
            int currLine;                   // Текущая строка
            currLine = vert;
            currLine++;

            ConsoleColor currentColor;

            // Выводим заголовок на всяк случай, если кнопку нажали первой
            windowFR.HeaderCenter(header, winWidth, vert, ConsoleColor.DarkYellow);

            Console.BackgroundColor = ConsoleColor.Black;

            // Выводим всю таблицу, и помечаем месяц с лучшей прибылью
            // или месяцы, если в двух или более месяцах прибыль была одинаковой

            for (i = 0; i < 12; i++)
            {
                if (finReport[i, 3] == worstMonth)
                {
                    currentColor = ConsoleColor.Red;
                }
                else
                {
                    currentColor = ConsoleColor.Gray;
                }
                windowFR.HeaderCenter($"{i + 1,5}" +                   // month number
                              $"{finReport[i, 1],21:### ##0}" +        // income  
                              $"{finReport[i, 2],19:### ##0}" +        // expenses  
                              $"{finReport[i, 3],23:### ##0}",         // profit  
                              winWidth, currLine + i, currentColor);
            }

            Console.ForegroundColor = ConsoleColor.Gray;

            windowFR.HeaderCenter($"ИТОГО" +                   // month number
                          $"{finReport[i, 1],21:### ### ##0}" +        // income  
                          $"{finReport[i, 2],19:### ### ##0}" +        // expenses  
                          $"{finReport[i, 3],23:### ### ##0}",         // profit  
                          winWidth, currLine + i, ConsoleColor.Yellow);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;

            // Очищаем строку под таблицей - чёрный цвет фона
            windowFR.CleanLine(winWidth, vert + worstMonthsOffset, ConsoleColor.Black);

            // Выводим наилучший месяц (ы)
            windowFR.HeaderCenter("Месяц(ы) с наихудшей прибылью: " + worstMonthS,
                                      winWidth, vert + worstMonthsOffset, ConsoleColor.Red);
            Console.ForegroundColor = ConsoleColor.Gray;

        }   //        printWorstMonth Пятый пункт меню

        /// <summary>
        /// Высвечивает все месяцы, значения прибыли которых входит в тройку худших.
        /// Печатает внизу строку с этими месяцами
        /// </summary>
        /// <param name="vert">Строка, с которой начинать вывод таблицы</param>
        public void printThreeWorstMonths(int vert)  // Шестой пункт меню МЕСЯЦЫ ТРЁХ ХУДШИХ ПОКАЗАТЕЛЕЙ ПО ПРИБЫЛИ
        {
            int i;
            int currLine;                   // Текущая строка
            currLine = vert;
            currLine++;

            ConsoleColor currentColor;

            // Выводим заголовок на всяк случай, если кнопку нажали первой
            windowFR.HeaderCenter(header, winWidth, vert, ConsoleColor.DarkYellow);

            Console.BackgroundColor = ConsoleColor.Black;

            // Выводим всю таблицу, и помечаем месяц с прибылью, попавшей в тройку лучших
            // или месяцы, если в двух или более месяцах прибыль была одинаковой

            for (i = 0; i < 12; i++)
            {
                switch ((finReport[i, 0] & worstMonthMask) |
                        (finReport[i, 0] & secondWorstMask) |
                        (finReport[i, 0] & thirdWorstMask))
                {
                    case worstMonthMask:
                        currentColor = ConsoleColor.Red;
                        break;

                    case secondWorstMask:
                        currentColor = ConsoleColor.DarkRed;
                        break;

                    case thirdWorstMask:
                        currentColor = ConsoleColor.Magenta;
                        break;

                    default:
                        currentColor = ConsoleColor.Gray;
                        break;
                }
                windowFR.HeaderCenter($"{i + 1,5}" +                   // month number
                              $"{finReport[i, 1],21:### ##0}" +        // income  
                              $"{finReport[i, 2],19:### ##0}" +        // expenses  
                              $"{finReport[i, 3],23:### ##0}",         // profit  
                              winWidth, currLine + i, currentColor);
            }

            Console.ForegroundColor = ConsoleColor.Gray;

            windowFR.HeaderCenter($"ИТОГО" +                   // month number
                          $"{finReport[i, 1],21:### ### ##0}" +        // income  
                          $"{finReport[i, 2],19:### ### ##0}" +        // expenses  
                          $"{finReport[i, 3],23:### ### ##0}",         // profit  
                          winWidth, currLine + i, ConsoleColor.Yellow);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;

            // Очищаем строку под таблицей - чёрный цвет фона
            windowFR.CleanLine(winWidth, vert + threeWorstMonthsOffset, ConsoleColor.Black);

            // Выводим номера трёх наилучший месяцев
            windowFR.HeaderCenter("МЕСЯЦЫ ТРЁХ ХУДШИХ ПОКАЗАТЕЛЕЙ ПО ПРИБЫЛИ: " +
                                   worstMonthS +
                                   ((secondWorstS != "") ? $", {secondWorstS}" : "") +
                                   ((thirdWorstS != "") ? $", {thirdWorstS}" : ""),
                                      winWidth, vert + threeWorstMonthsOffset, ConsoleColor.Magenta);
            Console.ForegroundColor = ConsoleColor.Gray;

        }   // printThreeWorstMonths(int vert)

    }   //  Class FinancialReport

}   // namespace Homework_Theme_04
