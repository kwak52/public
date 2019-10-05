using System.Linq;

namespace Dsu.PLCConvertor.Common
{




    public class ILSentence
    {
        public string Command { get; private set; }
        public string[] Args { get; private set; }
        public ILSentence(string ilSentence)
        {
            var tokens = ilSentence.Split(new[] { ' ', '\t' });
            Command = tokens[0];
            Args = tokens.Skip(1).ToArray();
        }

        public ILSentence(ILSentence other)
        {
            Command = other.Command;
            Args = other.Args;
        }

        public override string ToString()
        {
            return $"{Command} {string.Join(" ", Args)}";
        }
    }
}
