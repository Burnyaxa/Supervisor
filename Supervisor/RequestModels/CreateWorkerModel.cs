namespace Supervisor.RequestModels
{
    public class CreateWorkerModel
    {
        public int KeywordId { get; set; }
        public string Keyword { get; set; }
        public int Frequency { get; set; }
        public int NumberOfPages { get; set; }
    }
}