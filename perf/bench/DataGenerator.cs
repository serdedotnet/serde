
using System;

namespace Benchmarks
{
    internal static class DataGenerator
    {
        public static T GenerateSerialize<T>() where T : Serde.ISerialize<T>
        {
            if (typeof(T) == typeof(LoginViewModel))
                return (T)(object)CreateLoginViewModel();
            if (typeof(T) == typeof(Location))
                return (T)(object)Location.Sample;
            if (typeof(T) == typeof(Serde.Test.AllInOne))
                return (T)(object)Serde.Test.AllInOne.Sample;

            throw new InvalidOperationException();

            static LoginViewModel CreateLoginViewModel() => new LoginViewModel
            {
                Email = "name.familyname@not.com",
                // [SuppressMessage("Microsoft.Security", "CS002:SecretInNextLine", Justification="Dummy credentials for perf testing.")]
                Password = "abcdefgh123456!@",
                RememberMe = true
            };

        }

        public static Location CreateLocation() => new Location
        {
            Id = 1234,
            Address1 = "The Street Name",
            Address2 = "20/11",
            City = "The City",
            State = "The State",
            PostalCode = "abc-12",
            Name = "Nonexisting",
            PhoneNumber = "+0 11 222 333 44",
            Country = "The Greatest"
        };

        public static string GenerateDeserialize<T>()
        {
            if (typeof(T) == typeof(LoginViewModel))
                return LoginViewSample;
            if (typeof(T) == typeof(Location))
                return Location.SampleString;
            if (typeof(T) == typeof(Serde.Test.AllInOne))
                return Serde.Test.AllInOne.SampleSerialized;

            throw new InvalidOperationException("Unexpected type");
        }

        public const string LoginViewSample = """
{
    "email": "name.familyname@not.com",
    "password": "abcdefgh123456!@",
    "rememberMe": true
}
""";

    }
}