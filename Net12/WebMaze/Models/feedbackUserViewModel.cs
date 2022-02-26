using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.Models.ValidationAttributes;

namespace WebMaze.Models
{
    public class FeedBackUserViewModel
    {
        public long Id { get; set; }
        public UserViewModel Creator { get; set; }
        [Diapazone(1,5)]
        public int Rate { get; set; }
        [StopWord("blacklivesmatter", "whiteLooser")]
        public string TextInfo { get; set; }
        public string UserName { get; internal set; }
        public bool CanEdit {  get; set; }
        public IFormFile ReviewDoc {  get; set; }
    }
}
