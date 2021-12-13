using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Masters
{
    [Table("Version")]
    public class VersionModel
    {
        [Key]
        public int VersionId { get; set; }
        public string VersionName { get; set; }
        public int Code { get; set; }
    }
}