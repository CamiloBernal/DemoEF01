using System.Threading;
using System.Threading.Tasks;
using Domain;

namespace PersonRepository
{
    public interface IPersonRepository
    {

        Task<int> RegisterPersonAsync(Person person, CancellationToken cancellationToken = default(CancellationToken));

    }
}