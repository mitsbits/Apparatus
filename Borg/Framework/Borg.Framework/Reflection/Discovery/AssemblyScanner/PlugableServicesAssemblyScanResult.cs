using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DI;
using Borg.Infrastructure.Core.Reflection.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Borg.Framework.Reflection.Discovery.AssemblyScanner
{
    public class PlugableServicesAssemblyScanResult : AssemblyScanResult
    {
        public PlugableServicesAssemblyScanResult(Assembly assembly, IEnumerable<Instruction> instructions) : base(assembly, true, new string[0])
        {
            Instructions = Preconditions.NotNull(instructions, nameof(instructions));
        }

        public PlugableServicesAssemblyScanResult(Assembly assembly, string[] errors) : base(assembly, false, errors)
        {
            Instructions = null;
        }

        public IEnumerable<Instruction> Instructions { get; }

        public override string ToString()
        {
            string graph = string.Empty;
            if (Success)
            {
                graph = $"Assembly : {Assembly.GetName().Name} - Instructions : [{string.Join(",", Instructions.Select(x => x.ToString()))}]";
            }
            else
            {
                graph = $"Assembly : {Assembly.GetName().Name} - Errors : [{string.Join(",", Errors)}]";
            }
            return graph;
        }

        public struct Instruction
        {
            public Instruction(Type service, PlugableServiceAttribute[] attributes, Type[] implementedInterfaces)
            {
                Service = Preconditions.NotNull(service, nameof(service));
                Attributes = Preconditions.NotNull(attributes, nameof(attributes));
                ImplementedInterfaces = Preconditions.NotNull(implementedInterfaces, nameof(implementedInterfaces));
            }

            public Type Service { get; set; }
            public PlugableServiceAttribute[] Attributes { get; set; }
            public Type[] ImplementedInterfaces { get; set; }

            public override string ToString()
            {
                return $"Instruction: Service : {Service.Name} - Contracts [{string.Join(",", ImplementedInterfaces.Select(x => x.Name))}]";
            }
        }
    }
}