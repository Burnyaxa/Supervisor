using System;
using System.Collections.Generic;

#nullable disable

namespace Supervisor
{
    public partial class Keyword
    {
        public Keyword()
        {
            KeywordsHistories = new HashSet<KeywordsHistory>();
        }

        public int Id { get; set; }
        public string Keyword1 { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ObservedKeyword ObservedKeyword { get; set; }
        public virtual ICollection<KeywordsHistory> KeywordsHistories { get; set; }
    }
}
