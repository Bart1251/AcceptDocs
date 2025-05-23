using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AcceptDocs.SharedKernel.Dto
{
    public enum AcceptanceRequestStatusDto
    {
        [Description("Nie odpowiedziano")]
        NotAnswered,
        [Description("Zaakceptowano")]
        Accepted,
        [Description("Odrzucono")]
        Rejested,
    }

    public class AcceptanceRequestDto
    {
        public int AcceptanceRequestId { get; set; }
        public UserDto User { get; set; }
        public DocumentDto Document { get; set; }
        public AcceptanceRequestStatusDto AcceptanceRequestStatus { get; set; } = AcceptanceRequestStatusDto.NotAnswered;
    }
}
