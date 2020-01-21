using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunWithMvc.Models.Zoo
{
    public class EnumMetedataProvider : IDisplayMetadataProvider
    {
        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            var propertyAttributes = context.Attributes;
            var modelMetadata = context.DisplayMetadata;
            var propertyName = context.Key.Name;

            if (typeof(Enum).IsAssignableFrom(context.Key.ModelType))
            {
                modelMetadata.TemplateHint = "Enum";
            }

        }
    }
}
