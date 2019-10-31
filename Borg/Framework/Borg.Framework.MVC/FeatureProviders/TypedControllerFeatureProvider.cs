﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Borg.Framework.MVC.FeatureProviders
{
    public class TypedControllerFeatureProvider<TController> : ControllerFeatureProvider where TController : ControllerBase
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            if (!typeof(TController).GetTypeInfo().IsAssignableFrom(typeInfo)) return false;
            return base.IsController(typeInfo);
        }
    }
}
