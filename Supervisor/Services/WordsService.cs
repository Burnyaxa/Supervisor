using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Supervisor.Interfaces;

namespace Supervisor.Services
{
    public class WordsService : IWordsService
    {
        private readonly DreamContext _context;

        public WordsService(DreamContext context)
        {
            _context = context;
        }
        public IEnumerable<Keyword> GetWords()
        {
            return _context.ObservedKeywords.Include(x => x.Keyword).Select(x => x.Keyword);
        }
    }
}