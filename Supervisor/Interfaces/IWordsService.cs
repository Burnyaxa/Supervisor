using System.Collections.Generic;

namespace Supervisor.Interfaces
{
    public interface IWordsService
    {
        IEnumerable<Keyword> GetWords();
    }
}