namespace Funda_assignment_Silvestrova.Configuration
{
    public interface IConfigurationHelper
    {
        string CombineListingsRequestUrl(string [] sortParams, int pageNumber, int pageSize);
    }
}
