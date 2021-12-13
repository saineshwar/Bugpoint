using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Masters
{
    [Table("WebFrameworks")]
    public class WebFrameworkModel
    {
        [Key]
        public int WebFrameworkId { get; set; }
        public string WebFramework { get; set; }
        public int Code { get; set; }
    }
}