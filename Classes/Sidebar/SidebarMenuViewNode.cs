
using System;
using System.Collections.Generic;
using FontAwesome.Sharp;
using System.Windows.Forms;

namespace DuckFarm.Classes.Sidebar
{
    /// <summary>
    /// Menghubungkan satu node MenuTree dengan
    /// kontrol WinForms yang menampilkan node tersebut.
    /// </summary>
    internal sealed class SidebarMenuViewNode
    {
        private readonly List<SidebarMenuViewNode> _children =
            new List<SidebarMenuViewNode>();

        public SidebarMenuNode Node { get; }

        public IconButton Button { get; }

        public SidebarMenuViewNode Parent { get; private set; }

        public Panel Container { get; private set; }

        public Panel ChildrenPanel { get; private set; }

        public bool IsExpanded { get; set; }

        internal void SetContainer(Panel container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (Container != null &&
                !ReferenceEquals(Container, container))
            {
                throw new InvalidOperationException(
                    "Node tampilan sudah memiliki wadah.");
            }

            Container = container;
        }

        public IReadOnlyList<SidebarMenuViewNode> Children  
        {
            get { return _children.AsReadOnly(); }
        }
        internal void SetChildrenPanel(Panel childrenPanel)
        {
            if (childrenPanel == null)
            {
                throw new ArgumentNullException(
                    nameof(childrenPanel));
            }

            if (ChildrenPanel != null &&
                !ReferenceEquals(ChildrenPanel, childrenPanel))
            {
                throw new InvalidOperationException(
                    "Node tampilan sudah memiliki panel anak.");
            }

            ChildrenPanel = childrenPanel;
        }

        public SidebarMenuViewNode(
            SidebarMenuNode node,
            IconButton button,
            Panel childrenPanel)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (button == null)
            {
                throw new ArgumentNullException(nameof(button));
            }

            Node = node;
            Button = button;
            ChildrenPanel = childrenPanel;
        }


        internal void AddChild(SidebarMenuViewNode child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child));
            }

            if (ReferenceEquals(child, this))
            {
                throw new InvalidOperationException(
                    "Node tampilan tidak boleh menjadi anak dirinya sendiri.");
            }

            if (child.Parent != null)
            {
                throw new InvalidOperationException(
                    "Node tampilan sudah memiliki induk.");
            }

            SidebarMenuViewNode ancestor = this;

            while (ancestor != null)
            {
                if (ReferenceEquals(ancestor, child))
                {
                    throw new InvalidOperationException(
                        "Hierarki node tampilan tidak boleh membentuk siklus.");
                }

                ancestor = ancestor.Parent;
            }

            _children.Add(child);
            child.Parent = this;
        }

    }
}
