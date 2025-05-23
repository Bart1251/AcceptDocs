using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
using AutoMapper;

namespace AcceptDocs.Application.Services
{
    public class DocumentFlowService : IDocumentFlowService
    {
        private readonly IAppUnitOfWork _appUnitOfWork;
        private readonly IMapper _mapper;

        public DocumentFlowService(IAppUnitOfWork appUnitOfWork, IMapper mapper)
        {
            _appUnitOfWork = appUnitOfWork;
            _mapper = mapper;
        }
        public int Create(AddDocumentFlowDto dto)
        {
            var flow = _mapper.Map<DocumentFlow>(dto);
            int id = _appUnitOfWork.DocumentFlowRepository.Insert(flow);
            _appUnitOfWork.Commit();
            return id;
        }

        public void Delete(int id)
        {
            var flow = _appUnitOfWork.DocumentFlowRepository.Get(id);
            _appUnitOfWork.DocumentFlowRepository.Delete(flow);
            _appUnitOfWork.Commit();
        }

        public DocumentFlowDto Get(int id)
        {
            var flow = _appUnitOfWork.DocumentFlowRepository.Get(id);
            return _mapper.Map<DocumentFlowDto>(flow);
        }

        public List<DocumentFlowDto> GetAll()
        {
            var flows = _appUnitOfWork.DocumentFlowRepository.GetAll();
            return _mapper.Map<List<DocumentFlowDto>>(flows);
        }

        public void Update(UpdateDocumentFlowDto dto)
        {
            var flow = _appUnitOfWork.DocumentFlowRepository.Get(dto.DocumentFlowId);
            flow.Name = dto.Name;
            flow.Description = dto.Description;
            flow.SelectionMethod = _mapper.Map<SelectionMethod>(dto.SelectionMethod);
            flow.Value = dto.Value;
            _appUnitOfWork.Commit();
        }
    }
}
