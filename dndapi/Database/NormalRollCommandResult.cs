namespace Database
{
    public class NormalRollCommandResult : CommandResult
    {
        public NormalRollCommandResult()
        {
            Type = 2;
        }

        public int Result { get; set; }
    }
}
