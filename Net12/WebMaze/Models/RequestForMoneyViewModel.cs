using System;
using System.Collections.Generic;
using WebMaze.Services;
using WebMaze.Services.RequestForMoney;
using System.ComponentModel.DataAnnotations;
using WebMaze.Models.ValidationAttributes;
using WebMaze.EfStuff.DbModel;

namespace WebMaze.Models
{
    public class RequestForMoneyViewModel
    {
        public bool IsActive { get; set; }
        public long Id { get; set; }
        public DateTime RequestCreationDate { get; set; }
        public int RequestAmount { get; set; }
        public RequestStatusEnums RequestStatus { get; set; }
        public string RequestRecipient { get; set; }
        public string RequestCreator { get; set; }  
        public List<UserViewModel> Users { get; set; }
        public MassegeErrorsRequestEnums MassegeErrors { get; set; }
    }
}
