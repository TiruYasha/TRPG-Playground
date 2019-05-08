namespace Domain.Domain.Commands
{
    public class DefaultCommand : Command
    {
        public DefaultCommand() : base(CommandType.Default, "")
        {
            
        }

        public override void Execute()
        {
        }
    }
}
