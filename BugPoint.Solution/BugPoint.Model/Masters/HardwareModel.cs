using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Masters
{
    [Table("Hardware")]
    public class HardwareModel
    {

        [Key]
        public int HardwareId { get; set; }
        public string Hardware { get; set; }
        public int Code { get; set; }
    }
}