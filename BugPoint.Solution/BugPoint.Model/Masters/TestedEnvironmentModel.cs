using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Masters
{
    [Table("TestedEnvironment")]
    public class TestedEnvironmentModel
    {
        [Key]
        public int TestedOnId { get; set; }
        public string TestedOn { get; set; }
        public int Code { get; set; }
    }
}