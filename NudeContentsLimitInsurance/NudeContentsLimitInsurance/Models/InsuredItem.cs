namespace NudeContentsLimitInsurance.Models
{
    public class InsuredItem
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? Value { get; set; }

        public Category? Category { get; set; }
    }
}