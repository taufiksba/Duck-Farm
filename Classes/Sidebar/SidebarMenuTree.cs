
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckFarm.Classes.Sidebar
{
    public sealed class SidebarMenuTree
    {
        private readonly List<SidebarMenuNode> _roots =
            new List<SidebarMenuNode>();

        private readonly Dictionary<int, SidebarMenuNode> _nodesByIndex =
            new Dictionary<int, SidebarMenuNode>();

        private readonly Dictionary<string, SidebarMenuNode> _nodesById =
            new Dictionary<string, SidebarMenuNode>(
                StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<SidebarMenuNode> Roots
        {
            get { return _roots.AsReadOnly(); }
        }

        public int Count
        {
            get { return _nodesByIndex.Count; }
        }

        public void AddNode(
            SidebarMenuNode node,
            int? parentMenuIndex = null)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (string.IsNullOrWhiteSpace(node.MenuId))
            {
                throw new ArgumentException(
                    "MenuId tidak boleh kosong.",
                    nameof(node));
            }

            if (_nodesByIndex.ContainsKey(node.MenuIndex))
            {
                throw new InvalidOperationException(
                    "MenuIndex sudah digunakan: " + node.MenuIndex);
            }

            string normalizedMenuId = node.MenuId.Trim();

            if (_nodesById.ContainsKey(normalizedMenuId))
            {
                throw new InvalidOperationException(
                    "MenuId sudah digunakan: " + normalizedMenuId);
            }

            SidebarMenuNode parent = null;

            if (parentMenuIndex.HasValue)
            {
                parent = FindByMenuIndex(parentMenuIndex.Value);

                if (parent == null)
                {
                    throw new InvalidOperationException(
                        "Parent menu tidak ditemukan: " +
                        parentMenuIndex.Value);
                }

                if (node.Parent != null)
                {
                    throw new InvalidOperationException(
                        "Menu ini sudah memiliki parent.");
                }
            }
            else if (node.Parent != null)
            {
                throw new InvalidOperationException(
                    "Menu ini sudah memiliki parent.");
            }

            // Daftarkan node setelah seluruh validasi selesai.
            if (parent != null)
            {
                parent.AttachChild(node);
            }
            else
            {
                _roots.Add(node);
            }

            _nodesByIndex.Add(node.MenuIndex, node);
            _nodesById.Add(normalizedMenuId, node);
        }

        public SidebarMenuNode FindByMenuIndex(int menuIndex)
        {
            SidebarMenuNode node;

            _nodesByIndex.TryGetValue(menuIndex, out node);

            return node;
        }

        public SidebarMenuNode FindByMenuId(string menuId)
        {
            if (string.IsNullOrWhiteSpace(menuId))
            {
                return null;
            }

            SidebarMenuNode node;

            _nodesById.TryGetValue(menuId.Trim(), out node);

            return node;
        }

        public IReadOnlyList<SidebarMenuNode> GetRootMenus()
        {
            return _roots
                .OrderBy(node => node.SortOrder)
                .ThenBy(node => node.MenuIndex)
                .ToList()
                .AsReadOnly();
        }

        public IReadOnlyList<SidebarMenuNode> GetChildren(
            int parentMenuIndex)
        {
            SidebarMenuNode parent =
                FindByMenuIndex(parentMenuIndex);

            if (parent == null)
            {
                return Array.Empty<SidebarMenuNode>();
            }

            return parent.Children
                .OrderBy(node => node.SortOrder)
                .ThenBy(node => node.MenuIndex)
                .ToList()
                .AsReadOnly();
        }

        public IReadOnlyList<SidebarMenuNode>   GetAllNodes()
        {
            var result = new List<SidebarMenuNode>();

            foreach (SidebarMenuNode root in GetRootMenus())
            {
                AddNodeAndChildren(root, result);
            }

            return result.AsReadOnly();
        }

        private void AddNodeAndChildren(
            SidebarMenuNode node,
            List<SidebarMenuNode> result)
        {
            result.Add(node);

            foreach (SidebarMenuNode child in node.Children
                .OrderBy(item => item.SortOrder)
                .ThenBy(item => item.MenuIndex))
            {
                AddNodeAndChildren(child, result);
            }
        }
    }
}
