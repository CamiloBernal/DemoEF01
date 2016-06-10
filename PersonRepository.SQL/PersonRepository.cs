using System.Threading;
using System.Threading.Tasks;
using Data.SQL;
using Domain;

namespace PersonRepository.SQL
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PersonDbContext _personDbContext;

        public PersonRepository(string connectionString)
        {
            _personDbContext = new PersonDbContext(connectionString);
        }

        public async Task<int> RegisterPersonAsync(Person person, CancellationToken cancellationToken = new CancellationToken())
        {
            _personDbContext.Persons.Add(person);
            return await _personDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}