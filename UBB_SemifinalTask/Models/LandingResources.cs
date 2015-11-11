using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UBB_SemifinalTask.Models
{
    public class LandingResources
    {
        [Key]
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Content { get; set; }
        public string LinkOnUbb { get; set; }
        [Column("Css")]
        public string Styles { get; set; }
        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual ApplicationUser Author { get; set; }

    }
}