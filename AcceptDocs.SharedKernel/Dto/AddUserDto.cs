namespace AcceptDocs.SharedKernel.Dto
{
    public class AddUserDto
    {
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Login { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string Position { get; set; } = String.Empty;
        public int PositionLevelId { get; set; }
    }
}
