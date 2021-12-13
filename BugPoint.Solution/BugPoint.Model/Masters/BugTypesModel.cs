using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.Masters
{
    [Table("BugTypes")]
    public class BugTypesModel
    {
        [Key]
        public int BugTypeId { get; set; }
        public string BugType { get; set; }
        public int Code { get; set; }
    }
}