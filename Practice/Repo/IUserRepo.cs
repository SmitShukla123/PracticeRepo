using Practice.Model;

namespace Practice.Repo
{
    public interface IUserRepo
    {
        Task<Response<string>> SingUp(Register register);
        Task<Response<string>> SingIn(Login login);
    }
}
