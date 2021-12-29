#nullable disable

namespace Supervisor
{
    public partial class UserKeyword
    {
        public int KeywordId { get; set; }
        public int UserId { get; set; }

        public virtual Keyword Keyword { get; set; }
        public virtual User User { get; set; }
    }
}
