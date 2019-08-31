using System;

namespace Test.Borg
{
    public class TestData
    {
        private const string _objGuid = "89B302BC-25EE-4333-AF6A-75FBB75340EC";
        private const string _objText = "Hi there!";
        private const double _objNumeric = 42.5;

        private readonly ObjectToSerialize _obj;
        public readonly byte[] _objData;
        private readonly string _objDataString;

        public TestData()
        {
            _obj = new ObjectToSerialize() { Id = Guid.Parse(_objGuid), Numeric = _objNumeric, Textual = _objText };
            _objData = new byte[]
            {
                123,
                34,
                78,
                117,
                109,
                101,
                114,
                105,
                99,
                34,
                58,
                52,
                50,
                46,
                53,
                44,
                34,
                73,
                100,
                34,
                58,
                34,
                56,
                57,
                98,
                51,
                48,
                50,
                98,
                99,
                45,
                50,
                53,
                101,
                101,
                45,
                52,
                51,
                51,
                51,
                45,
                97,
                102,
                54,
                97,
                45,
                55,
                53,
                102,
                98,
                98,
                55,
                53,
                51,
                52,
                48,
                101,
                99,
                34,
                44,
                34,
                84,
                101,
                120,
                116,
                117,
                97,
                108,
                34,
                58,
                34,
                72,
                105,
                32,
                116,
                104,
                101,
                114,
                101,
                33,
                34,
                125
            };
            _objDataString = "{\"Numeric\":42.5,\"Id\":\"89b302bc-25ee-4333-af6a-75fbb75340ec\",\"Textual\":\"Hi there!\"}";
        }
    }

    internal class ObjectToSerialize
    {
        public double Numeric { get; set; }
        public Guid Id { get; set; }
        public string Textual { get; set; }
    }
}