namespace Borg.Infrastructure.Core.Reflection.Discovery.ObjectGraph
{
    public class ComplexTypeRecursorConfiguration
    {
        public bool ExcludeSimples { get; set; } = true;
        public bool ExcludeTuples { get; set; } = true;
        public bool ExcludeByAttribute { get; set; } = true;
    }
}