using AcceptDocs.SharedKernel.Dto;

namespace AcceptDocs.Application.Services
{
    public interface IUserService
    {
        int Create(AddUserDto dto);
        List<UserDto> GetAllWithPositionLevel();
        void Delete(int id);
        UserDto GetWithPositionLevel(int id);
        void Update(UpdateUserDto dto);
        string Login(UserLoginDto dto);
        bool CanDeleteUser(int id);
    }
}
