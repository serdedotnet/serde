using System;

namespace Serde;

/// <summary>
/// Serialize and deserialize proxies for the built-in <see cref="System.ValueTuple"/> types.
/// Tuples are represented in the data model as <see cref="InfoKind.Tuple"/>, a fixed-length
/// heterogeneous sequence, and are encoded on self-describing formats as an array (e.g. JSON
/// <c>[item1, item2, ...]</c>).
/// </summary>
public static class TupleProxy
{
    public class Ser<T1, TProvider1>() : ISerializeProvider<ValueTuple<T1>>
        where TProvider1 : ISerializeProvider<T1>
    {
        public static ISerialize<ValueTuple<T1>> Instance { get; } = new Impl();

        private sealed class Impl : ISerialize<ValueTuple<T1>>
        {
            public ISerdeInfo SerdeInfo { get; } =
                global::Serde.SerdeInfo.MakeTuple([TProvider1.Instance.SerdeInfo]);

            private readonly ISerialize<T1> _ser1 = TProvider1.Instance;

            public void Serialize(ValueTuple<T1> value, ISerializer serializer)
            {
                var _l_info = SerdeInfo;
                var _l_type = serializer.WriteCollection(_l_info, 1);
                _ser1.SerializeAsField(_l_type, _l_info, 0, value.Item1);
                _l_type.End(_l_info);
            }
        }
    }

