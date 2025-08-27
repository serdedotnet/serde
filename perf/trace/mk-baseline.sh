#!/bin/sh
dotnet publish
objdump --disassemble=trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize \
    -M intel --no-addresses --no-show-raw-insn \
    ../../artifacts/publish/trace/release/trace \
    > Location_Deserialize.asm