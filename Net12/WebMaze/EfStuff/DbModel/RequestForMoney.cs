using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.Services;

namespace WebMaze.EfStuff.DbModel
{
    public class RequestForMoney : BaseModel
    {
        public DateTime RequestCreationDate { get; set; }
        public int RequestAmount { get; set; }
        public RequestStatusEnums RequestStatus { get; set; }
        public virtual User RequestCreator { get; set; }
        public virtual User RequestRecipient { get; set; }     
    }
}
