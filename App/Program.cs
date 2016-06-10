using System;
using System.Configuration;
using System.Threading.Tasks;
using Domain;
using PersonRepository;

namespace App
{
    internal class Program
    {
        private static readonly IPersonRepository PersonRepository = new PersonRepository.SQL.PersonRepository(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString);

        private static void Main(string[] args)
        {
            RegisterPerson().Wait();
            Console.ReadKey();
        }

        private static async Task RegisterPerson()
        {
            var person = new Person
            {
                Name = "Camilo",
                LastName = "Bernal",
                Country = new Country
                {
                    Name = "Colombia",
                    Code = 57
                }
            };
            person.Company = new Company
            {
                Country = person.Country,
                Name = "Banlinea",
                Address = "Calle 77",
            };
            person.Phones.Add(new Phone
            {
                Country = person.Country,
                Number = "3143231701"
            });
            var result = await PersonRepository.RegisterPersonAsync(person).ConfigureAwait(false);
            Console.WriteLine($"Resultado de operacion:{result} ");
        }

    }
}