//HintName: OptsWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial record struct OptsWrap : Serde.IDeserialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS>
{
    static System.Runtime.InteropServices.ComTypes.BIND_OPTS Serde.IDeserialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "cbStruct",
            "dwTickCountDeadline",
            "grfFlags",
            "grfMode"
        };
        return deserializer.DeserializeType("BIND_OPTS", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<System.Runtime.InteropServices.ComTypes.BIND_OPTS>
    {
        public string ExpectedTypeName => "System.Runtime.InteropServices.ComTypes.BIND_OPTS";

        private struct FieldNameVisitor : Serde.IDeserialize<byte>, Serde.IDeserializeVisitor<byte>
        {
            public static byte Deserialize<D>(ref D deserializer)
                where D : IDeserializer => deserializer.DeserializeString(new FieldNameVisitor());
            public string ExpectedTypeName => "string";

            byte Serde.IDeserializeVisitor<byte>.VisitString(string s) => VisitUtf8Span(System.Text.Encoding.UTF8.GetBytes(s));
            public byte VisitUtf8Span(System.ReadOnlySpan<byte> s)
            {
                switch (s[0])
                {
                    case (byte)'c'when s.SequenceEqual("cbStruct"u8):
                        return 1;
                    case (byte)'d'when s.SequenceEqual("dwTickCountDeadline"u8):
                        return 2;
                    case (byte)'g'when s.SequenceEqual("grfFlags"u8):
                        return 3;
                    case (byte)'g'when s.SequenceEqual("grfMode"u8):
                        return 4;
                    default:
                        return 0;
                }
            }
        }

        System.Runtime.InteropServices.ComTypes.BIND_OPTS Serde.IDeserializeVisitor<System.Runtime.InteropServices.ComTypes.BIND_OPTS>.VisitDictionary<D>(ref D d)
        {
            int _l_cbstruct = default !;
            int _l_dwtickcountdeadline = default !;
            int _l_grfflags = default !;
            int _l_grfmode = default !;
            byte _r_assignedValid = 0b0;
            while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
            {
                switch (key)
                {
                    case 1:
                        _l_cbstruct = d.GetNextValue<int, Int32Wrap>();
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case 2:
                        _l_dwtickcountdeadline = d.GetNextValue<int, Int32Wrap>();
                        _r_assignedValid |= ((byte)1) << 1;
                        break;
                    case 3:
                        _l_grfflags = d.GetNextValue<int, Int32Wrap>();
                        _r_assignedValid |= ((byte)1) << 2;
                        break;
                    case 4:
                        _l_grfmode = d.GetNextValue<int, Int32Wrap>();
                        _r_assignedValid |= ((byte)1) << 3;
                        break;
                }
            }

            if (_r_assignedValid != 0b1111)
            {
                throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
            }

            var newType = new System.Runtime.InteropServices.ComTypes.BIND_OPTS()
            {
                cbStruct = _l_cbstruct,
                dwTickCountDeadline = _l_dwtickcountdeadline,
                grfFlags = _l_grfflags,
                grfMode = _l_grfmode,
            };
            return newType;
        }
    }
}