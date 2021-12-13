using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Masters
{
    [Table("Resolution")]
    public class ResolutionModel
    {
        [Key]
        public int ResolutionId { get; set; }
        public string Resolution { get; set; }
        public int Code { get; set; }
    }
}