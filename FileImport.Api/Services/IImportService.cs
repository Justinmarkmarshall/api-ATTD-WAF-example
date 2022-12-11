using System.Threading.Tasks;
using FileImport.Api.Models;

namespace FileImport.Api.Services
{
    public interface IImportService
    {
        Task<Result> Import(string importRequestInputLocation, string importRequestOutputLocation);
    }
}