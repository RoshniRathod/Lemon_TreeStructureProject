using System.ComponentModel.DataAnnotations;

namespace TreeStructures.Models
{
    public class NodeInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int ParentNodeId { get; set; }

        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; }


    }
}
