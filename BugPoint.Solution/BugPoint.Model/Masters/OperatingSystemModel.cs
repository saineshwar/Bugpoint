using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Masters
{
    [Table("OperatingSystem")]
    public class OperatingSystemModel
    {
        [Key]
        public int OperatingSystemId { get; set; }
        public string OperatingSystemName { get; set; }
        public int Code { get; set; }
    }
}