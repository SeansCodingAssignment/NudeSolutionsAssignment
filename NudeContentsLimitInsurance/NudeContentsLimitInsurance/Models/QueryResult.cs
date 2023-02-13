namespace NudeContentsLimitInsurance.Models
{
    public class QueryResult
    {
        public QueryResult(bool successfullyExecuted, string message)
        {
            this.SuccessfullyExecuted = successfullyExecuted;
            this.Message = message;
        }

        public bool SuccessfullyExecuted { get; }

        public string Message { get; }
    }
}
