using WebApplication1.DTO;
using static WebApplication1.DTO.Responses;

namespace WebApplication1.IService
{
    public interface IAuth
    {
        Task<GeneralResponse> CreateAccount(RegisterUserDTO userDTO);
        Task<LogInResponse> LogInAccount(LogInDTO logInDTO);
    }
}
