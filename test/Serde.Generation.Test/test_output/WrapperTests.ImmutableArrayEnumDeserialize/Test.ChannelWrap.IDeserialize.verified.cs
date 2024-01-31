//HintName: Test.ChannelWrap.IDeserialize.cs

#nullable enable
using System;
using Serde;

namespace Test
{
    partial record struct ChannelWrap : Serde.IDeserialize<Test.Channel>
    {
        static Test.Channel Serde.IDeserialize<Test.Channel>.Deserialize(IDeserializer deserializer)
        {
            var visitor = new SerdeVisitor();
            return deserializer.DeserializeString(visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Test.Channel>
        {
            public string ExpectedTypeName => "Test.Channel";

            Test.Channel Serde.IDeserializeVisitor<Test.Channel>.VisitString(string s) => s switch
            {
                "a" => Test.Channel.A,
                "b" => Test.Channel.B,
                "c" => Test.Channel.C,
                _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + s)};
            Test.Channel Serde.IDeserializeVisitor<Test.Channel>.VisitUtf8Span(System.ReadOnlySpan<byte> s) => s switch
            {
                _ when System.MemoryExtensions.SequenceEqual(s, "a"u8) => Test.Channel.A,
                _ when System.MemoryExtensions.SequenceEqual(s, "b"u8) => Test.Channel.B,
                _ when System.MemoryExtensions.SequenceEqual(s, "c"u8) => Test.Channel.C,
                _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + System.Text.Encoding.UTF8.GetString(s))};
        }
    }
}