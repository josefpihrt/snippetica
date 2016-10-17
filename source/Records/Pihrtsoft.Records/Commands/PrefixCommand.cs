namespace Pihrtsoft.Records.Commands
{
    internal class PrefixCommand : SetCommand
    {
        public PrefixCommand(string propertyName, string value)
            : base(propertyName, value)
        {
        }

        public override CommandKind Kind
        {
            get { return CommandKind.Prefix; }
        }

        public override void Execute(Record record)
        {
            record[PropertyName] = Value + record[PropertyName];
        }
    }
}
