using AcceptDocs.SharedKernel.Dto;

namespace AcceptDocs.Application.Services
{
    public interface IPositionLevelService
    {
        List<PositionLevelDto> GetAll();
        PositionLevelDto Get(int id);
        void Delete(int id);
        void Update(PositionLevelDto dto);
        int Create(PositionLevelDto dto);
    }
}
