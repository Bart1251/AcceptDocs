using AcceptDocs.Domain.Contracts;
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


        public List<PositionLevelDto> GetAll()
        {
            var positionLevels = _appUnitOfWork.PositionLevelRepository.GetAll();
            return _mapper.Map<List<PositionLevelDto>>(positionLevels);
        }
    }
}
