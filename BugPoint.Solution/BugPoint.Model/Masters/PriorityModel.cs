using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Masters
{
    [Table("Priority")]
    public class PriorityModel
    {
        [Key]
        public int PriorityId { get; set; }
        public string PriorityName { get; set; }
        public int Code { get; set; }
    }
}