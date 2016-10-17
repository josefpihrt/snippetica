namespace Pihrtsoft.Records.Commands
{
    internal class AppendCommand : SetCommand
    {
        public AppendCommand(string propertyName, string value)
            : base(propertyName, value)
        {
        }

        public override CommandKind Kind
        {
            get { return CommandKind.Append; }
        }

        public override void Execute(Record record)
        {
            record[PropertyName] += Value;
        }
    }
}
