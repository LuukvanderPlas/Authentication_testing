using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication_testing.Models {

    [Table("Employee")]
    public class Employee {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Column("birthdate", TypeName = "date")]
        public DateTime Birthdate { get; set; }

        [Column("salary")]
        public double Salary { get; set; }

        [Column("Job_title")]
        [StringLength(50)]
        public string JobTitle { get; set; } = null!;
    }
}
