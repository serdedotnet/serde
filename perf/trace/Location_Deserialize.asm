
../../artifacts/publish/trace/release/trace:     file format elf64-x86-64


Disassembly of section .init:

Disassembly of section .plt:

Disassembly of section .plt.got:

Disassembly of section .text:

Disassembly of section __managedcode:

<trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize>:
	push   rbp
	push   r15
	push   r14
	push   r13
	push   r12
	push   rbx
	sub    rsp,0x168
	lea    rbp,[rsp+0x190]
	xorps  xmm8,xmm8
	movaps XMMWORD PTR [rbp-0xb0],xmm8
	movaps XMMWORD PTR [rbp-0xa0],xmm8
	movabs rax,0xffffffffffffffa0
	movaps XMMWORD PTR [rbp+rax*1-0x30],xmm8
	movaps XMMWORD PTR [rbp+rax*1-0x20],xmm8
	movaps XMMWORD PTR [rbp+rax*1-0x10],xmm8
	add    rax,0x30
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x37>
	xor    ebx,ebx
	xor    r15d,r15d
	xor    r14d,r14d
	xor    r13d,r13d
	xor    r12d,r12d
	xor    eax,eax
	mov    QWORD PTR [rbp-0xc0],rax
	xor    ecx,ecx
	mov    QWORD PTR [rbp-0xc8],rcx
	xor    edx,edx
	mov    QWORD PTR [rbp-0xd0],rdx
	xor    r8d,r8d
	mov    QWORD PTR [rbp-0xd8],r8
	xor    r9d,r9d
	mov    DWORD PTR [rbp-0x2c],r9d
	mov    r10,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0xe0],r10
	mov    rdi,rsi
	mov    rsi,r10
	cmp    DWORD PTR [rdi],edi
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadType>
	mov    QWORD PTR [rbp-0xe8],rax
	mov    rcx,QWORD PTR [rax]
	mov    QWORD PTR [rbp-0xb8],rcx
	lea    rdi,[rip+0x1c57af]        # <_ZTV63Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>>
	cmp    rcx,rdi
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xd1>
	mov    rdi,rax
	mov    rsi,QWORD PTR [rbp-0xe0]
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__TryReadIndexWithName>
	mov    eax,edx
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xe2>
	mov    rdi,rax
	mov    rsi,QWORD PTR [rbp-0xe0]
	call   <Serde_Serde_Json_JsonDeserializer_1_DeCollection<Serde_Serde_IO_ArrayReader>__TryReadIndexWithName>
	mov    eax,edx
	mov    DWORD PTR [rbp-0x30],eax
	cmp    eax,0xffffffff
	je     <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xa4b>
	lea    ecx,[rax+0x2]
	cmp    ecx,0xa
	ja     <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xbd2>
	mov    edi,ecx
	lea    rcx,[rip+0xb2995]        # <__readonlydata_trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize>
	mov    ecx,DWORD PTR [rcx+rdi*4]
	lea    rdx,[rip+0xffffffffffffff42]        # <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x4f>
	add    rcx,rdx
	jmp    rcx
	lea    rdi,[rip+0x1c574f]        # <_ZTV63Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>>
	cmp    QWORD PTR [rbp-0xb8],rdi
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x146>
	mov    rdi,QWORD PTR [rbp-0xe8]
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadColon>
	mov    rdi,QWORD PTR [rbp-0xe8]
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadI64>
	mov    rdi,rax
	call   <S_P_CoreLib_System_Convert__ToInt32_9>
	mov    ebx,eax
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x166>
	mov    rbx,QWORD PTR [rbp-0xe8]
	mov    rdi,QWORD PTR [rbx+0x8]
	cmp    BYTE PTR [rdi],dil
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadI64>
	mov    rdi,rax
	call   <S_P_CoreLib_System_Convert__ToInt32_9>
	inc    DWORD PTR [rbx+0x10]
	mov    ebx,eax
	mov    edi,DWORD PTR [rbp-0x2c]
	or     edi,0x1
	movzx  edi,di
	mov    DWORD PTR [rbp-0x2c],edi
	mov    rax,QWORD PTR [rbp-0xe8]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xa8>
	lea    rdi,[rip+0x1c56e3]        # <_ZTV63Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>>
	cmp    QWORD PTR [rbp-0xb8],rdi
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x1ed>
	mov    rdi,QWORD PTR [rbp-0xe8]
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadColon>
	lea    rdi,[rip+0x1aa99f]        # <__NONGCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	cmp    QWORD PTR [rdi-0x8],0x0
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xafa>
	mov    rdi,QWORD PTR [rip+0x17669d]        # <__GCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	mov    r15,QWORD PTR [rdi+0x8]
	mov    rdi,QWORD PTR [rbp-0xe8]
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadUtf8Span>
	cmp    BYTE PTR [r15],r15b
	test   edx,edx
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x1d1>
	mov    edi,0x1
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x1d4>
	mov    rdi,rax
	mov    QWORD PTR [rbp-0x38],rdi
	mov    esi,edx
	mov    rdx,r15
	call   <String__CreateStringFromEncoding>
	xor    edi,edi
	mov    QWORD PTR [rbp-0x38],rdi
	mov    r15,rax
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x262>
	mov    r15,QWORD PTR [rbp-0xe8]
	mov    rdi,r15
	mov    rcx,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0xf8],rcx
	cmp    BYTE PTR [rcx],cl
	lea    rdi,[rip+0x1aa935]        # <__NONGCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	cmp    QWORD PTR [rdi-0x8],0x0
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xb04>
	mov    rdi,QWORD PTR [rip+0x176633]        # <__GCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	mov    rdx,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x100],rdx
	mov    rdi,rcx
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadUtf8Span>
	mov    rcx,QWORD PTR [rbp-0x100]
	cmp    BYTE PTR [rcx],cl
	test   edx,edx
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x244>
	mov    edi,0x1
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x247>
	mov    rdi,rax
	mov    QWORD PTR [rbp-0x40],rdi
	mov    esi,edx
	mov    rdx,rcx
	call   <String__CreateStringFromEncoding>
	xor    edi,edi
	mov    QWORD PTR [rbp-0x40],rdi
	inc    DWORD PTR [r15+0x10]
	mov    r15,rax
	mov    eax,DWORD PTR [rbp-0x2c]
	or     eax,0x2
	movzx  eax,ax
	mov    DWORD PTR [rbp-0x2c],eax
	mov    rax,QWORD PTR [rbp-0xe8]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xa8>
	lea    rdi,[rip+0x1c55e7]        # <_ZTV63Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>>
	cmp    QWORD PTR [rbp-0xb8],rdi
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x2e9>
	mov    rdi,QWORD PTR [rbp-0xe8]
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadColon>
	lea    rdi,[rip+0x1aa8a3]        # <__NONGCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	cmp    QWORD PTR [rdi-0x8],0x0
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xb15>
	mov    rdi,QWORD PTR [rip+0x1765a1]        # <__GCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	mov    r14,QWORD PTR [rdi+0x8]
	mov    rdi,QWORD PTR [rbp-0xe8]
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadUtf8Span>
	cmp    BYTE PTR [r14],r14b
	test   edx,edx
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x2cd>
	mov    edi,0x1
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x2d0>
	mov    rdi,rax
	mov    QWORD PTR [rbp-0x48],rdi
	mov    esi,edx
	mov    rdx,r14
	call   <String__CreateStringFromEncoding>
	xor    edi,edi
	mov    QWORD PTR [rbp-0x48],rdi
	mov    r14,rax
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x35e>
	mov    r14,QWORD PTR [rbp-0xe8]
	mov    rdi,r14
	mov    rcx,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x108],rcx
	cmp    BYTE PTR [rcx],cl
	lea    rdi,[rip+0x1aa839]        # <__NONGCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	cmp    QWORD PTR [rdi-0x8],0x0
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xb1f>
	mov    rdi,QWORD PTR [rip+0x176537]        # <__GCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	mov    rdx,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x110],rdx
	mov    rdi,rcx
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadUtf8Span>
	mov    rcx,QWORD PTR [rbp-0x110]
	cmp    BYTE PTR [rcx],cl
	test   edx,edx
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x340>
	mov    edi,0x1
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x343>
	mov    rdi,rax
	mov    QWORD PTR [rbp-0x50],rdi
	mov    esi,edx
	mov    rdx,rcx
	call   <String__CreateStringFromEncoding>
	xor    edi,edi
	mov    QWORD PTR [rbp-0x50],rdi
	inc    DWORD PTR [r14+0x10]
	mov    r14,rax
	mov    eax,DWORD PTR [rbp-0x2c]
	or     eax,0x4
	movzx  eax,ax
	mov    DWORD PTR [rbp-0x2c],eax
	mov    rax,QWORD PTR [rbp-0xe8]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xa8>
	lea    rdi,[rip+0x1c54eb]        # <_ZTV63Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>>
	cmp    QWORD PTR [rbp-0xb8],rdi
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x3e6>
	mov    rdi,QWORD PTR [rbp-0xe8]
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadColon>
	lea    rdi,[rip+0x1aa7a7]        # <__NONGCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	cmp    QWORD PTR [rdi-0x8],0x0
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xb30>
	mov    rdi,QWORD PTR [rip+0x1764a5]        # <__GCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	mov    r13,QWORD PTR [rdi+0x8]
	mov    rdi,QWORD PTR [rbp-0xe8]
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadUtf8Span>
	cmp    BYTE PTR [r13+0x0],r13b
	test   edx,edx
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x3ca>
	mov    edi,0x1
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x3cd>
	mov    rdi,rax
	mov    QWORD PTR [rbp-0x58],rdi
	mov    esi,edx
	mov    rdx,r13
	call   <String__CreateStringFromEncoding>
	xor    edi,edi
	mov    QWORD PTR [rbp-0x58],rdi
	mov    r13,rax
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x45b>
	mov    r13,QWORD PTR [rbp-0xe8]
	mov    rdi,r13
	mov    rcx,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x118],rcx
	cmp    BYTE PTR [rcx],cl
	lea    rdi,[rip+0x1aa73c]        # <__NONGCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	cmp    QWORD PTR [rdi-0x8],0x0
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xb3a>
	mov    rdi,QWORD PTR [rip+0x17643a]        # <__GCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	mov    rdx,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x120],rdx
	mov    rdi,rcx
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadUtf8Span>
	mov    rcx,QWORD PTR [rbp-0x120]
	cmp    BYTE PTR [rcx],cl
	test   edx,edx
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x43d>
	mov    edi,0x1
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x440>
	mov    rdi,rax
	mov    QWORD PTR [rbp-0x60],rdi
	mov    esi,edx
	mov    rdx,rcx
	call   <String__CreateStringFromEncoding>
	xor    edi,edi
	mov    QWORD PTR [rbp-0x60],rdi
	inc    DWORD PTR [r13+0x10]
	mov    r13,rax
	mov    eax,DWORD PTR [rbp-0x2c]
	or     eax,0x8
	movzx  eax,ax
	mov    DWORD PTR [rbp-0x2c],eax
	mov    rax,QWORD PTR [rbp-0xe8]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xa8>
	lea    rdi,[rip+0x1c53ee]        # <_ZTV63Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>>
	cmp    QWORD PTR [rbp-0xb8],rdi
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x4e3>
	mov    rdi,QWORD PTR [rbp-0xe8]
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadColon>
	lea    rdi,[rip+0x1aa6aa]        # <__NONGCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	cmp    QWORD PTR [rdi-0x8],0x0
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xb4b>
	mov    rdi,QWORD PTR [rip+0x1763a8]        # <__GCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	mov    r12,QWORD PTR [rdi+0x8]
	mov    rdi,QWORD PTR [rbp-0xe8]
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadUtf8Span>
	cmp    BYTE PTR [r12],r12b
	test   edx,edx
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x4c7>
	mov    edi,0x1
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x4ca>
	mov    rdi,rax
	mov    QWORD PTR [rbp-0x68],rdi
	mov    esi,edx
	mov    rdx,r12
	call   <String__CreateStringFromEncoding>
	xor    edi,edi
	mov    QWORD PTR [rbp-0x68],rdi
	mov    r12,rax
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x559>
	mov    r12,QWORD PTR [rbp-0xe8]
	mov    rdi,r12
	mov    rcx,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x128],rcx
	cmp    BYTE PTR [rcx],cl
	lea    rdi,[rip+0x1aa63f]        # <__NONGCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	cmp    QWORD PTR [rdi-0x8],0x0
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xb55>
	mov    rdi,QWORD PTR [rip+0x17633d]        # <__GCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	mov    rdx,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x130],rdx
	mov    rdi,rcx
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadUtf8Span>
	mov    rcx,QWORD PTR [rbp-0x130]
	cmp    BYTE PTR [rcx],cl
	test   edx,edx
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x53a>
	mov    edi,0x1
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x53d>
	mov    rdi,rax
	mov    QWORD PTR [rbp-0x70],rdi
	mov    esi,edx
	mov    rdx,rcx
	call   <String__CreateStringFromEncoding>
	xor    edi,edi
	mov    QWORD PTR [rbp-0x70],rdi
	inc    DWORD PTR [r12+0x10]
	mov    r12,rax
	mov    eax,DWORD PTR [rbp-0x2c]
	or     eax,0x10
	movzx  eax,ax
	mov    DWORD PTR [rbp-0x2c],eax
	mov    rax,QWORD PTR [rbp-0xe8]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xa8>
	lea    rdi,[rip+0x1c52f0]        # <_ZTV63Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>>
	cmp    QWORD PTR [rbp-0xb8],rdi
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x5f4>
	mov    rdi,QWORD PTR [rbp-0xe8]
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadColon>
	lea    rdi,[rip+0x1aa5ac]        # <__NONGCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	cmp    QWORD PTR [rdi-0x8],0x0
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xb66>
	mov    rdi,QWORD PTR [rip+0x1762aa]        # <__GCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	mov    rax,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x138],rax
	mov    rdi,QWORD PTR [rbp-0xe8]
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadUtf8Span>
	mov    rcx,QWORD PTR [rbp-0x138]
	cmp    BYTE PTR [rcx],cl
	test   edx,edx
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x5d1>
	mov    edi,0x1
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x5d4>
	mov    rdi,rax
	mov    QWORD PTR [rbp-0x78],rdi
	mov    esi,edx
	mov    rdx,rcx
	call   <String__CreateStringFromEncoding>
	xor    edi,edi
	mov    QWORD PTR [rbp-0x78],rdi
	mov    rsi,rax
	mov    rax,QWORD PTR [rbp-0xe8]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x66f>
	mov    rcx,QWORD PTR [rbp-0xe8]
	mov    rdi,rcx
	mov    rdx,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x140],rdx
	cmp    BYTE PTR [rdx],dl
	lea    rdi,[rip+0x1aa52e]        # <__NONGCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	cmp    QWORD PTR [rdi-0x8],0x0
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xb70>
	mov    rdi,QWORD PTR [rip+0x17622c]        # <__GCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	mov    rsi,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x148],rsi
	mov    rdi,rdx
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadUtf8Span>
	mov    rcx,QWORD PTR [rbp-0x148]
	cmp    BYTE PTR [rcx],cl
	test   edx,edx
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x64b>
	mov    edi,0x1
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x64e>
	mov    rdi,rax
	mov    QWORD PTR [rbp-0x80],rdi
	mov    esi,edx
	mov    rdx,rcx
	call   <String__CreateStringFromEncoding>
	mov    rsi,rax
	xor    edi,edi
	mov    QWORD PTR [rbp-0x80],rdi
	mov    rax,QWORD PTR [rbp-0xe8]
	inc    DWORD PTR [rax+0x10]
	mov    QWORD PTR [rbp-0xc0],rsi
	mov    edx,DWORD PTR [rbp-0x2c]
	or     edx,0x20
	movzx  edx,dx
	mov    DWORD PTR [rbp-0x2c],edx
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xa8>
	mov    rax,QWORD PTR [rbp-0xe8]
	lea    rdi,[rip+0x1c51d3]        # <_ZTV63Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>>
	cmp    QWORD PTR [rbp-0xb8],rdi
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x713>
	mov    rdi,rax
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadColon>
	lea    rdi,[rip+0x1aa493]        # <__NONGCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	cmp    QWORD PTR [rdi-0x8],0x0
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xb81>
	mov    rdi,QWORD PTR [rip+0x176191]        # <__GCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	mov    rax,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x150],rax
	mov    rdi,QWORD PTR [rbp-0xe8]
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadUtf8Span>
	mov    rcx,QWORD PTR [rbp-0x150]
	cmp    BYTE PTR [rcx],cl
	test   edx,edx
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x6ea>
	mov    edi,0x1
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x6ed>
	mov    rdi,rax
	mov    QWORD PTR [rbp-0x88],rdi
	mov    esi,edx
	mov    rdx,rcx
	call   <String__CreateStringFromEncoding>
	xor    edi,edi
	mov    QWORD PTR [rbp-0x88],rdi
	mov    rsi,rax
	mov    rax,QWORD PTR [rbp-0xe8]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x78e>
	mov    rdi,rax
	mov    r8,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x158],r8
	cmp    BYTE PTR [r8],r8b
	lea    rdi,[rip+0x1aa415]        # <__NONGCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	cmp    QWORD PTR [rdi-0x8],0x0
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xb8b>
	mov    rdi,QWORD PTR [rip+0x176113]        # <__GCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	mov    rsi,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x160],rsi
	mov    rdi,r8
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadUtf8Span>
	mov    rcx,QWORD PTR [rbp-0x160]
	cmp    BYTE PTR [rcx],cl
	test   edx,edx
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x764>
	mov    edi,0x1
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x767>
	mov    rdi,rax
	mov    QWORD PTR [rbp-0x90],rdi
	mov    esi,edx
	mov    rdx,rcx
	call   <String__CreateStringFromEncoding>
	mov    rsi,rax
	xor    edi,edi
	mov    QWORD PTR [rbp-0x90],rdi
	mov    rax,QWORD PTR [rbp-0xe8]
	inc    DWORD PTR [rax+0x10]
	mov    QWORD PTR [rbp-0xc8],rsi
	mov    edx,DWORD PTR [rbp-0x2c]
	or     edx,0x40
	movzx  edx,dx
	mov    DWORD PTR [rbp-0x2c],edx
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xa8>
	mov    rax,QWORD PTR [rbp-0xe8]
	lea    rdi,[rip+0x1c50b4]        # <_ZTV63Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>>
	cmp    QWORD PTR [rbp-0xb8],rdi
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x832>
	mov    rdi,rax
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadColon>
	lea    rdi,[rip+0x1aa374]        # <__NONGCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	cmp    QWORD PTR [rdi-0x8],0x0
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xb9c>
	mov    rdi,QWORD PTR [rip+0x176072]        # <__GCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	mov    rax,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x168],rax
	mov    rdi,QWORD PTR [rbp-0xe8]
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadUtf8Span>
	mov    rcx,QWORD PTR [rbp-0x168]
	cmp    BYTE PTR [rcx],cl
	test   edx,edx
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x809>
	mov    edi,0x1
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x80c>
	mov    rdi,rax
	mov    QWORD PTR [rbp-0x98],rdi
	mov    esi,edx
	mov    rdx,rcx
	call   <String__CreateStringFromEncoding>
	xor    edi,edi
	mov    QWORD PTR [rbp-0x98],rdi
	mov    rsi,rax
	mov    rax,QWORD PTR [rbp-0xe8]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x8ad>
	mov    rdi,rax
	mov    r8,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x170],r8
	cmp    BYTE PTR [r8],r8b
	lea    rdi,[rip+0x1aa2f6]        # <__NONGCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	cmp    QWORD PTR [rdi-0x8],0x0
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xba6>
	mov    rdi,QWORD PTR [rip+0x175ff4]        # <__GCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	mov    rsi,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x178],rsi
	mov    rdi,r8
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadUtf8Span>
	mov    rcx,QWORD PTR [rbp-0x178]
	cmp    BYTE PTR [rcx],cl
	test   edx,edx
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x883>
	mov    edi,0x1
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x886>
	mov    rdi,rax
	mov    QWORD PTR [rbp-0xa0],rdi
	mov    esi,edx
	mov    rdx,rcx
	call   <String__CreateStringFromEncoding>
	mov    rsi,rax
	xor    edi,edi
	mov    QWORD PTR [rbp-0xa0],rdi
	mov    rax,QWORD PTR [rbp-0xe8]
	inc    DWORD PTR [rax+0x10]
	mov    QWORD PTR [rbp-0xd0],rsi
	mov    edx,DWORD PTR [rbp-0x2c]
	or     edx,0x80
	movzx  edx,dx
	mov    DWORD PTR [rbp-0x2c],edx
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xa8>
	mov    rax,QWORD PTR [rbp-0xe8]
	lea    rdi,[rip+0x1c4f92]        # <_ZTV63Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>>
	cmp    QWORD PTR [rbp-0xb8],rdi
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x954>
	mov    rdi,rax
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadColon>
	lea    rdi,[rip+0x1aa252]        # <__NONGCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	cmp    QWORD PTR [rdi-0x8],0x0
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xbb7>
	mov    rdi,QWORD PTR [rip+0x175f50]        # <__GCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	mov    rax,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x180],rax
	mov    rdi,QWORD PTR [rbp-0xe8]
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadUtf8Span>
	mov    rcx,QWORD PTR [rbp-0x180]
	cmp    BYTE PTR [rcx],cl
	test   edx,edx
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x92b>
	mov    edi,0x1
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x92e>
	mov    rdi,rax
	mov    QWORD PTR [rbp-0xa8],rdi
	mov    esi,edx
	mov    rdx,rcx
	call   <String__CreateStringFromEncoding>
	xor    edi,edi
	mov    QWORD PTR [rbp-0xa8],rdi
	mov    rsi,rax
	mov    rax,QWORD PTR [rbp-0xe8]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x9cf>
	mov    rdi,rax
	mov    r8,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x188],r8
	cmp    BYTE PTR [r8],r8b
	lea    rdi,[rip+0x1aa1d4]        # <__NONGCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	cmp    QWORD PTR [rdi-0x8],0x0
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xbc1>
	mov    rdi,QWORD PTR [rip+0x175ed2]        # <__GCSTATICSS_P_CoreLib_System_Text_UTF8Encoding>
	mov    rsi,QWORD PTR [rdi+0x8]
	mov    QWORD PTR [rbp-0x190],rsi
	mov    rdi,r8
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadUtf8Span>
	mov    rcx,QWORD PTR [rbp-0x190]
	cmp    BYTE PTR [rcx],cl
	test   edx,edx
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x9a5>
	mov    edi,0x1
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x9a8>
	mov    rdi,rax
	mov    QWORD PTR [rbp-0xb0],rdi
	mov    esi,edx
	mov    rdx,rcx
	call   <String__CreateStringFromEncoding>
	mov    rsi,rax
	xor    edi,edi
	mov    QWORD PTR [rbp-0xb0],rdi
	mov    rax,QWORD PTR [rbp-0xe8]
	inc    DWORD PTR [rax+0x10]
	mov    QWORD PTR [rbp-0xd8],rsi
	mov    edx,DWORD PTR [rbp-0x2c]
	or     edx,0x100
	movzx  edx,dx
	mov    DWORD PTR [rbp-0x2c],edx
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xa8>
	mov    rax,QWORD PTR [rbp-0xe8]
	lea    rdi,[rip+0x1c4e70]        # <_ZTV63Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>>
	cmp    QWORD PTR [rbp-0xb8],rdi
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xa25>
	mov    rdi,rax
	call   <Serde_Serde_Json_JsonDeserializer_1<Serde_Serde_IO_ArrayReader>__ReadColon>
	mov    rax,QWORD PTR [rbp-0xe8]
	lea    rdi,[rax+0x18]
	call   <Serde_Serde_Json_Utf8JsonLexer_1<Serde_Serde_IO_ArrayReader>__Skip>
	mov    rax,QWORD PTR [rbp-0xe8]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xa8>
	mov    rdi,QWORD PTR [rax+0x8]
	cmp    BYTE PTR [rdi],dil
	add    rdi,0x18
	call   <Serde_Serde_Json_Utf8JsonLexer_1<Serde_Serde_IO_ArrayReader>__Skip>
	mov    rax,QWORD PTR [rbp-0xe8]
	inc    DWORD PTR [rax+0x10]
	mov    rax,QWORD PTR [rbp-0xe8]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xa8>
	cmp    DWORD PTR [rbp-0x2c],0x1ff
	jne    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0xc0b>
	lea    rdi,[rip+0x1c4761]        # <_ZTV25trace_Benchmarks_Location>
	call   <RhpNewFast>
	mov    QWORD PTR [rbp-0xf0],rax
	mov    DWORD PTR [rax+0x48],ebx
	lea    rdi,[rax+0x8]
	mov    rsi,r15
	call   <RhpAssignRefESI>
	mov    rbx,QWORD PTR [rbp-0xf0]
	lea    rdi,[rbx+0x10]
	mov    rsi,r14
	call   <RhpAssignRefESI>
	lea    rdi,[rbx+0x18]
	mov    rsi,r13
	call   <RhpAssignRefESI>
	lea    rdi,[rbx+0x20]
	mov    rsi,r12
	call   <RhpAssignRefESI>
	lea    rdi,[rbx+0x28]
	mov    rsi,QWORD PTR [rbp-0xc0]
	call   <RhpAssignRefESI>
	lea    rdi,[rbx+0x30]
	mov    rsi,QWORD PTR [rbp-0xc8]
	call   <RhpAssignRefESI>
	lea    rdi,[rbx+0x38]
	mov    rsi,QWORD PTR [rbp-0xd0]
	call   <RhpAssignRefESI>
	lea    rdi,[rbx+0x40]
	mov    rsi,QWORD PTR [rbp-0xd8]
	call   <RhpAssignRefESI>
	mov    rax,rbx
	add    rsp,0x168
	pop    rbx
	pop    r12
	pop    r13
	pop    r14
	pop    r15
	pop    rbp
	ret
	call   <__GetGCStaticBase_S_P_CoreLib_System_Text_UTF8Encoding>
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x1ac>
	call   <__GetGCStaticBase_S_P_CoreLib_System_Text_UTF8Encoding>
	mov    rcx,QWORD PTR [rbp-0xf8]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x216>
	call   <__GetGCStaticBase_S_P_CoreLib_System_Text_UTF8Encoding>
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x2a8>
	call   <__GetGCStaticBase_S_P_CoreLib_System_Text_UTF8Encoding>
	mov    rcx,QWORD PTR [rbp-0x108]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x312>
	call   <__GetGCStaticBase_S_P_CoreLib_System_Text_UTF8Encoding>
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x3a4>
	call   <__GetGCStaticBase_S_P_CoreLib_System_Text_UTF8Encoding>
	mov    rcx,QWORD PTR [rbp-0x118]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x40f>
	call   <__GetGCStaticBase_S_P_CoreLib_System_Text_UTF8Encoding>
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x4a1>
	call   <__GetGCStaticBase_S_P_CoreLib_System_Text_UTF8Encoding>
	mov    rcx,QWORD PTR [rbp-0x128]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x50c>
	call   <__GetGCStaticBase_S_P_CoreLib_System_Text_UTF8Encoding>
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x59f>
	call   <__GetGCStaticBase_S_P_CoreLib_System_Text_UTF8Encoding>
	mov    rdx,QWORD PTR [rbp-0x140]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x61d>
	call   <__GetGCStaticBase_S_P_CoreLib_System_Text_UTF8Encoding>
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x6b8>
	call   <__GetGCStaticBase_S_P_CoreLib_System_Text_UTF8Encoding>
	mov    r8,QWORD PTR [rbp-0x158]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x736>
	call   <__GetGCStaticBase_S_P_CoreLib_System_Text_UTF8Encoding>
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x7d7>
	call   <__GetGCStaticBase_S_P_CoreLib_System_Text_UTF8Encoding>
	mov    r8,QWORD PTR [rbp-0x170]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x855>
	call   <__GetGCStaticBase_S_P_CoreLib_System_Text_UTF8Encoding>
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x8f9>
	call   <__GetGCStaticBase_S_P_CoreLib_System_Text_UTF8Encoding>
	mov    r8,QWORD PTR [rbp-0x188]
	jmp    <trace_Benchmarks_LocationWrap__Serde_IDeserialize_Benchmarks_Location__Deserialize+0x977>
	lea    rdi,[rip+0x1b0367]        # <_ZTV44S_P_CoreLib_System_InvalidOperationException>
	call   <RhpNewFast>
	mov    rbx,rax
	mov    edi,DWORD PTR [rbp-0x30]
	call   <S_P_CoreLib_System_Number__Int32ToDecStr>
	mov    rsi,rax
	lea    rdi,[rip+0x1a148d]        # <__Str_Unexpected_index___DD431A17A22429C2F6F288DD37AAF50519152CCD3C02D89ADE2CA33CD2FF94F0>
	call   <String__Concat_5>
	mov    rsi,rax
	mov    rdi,rbx
	call   <S_P_CoreLib_System_InvalidOperationException___ctor_0>
	mov    rdi,rbx
	call   <RhpThrowEx>
	call   <Serde_Serde_DeserializeException__UnassignedMember>
	mov    rdi,rax
	call   <RhpThrowEx>
	int3

Disassembly of section __unbox:

Disassembly of section .fini:
