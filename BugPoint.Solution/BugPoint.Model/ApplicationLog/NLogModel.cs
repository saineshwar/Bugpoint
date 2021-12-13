using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugPoint.Model.ApplicationLog
{
    [Table("NLog")]
    public class NLogModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [MaxLength(200)]
        public string MachineName { get; set; }
        public DateTime Logged { get; set; }
        [MaxLength(5)]
        public string Level { get; set; }
        [MaxLength]
        public string Message { get; set; }
        [MaxLength(300)]
        public string Logger { get; set; }
        [MaxLength]
        public string Properties { get; set; }
        [MaxLength(300)]
        public string Callsite { get; set; }
        [MaxLength]
        public string Exception { get; set; }
    }

}