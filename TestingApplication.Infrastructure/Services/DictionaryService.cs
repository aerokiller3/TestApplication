using Autofac;
using System.Text.RegularExpressions;
using TestingApplication.Infrastructure.AutofacModule;
using TestingApplication.Infrastructure.Base.Interface;
using TestingApplication.Infrastructure.Domains;

namespace TestingApplication.Infrastructure.Services
{
    public class DictionaryService
    {
        private static IRepository<Word> _repository;

        private static void InitService()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<TestApplicationAutofacModuleBuilder>();
            var container = builder.Build();

            _repository= container.Resolve<IRepository<Word>>();
        }

        public static int CreateDictionary(string filePath)
        {
            var words = CreateDictionaryFromFile(filePath);

            if (_repository == null)
                InitService();

            _repository.DeleteAll();
            _repository.InsertRange(words);

            return words.Count();
        }

        public static int UpdateDictionary(string filePath)
        {
            var wordsFromFile = CreateDictionaryFromFile(filePath);
            var size = wordsFromFile.Count();

            if (_repository == null)
                InitService();

            var domains = _repository.Table.ToArray();

            var wordsForInsert = wordsFromFile.Select(x => x.Name).Except(domains.Select(x => x.Name));
            var wordsForUpdate = wordsFromFile.Where(x => !wordsForInsert.Contains(x.Name));

            var domainsForUpdate = new List<Word>();
            foreach (var word in wordsForUpdate)
            {
                var domain = domains.First(x => x.Name == word.Name);

                domain.Quantity += word.Quantity;
                domainsForUpdate.Add(domain);
            }

            _repository.UpdateRange(domainsForUpdate);
            _repository.InsertRange(wordsFromFile.Where(x => wordsForInsert.Contains(x.Name)));

            return size;
        }

        public static int ClearDictionary()
        {
            if (_repository == null)
                InitService();

            var size = _repository.Table.Count();
            _repository.DeleteAll();

            return size;
        }

        public static void CheckConnection()
        {
            if (_repository == null)
                InitService();

            _repository.Table.FirstOrDefault();
        }

        private static IEnumerable<Word> CreateDictionaryFromFile(string filePath)
        {
            var file = File.ReadAllText(filePath);

            Console.WriteLine("Обработка файла");

            file = new string(file.Where(x => char.IsLetter(x) || char.IsWhiteSpace(x)).ToArray()).ToLowerInvariant();

            var words = Regex.Split(file, @"\s+").Where(x => x.Length > 3 && x.Length <= 20);

            var dict = words.GroupBy(x => x).Where(x => x.Count() >= 3).ToDictionary(k => k.Key, v => v.Count());

            return dict.Select(x => new Word
            {
                Name = x.Key,
                Quantity = x.Value
            });
        }
    }
}
