using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Omega_VocabularyConsole
{
    class VocabularyDico
    {
        static private readonly string SAVE_FILE_PATH_VOCAB = "vocabulary.omega";
        static private readonly string SAVE_FILE_PATH_DICO = "dictionary.omega";
        public List<Languages> languagesSupported = new List<Languages>();
        public List<VocabularyWord> vocabList = new List<VocabularyWord>();
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            IncludeFields = true,
            WriteIndented = true
        };

        #region HELPERS
        /// <summary>
        /// Check if the word received from the user is already in the saved list
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool IsWordInList(string word, bool showError)
        {
            foreach (VocabularyWord item in vocabList)
            {
                foreach (VocabWord wordTranslated in item.translations)
                {
                    if (wordTranslated.word == word)
                        return true;
                    else continue;
                }
            }

            if (showError)
            {
                Console.WriteLine("Word not found...");
                PressAnyKeyPrompt();
            }

            return false;
        }

        /// <summary>
        /// Get the VocabularyWord object corresponding to the word passed by the user
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public VocabularyWord GetVocabularyWord(string word)
        {
            if (!IsWordInList(word, true))
                return null;

            foreach (VocabularyWord item in vocabList)
            {
                foreach (VocabWord wordTranslated in item.translations)
                {
                    if (wordTranslated.word == word)
                        return item;
                    else continue;
                }
            }

            return null;
        }

        /// <summary>
        /// Get the VocabularyWord object corresponding to the word passed by the user
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public VocabWord GetVocabWord(string word)
        {
            if (!IsWordInList(word, false))
                return new VocabWord();

            foreach (VocabularyWord item in vocabList)
            {
                foreach (VocabWord wordTranslated in item.translations)
                {
                    if (wordTranslated.word == word)
                        return wordTranslated;
                    else continue;
                }
            }

            return new VocabWord();
        }

        public void PressAnyKeyPrompt()
        {
            Console.WriteLine("--- Press any key to continue ---");
        }
        #endregion

        #region SET DICO DATAS
        public void AddLanguage(string newLanguage)
        {
            if (languagesSupported.Count != 0)
            {
                foreach (Languages languages in languagesSupported)
                {
                    if (languages.name == newLanguage)
                    {
                        Console.WriteLine("Language already set");
                        PressAnyKeyPrompt();
                        return;
                    }
                }
            }

            Languages lang = new Languages(newLanguage);

            languagesSupported.Add(lang);
            SaveDico();

            if (vocabList.Count != 0)
            {
                foreach (VocabularyWord word in vocabList)
                {
                    word.translations.Add(new VocabWord()
                    {
                        language = lang,
                        word = null
                    });
                    SaveVocab();
                }
            }

            PressAnyKeyPrompt();
        }

        public void RemoveLanguage(string languageToRemove)
        {
            foreach (Languages languages in languagesSupported)
            {
                if (languages.name == languageToRemove)
                {
                    languagesSupported.Remove(languages);
                    SaveDico();

                    foreach (VocabularyWord word in vocabList)
                    {
                        foreach (VocabWord item in word.translations)
                        {
                            if (item.language.name == languageToRemove)
                            {
                                word.translations.Remove(item);
                                SaveVocab();
                                return;
                            }
                        }
                    }

                    PressAnyKeyPrompt();
                    return;
                }
            }

            Console.WriteLine("Language unknown");
            PressAnyKeyPrompt();
        }

        public void RemoveEverything()
        {
            vocabList.Clear();
            languagesSupported.Clear();
            Console.WriteLine("Languages and words deleted successfully!");
            SaveDico();
            SaveVocab();
            PressAnyKeyPrompt();
        }
        #endregion

        #region ADD / REMOVE / EDIT
        public void AddToVocabularyList(VocabularyWord newWord)
        {
            foreach (VocabWord vWord in newWord.translations)
            {
                if (IsWordInList(vWord.word, false))
                {
                    Console.WriteLine("This word is already saved");
                    PressAnyKeyPrompt();
                    return;
                }
                else
                {
                    vocabList.Add(newWord);
                    SaveVocab();
                    Console.WriteLine("Word added successfully!");
                    PressAnyKeyPrompt();
                    break;
                }
            }
        }

        public void RemoveFromVocabularyList(string word)
        {
            vocabList.Remove(GetVocabularyWord(word));
            SaveVocab();
            Console.WriteLine("Word removed successfully!");
            PressAnyKeyPrompt();
        }

        public void RemoveAll()
        {
            vocabList.Clear();
            SaveVocab();
            PressAnyKeyPrompt();
        }

        public void EditVocabularyWord(VocabularyWord oldWord, VocabularyWord newWord)
        {
            vocabList[vocabList.IndexOf(oldWord)] = newWord;
            SaveVocab();
            Console.WriteLine("Word edited successfully!");
            PressAnyKeyPrompt();
        }
        #endregion

        #region SEARCH / STATS
        public VocabularyWord SearchFromWord(string wordToSearch)
        {
            return GetVocabularyWord(wordToSearch);
        }

        public List<string> SearchByLanguage(int langIndex)
        {
            List<string> wordList = new List<string>();

            foreach (VocabularyWord item in vocabList)
            {
                if (item.translations[langIndex].word != null)
                    wordList.Add(item.translations[langIndex].word);
            }

            return wordList;
        }

        public int SeeStats(int index)
        {
            switch (index)
            {
                case 0:
                    return vocabList.Count;
                case 1:
                    return languagesSupported.Count;
            }
            return 0;
        }
        #endregion

        #region SAVE / LOAD
        private void SaveVocab()
        {
            // Serialize it to JSON
            string json = JsonSerializer.Serialize(vocabList, options);

            // Save it to a file
            File.WriteAllText(SAVE_FILE_PATH_VOCAB, json);
        }

        public void LoadVocab()
        {
            string json;

            if (File.Exists(SAVE_FILE_PATH_VOCAB))
            {
                json = File.ReadAllText(SAVE_FILE_PATH_VOCAB);
                vocabList.Clear();
                vocabList = JsonSerializer.Deserialize<List<VocabularyWord>>(json, options)!;
            }
        }

        private void SaveDico()
        {
            // Serialize it to JSON
            string json = JsonSerializer.Serialize(languagesSupported, options);

            // Save it to a file
            File.WriteAllText(SAVE_FILE_PATH_DICO, json);
        }

        public void LoadDico()
        {
            string json;

            if (File.Exists(SAVE_FILE_PATH_DICO))
            {
                json = File.ReadAllText(SAVE_FILE_PATH_DICO);
                languagesSupported.Clear();
                languagesSupported = JsonSerializer.Deserialize<List<Languages>>(json, options)!;
            }
        }
        #endregion
    }

    public class Languages
    {
        public string name;

        public Languages(string name)
        {
            this.name = name;
        }
    }

    //public enum Languages
    //{
    //    None = 0,
    //    French = 1,
    //    English = 2,
    //    PortugueseBrazil = 3
    //}

    public struct VocabWord
    {
        public Languages language;
        public string word;
    }
}
