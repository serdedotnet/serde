<div style="text-align: right;">
  <a style="color: var(--icons);" href="https://github.com/agocke/serde-dn">View on GitHub</a>
</div>

# About 

Like Serde, SerdeDn is a framework for types which control how they serialize themselves. Basic
cases are fully automated using a C# source generator. It has no reflection or run-time dispatch so
it's trimming and AOT friendly. It is high-performance—the built-in JsonSerializer is as fast or
faster than `System.Text.Json`, and much faster than `Newtonsoft.Json`.

SerdeDn is also modular. Any type can control how it will be serialized, and 3rd-party output
serializers can be provided for any output format. A JSON serializer is provided as the only
built-in output format.
