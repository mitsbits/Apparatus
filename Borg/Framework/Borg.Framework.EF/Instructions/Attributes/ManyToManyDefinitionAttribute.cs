using System;

namespace Borg.Framework.EF.Instructions.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ManyToManyDefinitionAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class HasManyDefinitionAttribute : Attribute
    {
        public HasManyDefinitionAttribute(string foreighnKey)
        {
            ForeighnKeyColumnName = foreighnKey;
        }

        public string ForeighnKeyColumnName { get; }
    }
}