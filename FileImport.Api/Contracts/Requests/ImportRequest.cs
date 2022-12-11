namespace FileImport.Api.Contracts.Requests
{
    public class ImportRequest
    {
        public string InputLocation { get; set; }
        
        public string OutputLocation { get; set; }
    }
}