using System;
using System.Collections.Generic;
using System.Linq;

namespace Omega_VocabularyConsole
{
    class UserInteraction
    {
        public static VocabularyDico dico = new VocabularyDico();
        string command;
        string word;
        bool launched = false;

        VocabularyWord tempVocabWord = new VocabularyWord();

        public void UserApp()
        {
            if (launched)
                Console.Clear();
            else
                launched = true;

            dico.LoadDico();
            dico.LoadVocab();

            if (dico.languagesSupported.Count == 0) // No language added yet
                AddNewLanguage();

            Console.WriteLine("\nWhat do you wish to do?");
            Console.WriteLine("\n1: Add Word \n2: Delete Word \n3: Delete all words " +
                "\n4: Edit \n5: Search by Word \n6: Search by Language \n7: See Stats" +
                "\n8: Add Language \n9: Remove Language \n10: Delete all words and languages");

            switch (Console.ReadLine())
            {
                case "1": // Add
                    Add();
                    break;
                case "2": // Remove
                    Remove();
                    break;
                case "3": // Remove all
                    RemoveAll();
                    break;
                case "4": // Edit
                    Edit();
                    break;
                case "5": // Search Word
                    SearchByWord();
                    break;
                case "6": // Search Language
                    SearchByLanguage();
                    break;
                case "7": // See Stats
                    ShowStats();
                    break;
                case "8": // Add language
                    AddNewLanguage();
                    break;
                case "9": // Remove language
                    RemoveLanguage();
                    break;
                case "10": // Delete all languages + Words
                    RemoveEverything();
                    break;
                default:
                    Console.WriteLine("Please enter a valid command");
                    Console.ReadKey();
                    UserApp();
                    break;
            }
        }

        #region ADD / EDIT
        public void Add()
        {
            Console.Clear();

            WordEdition();
        }

        public void Edit()
        {
            Console.Clear();

            Console.WriteLine("Enter the word you wish to edit: ");
            word = Console.ReadLine();
            VocabularyWord vw = dico.GetVocabularyWord(word);

            WordEdition(vw);
        }

        public void WordEdition(VocabularyWord vWord = null)
        {
            tempVocabWord = new VocabularyWord();

            foreach (Languages languages in dico.languagesSupported)
            {
                Console.WriteLine("\n Please enter the " + languages.name + " word: ");

                tempVocabWord.translations.Add(new VocabWord()
                {
                    language = languages,
                    word = Console.ReadLine(),
                });
            }

            ConfirmWord(vWord);
        }

        private void ConfirmWord(VocabularyWord vWord)
        {
            Console.Clear();

            Console.WriteLine("Your word is: ");
            for (int i = 0; i < tempVocabWord.translations.Count; i++)
            {
                Console.WriteLine("\n" + tempVocabWord.translations[i].language.name + " - " + tempVocabWord.translations[i].word);
            }

            if (vWord != null)
            {
                Console.WriteLine("\nYour old word was: ");

                for (int i = 0; i < vWord.translations.Count; i++)
                {
                    Console.WriteLine("\n" + vWord.translations[i].language.name + " - " + vWord.translations[i].word);
                }
            }

            Console.WriteLine("\nPress 1 to validate or press any key to Cancel");
            command = Console.ReadLine();

            if (command == "1")
            {
                if (vWord != null)
                    dico.EditVocabularyWord(vWord, tempVocabWord);
                else
                    dico.AddToVocabularyList(tempVocabWord);
            }

            Console.ReadKey();
            UserApp();
        }
        #endregion

        #region REMOVE
        public void Remove()
        {
            Console.Clear();

            Console.WriteLine("Please enter the word you wish to remove: ");

            string word = Console.ReadLine();
            dico.RemoveFromVocabularyList(word);

            Console.ReadKey();
            UserApp();
        }

        public void RemoveAll()
        {
            Console.Clear();

            dico.RemoveAll();
            Console.WriteLine("Vocabulary list deleted successfully!");

            Console.ReadKey();
            UserApp();
        }
        #endregion

        #region SEARCH
        public void SearchByWord()
        {
            Console.Clear();

            Console.WriteLine("Please enter the word you wish to search: ");

            word = Console.ReadLine();
            VocabularyWord vw = dico.SearchFromWord(word);

            Console.Clear();

            if (vw == null)
            {
                SearchByWord();
                return;
            } 
            
            Console.WriteLine("Your word is: ");
            for (int i = 0; i < vw.translations.Count; i++)
            {
                Console.WriteLine("\n " + vw.translations[i].language.name + " - " + vw.translations[i].word);
            }

            Console.ReadKey();
            UserApp();
        }

        public void SearchByLanguage()
        {
            Console.Clear();

            Console.WriteLine("Please enter the Language index you wish to search: ");

            for (int i = 0; i < dico.languagesSupported.Count; i++)
            {
                Console.WriteLine("\n" + i + " - " + dico.languagesSupported[i].name);
            }

            word = Console.ReadLine();

            List<string> words = new List<string>();

            if (word.All(char.IsDigit) && int.Parse(word) >= 0 && int.Parse(word) < dico.languagesSupported.Count)
            {
                for (int i = 0; i < dico.languagesSupported.Count; i++)
                {
                    if (i == int.Parse(word))
                    {
                        words = dico.SearchByLanguage(int.Parse(word));
                        break;
                    }
                    else continue;
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid command");
                Console.ReadKey();
                SearchByLanguage();
                return;
            }

            Console.WriteLine("Here are your " + dico.languagesSupported[int.Parse(word)].name + " words: ");

            foreach (string word in words)
            {
                Console.WriteLine("\n" + word);
            }

            Console.ReadKey();
            UserApp();
        }

        public void ShowStats()
        {
            Console.Clear();

            Console.WriteLine("You have saved " + dico.SeeStats(0) + " words!");
            Console.WriteLine("You have added " + dico.SeeStats(1) + " languages!");

            Console.ReadKey();

            UserApp();
        }
        #endregion

        #region DICO DATAS
        public void AddNewLanguage()
        {
            Console.Clear();

            Console.WriteLine("Please enter the name of the language you wish to add: ");
            word = Console.ReadLine();

            Console.WriteLine("\nPress 1 to validate or press any key to Cancel");
            command = Console.ReadLine();

            if (command == "1")
                dico.AddLanguage(word);

            Console.ReadKey();
            UserApp();
        }

        public void RemoveLanguage()
        {
            Console.Clear();

            Console.WriteLine("Please enter the name of the language you wish to remove: ");
            word = Console.ReadLine();

            dico.RemoveLanguage(word);

            Console.ReadKey();
            UserApp();
        }

        public void RemoveEverything()
        {
            Console.Clear();

            dico.RemoveEverything();

            Console.ReadKey();
            UserApp();
        }
        #endregion
    }
}
