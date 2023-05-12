
using System;

namespace Benchmarks
{
    internal static class DataGenerator
    {
        public static T GenerateSerialize<T>() where T : Serde.ISerialize
        {
            if (typeof(T) == typeof(LoginViewModel))
                return (T)(object)CreateLoginViewModel();
            if (typeof(T) == typeof(Location))
                return (T)(object)CreateLocation();
            if (typeof(T) == typeof(Serde.Test.AllInOne))
                return (T)(object)new Serde.Test.AllInOne();

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
                return LocationSample;
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

        public const string LocationSample = """
{
    "id": 1234,
    "address1": "The Street Name",
    "address2": "20/11",
    "city": "The City",
    "state": "The State",
    "postalCode": "abc-12",
    "name": "Nonexisting",
    "phoneNumber": "+0 11 222 333 44",
    "country": "The Greatest"
}
""";
    }
}