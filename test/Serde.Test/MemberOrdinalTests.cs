using Serde.Json;
using Xunit;

namespace Serde.Test;

public sealed partial class MemberOrdinalTests
{
    [GenerateSerde]
    [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
    public partial record Reordered
    {
        [SerdeMemberOptions(Ordinal = 2)]
        public int A { get; init; }

        [SerdeMemberOptions(Ordinal = 0)]
        public int B { get; init; }

        [SerdeMemberOptions(Ordinal = 1)]
        public int C { get; init; }
    }

    [Fact]
    public void ExplicitOrdinalControlsSerdeInfoOrder()
    {
        var info = SerdeInfoProvider.GetSerializeInfo<Reordered>();
        Assert.Equal(3, info.FieldCount);
        Assert.True(info.HasExplicitFieldOrdinals);
        Assert.Equal("B", info.GetFieldStringName(0));
        Assert.Equal("C", info.GetFieldStringName(1));
        Assert.Equal("A", info.GetFieldStringName(2));

        // Contiguous ordinals: the logical ordinal matches the physical position.
        Assert.Equal(0, info.GetFieldOrdinal(0));
        Assert.Equal(1, info.GetFieldOrdinal(1));
        Assert.Equal(2, info.GetFieldOrdinal(2));
    }

    [Fact]
    public void ExplicitOrdinalControlsSerializationOrder()
    {
        var value = new Reordered { A = 1, B = 2, C = 3 };
        var json = JsonSerializer.Serialize(value);
        Assert.Equal("""{"B":2,"C":3,"A":1}""", json);

        var back = JsonSerializer.Deserialize<Reordered>(json);
        Assert.Equal(value, back);
    }

    [GenerateSerde]
    [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
    public partial record FullyOrdered
    {
        [SerdeMemberOptions(Ordinal = 3)]
        public int A { get; init; }

        [SerdeMemberOptions(Ordinal = 0)]
        public int B { get; init; }

        [SerdeMemberOptions(Ordinal = 2)]
        public int C { get; init; }

        [SerdeMemberOptions(Ordinal = 1)]
        public int D { get; init; }
    }

    [Fact]
    public void FullyOrderedTypeReordersByOrdinal()
    {
        var info = SerdeInfoProvider.GetSerializeInfo<FullyOrdered>();
        Assert.Equal("B", info.GetFieldStringName(0));
        Assert.Equal("D", info.GetFieldStringName(1));
        Assert.Equal("C", info.GetFieldStringName(2));
        Assert.Equal("A", info.GetFieldStringName(3));

        var value = new FullyOrdered { A = 1, B = 2, C = 3, D = 4 };
        var json = JsonSerializer.Serialize(value);
        Assert.Equal("""{"B":2,"D":4,"C":3,"A":1}""", json);

        var back = JsonSerializer.Deserialize<FullyOrdered>(json);
        Assert.Equal(value, back);
    }

    [GenerateSerde]
    [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
    public partial record SparseOrdinals
    {
        [SerdeMemberOptions(Ordinal = 5)]
        public int A { get; init; }

        [SerdeMemberOptions(Ordinal = 0)]
        public int B { get; init; }

        [SerdeMemberOptions(Ordinal = 2)]
        public int C { get; init; }
    }

    [Fact]
    public void SparseOrdinalsArePreservedAsHoles()
    {
        // Ordinals 0, 2, 5 leave holes at 1, 3, 4. Physical packing stays dense (FieldCount == 3,
        // ordered ascending by ordinal), but the logical ordinal of each field is preserved.
        var info = SerdeInfoProvider.GetSerializeInfo<SparseOrdinals>();
        Assert.Equal(3, info.FieldCount);
        Assert.True(info.HasExplicitFieldOrdinals);

        Assert.Equal("B", info.GetFieldStringName(0));
        Assert.Equal("C", info.GetFieldStringName(1));
        Assert.Equal("A", info.GetFieldStringName(2));

        Assert.Equal(0, info.GetFieldOrdinal(0));
        Assert.Equal(2, info.GetFieldOrdinal(1));
        Assert.Equal(5, info.GetFieldOrdinal(2));

        // JSON ignores ordinals (it keys by name), so holes are irrelevant to the wire form.
        var value = new SparseOrdinals { A = 1, B = 2, C = 3 };
        var json = JsonSerializer.Serialize(value);
        Assert.Equal("""{"B":2,"C":3,"A":1}""", json);

        var back = JsonSerializer.Deserialize<SparseOrdinals>(json);
        Assert.Equal(value, back);
    }

    [GenerateSerde]
    [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
    public partial record NoOrdinals
    {
        public int A { get; init; }
        public int B { get; init; }
    }

    [Fact]
    public void DefaultOrdinalEqualsPhysicalPosition()
    {
        var info = SerdeInfoProvider.GetSerializeInfo<NoOrdinals>();
        Assert.Equal(2, info.FieldCount);
        Assert.False(info.HasExplicitFieldOrdinals);
        Assert.Equal(0, info.GetFieldOrdinal(0));
        Assert.Equal(1, info.GetFieldOrdinal(1));
    }

    [GenerateSerde]
    [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
    public partial record OrderedWithSkip
    {
        [SerdeMemberOptions(Ordinal = 1)]
        public int A { get; init; }

        [SerdeMemberOptions(Skip = true)]
        public int Ignored { get; init; }

        [SerdeMemberOptions(Ordinal = 0)]
        public int B { get; init; }
    }

    [Fact]
    public void SkippedMemberIsExcludedFromOrdering()
    {
        // A skipped member is removed before ordinal validation, so it does not participate in the
        // all-or-nothing requirement and leaves no hole of its own.
        var info = SerdeInfoProvider.GetSerializeInfo<OrderedWithSkip>();
        Assert.Equal(2, info.FieldCount);
        Assert.Equal("B", info.GetFieldStringName(0));
        Assert.Equal("A", info.GetFieldStringName(1));

        var value = new OrderedWithSkip { A = 1, Ignored = 99, B = 2 };
        var json = JsonSerializer.Serialize(value);
        Assert.Equal("""{"B":2,"A":1}""", json);

        var back = JsonSerializer.Deserialize<OrderedWithSkip>(json);
        Assert.Equal(1, back.A);
        Assert.Equal(2, back.B);
        Assert.Equal(0, back.Ignored);
    }
}
