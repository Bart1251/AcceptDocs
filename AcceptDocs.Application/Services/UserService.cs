using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
using AutoMapper;

namespace AcceptDocs.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAppUnitOfWork _appUnitOfWork;
        private readonly IMapper _mapper;

        public UserService(IAppUnitOfWork appUnitOfWork, IMapper mapper)
        {
            _appUnitOfWork = appUnitOfWork;
            _mapper = mapper;
        }

        public int Create(AddUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            int id = _appUnitOfWork.UserRepository.Insert(user);
            _appUnitOfWork.Commit();
            return id;
        }

        public List<UserDto> GetAllWithPositionLevel()
        {
            var users = _appUnitOfWork.UserRepository.GetAllWithPositionLevel();
            return _mapper.Map<List<UserDto>>(users);
        }

        public void Delete(int id)
        {
            var user = _appUnitOfWork.UserRepository.Get(id);
            _appUnitOfWork.UserRepository.Delete(user);
            _appUnitOfWork.Commit();
        }

        public UserDto GetWithPositionLevel(int id)
        {
            var user = _appUnitOfWork.UserRepository.GetWithPositionLevel(id);
            return _mapper.Map<UserDto>(user);
        }

        public void Update(UpdateUserDto dto)
        {
            var user = _appUnitOfWork.UserRepository.Get(dto.UserId);
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Login = dto.Login;
            user.Password = dto.Password;
            user.PositionLevelId = dto.PositionLevelId;
            user.Position = dto.Position;
            _appUnitOfWork.Commit();
        }
    }
}
