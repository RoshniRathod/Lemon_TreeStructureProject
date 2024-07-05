using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TreeStructures.Models
{
    public class NodeInfoModel
    {
        public NodeInfoModel()
        {
            ParentNodes = new List<SelectListItem>();
        }
        public int Id { get; set; }

        [Required (ErrorMessage = "The node name is required.")]
        public string Name { get; set; }

        public int ParentNodeId { get; set; }

        public string ParentNodeName { get; set; }

        public IList<SelectListItem> ParentNodes { get; set; }

        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; }
    }
}
