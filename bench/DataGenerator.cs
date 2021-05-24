
using System;

namespace Benchmarks
{
    internal static class DataGenerator
    {
        public static T Generate<T>() where T : Serde.ISerialize
        {
            if (typeof(T) == typeof(LoginViewModel))
                return (T)(object)CreateLoginViewModel();
            if (typeof(T) == typeof(Location))
                return (T)(object)CreateLocation();

            throw new InvalidOperationException();

            static LoginViewModel CreateLoginViewModel() => new LoginViewModel
            {
                Email = "name.familyname@not.com",
                // [SuppressMessage("Microsoft.Security", "CS002:SecretInNextLine", Justification="Dummy credentials for perf testing.")]
                Password = "abcdefgh123456!@",
                RememberMe = true
            };

            static Location CreateLocation() => new Location
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
        }
    }
}