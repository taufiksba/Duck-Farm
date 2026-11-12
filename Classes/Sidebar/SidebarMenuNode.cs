
using System;
using System.Collections.Generic;

namespace DuckFarm.Classes.Sidebar
{
    public sealed class SidebarMenuNode
    {
        private readonly List<SidebarMenuNode> _children =
            new List<SidebarMenuNode>();

        private string _text;

        public int MenuIndex { get; }

        public string MenuId { get; }

        public string Text
        {   
            get { return _text; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(
                        "Nama menu tidak boleh kosong.",
                        nameof(value));
                }

                _text = value.Trim();
            }
        }

        public string IconKey { get; set; }

        public string ActionKey { get; set; }

        public int SortOrder { get; set; }

        public int? ParentMenuIndex
        {
            get
            {
                return Parent == null
                    ? (int?)null
                    : Parent.MenuIndex;
            }
        }

        public SidebarMenuNode Parent { get; private set; }

        public IReadOnlyList<SidebarMenuNode> Children
        {
            get { return _children.AsReadOnly(); }
        }

        public bool IsExpanded { get; set; }

        public bool IsVisible { get; set; } = true;

        public bool IsEnabled { get; set; } = true;

        public SidebarMenuNode(
            int menuIndex,
            string menuId,
            string text,
            string iconKey = "",
            string actionKey = "",
            int sortOrder = 0)
        {
            if (menuIndex <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(menuIndex),
                    "MenuIndex harus lebih besar dari 0.");
            }

            if (string.IsNullOrWhiteSpace(menuId))
            {
                throw new ArgumentException(
                    "MenuId tidak boleh kosong.",
                    nameof(menuId));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(
                    "Nama menu tidak boleh kosong.",
                    nameof(text));
            }

            MenuIndex = menuIndex;
            MenuId = menuId.Trim();
            _text = text.Trim();
            IconKey = iconKey ?? string.Empty;
            ActionKey = actionKey ?? string.Empty;
            SortOrder = sortOrder;
        }

        internal void AttachChild(SidebarMenuNode child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child));
            }

            if (ReferenceEquals(this, child))
            {
                throw new InvalidOperationException(
                    "Menu tidak boleh menjadi anak dari dirinya sendiri.");
            }

            if (child.Parent != null)
            {
                throw new InvalidOperationException(
                    "Menu ini sudah memiliki parent.");
            }

            for (SidebarMenuNode current = this;
                 current != null;
                 current = current.Parent)
            {
                if (ReferenceEquals(current, child))
                {
                    throw new InvalidOperationException(
                        "Penambahan menu akan membentuk siklus.");
                }
            }

            child.Parent = this;
            _children.Add(child);
        }
    }
}
