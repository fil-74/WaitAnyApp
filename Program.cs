using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WaitAnyApp
{
    class Program
    {

        static public int countofTasks = 25;
        static void Main(string[] args)
        {
            try
            {
                var tasks = new List<Task<string>>();
                var taskGenerator = new TaskGenerator();
                for (int i = 0; i < countofTasks; i++)
                {
                    var newtask = taskGenerator.createTask($"task -- {i + 1}");
                    tasks.Add(newtask);
                }

                tasks.ForEach(t => t.Start());
                var firstTask = Task.WaitAny(tasks.ToArray());

                var result = String.Empty;
                if (firstTask >= 0)
                {
                    Task<string> task = tasks[firstTask];
                    result = task.Result;
                }
                else
                {
                    result = "Возникла ошибка вычисления";
                }
                Console.WriteLine(result);
            }   
            catch(Exception E)
            {
                Console.WriteLine("Возникла ошибка выполнения:\n" + E.Message);
            }
        }


    }
    class TaskGenerator
    {
        private readonly Random random = new Random();
        private const int minDelay = 600;
        private const int maxDelay = 40000;

        public Task<string> createTask(string tag)
        {
            var delay = random.Next(minDelay, maxDelay);
            return getTagWithDelay(tag, delay);
        }

        private Task<string> getTagWithDelay(string tag, int delay)
        {
            Console.WriteLine($"==> {tag} has delay {delay}");
            return new Task<string>(() =>
            {
                Thread.Sleep(delay);
                Console.WriteLine($"==> {tag} executed with delay {delay}");
                return tag;
            });
        }
    }
}
