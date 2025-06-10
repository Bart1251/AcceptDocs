using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace AcceptDocs.Application.Services
{
    public class PositionLevelService : IPositionLevelService
    {
        private readonly IAppUnitOfWork _appUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PositionLevelService> _logger;

        public PositionLevelService(IAppUnitOfWork appUnitOfWork, IMapper mapper, ILogger<PositionLevelService> logger)
        {
            _appUnitOfWork = appUnitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public bool CanDelete(int id)
        {
            return _appUnitOfWork.PositionLevelRepository.CanDelete(id);
        }

        public int Create(PositionLevelDto dto)
        {
            var positionLevel = _mapper.Map<PositionLevel>(dto);
            PositionLevel dbLevel = _appUnitOfWork.PositionLevelRepository.Insert(positionLevel);
            _appUnitOfWork.Commit();
            _logger.LogInformation("Utworzono poziom stanowiska o id: " + dbLevel.PositionLevelId);
            return dbLevel.PositionLevelId;
        }

        public void Delete(int id)
        {
            var positionLevel = _appUnitOfWork.PositionLevelRepository.Get(id);
            _appUnitOfWork.PositionLevelRepository.Delete(positionLevel);
            _appUnitOfWork.Commit();
            _logger.LogInformation("Usunięto poziom stanowiska o id: " + id);
        }

        public PositionLevelDto Get(int id)
        {
            var positionLevel = _appUnitOfWork.PositionLevelRepository.Get(id);
            var mapped = _mapper.Map<PositionLevelDto>(positionLevel);
            _logger.LogInformation("Pobrano poziom stanowiska o id: " + id);
            return mapped;
        }

        public List<PositionLevelDto> GetAll()
        {
            var positionLevels = _appUnitOfWork.PositionLevelRepository.GetAll();
            var mapped = _mapper.Map<List<PositionLevelDto>>(positionLevels);
            _logger.LogInformation("Pobrano listę wszystkich poziomów stanowisk");
            return mapped;
        }

        public void Update(PositionLevelDto dto)
        {
            var positionLevel = _appUnitOfWork.PositionLevelRepository.Get(dto.PositionLevelId);
            positionLevel.Name = dto.Name;
            _appUnitOfWork.Commit();
            _logger.LogInformation("Zaktualizowano poziom stanowiska o id: " + dto.PositionLevelId);
        }
    }
}
