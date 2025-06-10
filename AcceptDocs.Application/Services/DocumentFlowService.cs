using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AcceptDocs.Application.Services
{
    public class DocumentFlowService : IDocumentFlowService
    {
        private readonly IAppUnitOfWork _appUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DocumentFlowService> _logger;

        public DocumentFlowService(IAppUnitOfWork appUnitOfWork, IMapper mapper, ILogger<DocumentFlowService> logger)
        {
            _appUnitOfWork = appUnitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public void AttachUser(AttachUserDto dto)
        {
            _appUnitOfWork.DocumentFlowRepository.AttachUser(_mapper.Map<DocumentFlowUser>(dto));
            _appUnitOfWork.Commit();
            _logger.LogInformation("Dołączono użytkownika o id: " + dto.UserId + " do przepływu o id: " + dto.DocumentFlowId);
        }

        public int Create(AddDocumentFlowDto dto)
        {
            var flow = _mapper.Map<DocumentFlow>(dto);
            DocumentFlow dbFlow = _appUnitOfWork.DocumentFlowRepository.Insert(flow);
            _appUnitOfWork.Commit();
            _logger.LogInformation("Utworzono przepływ o id: " + dbFlow.DocumentFlowId);
            return dbFlow.DocumentFlowId;
        }

        public void Delete(int id)
        {
            var flow = _appUnitOfWork.DocumentFlowRepository.Get(id);
            _appUnitOfWork.DocumentFlowRepository.Delete(flow);
            _appUnitOfWork.Commit();
            _logger.LogInformation("Usunięto przepływ o id: " + id);
        }

        public void DetachUser(int documentFlowId, int userId)
        {
            _appUnitOfWork.DocumentFlowRepository.DetachUser(documentFlowId, userId);
            _appUnitOfWork.Commit();
            _logger.LogInformation("Odłączono użytkownika o id: " + userId + " od przepływu o id: " + documentFlowId);
        }

        public DocumentFlowDto Get(int id)
        {
            var flow = _appUnitOfWork.DocumentFlowRepository.Get(id);
            var mapped = _mapper.Map<DocumentFlowDto>(flow);
            _logger.LogInformation("Pobrano przepływ o id: " + id);
            return mapped;
        }

        public List<DocumentFlowDto> GetAll()
        {
            var flows = _appUnitOfWork.DocumentFlowRepository.GetAll();
            var mapped = _mapper.Map<List<DocumentFlowDto>>(flows);
            _logger.LogInformation("Pobrano listę wszystkich przepływów");
            return mapped;
        }

        public List<DocumentFlowDto> GetAllWithUsers()
        {
            var flows = _appUnitOfWork.DocumentFlowRepository.GetAllWithUsers();
            var mapped = _mapper.Map<List<DocumentFlowDto>>(flows);
            _logger.LogInformation("Pobrano listę wszystkich przepływów");
            return mapped;
        }

        public List<DocumentFlowUserDto> GetAttachedUsers(int id)
        {
            var documentFlowUsers = _appUnitOfWork.DocumentFlowRepository.GetAttachedUsers(id);
            var mapped = _mapper.Map<List<DocumentFlowUserDto>>(documentFlowUsers);
            _logger.LogInformation("Pobrano listę wszystkich użytkowników przypisanych do przepływu o id: " + id);
            return mapped;
        }

        public List<UserDto> GetNotAttachedUsers(int id)
        {
            var documentFlowUsers = _appUnitOfWork.DocumentFlowRepository.GetNotAttachedUsers(id);
            var mapped = _mapper.Map<List<UserDto>>(documentFlowUsers);
            _logger.LogInformation("Pobrano listę wszystkich użytkowników nie przypisanych do przepływu o id: " + id);
            return mapped;
        }

        public void Update(UpdateDocumentFlowDto dto)
        {
            var flow = _appUnitOfWork.DocumentFlowRepository.Get(dto.DocumentFlowId);
            flow.Name = dto.Name;
            flow.Description = dto.Description;
            flow.SelectionMethod = _mapper.Map<SelectionMethod>(dto.SelectionMethod);
            _appUnitOfWork.Commit();
            _logger.LogInformation("Zaktualizowano przepływ o id: " + dto.DocumentFlowId);
        }

        public void UpdateAttachedUserValue(AttachUserDto dto)
        {
            var userAttachment = _appUnitOfWork.DocumentFlowRepository.GetUserAttachment(dto.UserId, dto.DocumentFlowId);
            userAttachment.Value = dto.Value;
            _appUnitOfWork.Commit();
            _logger.LogInformation("Zaktualizowano kryterium wartości użytkownika o id: " + dto.UserId + " w przepływie o id: " + dto.DocumentFlowId);
        }
    }
}
