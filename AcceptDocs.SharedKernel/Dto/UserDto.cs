namespace AcceptDocs.SharedKernel.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Login { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string Position { get; set; } = String.Empty;
        public PositionLevelDto PositionLevel { get; set; }
        public List<DocumentDto> Documents { get; set; } = new();
        public List<DocumentFlowDto> DocumentFlows { get; set; } = new();
        public List<AcceptanceRequestDto> AcceptanceRequests { get; set; } = new();
    }
}
