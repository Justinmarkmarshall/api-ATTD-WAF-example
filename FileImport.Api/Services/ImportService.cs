using System;
using System.IO;
using System.Threading.Tasks;
using FileImport.Api.Models;

namespace FileImport.Api.Services
{
    public class ImportService : IImportService
    {
        public async Task<Result> Import(string importRequestInputLocation, string importRequestOutputLocation)
        {
            try
            {
                var text = "";
                var readLocation = $"{Directory.GetCurrentDirectory()}";
                var entireReadLocation = $"{readLocation}{importRequestInputLocation}";
                using (var sr = new StreamReader(entireReadLocation))
                {
                    text = await sr.ReadToEndAsync();
                }

                // string docPath =
                //     Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var entireWriteLocation = $"{readLocation}{importRequestOutputLocation}";
                await using (StreamWriter outputFile = new StreamWriter(entireWriteLocation))
                {
                    await outputFile.WriteLineAsync(text);
                }

                return new Result() { Success = true };
            }
            catch (Exception ex)
            {
                return new Result() { Success = false, Message = ex.Message };
            }
        }
    }
}