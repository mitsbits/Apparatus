namespace Borg.Infrastructure.Core.Reflection.Discovery
{
    //public class PlugableServicesAssemblyScanResult : AssemblyScanResult
    //{
    //    public PlugableServicesAssemblyScanResult(Assembly assembly, IEnumerable<Instruction> instructions) : base(assembly, true, new string[0])
    //    {
    //        Instructions = Preconditions.NotNull(instructions, nameof(instructions));
    //    }

    //    public PlugableServicesAssemblyScanResult(Assembly assembly, string[] errors) : base(assembly, false, errors)
    //    {
    //        Instructions = null;
    //    }

    //    public IEnumerable<Instruction> Instructions { get; }

    //    public struct Instruction
    //    {
    //        public Instruction(Type service, PlugableServiceAttribute[] attributes, Type[] implementedInterfaces)
    //        {
    //            Service = Preconditions.NotNull(service, nameof(service));
    //            Attributes = Preconditions.NotNull(attributes, nameof(attributes));
    //            ImplementedInterfaces = Preconditions.NotNull(implementedInterfaces, nameof(implementedInterfaces));
    //        }

    //        public Type Service { get; set; }
    //        public PlugableServiceAttribute[] Attributes { get; set; }
    //        public Type[] ImplementedInterfaces { get; set; }
    //    }

    //    public override string ToString()
    //    {
    //        dynamic graph;
    //        if (Success)
    //        {
    //            graph = new { Assembly = Assembly.FullName, Instructions = Instructions };
    //        }
    //        else
    //        {
    //             graph = new { Assembly = Assembly.FullName, Errors = Errors };
    //        }
    //        return new
    //    }
    //}
}