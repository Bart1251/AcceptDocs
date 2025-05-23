namespace AcceptDocs.Domain.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Login {  get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string Position { get; set; } = String.Empty;
        public int PositionLevelId { get; set; }
        public PositionLevel PositionLevel { get; set; }
        public List<Document> Documents { get; set; } = new();
        public List<DocumentFlow> DocumentFlows { get; set; } = new();
        public List<AcceptanceRequest> AcceptanceRequests { get; set; } = new();
    }
}
