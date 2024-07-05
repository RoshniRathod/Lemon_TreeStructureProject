using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TreeStructures.Data;
using TreeStructures.Models;

namespace TreeStructures.Controllers
{
    public class TreeController : Controller
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region Ctor

        public TreeController(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Utility

        protected List<TreeStructureModel> GetChildNodes(NodeInfo node)
        {
            var model = new List<TreeStructureModel>();
           
            if (node != null)
            {
                var childNodes = _context.Nodes.Where(x => x.ParentNodeId == node.Id && x.IsActive == true).ToList();

                foreach (var childNode in childNodes)
                {
                    var treeStructureModel = new TreeStructureModel()
                    {
                        Id = childNode.Id,
                        Name = childNode.Name,
                        IsActive = childNode.IsActive,
                        ParentNodeId = childNode.ParentNodeId,
                        ChildNodes = GetChildNodes(childNode)
                    };
                    model.Add(treeStructureModel);                                 
                }
            }
            return model;
        }

        #endregion

        #region Methods

        public IActionResult AllNodes()
        {
            var model = new List<NodeInfoModel>();
            var nodes = _context.Nodes.ToList();
            foreach (var node in nodes)
            {
                var nodeInfoModel = new NodeInfoModel()
                {
                    Id = node.Id,
                    Name = node.Name,
                    IsActive = node.IsActive,
                    ParentNodeName = _context.Nodes.Where(x => x.Id == node.ParentNodeId).Select(x => x.Name).FirstOrDefault(),
                    StartDate = node.StartDate
                };
                model.Add(nodeInfoModel);
            }
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new NodeInfoModel();
            var allNodes = _context.Nodes.ToList();
            model.ParentNodes.Add(new SelectListItem { Value = "0", Text = "Select Parent node." });
            foreach (var node in allNodes)
            {
                model.ParentNodes.Add(new SelectListItem { Value = node.Id.ToString(), Text = node.Name });
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult AddNode(NodeInfoModel model)
        {
            if (ModelState.IsValid)
            {
                var node = new NodeInfo()
                {
                    Name = model.Name,
                    IsActive = model.IsActive,
                    ParentNodeId = model.ParentNodeId,
                    StartDate = DateTime.UtcNow
                };
                _context.Nodes.Add(node);
                _context.SaveChanges();
                TempData["Success"] = "The new node has been added successfully.";
            }
            return RedirectToAction("AllNodes");
        }

        public IActionResult Edit(int id)
        {
            var node = _context.Nodes.Find(id);
            var model = new NodeInfoModel();
            var selected = false;

            var allNodes = _context.Nodes.ToList();
            model.ParentNodes.Add(new SelectListItem { Value = "0", Text = "Select Parent node." });
            foreach (var allNode in allNodes)
            {
                if (allNode.Id == node.ParentNodeId)
                    selected = true;

                model.ParentNodes.Add(new SelectListItem { Value = allNode.Id.ToString(), Text = allNode.Name, Selected = selected });
            }

            if (node != null)
            {
                model.Id = node.Id;
                model.Name = node.Name;
                model.IsActive = node.IsActive;
                model.ParentNodeId = node.ParentNodeId;
                model.StartDate = DateTime.UtcNow;

                return View(model);
            }
            else
            {
                TempData["Error"] = $"No node found with specified id {id}.";
                return RedirectToAction("AllNodes");
            }

        }

        [HttpPost]
        public IActionResult EditNode(NodeInfoModel model)
        {
            if (ModelState.IsValid)
            {
                var node = _context.Nodes.Find(model.Id);
                if (node != null)
                {
                    node.Name = model.Name;
                    node.IsActive = model.IsActive;
                    node.ParentNodeId = model.ParentNodeId;

                    _context.Nodes.Update(node);
                    _context.SaveChanges();
                    TempData["Success"] = "The node has been updated successfully.";
                }
                else
                {
                    TempData["Error"] = $"No node found with specified id Record not updated.";
                }
            }
            return RedirectToAction("AllNodes");
        }

        public IActionResult Delete(int id)
        {
            var node = _context.Nodes.Find(id);
            if (node != null)
            {
                _context.Nodes.Remove(node);
                _context.SaveChanges();
                TempData["Success"] = "The node has been deleted successfully.";
            }
            else
            {
                TempData["Error"] = $"No node found with specified id.";
            }
            return RedirectToAction("AllNodes");
        }

        public IActionResult DisplayTree()
        {
            var allNodes = _context.Nodes.Where(x => x.IsActive == true && x.ParentNodeId == 0).ToList();
            var model = new List<TreeStructureModel>();
            foreach (var node in allNodes)
            {
                var treeStructureModel = new TreeStructureModel()
                {
                    Id = node.Id,
                    Name = node.Name,
                    IsActive = node.IsActive,
                    ParentNodeId = node.ParentNodeId,                    
                };
                model.Add(treeStructureModel);
                treeStructureModel.ChildNodes = GetChildNodes(node);
            }
            return View(model);
        }

        #endregion
    }
}
