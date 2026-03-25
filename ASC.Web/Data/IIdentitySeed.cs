using System.Threading.Tasks;

namespace ASC.Web.Data
{
    public interface IIdentitySeed
    {
        Task Seed();
    }
}