using System;
using FileImport.Api.Contracts.Requests;
using FileImport.Api.Models;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace FileImport.Api.Mapping
{
    public static class Mapper
    {
        public static ImportRequestDto MapToImportRequest(this ImportRequest request)
        {
            return new ImportRequestDto()
            {
                InputLocation = request.InputLocation,
                OutputLocation = request.OutputLocation
            };
        }
    }
}