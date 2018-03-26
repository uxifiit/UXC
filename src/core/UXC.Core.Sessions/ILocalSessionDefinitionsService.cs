namespace UXC.Sessions
{
    public interface ILocalSessionDefinitionsService
    {
        SessionDefinition LoadFromFile(string filepath);
    }
}
