using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kayzer.project.Models
{
    public class ColocationModel
    {

        public ColocationModel()
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }


        [Key]
        public int CoID { get; set; }
        public string CoName { get; set; }
        public int CoFast { get; set; }
        public string Management { get; set; }
        public string BackUp { get; set; }
        public string CoIp { get; set; }
        public int CoPrice { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsPublished { get; set; }
    }
}