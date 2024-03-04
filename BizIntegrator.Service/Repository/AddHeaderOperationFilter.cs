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

            // Add the custom header to the Swagger request parameters
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = headerName,
                In = ParameterLocation.Header,
                Description = headerDescription,
                Required = false, // Set to true if the header is required
                Schema = new OpenApiSchema { Type = "string" } // Set the schema type accordingly
            });
        }
    }
}
