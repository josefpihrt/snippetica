namespace Pihrtsoft.Records
{
    public class DocumentReaderSettings
    {
        public bool UseVariables { get; set; } = true;

        public char VariableOpenDelimiter { get; set; } = '{';

        public char VariableCloseDelimiter { get; set; } = '}';

        public void SetVariableDelimiter(char delimiter)
        {
            VariableOpenDelimiter = delimiter;
            VariableCloseDelimiter = delimiter;
        }
    }
}
