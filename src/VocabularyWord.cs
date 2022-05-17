using System.Collections.Generic;

namespace Omega_VocabularyConsole
{
    class VocabularyWord
    {
        public List<VocabWord> translations = new List<VocabWord>();
    }

    public struct VocabWord
    {
        public Languages language;
        public string word;
        public string def;
        public List<string> synonymes;
    }
}
