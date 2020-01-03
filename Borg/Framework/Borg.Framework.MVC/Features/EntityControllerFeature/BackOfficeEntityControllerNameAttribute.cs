using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using Borg;

namespace Borg.Framework.MVC.Features.EntityControllerFeature
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class BackOfficeEntityControllerNameAttribute : Attribute, IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.IsSubclassOfRawGeneric(typeof(BackOfficeEntityController<>)))
            {
                var entityType = controller.ControllerType.GenericTypeArguments[0];
                controller.ControllerName = entityType.Name;
            }
        }
    }
}