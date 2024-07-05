namespace TreeStructures.Models
{
    public class TreeStructureModel
    {
        public TreeStructureModel()
        {
            ChildNodes = new List<TreeStructureModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int ParentNodeId { get; set; }
        public List<TreeStructureModel> ChildNodes { get; set; }
    }
}
