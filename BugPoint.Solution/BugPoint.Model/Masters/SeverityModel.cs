using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Masters
{
    [Table("Severity")]
    public class SeverityModel
    {
        [Key]
        public int SeverityId { get; set; }
        public string Severity { get; set; }
        public int Code { get; set; }
    }
}