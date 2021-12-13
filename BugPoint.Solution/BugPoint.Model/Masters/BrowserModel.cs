using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Masters
{
    [Table("Browsers")]
    public class BrowserModel
    {
        [Key]
        public int BrowserId { get; set; }
        public string BrowserName { get; set; }
        public int Code { get; set; }
    }
}