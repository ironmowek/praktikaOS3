using System;
using System.Collections.Generic;
using System.Threading;


namespace Negovora_A_OS_LAB_3
{
    class Program
    {
        static public Conveyer conveyer = new Conveyer();

        static void Main()
        {
            Storage storage = new Storage();
            storage.Work();

        }
    }

    public class Storage
    {
        Developer dev1;
        Developer dev2;
        Developer dev3;

        Consumer consumer1;
        Consumer consumer2;

        Conveyer conveyer1;

        static bool isQPressed = false;

        public Storage()
        {
            /* заполняем конвеер элементами */
            Queue<int> q = new Queue<int>();
            for (int i = 0; i < 200; i++)
            {
                q.Enqueue(1);
            }

            conveyer1 = new Conveyer(q);

            /* покупатели */
            consumer1 = new Consumer(conveyer1);
            consumer2 = new Consumer(conveyer1);

            /* производители */
            dev1 = new Developer(conveyer1);
            dev2 = new Developer(conveyer1);
            dev3 = new Developer(conveyer1);
        }

        public void Work()
        {
            Thread[] devThreads = new Thread[3];
            Thread[] consumerThreads = new Thread[2];

            Developing[] develops = new Developing[3] { new Developing(dev1), new Developing(dev2), new Developing(dev3) };

            for (int i = 0; i < 3; i++)
            {
                devThreads[i] = new Thread(develops[i].StartDeveloping);
            }

            Consuming[] consums = new Consuming[2] { new Consuming(consumer1), new Consuming(consumer2) };

            for (int i = 0; i < 2; i++)
            {
                consumerThreads[i] = new Thread(consums[i].StartConsuming);
            }

            for (int i = 0; i < 3; i++)
            {
                devThreads[i].Name = $"Производитель номер {i + 1}";
                devThreads[i].Start();
            }

            for (int i = 0; i < 2; i++)
            {
                consumerThreads[i].Name = $"Покупатель номер {i + 1}";
                consumerThreads[i].Start();
            }


            while (true)
            {
                if (isButtonPressed())
                {
                    isQPressed = true;
                }
                if (isQPressed)
                {
                    Console.WriteLine("Продажа остановлена");
                    break;
                }
            }
        }

        //выполнение условия с (Q)
        static public bool isButtonPressed()
        {
            return Console.ReadKey().Key == ConsoleKey.Q;
        }

        record class Developing(Developer dev)
        {
            public void StartDeveloping()
            {
                while (!isQPressed)
                {
                    if (dev.conveyer.Size() <= 80)
                    {
                        Console.WriteLine("Начало производства");
                        dev.Develope();
                        Thread.Sleep(300); //задержка
                    }
                    else if (dev.conveyer.Size() >= 100)
                    {

                    }

                }

                Console.WriteLine($"Поток {Thread.CurrentThread.Name} закончил работу");
            }
        }


        record class Consuming(Consumer consumer)
        {
            static bool isButtonPressed = false;
            public void StartConsuming()
            {
                while (true)
                {

                    if (consumer.conveyer.Size() != 0)
                    {
                        Console.WriteLine("Покупка");
                        consumer.Consume();
                        Thread.Sleep(300); //задержка
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

    }

    public class Developer
    {
        public Conveyer conveyer;

        public Developer(Conveyer c)
        {
            conveyer = c;
        }

        //если элементов меньше 80
        public void Develope()
        {
            Random rand = new Random();
            int numberEl = rand.Next(1, 100);

            for (int i = 0; i < numberEl; i++)
            {
                conveyer.Add();
            }
            Console.WriteLine($"Создание {numberEl} элементов");

        }
    }

    public class Conveyer
    {
        private Queue<int> queue;
        public Conveyer()
        {
            queue = new Queue<int>();
        }

        public Conveyer(Queue<int> q)
        {
            queue = q;
        }

        public void Add()
        {
            queue.Enqueue(1);//создание и инициализация 
        }

        public void Delete()
        {
            queue.Dequeue();//удаление
        }

        public int Size()
        {
            return queue.Count;//подсчет размера
        }

    }

    public class Consumer
    {
        public Conveyer conveyer;

        public Consumer(Conveyer q)
        {
            conveyer = q;
        }

        public void Consume()
        {
            conveyer.Delete();
            Console.WriteLine($"Количество элементов: {conveyer.Size()}");
        }
    }
}