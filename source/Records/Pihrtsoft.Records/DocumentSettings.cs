using System.Xml.Linq;

namespace Pihrtsoft.Records
{
    public class DocumentSettings
    {
        public bool UseVariables { get; set; }

        public char OpenVariableDelimiter { get; set; } = '{';

        public char CloseVariableDelimiter { get; set; } = '}';

        public bool SetLineInfo { get; set; } = true;

        internal LoadOptions LoadOptions
        {
            get
            {
                if (SetLineInfo)
                {
                    return LoadOptions.SetLineInfo;
                }
                else
                {
                    return LoadOptions.None;
                }
            }
        }

        public void SetVariableDelimiter(char delimiter)
        {
            OpenVariableDelimiter = delimiter;
            CloseVariableDelimiter = delimiter;
        }
    }
}
