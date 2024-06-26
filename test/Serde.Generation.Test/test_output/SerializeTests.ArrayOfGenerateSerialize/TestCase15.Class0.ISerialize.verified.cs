﻿//HintName: TestCase15.Class0.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class TestCase15
{
    partial class Class0 : Serde.ISerialize<TestCase15.Class0>
    {
        void ISerialize<TestCase15.Class0>.Serialize(TestCase15.Class0 value, ISerializer serializer)
        {
            var _l_typeInfo = Class0SerdeTypeInfo.TypeInfo;
            var type = serializer.SerializeType(_l_typeInfo);
            type.SerializeField<TestCase15.Class1[], Serde.ArrayWrap.SerializeImpl<TestCase15.Class1, IdWrap<TestCase15.Class1>>>(_l_typeInfo, 0, value.Field0);
            type.SerializeField<bool[], Serde.ArrayWrap.SerializeImpl<bool, BoolWrap>>(_l_typeInfo, 1, value.Field1);
            type.End();
        }
    }
}