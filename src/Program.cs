using System;

namespace Omega_VocabularyConsole
{
    class Program
    {
        static UserInteraction userInteraction = new UserInteraction();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to your personal vocabulary dictionary: OMEGA Vocabulary!");

            userInteraction.UserApp();
        }
    }
}
