using System;

using StringEncryption.Core;

namespace StringEncryption
{
    class Program
    {
        static void Main(string[] args) => new Program().Run(args);

        void Run(string[] args)
        {
            if (args.Length is 0) Exit();

            Obfuscator obfuscator = new Obfuscator(args[0]);
            Console.WriteLine("String encryption started.");
            bool isObfuscated = obfuscator.Run();
            if (isObfuscated)
            {
                Console.WriteLine("Strings encrypted.");
                Exit();
            }
            else
            {
                Console.WriteLine("Encryption failed.");
                Exit();
            }
        }
   
        void Exit()
        {
            Console.WriteLine("Type enter to close.");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}