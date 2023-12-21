using Microsoft.OpenApi.Models;
using PedalacomOfficial.Models.DTO;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PedalacomOfficial
{
    public class ExcludePropertiesSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(UpdateAddress))
            {
                //in questo modo dovremmo rimuovere questi due campi dallo swagger, speriam
                schema.Properties.Remove("Rowguid");
                schema.Properties.Remove("SomeDateTime");
            }
        }
    }
}
