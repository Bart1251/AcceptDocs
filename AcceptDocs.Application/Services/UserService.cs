using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Exceptions;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
using AutoMapper;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace AcceptDocs.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAppUnitOfWork _appUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UserService(IAppUnitOfWork appUnitOfWork, IMapper mapper, IConfiguration config)
        {
            _appUnitOfWork = appUnitOfWork;
            _mapper = mapper;
            _config = config;
        }

        public int Create(AddUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
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

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
