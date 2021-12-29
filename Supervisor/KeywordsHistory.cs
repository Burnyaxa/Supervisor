using System;

#nullable disable

namespace Supervisor
{
    public partial class KeywordsHistory
    {
        public int Id { get; set; }
        public int KeywordId { get; set; }
        public string Data { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Keyword Keyword { get; set; }
    }
}
