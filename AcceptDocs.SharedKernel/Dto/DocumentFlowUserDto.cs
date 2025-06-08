namespace AcceptDocs.SharedKernel.Dto
{
    public class DocumentFlowUserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Position { get; set; } = String.Empty;
        public string PositionLevel { get; set; } = String.Empty;
        public decimal Value { get; set; }
    }
}
