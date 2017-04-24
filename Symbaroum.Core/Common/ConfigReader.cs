namespace Symbaroum.Core.Common
{
    public class ConfigReader : IConfigReader
    {
        public string ConnectionString => @"Server=(localdb)\SymbLocalDb;Database=Symbaroum;Integrated Security=True;";
    }
}