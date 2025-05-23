using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptDocs.SharedKernel.Dto
{
    public class UpdateUserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Login { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string Position { get; set; } = String.Empty;
        public int PositionLevelId { get; set; }
    }
}
