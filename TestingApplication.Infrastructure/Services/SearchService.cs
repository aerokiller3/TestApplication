using TestingApplication.Infrastructure.Base.Interface;
using TestingApplication.Infrastructure.Domains;
using TestingApplication.Infrastructure.Services.Interfaces;

namespace TestingApplication.Infrastructure.Services
{
    public class SearchService : ISearchService
    {
        private IRepository<Word> _repository;

        public SearchService(IRepository<Word> repository)
        {
            _repository = repository;
        }

        public string SearchWordsBySubstring(string subString)
        {
            subString = subString.ToLowerInvariant();
            var domains = _repository.Table.Where(x => x.Name.StartsWith(subString)).OrderByDescending(x => x.Quantity).ThenBy(x => x.Name);

            var words = domains.Take(5).Select(x => x.Name);

            return words.Count() > 0 ? $">   {string.Join("\n    ", words)}" : "Совпадений нет!";
        }
    }
}
