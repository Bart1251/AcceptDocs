using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
using AutoMapper;

namespace AcceptDocs.Application.Services
{
    public class PositionLevelService : IPositionLevelService
    {
        private readonly IAppUnitOfWork _appUnitOfWork;
        private readonly IMapper _mapper;

        public PositionLevelService(IAppUnitOfWork appUnitOfWork, IMapper mapper)
        {
            _appUnitOfWork = appUnitOfWork;
            _mapper = mapper;
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
            return dbLevel.PositionLevelId;
        }

        public void Delete(int id)
        {
            var positionLevel = _appUnitOfWork.PositionLevelRepository.Get(id);
            _appUnitOfWork.PositionLevelRepository.Delete(positionLevel);
            _appUnitOfWork.Commit();
        }

        public PositionLevelDto Get(int id)
        {
            var positionLevel = _appUnitOfWork.PositionLevelRepository.Get(id);
            return _mapper.Map<PositionLevelDto>(positionLevel);
        }

        public List<PositionLevelDto> GetAll()
        {
            var positionLevels = _appUnitOfWork.PositionLevelRepository.GetAll();
            return _mapper.Map<List<PositionLevelDto>>(positionLevels);
        }

        public void Update(PositionLevelDto dto)
        {
            var positionLevel = _appUnitOfWork.PositionLevelRepository.Get(dto.PositionLevelId);
            positionLevel.Name = dto.Name;
            _appUnitOfWork.Commit();
        }
    }
}
