using Practice.Model;

namespace Practice.Repo
{
    public interface IAdmin
    {
        Task<Response<string>> InsertAdmin(Produ produc);
        Task<Response<Produ>> GetOneAd(int id);
        Task<Response<List<Produ>>> GetAllAdmin(int pagenumber, int pagesize);
    }
}
