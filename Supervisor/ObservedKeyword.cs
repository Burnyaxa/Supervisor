using System;

#nullable disable

namespace Supervisor
{
    public partial class ObservedKeyword
    {
        public int KeywordId { get; set; }
        public DateTime? AddedAt { get; set; }

        public virtual Keyword Keyword { get; set; }
    }
}
