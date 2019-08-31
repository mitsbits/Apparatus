using Borg.Infrastructure.Core.Reflection.Discovery.ObjectGraph;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Test.Borg.Infrastructure.Core.Reflection
{
    public class ComplexTypeRecursorTests : TestBase
    {
        private ComplexTypeRecursor recursor;
        private readonly List<Type> ExistingTypes;

        public ComplexTypeRecursorTests(ITestOutputHelper output) : base(output)
        {
            ExistingTypes = new List<Type>
            {
                typeof(Root),
                typeof(ComplexProperty),
                typeof(ObjectGraph),
                typeof(SecondObjectGraph),
                typeof(ThirdObjectGraph),
                typeof(ValueOject),
                typeof(OpenGeneric<ObjectGraph, SecondObjectGraph>)
            };
        }

        [Fact]
        private void recursor_discovers_structs()
        {
            Should.NotThrow(() =>
            {
                using (recursor = new ComplexTypeRecursor(typeof(Root), null, _moqLoggerFactory.CreateLogger(nameof(ComplexTypeRecursorTests))))
                {
                    var results = recursor.Results();
                    var hit = results.FirstOrDefault(x => x.Type == typeof(ValueOject));
                    hit.ShouldNotBeNull();
                }
            });
        }

        [Fact]
        private void recursor_discovers_generic_types()
        {
            Should.NotThrow(() =>
            {
                using (recursor = new ComplexTypeRecursor(typeof(Root), null, _moqLoggerFactory.CreateLogger(nameof(ComplexTypeRecursorTests))))
                {
                    var results = recursor.Results();
                    var hit = results.FirstOrDefault(x => x.Type == typeof(OpenGeneric<ObjectGraph, SecondObjectGraph>));
                    hit.ShouldNotBeNull();
                }
            });
        }

        [Fact]
        private void recursor_discovers_all_required_types()
        {
            Should.NotThrow(() =>
            {
                using (recursor = new ComplexTypeRecursor(typeof(Root), null, _moqLoggerFactory.CreateLogger(nameof(ComplexTypeRecursorTests))))
                {
                    var results = recursor.Results();
                    foreach (var type in ExistingTypes)
                    {
                        results.Any(x => x.Type == type).ShouldBeTrue();
                    }
                }
            });
        }

        [Fact]
        private void recursor_discovers_only_complex_types()
        {
            Should.NotThrow(() =>
            {
                using (recursor = new ComplexTypeRecursor(typeof(Root), null, _moqLoggerFactory.CreateLogger(nameof(ComplexTypeRecursorTests))))
                {
                    var results = recursor.Results();
                    var whatsleft = results.Where(x => !ExistingTypes.Contains(x.Type)).ToList();
                    whatsleft.Count.ShouldBe(0);
                }
            });
        }

        [Fact]
        private void recursor_entities_property_has_all_required_types()
        {
            Should.NotThrow(() =>
            {
                using (recursor = new ComplexTypeRecursor(typeof(Root), null, _moqLoggerFactory.CreateLogger(nameof(ComplexTypeRecursorTests))))
                {
                    var results = recursor.Results();
                    results.Entities().Length.ShouldBe(ExistingTypes.Count);
                }
            });
        }
    }

    internal class Root
    {
        public ICollection<ComplexProperty> ComplexProperties { get; }
        public int Id { get; }
        public ObjectGraph ObjectGraph { get; }
        public OpenGeneric<ObjectGraph, SecondObjectGraph> OpenGeneric { get; }
    }

    internal class ComplexProperty
    {
        public decimal Price { get; }
        public Root Root { get; }
    }

    internal class ObjectGraph
    {
        public ComplexProperty ComplexProperty { get; set; }
        public ICollection<string> Strings { get; }
        public ICollection<SecondObjectGraph> ObjectGraphs { get; }
        public bool Bool { get; }
    }

    internal class SecondObjectGraph
    {
        public IList<ThirdObjectGraph> Items { get; }
        private Guid Guid { get; }
    }

    internal class ThirdObjectGraph
    {
        public ValueOject ValueOject { get; }
    }

    internal struct ValueOject
    {
        public int X { get; }
        public int Y { get; }
    }

    internal class OpenGeneric<T, W>
    {
        public Guid Id { get; }
    }
}