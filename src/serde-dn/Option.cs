
using System;

namespace Serde
{
    public readonly struct Option<T>
    {
        private readonly T _value;
        private readonly bool _hasValue;

        public Option(T value)
        {
            _hasValue = true;
            _value = value;
        }

        public bool HasValue => _hasValue;

        public T Value
        {
            get
            {
                if (!_hasValue)
                {
                    throw new InvalidOperationException("No value");
                }
                return _value;
            }
        }

        public T GetValueOrDefault() => _value;

        public static implicit operator Option<T>(T value) => new Option<T>(value);
    }
}