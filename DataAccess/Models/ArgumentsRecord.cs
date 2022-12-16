
namespace DataAccess.Models
{
    public class ArgumentsRecord : EntityBase
    {
        public string? Arg1 { get; set; }
        public string? Arg2 { get; set; }

        public ArgumentsRecord() {}

        public ArgumentsRecord(string arg1, string arg2)
        {
            Arg1 = arg1;
            Arg2 = arg2;
        }

        public ArgumentsRecord(ArgumentsRecord record)
        {
            Id = record.Id;
            Arg1 = record.Arg1;
            Arg2 = record.Arg2;
        }
    }
}
