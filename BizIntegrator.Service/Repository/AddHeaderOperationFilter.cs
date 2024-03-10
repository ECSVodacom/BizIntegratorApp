using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace BizIntegrator.Service.Repository
{
    public class AddHeaderOperationFilter : IOperationFilter
    {
        private readonly string headerName;
        private readonly string headerDescription;

        public AddHeaderOperationFilter(string name, string description)
        {
            headerName = name;
            headerDescription = description;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = headerName,
                In = ParameterLocation.Header,
                Description = headerDescription,
                Required = false, 
                Schema = new OpenApiSchema { Type = "string" } 
            });
        }
    }
}