    public class De<T1, TProvider1>() : IDeserializeProvider<ValueTuple<T1>>
        where TProvider1 : IDeserializeProvider<T1>
    {
        public static IDeserialize<ValueTuple<T1>> Instance { get; } = new Impl();

        private sealed class Impl : IDeserialize<ValueTuple<T1>>
        {
            public ISerdeInfo SerdeInfo { get; } =
                global::Serde.SerdeInfo.MakeTuple([TProvider1.Instance.SerdeInfo]);

            private readonly IDeserialize<T1> _de1 = TProvider1.Instance;

            public ValueTuple<T1> Deserialize(IDeserializer deserializer)
            {
                var _l_info = SerdeInfo;
                var _l_type = deserializer.ReadType(_l_info);
                T1 _l_item1 = default!;
                byte _r_assigned = 0;
                while (true)
                {
                    var _l_index = _l_type.TryReadIndex(_l_info);
                    if (_l_index == ITypeDeserializer.EndOfType)
                    {
                        break;
                    }
                    switch (_l_index)
                    {
                        case 0:
                            _l_item1 = _de1.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 0);
                            break;
                        case ITypeDeserializer.IndexNotFound:
                            _l_type.SkipValue(_l_info, _l_index);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index);
                    }
                }
                if ((_r_assigned & 0b1) != 0b1)
                {
                    throw DeserializeException.UnassignedMember();
                }
                _l_type.End(_l_info);
                return new ValueTuple<T1>(_l_item1);
            }
        }
    }

    public class Ser<T1, T2, TProvider1, TProvider2>() : ISerializeProvider<(T1, T2)>
        where TProvider1 : ISerializeProvider<T1>
        where TProvider2 : ISerializeProvider<T2>
    {
        public static ISerialize<(T1, T2)> Instance { get; } = new Impl();

        private sealed class Impl : ISerialize<(T1, T2)>
        {
            public ISerdeInfo SerdeInfo { get; } =
                global::Serde.SerdeInfo.MakeTuple([
                    TProvider1.Instance.SerdeInfo,
                    TProvider2.Instance.SerdeInfo,
                ]);

            private readonly ISerialize<T1> _ser1 = TProvider1.Instance;
            private readonly ISerialize<T2> _ser2 = TProvider2.Instance;

            public void Serialize((T1, T2) value, ISerializer serializer)
            {
                var _l_info = SerdeInfo;
                var _l_type = serializer.WriteCollection(_l_info, 2);
                _ser1.SerializeAsField(_l_type, _l_info, 0, value.Item1);
                _ser2.SerializeAsField(_l_type, _l_info, 1, value.Item2);
                _l_type.End(_l_info);
            }
        }
    }

    public class De<T1, T2, TProvider1, TProvider2>() : IDeserializeProvider<(T1, T2)>
        where TProvider1 : IDeserializeProvider<T1>
        where TProvider2 : IDeserializeProvider<T2>
    {
        public static IDeserialize<(T1, T2)> Instance { get; } = new Impl();

        private sealed class Impl : IDeserialize<(T1, T2)>
        {
            public ISerdeInfo SerdeInfo { get; } =
                global::Serde.SerdeInfo.MakeTuple([
                    TProvider1.Instance.SerdeInfo,
                    TProvider2.Instance.SerdeInfo,
                ]);

            private readonly IDeserialize<T1> _de1 = TProvider1.Instance;
            private readonly IDeserialize<T2> _de2 = TProvider2.Instance;

            public (T1, T2) Deserialize(IDeserializer deserializer)
            {
                var _l_info = SerdeInfo;
                var _l_type = deserializer.ReadType(_l_info);
                T1 _l_item1 = default!;
                T2 _l_item2 = default!;
                byte _r_assigned = 0;
                while (true)
                {
                    var _l_index = _l_type.TryReadIndex(_l_info);
                    if (_l_index == ITypeDeserializer.EndOfType)
                    {
                        break;
                    }
                    switch (_l_index)
                    {
                        case 0:
                            _l_item1 = _de1.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 0);
                            break;
                        case 1:
                            _l_item2 = _de2.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 1);
                            break;
                        case ITypeDeserializer.IndexNotFound:
                            _l_type.SkipValue(_l_info, _l_index);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index);
                    }
                }
                if ((_r_assigned & 0b11) != 0b11)
                {
                    throw DeserializeException.UnassignedMember();
                }
                _l_type.End(_l_info);
                return (_l_item1, _l_item2);
            }
        }
    }

    public class Ser<T1, T2, T3, TProvider1, TProvider2, TProvider3>()
        : ISerializeProvider<(T1, T2, T3)>
        where TProvider1 : ISerializeProvider<T1>
        where TProvider2 : ISerializeProvider<T2>
        where TProvider3 : ISerializeProvider<T3>
    {
        public static ISerialize<(T1, T2, T3)> Instance { get; } = new Impl();

        private sealed class Impl : ISerialize<(T1, T2, T3)>
        {
            public ISerdeInfo SerdeInfo { get; } =
                global::Serde.SerdeInfo.MakeTuple([
                    TProvider1.Instance.SerdeInfo,
                    TProvider2.Instance.SerdeInfo,
                    TProvider3.Instance.SerdeInfo,
                ]);

            private readonly ISerialize<T1> _ser1 = TProvider1.Instance;
            private readonly ISerialize<T2> _ser2 = TProvider2.Instance;
            private readonly ISerialize<T3> _ser3 = TProvider3.Instance;

            public void Serialize((T1, T2, T3) value, ISerializer serializer)
            {
                var _l_info = SerdeInfo;
                var _l_type = serializer.WriteCollection(_l_info, 3);
                _ser1.SerializeAsField(_l_type, _l_info, 0, value.Item1);
                _ser2.SerializeAsField(_l_type, _l_info, 1, value.Item2);
                _ser3.SerializeAsField(_l_type, _l_info, 2, value.Item3);
                _l_type.End(_l_info);
            }
        }
    }

    public class De<T1, T2, T3, TProvider1, TProvider2, TProvider3>()
        : IDeserializeProvider<(T1, T2, T3)>
        where TProvider1 : IDeserializeProvider<T1>
        where TProvider2 : IDeserializeProvider<T2>
        where TProvider3 : IDeserializeProvider<T3>
    {
        public static IDeserialize<(T1, T2, T3)> Instance { get; } = new Impl();

        private sealed class Impl : IDeserialize<(T1, T2, T3)>
        {
            public ISerdeInfo SerdeInfo { get; } =
                global::Serde.SerdeInfo.MakeTuple([
                    TProvider1.Instance.SerdeInfo,
                    TProvider2.Instance.SerdeInfo,
                    TProvider3.Instance.SerdeInfo,
                ]);

            private readonly IDeserialize<T1> _de1 = TProvider1.Instance;
            private readonly IDeserialize<T2> _de2 = TProvider2.Instance;
            private readonly IDeserialize<T3> _de3 = TProvider3.Instance;

            public (T1, T2, T3) Deserialize(IDeserializer deserializer)
            {
                var _l_info = SerdeInfo;
                var _l_type = deserializer.ReadType(_l_info);
                T1 _l_item1 = default!;
                T2 _l_item2 = default!;
                T3 _l_item3 = default!;
                byte _r_assigned = 0;
                while (true)
                {
                    var _l_index = _l_type.TryReadIndex(_l_info);
                    if (_l_index == ITypeDeserializer.EndOfType)
                    {
                        break;
                    }
                    switch (_l_index)
                    {
                        case 0:
                            _l_item1 = _de1.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 0);
                            break;
                        case 1:
                            _l_item2 = _de2.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 1);
                            break;
                        case 2:
                            _l_item3 = _de3.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 2);
                            break;
                        case ITypeDeserializer.IndexNotFound:
                            _l_type.SkipValue(_l_info, _l_index);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index);
                    }
                }
                if ((_r_assigned & 0b111) != 0b111)
                {
                    throw DeserializeException.UnassignedMember();
                }
                _l_type.End(_l_info);
                return (_l_item1, _l_item2, _l_item3);
            }
        }
    }

    public class Ser<T1, T2, T3, T4, TProvider1, TProvider2, TProvider3, TProvider4>()
        : ISerializeProvider<(T1, T2, T3, T4)>
        where TProvider1 : ISerializeProvider<T1>
        where TProvider2 : ISerializeProvider<T2>
        where TProvider3 : ISerializeProvider<T3>
        where TProvider4 : ISerializeProvider<T4>
    {
        public static ISerialize<(T1, T2, T3, T4)> Instance { get; } = new Impl();

        private sealed class Impl : ISerialize<(T1, T2, T3, T4)>
        {
            public ISerdeInfo SerdeInfo { get; } =
                global::Serde.SerdeInfo.MakeTuple([
                    TProvider1.Instance.SerdeInfo,
                    TProvider2.Instance.SerdeInfo,
                    TProvider3.Instance.SerdeInfo,
                    TProvider4.Instance.SerdeInfo,
                ]);

            private readonly ISerialize<T1> _ser1 = TProvider1.Instance;
            private readonly ISerialize<T2> _ser2 = TProvider2.Instance;
            private readonly ISerialize<T3> _ser3 = TProvider3.Instance;
            private readonly ISerialize<T4> _ser4 = TProvider4.Instance;

            public void Serialize((T1, T2, T3, T4) value, ISerializer serializer)
            {
                var _l_info = SerdeInfo;
                var _l_type = serializer.WriteCollection(_l_info, 4);
                _ser1.SerializeAsField(_l_type, _l_info, 0, value.Item1);
                _ser2.SerializeAsField(_l_type, _l_info, 1, value.Item2);
                _ser3.SerializeAsField(_l_type, _l_info, 2, value.Item3);
                _ser4.SerializeAsField(_l_type, _l_info, 3, value.Item4);
                _l_type.End(_l_info);
            }
        }
    }

    public class De<T1, T2, T3, T4, TProvider1, TProvider2, TProvider3, TProvider4>()
        : IDeserializeProvider<(T1, T2, T3, T4)>
        where TProvider1 : IDeserializeProvider<T1>
        where TProvider2 : IDeserializeProvider<T2>
        where TProvider3 : IDeserializeProvider<T3>
        where TProvider4 : IDeserializeProvider<T4>
    {
        public static IDeserialize<(T1, T2, T3, T4)> Instance { get; } = new Impl();

        private sealed class Impl : IDeserialize<(T1, T2, T3, T4)>
        {
            public ISerdeInfo SerdeInfo { get; } =
                global::Serde.SerdeInfo.MakeTuple([
                    TProvider1.Instance.SerdeInfo,
                    TProvider2.Instance.SerdeInfo,
                    TProvider3.Instance.SerdeInfo,
                    TProvider4.Instance.SerdeInfo,
                ]);

            private readonly IDeserialize<T1> _de1 = TProvider1.Instance;
            private readonly IDeserialize<T2> _de2 = TProvider2.Instance;
            private readonly IDeserialize<T3> _de3 = TProvider3.Instance;
            private readonly IDeserialize<T4> _de4 = TProvider4.Instance;

            public (T1, T2, T3, T4) Deserialize(IDeserializer deserializer)
            {
                var _l_info = SerdeInfo;
                var _l_type = deserializer.ReadType(_l_info);
                T1 _l_item1 = default!;
                T2 _l_item2 = default!;
                T3 _l_item3 = default!;
                T4 _l_item4 = default!;
                byte _r_assigned = 0;
                while (true)
                {
                    var _l_index = _l_type.TryReadIndex(_l_info);
                    if (_l_index == ITypeDeserializer.EndOfType)
                    {
                        break;
                    }
                    switch (_l_index)
                    {
                        case 0:
                            _l_item1 = _de1.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 0);
                            break;
                        case 1:
                            _l_item2 = _de2.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 1);
                            break;
                        case 2:
                            _l_item3 = _de3.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 2);
                            break;
                        case 3:
                            _l_item4 = _de4.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 3);
                            break;
                        case ITypeDeserializer.IndexNotFound:
                            _l_type.SkipValue(_l_info, _l_index);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index);
                    }
                }
                if ((_r_assigned & 0b1111) != 0b1111)
                {
                    throw DeserializeException.UnassignedMember();
                }
                _l_type.End(_l_info);
                return (_l_item1, _l_item2, _l_item3, _l_item4);
            }
        }
    }

    public class Ser<
        T1,
        T2,
        T3,
        T4,
        T5,
        TProvider1,
        TProvider2,
        TProvider3,
        TProvider4,
        TProvider5
    >() : ISerializeProvider<(T1, T2, T3, T4, T5)>
        where TProvider1 : ISerializeProvider<T1>
        where TProvider2 : ISerializeProvider<T2>
        where TProvider3 : ISerializeProvider<T3>
        where TProvider4 : ISerializeProvider<T4>
        where TProvider5 : ISerializeProvider<T5>
    {
        public static ISerialize<(T1, T2, T3, T4, T5)> Instance { get; } = new Impl();

        private sealed class Impl : ISerialize<(T1, T2, T3, T4, T5)>
        {
            public ISerdeInfo SerdeInfo { get; } =
                global::Serde.SerdeInfo.MakeTuple([
                    TProvider1.Instance.SerdeInfo,
                    TProvider2.Instance.SerdeInfo,
                    TProvider3.Instance.SerdeInfo,
                    TProvider4.Instance.SerdeInfo,
                    TProvider5.Instance.SerdeInfo,
                ]);

            private readonly ISerialize<T1> _ser1 = TProvider1.Instance;
            private readonly ISerialize<T2> _ser2 = TProvider2.Instance;
            private readonly ISerialize<T3> _ser3 = TProvider3.Instance;
            private readonly ISerialize<T4> _ser4 = TProvider4.Instance;
            private readonly ISerialize<T5> _ser5 = TProvider5.Instance;

            public void Serialize((T1, T2, T3, T4, T5) value, ISerializer serializer)
            {
                var _l_info = SerdeInfo;
                var _l_type = serializer.WriteCollection(_l_info, 5);
                _ser1.SerializeAsField(_l_type, _l_info, 0, value.Item1);
                _ser2.SerializeAsField(_l_type, _l_info, 1, value.Item2);
                _ser3.SerializeAsField(_l_type, _l_info, 2, value.Item3);
                _ser4.SerializeAsField(_l_type, _l_info, 3, value.Item4);
                _ser5.SerializeAsField(_l_type, _l_info, 4, value.Item5);
                _l_type.End(_l_info);
            }
        }
    }

    public class De<
        T1,
        T2,
        T3,
        T4,
        T5,
        TProvider1,
        TProvider2,
        TProvider3,
        TProvider4,
        TProvider5
    >() : IDeserializeProvider<(T1, T2, T3, T4, T5)>
        where TProvider1 : IDeserializeProvider<T1>
        where TProvider2 : IDeserializeProvider<T2>
        where TProvider3 : IDeserializeProvider<T3>
        where TProvider4 : IDeserializeProvider<T4>
        where TProvider5 : IDeserializeProvider<T5>
    {
        public static IDeserialize<(T1, T2, T3, T4, T5)> Instance { get; } = new Impl();

        private sealed class Impl : IDeserialize<(T1, T2, T3, T4, T5)>
        {
            public ISerdeInfo SerdeInfo { get; } =
                global::Serde.SerdeInfo.MakeTuple([
                    TProvider1.Instance.SerdeInfo,
                    TProvider2.Instance.SerdeInfo,
                    TProvider3.Instance.SerdeInfo,
                    TProvider4.Instance.SerdeInfo,
                    TProvider5.Instance.SerdeInfo,
                ]);

            private readonly IDeserialize<T1> _de1 = TProvider1.Instance;
            private readonly IDeserialize<T2> _de2 = TProvider2.Instance;
            private readonly IDeserialize<T3> _de3 = TProvider3.Instance;
            private readonly IDeserialize<T4> _de4 = TProvider4.Instance;
            private readonly IDeserialize<T5> _de5 = TProvider5.Instance;

            public (T1, T2, T3, T4, T5) Deserialize(IDeserializer deserializer)
            {
                var _l_info = SerdeInfo;
                var _l_type = deserializer.ReadType(_l_info);
                T1 _l_item1 = default!;
                T2 _l_item2 = default!;
                T3 _l_item3 = default!;
                T4 _l_item4 = default!;
                T5 _l_item5 = default!;
                byte _r_assigned = 0;
                while (true)
                {
                    var _l_index = _l_type.TryReadIndex(_l_info);
                    if (_l_index == ITypeDeserializer.EndOfType)
                    {
                        break;
                    }
                    switch (_l_index)
                    {
                        case 0:
                            _l_item1 = _de1.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 0);
                            break;
                        case 1:
                            _l_item2 = _de2.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 1);
                            break;
                        case 2:
                            _l_item3 = _de3.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 2);
                            break;
                        case 3:
                            _l_item4 = _de4.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 3);
                            break;
                        case 4:
                            _l_item5 = _de5.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 4);
                            break;
                        case ITypeDeserializer.IndexNotFound:
                            _l_type.SkipValue(_l_info, _l_index);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index);
                    }
                }
                if ((_r_assigned & 0b11111) != 0b11111)
                {
                    throw DeserializeException.UnassignedMember();
                }
                _l_type.End(_l_info);
                return (_l_item1, _l_item2, _l_item3, _l_item4, _l_item5);
            }
        }
    }

    public class Ser<
        T1,
        T2,
        T3,
        T4,
        T5,
        T6,
        TProvider1,
        TProvider2,
        TProvider3,
        TProvider4,
        TProvider5,
        TProvider6
    >() : ISerializeProvider<(T1, T2, T3, T4, T5, T6)>
        where TProvider1 : ISerializeProvider<T1>
        where TProvider2 : ISerializeProvider<T2>
        where TProvider3 : ISerializeProvider<T3>
        where TProvider4 : ISerializeProvider<T4>
        where TProvider5 : ISerializeProvider<T5>
        where TProvider6 : ISerializeProvider<T6>
    {
        public static ISerialize<(T1, T2, T3, T4, T5, T6)> Instance { get; } = new Impl();

        private sealed class Impl : ISerialize<(T1, T2, T3, T4, T5, T6)>
        {
            public ISerdeInfo SerdeInfo { get; } =
                global::Serde.SerdeInfo.MakeTuple([
                    TProvider1.Instance.SerdeInfo,
                    TProvider2.Instance.SerdeInfo,
                    TProvider3.Instance.SerdeInfo,
                    TProvider4.Instance.SerdeInfo,
                    TProvider5.Instance.SerdeInfo,
                    TProvider6.Instance.SerdeInfo,
                ]);

            private readonly ISerialize<T1> _ser1 = TProvider1.Instance;
            private readonly ISerialize<T2> _ser2 = TProvider2.Instance;
            private readonly ISerialize<T3> _ser3 = TProvider3.Instance;
            private readonly ISerialize<T4> _ser4 = TProvider4.Instance;
            private readonly ISerialize<T5> _ser5 = TProvider5.Instance;
            private readonly ISerialize<T6> _ser6 = TProvider6.Instance;

            public void Serialize((T1, T2, T3, T4, T5, T6) value, ISerializer serializer)
            {
                var _l_info = SerdeInfo;
                var _l_type = serializer.WriteCollection(_l_info, 6);
                _ser1.SerializeAsField(_l_type, _l_info, 0, value.Item1);
                _ser2.SerializeAsField(_l_type, _l_info, 1, value.Item2);
                _ser3.SerializeAsField(_l_type, _l_info, 2, value.Item3);
                _ser4.SerializeAsField(_l_type, _l_info, 3, value.Item4);
                _ser5.SerializeAsField(_l_type, _l_info, 4, value.Item5);
                _ser6.SerializeAsField(_l_type, _l_info, 5, value.Item6);
                _l_type.End(_l_info);
            }
        }
    }

    public class De<
        T1,
        T2,
        T3,
        T4,
        T5,
        T6,
        TProvider1,
        TProvider2,
        TProvider3,
        TProvider4,
        TProvider5,
        TProvider6
    >() : IDeserializeProvider<(T1, T2, T3, T4, T5, T6)>
        where TProvider1 : IDeserializeProvider<T1>
        where TProvider2 : IDeserializeProvider<T2>
        where TProvider3 : IDeserializeProvider<T3>
        where TProvider4 : IDeserializeProvider<T4>
        where TProvider5 : IDeserializeProvider<T5>
        where TProvider6 : IDeserializeProvider<T6>
    {
        public static IDeserialize<(T1, T2, T3, T4, T5, T6)> Instance { get; } = new Impl();

        private sealed class Impl : IDeserialize<(T1, T2, T3, T4, T5, T6)>
        {
            public ISerdeInfo SerdeInfo { get; } =
                global::Serde.SerdeInfo.MakeTuple([
                    TProvider1.Instance.SerdeInfo,
                    TProvider2.Instance.SerdeInfo,
                    TProvider3.Instance.SerdeInfo,
                    TProvider4.Instance.SerdeInfo,
                    TProvider5.Instance.SerdeInfo,
                    TProvider6.Instance.SerdeInfo,
                ]);

            private readonly IDeserialize<T1> _de1 = TProvider1.Instance;
            private readonly IDeserialize<T2> _de2 = TProvider2.Instance;
            private readonly IDeserialize<T3> _de3 = TProvider3.Instance;
            private readonly IDeserialize<T4> _de4 = TProvider4.Instance;
            private readonly IDeserialize<T5> _de5 = TProvider5.Instance;
            private readonly IDeserialize<T6> _de6 = TProvider6.Instance;

            public (T1, T2, T3, T4, T5, T6) Deserialize(IDeserializer deserializer)
            {
                var _l_info = SerdeInfo;
                var _l_type = deserializer.ReadType(_l_info);
                T1 _l_item1 = default!;
                T2 _l_item2 = default!;
                T3 _l_item3 = default!;
                T4 _l_item4 = default!;
                T5 _l_item5 = default!;
                T6 _l_item6 = default!;
                byte _r_assigned = 0;
                while (true)
                {
                    var _l_index = _l_type.TryReadIndex(_l_info);
                    if (_l_index == ITypeDeserializer.EndOfType)
                    {
                        break;
                    }
                    switch (_l_index)
                    {
                        case 0:
                            _l_item1 = _de1.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 0);
                            break;
                        case 1:
                            _l_item2 = _de2.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 1);
                            break;
                        case 2:
                            _l_item3 = _de3.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 2);
                            break;
                        case 3:
                            _l_item4 = _de4.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 3);
                            break;
                        case 4:
                            _l_item5 = _de5.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 4);
                            break;
                        case 5:
                            _l_item6 = _de6.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 5);
                            break;
                        case ITypeDeserializer.IndexNotFound:
                            _l_type.SkipValue(_l_info, _l_index);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index);
                    }
                }
                if ((_r_assigned & 0b111111) != 0b111111)
                {
                    throw DeserializeException.UnassignedMember();
                }
                _l_type.End(_l_info);
                return (_l_item1, _l_item2, _l_item3, _l_item4, _l_item5, _l_item6);
            }
        }
    }

    public class Ser<
        T1,
        T2,
        T3,
        T4,
        T5,
        T6,
        T7,
        TProvider1,
        TProvider2,
        TProvider3,
        TProvider4,
        TProvider5,
        TProvider6,
        TProvider7
    >() : ISerializeProvider<(T1, T2, T3, T4, T5, T6, T7)>
        where TProvider1 : ISerializeProvider<T1>
        where TProvider2 : ISerializeProvider<T2>
        where TProvider3 : ISerializeProvider<T3>
        where TProvider4 : ISerializeProvider<T4>
        where TProvider5 : ISerializeProvider<T5>
        where TProvider6 : ISerializeProvider<T6>
        where TProvider7 : ISerializeProvider<T7>
    {
        public static ISerialize<(T1, T2, T3, T4, T5, T6, T7)> Instance { get; } = new Impl();

        private sealed class Impl : ISerialize<(T1, T2, T3, T4, T5, T6, T7)>
        {
            public ISerdeInfo SerdeInfo { get; } =
                global::Serde.SerdeInfo.MakeTuple([
                    TProvider1.Instance.SerdeInfo,
                    TProvider2.Instance.SerdeInfo,
                    TProvider3.Instance.SerdeInfo,
                    TProvider4.Instance.SerdeInfo,
                    TProvider5.Instance.SerdeInfo,
                    TProvider6.Instance.SerdeInfo,
                    TProvider7.Instance.SerdeInfo,
                ]);

            private readonly ISerialize<T1> _ser1 = TProvider1.Instance;
            private readonly ISerialize<T2> _ser2 = TProvider2.Instance;
            private readonly ISerialize<T3> _ser3 = TProvider3.Instance;
            private readonly ISerialize<T4> _ser4 = TProvider4.Instance;
            private readonly ISerialize<T5> _ser5 = TProvider5.Instance;
            private readonly ISerialize<T6> _ser6 = TProvider6.Instance;
            private readonly ISerialize<T7> _ser7 = TProvider7.Instance;

            public void Serialize((T1, T2, T3, T4, T5, T6, T7) value, ISerializer serializer)
            {
                var _l_info = SerdeInfo;
                var _l_type = serializer.WriteCollection(_l_info, 7);
                _ser1.SerializeAsField(_l_type, _l_info, 0, value.Item1);
                _ser2.SerializeAsField(_l_type, _l_info, 1, value.Item2);
                _ser3.SerializeAsField(_l_type, _l_info, 2, value.Item3);
                _ser4.SerializeAsField(_l_type, _l_info, 3, value.Item4);
                _ser5.SerializeAsField(_l_type, _l_info, 4, value.Item5);
                _ser6.SerializeAsField(_l_type, _l_info, 5, value.Item6);
                _ser7.SerializeAsField(_l_type, _l_info, 6, value.Item7);
                _l_type.End(_l_info);
            }
        }
    }

    public class De<
        T1,
        T2,
        T3,
        T4,
        T5,
        T6,
        T7,
        TProvider1,
        TProvider2,
        TProvider3,
        TProvider4,
        TProvider5,
        TProvider6,
        TProvider7
    >() : IDeserializeProvider<(T1, T2, T3, T4, T5, T6, T7)>
        where TProvider1 : IDeserializeProvider<T1>
        where TProvider2 : IDeserializeProvider<T2>
        where TProvider3 : IDeserializeProvider<T3>
        where TProvider4 : IDeserializeProvider<T4>
        where TProvider5 : IDeserializeProvider<T5>
        where TProvider6 : IDeserializeProvider<T6>
        where TProvider7 : IDeserializeProvider<T7>
    {
        public static IDeserialize<(T1, T2, T3, T4, T5, T6, T7)> Instance { get; } = new Impl();

        private sealed class Impl : IDeserialize<(T1, T2, T3, T4, T5, T6, T7)>
        {
            public ISerdeInfo SerdeInfo { get; } =
                global::Serde.SerdeInfo.MakeTuple([
                    TProvider1.Instance.SerdeInfo,
                    TProvider2.Instance.SerdeInfo,
                    TProvider3.Instance.SerdeInfo,
                    TProvider4.Instance.SerdeInfo,
                    TProvider5.Instance.SerdeInfo,
                    TProvider6.Instance.SerdeInfo,
                    TProvider7.Instance.SerdeInfo,
                ]);

            private readonly IDeserialize<T1> _de1 = TProvider1.Instance;
            private readonly IDeserialize<T2> _de2 = TProvider2.Instance;
            private readonly IDeserialize<T3> _de3 = TProvider3.Instance;
            private readonly IDeserialize<T4> _de4 = TProvider4.Instance;
            private readonly IDeserialize<T5> _de5 = TProvider5.Instance;
            private readonly IDeserialize<T6> _de6 = TProvider6.Instance;
            private readonly IDeserialize<T7> _de7 = TProvider7.Instance;

            public (T1, T2, T3, T4, T5, T6, T7) Deserialize(IDeserializer deserializer)
            {
                var _l_info = SerdeInfo;
                var _l_type = deserializer.ReadType(_l_info);
                T1 _l_item1 = default!;
                T2 _l_item2 = default!;
                T3 _l_item3 = default!;
                T4 _l_item4 = default!;
                T5 _l_item5 = default!;
                T6 _l_item6 = default!;
                T7 _l_item7 = default!;
                byte _r_assigned = 0;
                while (true)
                {
                    var _l_index = _l_type.TryReadIndex(_l_info);
                    if (_l_index == ITypeDeserializer.EndOfType)
                    {
                        break;
                    }
                    switch (_l_index)
                    {
                        case 0:
                            _l_item1 = _de1.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 0);
                            break;
                        case 1:
                            _l_item2 = _de2.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 1);
                            break;
                        case 2:
                            _l_item3 = _de3.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 2);
                            break;
                        case 3:
                            _l_item4 = _de4.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 3);
                            break;
                        case 4:
                            _l_item5 = _de5.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 4);
                            break;
                        case 5:
                            _l_item6 = _de6.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 5);
                            break;
                        case 6:
                            _l_item7 = _de7.DeserializeAsField(_l_type, _l_info, _l_index);
                            _r_assigned |= (byte)(1 << 6);
                            break;
                        case ITypeDeserializer.IndexNotFound:
                            _l_type.SkipValue(_l_info, _l_index);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index);
                    }
                }
                if ((_r_assigned & 0b1111111) != 0b1111111)
                {
                    throw DeserializeException.UnassignedMember();
                }
                _l_type.End(_l_info);
                return (_l_item1, _l_item2, _l_item3, _l_item4, _l_item5, _l_item6, _l_item7);
            }
        }
    }
}
