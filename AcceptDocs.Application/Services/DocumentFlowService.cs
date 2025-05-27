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

        public void AttachUser(AttachUserDto dto)
        {
            _appUnitOfWork.DocumentFlowRepository.AttachUser(_mapper.Map<DocumentFlowUser>(dto));
            _appUnitOfWork.Commit();
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

        public void DetachUser(int documentFlowId, int userId)
        {
            _appUnitOfWork.DocumentFlowRepository.DetachUser(documentFlowId, userId);
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

        public List<DocumentFlowDto> GetAllWithUsers()
        {
            var flows = _appUnitOfWork.DocumentFlowRepository.GetAllWithUsers();
            return _mapper.Map<List<DocumentFlowDto>>(flows);
        }

        public List<DocumentFlowUserDto> GetAttachedUsers(int id)
        {
            var documentFlowUsers = _appUnitOfWork.DocumentFlowRepository.GetAttachedUsers(id);
            return _mapper.Map<List<DocumentFlowUserDto>>(documentFlowUsers);
        }

        public List<UserDto> GetNotAttachedUsers(int id)
        {
            var documentFlowUsers = _appUnitOfWork.DocumentFlowRepository.GetNotAttachedUsers(id);
            return _mapper.Map<List<UserDto>>(documentFlowUsers);
        }

        public void Update(UpdateDocumentFlowDto dto)
        {
            var flow = _appUnitOfWork.DocumentFlowRepository.Get(dto.DocumentFlowId);
            flow.Name = dto.Name;
            flow.Description = dto.Description;
            flow.SelectionMethod = _mapper.Map<SelectionMethod>(dto.SelectionMethod);
            _appUnitOfWork.Commit();
        }

        public void UpdateAttachedUserValue(AttachUserDto dto)
        {
            var userAttachment = _appUnitOfWork.DocumentFlowRepository.GetUserAttachment(dto.UserId, dto.DocumentFlowId);
            userAttachment.Value = dto.Value;
            _appUnitOfWork.Commit();
        }
    }
}
