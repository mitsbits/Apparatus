using FunWithMvc.Models.Zoo;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FunWithMvc.Models.Zoo
{
    public class CustomDisplayMetadataProvider : IDisplayMetadataProvider
    {
        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            if (ShouldBind(context))
            {
                var propertyAttributes = context.Attributes;
                var modelMetadata = context.DisplayMetadata;
                var propertyName = context.Key.Name;


            }
        }

        private static bool ShouldBind(DisplayMetadataProviderContext context)
        {
            return typeof(DataRecord).IsAssignableFrom(context.Key.ModelType) || typeof(DataRecord).IsAssignableFrom(context.Key.ContainerType);
        }
    }

    public class CustomMetadataProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return null;
        }
    }
}

