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
            if (typeof(Enum).IsAssignableFrom(context.Key.ModelType))
            {
                context.DisplayMetadata.TemplateHint = "Enum";
            }
        }
    }
}
