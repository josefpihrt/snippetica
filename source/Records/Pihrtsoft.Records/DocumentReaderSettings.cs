namespace Pihrtsoft.Records
{
    public class DocumentReaderSettings
    {
        public bool UseVariables { get; set; }

        public char OpenVariableDelimiter { get; set; } = '{';

        public char CloseVariableDelimiter { get; set; } = '}';

        public void SetVariableDelimiter(char delimiter)
        {
            OpenVariableDelimiter = delimiter;
            CloseVariableDelimiter = delimiter;
        }
    }
}
