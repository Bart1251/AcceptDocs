using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Exceptions;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AcceptDocs.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAppUnitOfWork _appUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly ILogger<UserService> _logger;

        public UserService(IAppUnitOfWork appUnitOfWork, IMapper mapper, IConfiguration config, ILogger<UserService> logger)
        {
            _appUnitOfWork = appUnitOfWork;
            _mapper = mapper;
            _config = config;
            _logger = logger;
        }

        public int Create(AddUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            User dbUser = _appUnitOfWork.UserRepository.Insert(user);
            _appUnitOfWork.Commit();
            _logger.LogInformation("Utworzono użytkownika o id: " + dbUser.UserId);
            return dbUser.UserId;
        }

        public List<UserDto> GetAllWithPositionLevel()
        {
            var users = _appUnitOfWork.UserRepository.GetAllWithPositionLevel();
            var mapped = _mapper.Map<List<UserDto>>(users);
            _logger.LogInformation("Pobrano listę wszystkich użytkowników");
            return mapped;
        }

        public void Delete(int id)
        {
            var user = _appUnitOfWork.UserRepository.Get(id);
            _appUnitOfWork.UserRepository.Delete(user);
            _appUnitOfWork.Commit();
            _logger.LogInformation("Usunięto użytkownika o id: " + id);
        }

        public UserDto GetWithPositionLevel(int id)
        {
            var user = _appUnitOfWork.UserRepository.GetWithPositionLevel(id);
            var mapped = _mapper.Map<UserDto>(user);
            _logger.LogInformation("Pobrano użytkownika o id: " + id);
            return mapped;
        }

        public void Update(UpdateUserDto dto)
        {
            var user = _appUnitOfWork.UserRepository.Get(dto.UserId);
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Login = dto.Login;
            if(!string.IsNullOrEmpty(dto.Password))
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.PositionLevelId = dto.PositionLevelId;
            user.Position = dto.Position;
            _appUnitOfWork.Commit();
            _logger.LogInformation("Zaktualizowano użytkownika o id: " + dto.UserId);
        }

        public string Login(UserLoginDto dto)
        {
            var user = _appUnitOfWork.UserRepository.Find(u => u.Login == dto.Login).FirstOrDefault();
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                throw new UnauthorizedException("Failed to authenticate. Wrong login or password");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, dto.Login),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds)
                );

            _logger.LogInformation("Zalogowano użytkownika o id: " + user.UserId);
            return token;
        }

        public bool CanDeleteUser(int id)
        {
            return _appUnitOfWork.UserRepository.CanDeleteUser(id);
        }
    }
}
