using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Masters
{
    [Table("Status")]
    public class StatusModel
    {
        [Key]
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int Code { get; set; }
        public bool ViewReporter { get; set; }
        public bool ViewUser { get; set; }
    }
}