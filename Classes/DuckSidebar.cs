
using FontAwesome.Sharp;
using DuckFarm.Classes.Sidebar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DuckFarm.Classes
{

    public sealed class DuckSidebar : UserControl
    {
        private readonly Panel headerPanel;
        private readonly Panel menuPanel;
        private readonly IconButton toggleButton;
        private readonly Label titleLabel;
        private readonly Label subtitleLabel;
        private readonly Panel brandPanel;
        private readonly ToolTip toolTip;
        private Label logoLabel;
        private readonly IconButton logoutButton;

        // Daftar form MDI yang ditampilkan secara dinamis
        private Panel windowsFormsPanel;

        public event Action<Form> WindowsFormSelected;

        private readonly Dictionary<string, MenuGroup> groups =
            new Dictionary<string, MenuGroup>(
                StringComparer.OrdinalIgnoreCase);

        // Sumber struktur menu utama.
        private readonly SidebarMenuTree menuTree =
            new SidebarMenuTree();

        // Menghubungkan nama grup tampilan dengan node struktur menu.
        private readonly Dictionary<string, SidebarMenuNode> groupNodes =
            new Dictionary<string, SidebarMenuNode>(
                StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<int, SidebarMenuViewNode>
            viewNodesByMenuIndex =
                new Dictionary<int, SidebarMenuViewNode>();
        private readonly HashSet<int> viewNodeClickHandlers =
            new HashSet<int>();

        // Nomor menu sementara berdasarkan urutan pendaftaran.
        private int nextMenuIndex = 1;

        public SidebarMenuTree MenuTree
        {
            get { return menuTree; }
        }

        private bool isCollapsed;
        private bool isUpdatingLayout;
        private bool isApplyingTheme;

        private const int ExpandedWidth = 260;
        private const int CollapsedWidth = 64;
        private const int HeaderHeight = 64;
        private const int GroupHeaderHeight = 44;
        private const int MenuItemMinHeight = 42;
        private const int SubMenuIndent = 18;

        private bool isDarkTheme = true;

        private Color BackgroundColor =>
            isDarkTheme ? Color.FromArgb(20, 27, 38) : Color.FromArgb(248, 250, 252);

        private Color PanelBackgroundColor =>
            isDarkTheme
                ? Color.Transparent
                : BackgroundColor;

        private Color MenuItemBackgroundColor =>
            isDarkTheme
                ? Color.Transparent
                : BackgroundColor;
        private Color MenuItemHoverColor =>
            isDarkTheme
                ? Color.FromArgb(65, 80, 100)
                : Color.FromArgb(226, 237, 249);

        private Color HeaderBackgroundColor =>
            isDarkTheme ? Color.FromArgb(27, 37, 51) : Color.FromArgb(238, 242, 246);

        private Color HeaderBorderColor =>
            isDarkTheme
        ? Color.FromArgb(65, 82, 102)
        : HeaderBackgroundColor;

        private Color ForegroundColor =>
            isDarkTheme
                ? Color.FromArgb(235, 241, 248)
                : Color.FromArgb(38, 49, 62);

        private Color SecondaryTextColor =>
            isDarkTheme
                ? Color.FromArgb(160, 178, 199)
                : Color.FromArgb(100, 116, 139);
        private Color MenuTextColor =>
            isDarkTheme
                ? ForegroundColor
                : Color.FromArgb(36, 91, 145); // NetSuite Blue

        private Color MenuIconColor =>
            isDarkTheme
                ? SecondaryTextColor
                : Color.FromArgb(36, 91, 145); // NetSuite Blue
        private Color HoverColor =>
            isDarkTheme
                ? Color.FromArgb(48, 66, 86)
                : Color.FromArgb(226, 232, 240);

        private Color ActiveColor =>
            isDarkTheme
                ? Color.FromArgb(57, 82, 108)
                : Color.FromArgb(215, 224, 234);

        private Color BorderColor =>
            isDarkTheme
                ? Color.FromArgb(70, 91, 115)
                : Color.FromArgb(220, 227, 235);

        public event EventHandler ToggleChanged;
        public event Action<string> MenuSelected;

        public bool IsCollapsed => isCollapsed;

        public Panel MenuPanel => menuPanel;

        public DuckSidebar()
        {
            Dock = DockStyle.Left;
            Width = ExpandedWidth;
            MinimumSize = new Size(CollapsedWidth, 0);
            BackColor = BackgroundColor;

            toolTip = new ToolTip
            {
                ShowAlways = true,
                InitialDelay = 350,
                ReshowDelay = 100
            };

            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = HeaderHeight,
                Padding = new Padding(0),
                BackColor = HeaderBackgroundColor
            };

            toggleButton = CreateIconButton(IconChar.Bars);

            toggleButton.Dock = DockStyle.Left;
            toggleButton.Width = CollapsedWidth;
            toggleButton.IconSize = 21;
            toggleButton.Text = string.Empty;
            toggleButton.TextImageRelation =
                TextImageRelation.Overlay;
            toggleButton.ImageAlign =
                ContentAlignment.MiddleCenter;

            toggleButton.Click += ToggleButton_Click;


            titleLabel = new Label
            {
                Text = "DuckFarm",
                Dock = DockStyle.Top,
                Height = 27,
                ForeColor = ForegroundColor,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.BottomLeft,
                Font = new Font(
                    "Segoe UI Semibold",
                    12F,
                    FontStyle.Bold),
                AutoEllipsis = true,
                Padding = new Padding(0, 0, 4, 0)
            };

            subtitleLabel = new Label
            {
                Text = "FARM MANAGEMENT",
                Dock = DockStyle.Fill,
                ForeColor = SecondaryTextColor,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Segoe UI", 7.0F),
                AutoEllipsis = true,
                Padding = new Padding(0, 0, 4, 0)
            };

            brandPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = HeaderBackgroundColor,
                Padding = new Padding(5, 4, 4, 2)
            };

            // Identitas visual sederhana untuk DuckFarm.
            logoLabel = new Label
            {
                Dock = DockStyle.Left,
                Width = 38,
                BackColor = Color.FromArgb(37, 99, 166),
                ForeColor = Color.White,
                Text = "D",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            Panel brandTextPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = HeaderBackgroundColor,
                Padding = new Padding(8, 0, 0, 0)
            };
            brandTextPanel.Controls.Add(subtitleLabel);
            brandTextPanel.Controls.Add(titleLabel);

            brandPanel.Controls.Add(brandTextPanel);
            brandPanel.Controls.Add(logoLabel);

            menuPanel = new GlassPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = BackgroundColor,
                Padding = new Padding(4)
            };
            headerPanel.Controls.Add(brandPanel);
            headerPanel.Controls.Add(toggleButton);

            Controls.Add(menuPanel);
            Controls.Add(headerPanel);

            ApplyCollapsedState();
            ApplyTheme();
        }

        // =====================================================
        // TEMA
        // =====================================================

        public void SetDarkTheme(bool dark)
        {
            isDarkTheme = dark;
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            if (isApplyingTheme)
                return;

            isApplyingTheme = true;

            try
            {

                BackColor = BackgroundColor;
                headerPanel.BackColor = HeaderBackgroundColor;
                brandPanel.BackColor = HeaderBackgroundColor;
                logoLabel.BackColor = HeaderBackgroundColor;

                if (menuPanel is GlassPanel glassMenu)
                {
                    glassMenu.DarkGlass = isDarkTheme;
                    glassMenu.BackColor = BackgroundColor;
                    glassMenu.Invalidate();
                }

                titleLabel.ForeColor = ForegroundColor;
                subtitleLabel.ForeColor = SecondaryTextColor;

                toggleButton.BackColor = HeaderBackgroundColor;
                toggleButton.ForeColor = ForegroundColor;
                toggleButton.IconColor = ForegroundColor;

                ApplyButtonColors(toggleButton);


                foreach (MenuGroup group in groups.Values)
                {
                    group.Container.BackColor = isDarkTheme
                        ? Color.Transparent
                        : BackgroundColor;

                    group.SubMenu.BackColor = isDarkTheme
                        ? Color.Transparent
                        : BackgroundColor;

                    ApplyGroupState(group);

                    // Panel submenu bertingkat lama.
                    foreach (NestedMenuData parent in group.NestedMenus)
                    {
                        parent.Container.BackColor = PanelBackgroundColor;
                        parent.ChildPanel.BackColor = PanelBackgroundColor;
                    }

                    // Terapkan warna tema ke semua menu bertingkat
                    // dalam grup yang sedang diproses.
                    if (!groupNodes.TryGetValue(
                            group.Name,
                            out SidebarMenuNode groupRoot))
                    {
                        continue;
                    }

                    foreach (SidebarMenuViewNode viewNode
                        in viewNodesByMenuIndex.Values)
                    {
                        if (viewNode == null ||
                            viewNode.Button == null)
                        {
                            continue;
                        }

                        // Cari grup induk dari node ini.
                        SidebarMenuViewNode rootNode = viewNode;

                        while (rootNode.Parent != null)
                        {
                            rootNode = rootNode.Parent;
                        }

                        if (!ReferenceEquals(rootNode.Node, groupRoot))
                        {
                            continue;
                        }

                        //// Samakan warna dengan menu utama.

                        //viewNode.Button.BackColor = BackgroundColor;
                        viewNode.Button.BackColor = MenuItemBackgroundColor;
                        viewNode.Button.ForeColor = MenuTextColor;
                        viewNode.Button.IconColor = MenuIconColor;

                        ApplyButtonColors(viewNode.Button);
                    }
                }
                // Panel untuk menu bertingkat yang menggunakan SidebarMenuViewNode.
                foreach (SidebarMenuViewNode viewNode
                    in viewNodesByMenuIndex.Values)
                {
                    if (viewNode == null)
                        continue;

                    if (viewNode.Container != null)
                        viewNode.Container.BackColor = PanelBackgroundColor;

                    if (viewNode.ChildrenPanel != null)
                        viewNode.ChildrenPanel.BackColor = PanelBackgroundColor;
                }



                if (windowsFormsPanel != null)
                {
                    foreach (Control control in windowsFormsPanel.Controls)
                    {
                        Button button = control as Button;

                        if (button == null)
                            continue;

                        button.BackColor = MenuItemBackgroundColor;
                        button.ForeColor = MenuTextColor;

                        button.FlatStyle = FlatStyle.Flat;
                        button.UseVisualStyleBackColor = false;

                        button.FlatAppearance.BorderSize = 0;
                        button.FlatAppearance.MouseOverBackColor =
                            MenuItemHoverColor;

                        button.FlatAppearance.MouseDownBackColor =
                            MenuItemHoverColor;
                    }
                }



                Invalidate(true);
            }
            finally
            {
                isApplyingTheme = false;
            }
        }

        private void ApplyButtonColors(IconButton button)
        {
            if (button == null)
                return;

            button.FlatStyle = FlatStyle.Flat;
            button.UseVisualStyleBackColor = false;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = HoverColor;
            button.FlatAppearance.MouseDownBackColor = ActiveColor;
        }

        // Kompatibilitas untuk pemanggilan lama.
        public void AddGroup(string groupName, string icon)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                throw new ArgumentException(
                    "Nama group menu wajib diisi.",
                    nameof(groupName));
            }

            AddGroup(
                groupName,
                icon,
                GetGroupMenuId(groupName));
        }

        // =====================================================
        // GRUP MENU
        // =====================================================

        public void AddGroup(
            string groupName,
            string icon,
            string menuId)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                throw new ArgumentException(
                    "Nama group menu wajib diisi.",
                    nameof(groupName));
            }

            if (groups.ContainsKey(groupName))
                return;

            MenuGroup group = new MenuGroup
            {
                Name = groupName,
                Icon = GetGroupIcon(groupName),
                IsExpanded = false
            };

            if (string.IsNullOrWhiteSpace(menuId))
            {
                throw new ArgumentException(
                    "MenuId group wajib diisi.",
                    nameof(menuId));
            }
            group.Container = new Panel
            {
                Dock = DockStyle.Top,
                Height = GroupHeaderHeight,
                BackColor = BackgroundColor,
                Margin = new Padding(0)
            };

            group.Header = CreateIconButton(group.Icon);
            group.Header.Dock = DockStyle.Top;
            group.Header.Height = GroupHeaderHeight;
            group.Header.Font = new Font(
                "Segoe UI Semibold",
                9.5F,
                FontStyle.Bold);
            group.Header.TextAlign = ContentAlignment.MiddleLeft;
            group.Header.TextImageRelation =
                TextImageRelation.ImageBeforeText;
            group.Header.Padding = new Padding(8, 0, 4, 0);
            group.Header.ImageAlign = ContentAlignment.MiddleLeft;
            group.Header.Tag = group;
            group.Header.Click += GroupHeader_Click;

            group.SubMenu = new Panel
            {
                Dock = DockStyle.Top,
                Height = 0,
                BackColor = BackgroundColor,
                Margin = new Padding(0),
                Padding = new Padding(SubMenuIndent, 0, 0, 0)
            };

            group.Container.Controls.Add(group.SubMenu);
            group.Container.Controls.Add(group.Header);

            menuPanel.Controls.Add(group.Container);
            menuPanel.Controls.SetChildIndex(
                group.Container, 0);

            groups.Add(groupName, group);

            // Daftarkan grup sebagai root menu.
            string groupMenuId = GetGroupMenuId(groupName);

            SidebarMenuNode groupNode = new SidebarMenuNode(
                nextMenuIndex++,
                menuId.Trim(),
                groupName,
                icon,
                sortOrder: groups.Count);

            menuTree.AddNode(groupNode);

            groupNodes.Add(groupName, groupNode);

            viewNodesByMenuIndex.Add(
                groupNode.MenuIndex,
                new SidebarMenuViewNode(
                    groupNode,
                    group.Header,
                    group.SubMenu));

            ApplyGroupState(group);
            ApplyTheme();
        }

        private string GetGroupMenuId(string groupName)
        {
            switch (groupName.Trim().ToUpperInvariant())
            {
                case "DASHBOARD":
                    return "dashboard";

                case "MASTER":
                    return "master";

                case "TRANSAKSI":
                    return "transaksi";

                case "MONITORING":
                    return "monitoring";

                case "REPORT":
                    return "report";

                case "UTILITY":
                    return "utility";

                case "WINDOWS":
                    return "windows";

                case "APPLICATION":
                    return "application";

                default:
                    throw new ArgumentException(
                        "Group menu belum memiliki MenuId: " + groupName,
                        nameof(groupName));
            }
        }


        private string GetMenuId(string groupName, string text)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                throw new ArgumentException(
                    "Nama grup menu wajib diisi.",
                    nameof(groupName));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(
                    "Nama menu wajib diisi.",
                    nameof(text));
            }

            string group = groupName.Trim().ToLowerInvariant();
            string menu = text.Trim().ToLowerInvariant();

            // Pertahankan MenuId lama yang sudah digunakan.
            if (group == "dashboard" && menu == "dashboard")
                return "dashboard.home";

            if (group == "master")
            {
                if (menu == "pemborong")
                    return "master.pemborong";

                if (menu == "pekerjaan")
                    return "master.pekerjaan";

                if (menu == "tenaga kerja")
                    return "master.tenaga-kerja";
            }

            if (group == "windows")
            {
                if (menu == "close all")
                    return "windows.close-all";

                if (menu == "tile horizontally")
                    return "windows.tile-horizontal";

                if (menu == "tile vertically")
                    return "windows.tile-vertical";

                if (menu == "cascade")
                    return "windows.cascade";
            }

            if (group == "application" && menu == "exit")
                return "application.exit";

            // Menu lain mendapatkan ID berdasarkan nama grup dan menu.
            return group + "." + ToMenuIdSegment(menu);
        }

        private string GetNestedMenuId(
            string groupName,
            string parentText,
            string childText,
            bool isParent)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                throw new ArgumentException(
                    "Nama grup menu wajib diisi.",
                    nameof(groupName));
            }

            if (string.IsNullOrWhiteSpace(parentText))
            {
                throw new ArgumentException(
                    "Nama menu induk wajib diisi.",
                    nameof(parentText));
            }

            if (!isParent && string.IsNullOrWhiteSpace(childText))
            {
                throw new ArgumentException(
                    "Nama submenu wajib diisi.",
                    nameof(childText));
            }

            string group = groupName.Trim().ToLowerInvariant();
            string parent = parentText.Trim().ToLowerInvariant();

            string parentMenuId =
                group + "." + ToMenuIdSegment(parent);

            if (isParent)
            {
                return parentMenuId;
            }

            string child = childText.Trim().ToLowerInvariant();

            return parentMenuId + "." + ToMenuIdSegment(child);
        }

        private string ToMenuIdSegment(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(
                    "Nama menu tidak boleh kosong.",
                    nameof(text));
            }

            string normalized = text.Trim().ToLowerInvariant();
            System.Text.StringBuilder result =
                new System.Text.StringBuilder();

            bool previousWasSeparator = false;

            foreach (char character in normalized)
            {
                if (char.IsLetterOrDigit(character))
                {
                    result.Append(character);
                    previousWasSeparator = false;
                }
                else if (!previousWasSeparator && result.Length > 0)
                {
                    result.Append('-');
                    previousWasSeparator = true;
                }
            }

            return result.ToString().Trim('-');
        }


        private void AddGroupIfMissing(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                throw new ArgumentException(
                    "Nama group menu wajib diisi.",
                    nameof(groupName));
            }

            if (!groups.ContainsKey(groupName))
                AddGroup(groupName, string.Empty);
        }

        // =====================================================
        // SUBMENU TINGKAT PERTAMA
        // =====================================================

        public Button AddMenuItem(
            string text,
            string icon,
            EventHandler onClick)
        {
            return AddMenuItem(
                "Master",
                text,
                icon,
                onClick);
        }

        public Button AddMenuItem(
            string groupName,
            string text,
            string icon,
            EventHandler onClick)
        {
            return AddMenuItem(
                groupName,
                text,
                icon,
                GetMenuId(groupName, text),
                onClick);
        }

        public Button AddMenuItem(
            string groupName,
            string text,
            string icon,
            string menuId,
            EventHandler onClick)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(
                    "Nama submenu wajib diisi.",
                    nameof(text));
            }

            if (string.IsNullOrWhiteSpace(menuId))
            {
                throw new ArgumentException(
                    "MenuId tidak boleh kosong.",
                    nameof(menuId));
            }

            AddGroupIfMissing(groupName);

            MenuGroup group = groups[groupName];

            IconButton button = CreateIconButton(
                GetMenuIcon(text));

            button.Dock = DockStyle.Top;
            button.MinimumSize =
                new Size(0, MenuItemMinHeight);
            button.Height = MenuItemMinHeight;
            button.Font = new Font("Segoe UI", 9.25F);
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.TextImageRelation =
                TextImageRelation.ImageBeforeText;
            button.ImageAlign = ContentAlignment.MiddleLeft;
            button.Padding = new Padding(4, 5, 6, 5);
            button.Margin = new Padding(0);
            button.Tag = new MenuItemData(text, icon);

            button.SizeChanged += (sender, e) =>
            {
                UpdateMenuItemHeight(button);
            };


            button.Click += (sender, e) =>
            {
                NotifyMenuSelected(text);

                if (onClick != null)
                {
                    onClick(sender, e);
                }
            };

            toolTip.SetToolTip(button, text);

            group.SubMenu.Controls.Add(button);
            group.SubMenu.Controls.SetChildIndex(button, 0);
            group.Items.Add(button);

            // Catat submenu pada struktur tree.
            SidebarMenuNode parentNode = groupNodes[groupName];

            SidebarMenuNode menuNode = new SidebarMenuNode(
                nextMenuIndex++,
                menuId.Trim(),
                text,
                icon,
                sortOrder: group.Items.Count);

            menuTree.AddNode(menuNode, parentNode.MenuIndex);

            viewNodesByMenuIndex.Add(
            menuNode.MenuIndex,
            new SidebarMenuViewNode(
                menuNode,
                button,
                null));

            viewNodesByMenuIndex[parentNode.MenuIndex].AddChild(
                viewNodesByMenuIndex[menuNode.MenuIndex]);

            UpdateMenuItemHeight(button);
            ApplyGroupState(group);
            ApplyTheme();

            return button;
        }

        // =====================================================
        // SUBMENU BERTINGKAT
        // =====================================================

        public Button AddNestedMenuItem(
            string groupName,
            string parentText,
            string childText,
            string icon,
            EventHandler onClick)
        {
            return AddNestedMenuItem(
                groupName,
                parentText,
                childText,
                icon,
                GetNestedMenuId(
                    groupName,
                    parentText,
                    string.Empty,
                    true),
                GetNestedMenuId(
                    groupName,
                    parentText,
                    childText,
                    false),
                onClick);
        }

        public Button AddNestedMenuItem(
            string groupName,
            string parentText,
            string childText,
            string icon,
            string parentMenuId,
            string childMenuId,
            EventHandler onClick)
        {
            if (string.IsNullOrWhiteSpace(parentText))
            {
                throw new ArgumentException(
                    "Nama menu induk wajib diisi.",
                    nameof(parentText));
            }

            if (string.IsNullOrWhiteSpace(childText))
            {
                throw new ArgumentException(
                    "Nama submenu wajib diisi.",
                    nameof(childText));
            }

            if (string.IsNullOrWhiteSpace(parentMenuId))
            {
                throw new ArgumentException(
                    "MenuId induk tidak boleh kosong.",
                    nameof(parentMenuId));
            }

            if (string.IsNullOrWhiteSpace(childMenuId))
            {
                throw new ArgumentException(
                    "MenuId anak tidak boleh kosong.",
                    nameof(childMenuId));
            }

            AddGroupIfMissing(groupName);

            MenuGroup group = groups[groupName];

            NestedMenuData parent = group.NestedMenus.Find(
                item => string.Equals(
                    item.Text,
                    parentText,
                    StringComparison.OrdinalIgnoreCase));

            if (parent == null)
            {
                IconButton parentButton = CreateIconButton(
                    GetMenuIcon(parentText));

                parentButton.Dock = DockStyle.None;
                parentButton.Location = new Point(0, 0);
                parentButton.MinimumSize =
                    new Size(0, MenuItemMinHeight);
                parentButton.Height = MenuItemMinHeight;
                parentButton.Font = new Font("Segoe UI", 9.25F);
                parentButton.TextAlign =
                    ContentAlignment.MiddleLeft;
                parentButton.TextImageRelation =
                    TextImageRelation.ImageBeforeText;
                parentButton.ImageAlign =
                    ContentAlignment.MiddleLeft;
                parentButton.Padding = new Padding(4, 5, 6, 5);
                parentButton.Margin = new Padding(0);

                Panel childPanel = new Panel
                {
                    Dock = DockStyle.None,
                    Location = new Point(0, MenuItemMinHeight),
                    Height = 0,
                    Visible = false,
                    Margin = new Padding(0),
                    Padding = new Padding(
                        SubMenuIndent, 0, 0, 0),
                    //BackColor = BackgroundColor
                    BackColor = PanelBackgroundColor
                };


                Panel parentContainer = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = MenuItemMinHeight,
                    Margin = new Padding(0),
                    Padding = new Padding(0),
                    //BackColor = BackgroundColor
                    BackColor = PanelBackgroundColor
                };

                parentContainer.Controls.Add(parentButton);
                parentContainer.Controls.Add(childPanel);

                // Tombol induk harus berada di atas panel anak.
                parentContainer.Controls.SetChildIndex(
                    parentButton, 0);

                parentContainer.Controls.SetChildIndex(
                    childPanel, 1);


                SidebarMenuNode parentNode = new SidebarMenuNode(
                    nextMenuIndex++,
                    parentMenuId.Trim(),
                    parentText,
                    sortOrder: group.Items.Count);

                menuTree.AddNode(
                parentNode,
                groupNodes[groupName].MenuIndex);

                viewNodesByMenuIndex.Add(
                    parentNode.MenuIndex,
                    new SidebarMenuViewNode(
                        parentNode,
                        parentButton,
                        childPanel));
                // Hubungkan menu induk dengan group tampilan.
                viewNodesByMenuIndex[
                    groupNodes[groupName].MenuIndex
                ].AddChild(
                    viewNodesByMenuIndex[parentNode.MenuIndex]);

                parent = new NestedMenuData(
                    parentText,
                    parentButton,
                    childPanel,
                    parentContainer,
                    parentNode);

                parentButton.Tag = parent;

                NestedMenuData capturedParent = parent;

                parentButton.Click += (sender, e) =>
                {
                    if (isCollapsed)
                        SetCollapsed(false);

                    capturedParent.IsExpanded =
                        !capturedParent.IsExpanded;

                    ApplyNestedMenuState(group, capturedParent);
                    UpdateGroupHeight(group);
                };

                toolTip.SetToolTip(parentButton, parentText);

                group.SubMenu.Controls.Add(parentContainer);

                group.Items.Add(parentButton);
                group.NestedMenus.Add(parent);
            }
            else if (!string.Equals(
                parent.Node.MenuId,
                parentMenuId.Trim(),
                StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Menu induk '" + parentText +
                    "' sudah terdaftar dengan MenuId '" +
                    parent.Node.MenuId + "'.");
            }

            // Hindari duplikasi nama anak dalam menu induk yang sama.
            foreach (IconButton existingChild in parent.Children)
            {
                MenuItemData existingData =
                    existingChild.Tag as MenuItemData;

                if (existingData != null &&
                    string.Equals(
                        existingData.Text,
                        childText,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return existingChild;
                }
            }

            IconButton childButton = CreateIconButton(GetMenuIcon(childText));

            childButton.Dock = DockStyle.Top;
            childButton.MinimumSize =
                new Size(0, MenuItemMinHeight);
            childButton.Height = MenuItemMinHeight;
            childButton.Font = new Font("Segoe UI", 9.25F);
            childButton.TextAlign = ContentAlignment.MiddleLeft;
            childButton.TextImageRelation =
                TextImageRelation.ImageBeforeText;
            childButton.ImageAlign = ContentAlignment.MiddleLeft;
            childButton.Padding = new Padding(4, 5, 6, 5);
            childButton.Margin = new Padding(0);
            childButton.Tag = new MenuItemData(childText, icon);

            //if (onClick != null)
            //    childButton.Click += onClick;
            childButton.Click += (sender, e) =>
            {
                NotifyMenuSelected(childText);

                if (onClick != null)
                {
                    onClick(sender, e);
                }
            };

            childButton.SizeChanged += (sender, e) =>
            {
                UpdateMenuItemHeight(childButton);
            };

            toolTip.SetToolTip(childButton, childText);

            parent.ChildPanel.Controls.Add(childButton);

            parent.ChildPanel.Controls.SetChildIndex(
                childButton, 0);

            parent.Children.Add(childButton);

            // Catat anak menu ke SidebarMenuTree.
            SidebarMenuNode childNode = new SidebarMenuNode(
                nextMenuIndex++,
                childMenuId.Trim(),
                childText,
                icon,
                actionKey: string.Empty,
                sortOrder: parent.Children.Count);

            menuTree.AddNode(
                childNode,
                parent.Node.MenuIndex);

            viewNodesByMenuIndex.Add(
                childNode.MenuIndex,
                new SidebarMenuViewNode(
                    childNode,
                    childButton,
                    null));

            viewNodesByMenuIndex[parent.Node.MenuIndex].AddChild(
                viewNodesByMenuIndex[childNode.MenuIndex]);

            ApplyNestedMenuState(group, parent);
            ApplyGroupState(group);
            ApplyTheme();

            return childButton;
        }
        public IconButton AddChildMenuItem(
            string parentMenuId,
            string childText,
            string icon,
            string childMenuId,
            EventHandler onClick)
        {
            if (string.IsNullOrWhiteSpace(parentMenuId))
            {
                throw new ArgumentException(
                    "Parent MenuId tidak boleh kosong.",
                    nameof(parentMenuId));
            }

            if (string.IsNullOrWhiteSpace(childText))
            {
                throw new ArgumentException(
                    "Nama menu anak tidak boleh kosong.",
                    nameof(childText));
            }

            if (string.IsNullOrWhiteSpace(childMenuId))
            {
                throw new ArgumentException(
                    "Child MenuId tidak boleh kosong.",
                    nameof(childMenuId));
            }

            SidebarMenuNode parentNode =
                menuTree.FindByMenuId(parentMenuId);

            if (parentNode == null)
            {
                throw new InvalidOperationException(
                    "Parent menu tidak ditemukan: " + parentMenuId);
            }

            SidebarMenuViewNode parentViewNode =
                FindViewNodeByMenuId(parentMenuId);

            if (parentViewNode == null)
            {
                throw new InvalidOperationException(
                    "Parent view node tidak ditemukan: " + parentMenuId);
            }

            if (menuTree.FindByMenuId(childMenuId) != null)
            {
                throw new InvalidOperationException(
                    "Child MenuId sudah digunakan: " + childMenuId);
            }

            MenuGroup group = null;
            SidebarMenuNode ancestorNode = parentNode;

            while (ancestorNode != null)
            {
                foreach (KeyValuePair<string, SidebarMenuNode> entry
                    in groupNodes)
                {
                    if (ReferenceEquals(entry.Value, ancestorNode))
                    {
                        if (groups.TryGetValue(
                            entry.Key,
                            out MenuGroup foundGroup))
                        {
                            group = foundGroup;
                        }

                        break;
                    }
                }

                if (group != null)
                {
                    break;
                }

                ancestorNode = ancestorNode.Parent;
            }

            if (group == null)
            {
                throw new InvalidOperationException(
                    "Grup sidebar untuk parent tidak ditemukan: " +
                    parentMenuId);
            }

            Panel childrenPanel =
                EnsureChildrenPanel(parentViewNode);

            IconButton childButton = CreateIconButton(
                GetMenuIcon(childText));

            // Samakan tampilan tombol dengan AddMenuItem().
            childButton.Dock = DockStyle.Top;
            childButton.MinimumSize =
                new Size(0, MenuItemMinHeight);
            childButton.Height = MenuItemMinHeight;
            childButton.Font = new Font("Segoe UI", 9.25F);
            childButton.Text = childText;
            childButton.TextAlign = ContentAlignment.MiddleLeft;
            childButton.TextImageRelation =
                TextImageRelation.ImageBeforeText;
            childButton.ImageAlign = ContentAlignment.MiddleLeft;

            int menuDepth = 0;
            SidebarMenuNode currentParent = parentNode;

            while (currentParent.Parent != null &&
                   !groupNodes.ContainsValue(currentParent.Parent))
            {
                menuDepth++;
                currentParent = currentParent.Parent;
            }

            childButton.Padding = new Padding(
                4 + (SubMenuIndent * (menuDepth + 1)),
                5,
                6,
                5);
            childButton.Margin = new Padding(0);
            childButton.Tag = new MenuItemData(childText, icon);
            childButton.Visible = false;

            // Samakan warna dasar dengan menu utama.
            childButton.BackColor = MenuItemBackgroundColor;
            childButton.ForeColor = MenuTextColor;
            childButton.IconColor = MenuIconColor;

            // Sesuaikan tinggi ketika ukuran tombol berubah.
            childButton.SizeChanged += (sender, e) =>
            {
                UpdateMenuItemHeight(childButton);
            };

            childButton.Click += (sender, e) =>
            {
                NotifyMenuSelected(childText);

                if (onClick != null)
                {
                    onClick(sender, e);
                }
            };

            SidebarMenuNode childNode = new SidebarMenuNode(
                nextMenuIndex++,
                childMenuId.Trim(),
                childText,
                icon ?? string.Empty);

            menuTree.AddNode(childNode, parentNode.MenuIndex);

            SidebarMenuViewNode childViewNode =
                new SidebarMenuViewNode(
                    childNode,
                    childButton,
                    null);

            parentViewNode.AddChild(childViewNode);

            // Hubungkan event klik induk satu kali saja.
            if (viewNodeClickHandlers.Add(parentNode.MenuIndex))
            {
                parentViewNode.Button.Click += ParentViewNode_Click;
            }

            viewNodesByMenuIndex.Add(
                childNode.MenuIndex,
                childViewNode);

            childrenPanel.Controls.Add(childButton);

            childrenPanel.Controls.SetChildIndex(
                childButton,
                0);

            toolTip.SetToolTip(
                childButton,
                childText);

            ApplyButtonColors(childButton);
            UpdateMenuItemHeight(childButton);

            return childButton;
        }

        private void ApplyNestedMenuState(
            MenuGroup group,
            NestedMenuData parent)
        {
            if (group == null || parent == null)
                return;

            //parent.Button.BackColor = BackgroundColor;
            parent.Button.BackColor = MenuItemBackgroundColor;
            //parent.Button.ForeColor = ForegroundColor;
            //parent.Button.IconColor = SecondaryTextColor;
            parent.Button.ForeColor = MenuTextColor;
            parent.Button.IconColor = MenuIconColor;

            parent.Button.Text = isCollapsed
                ? string.Empty
                : parent.Text +
                  (parent.IsExpanded ? "   ▾" : "   ▸");

            parent.Button.TextAlign = isCollapsed
                ? ContentAlignment.MiddleCenter
                : ContentAlignment.MiddleLeft;

            parent.Button.TextImageRelation = isCollapsed
                ? TextImageRelation.Overlay
                : TextImageRelation.ImageBeforeText;

            parent.Button.ImageAlign = isCollapsed
                ? ContentAlignment.MiddleCenter
                : ContentAlignment.MiddleLeft;

            parent.Button.Padding = isCollapsed
                ? new Padding(0)
                : new Padding(4, 5, 6, 5);

            //parent.ChildPanel.BackColor = BackgroundColor;
            parent.ChildPanel.BackColor = PanelBackgroundColor;

            parent.Container.BackColor = PanelBackgroundColor;

            parent.ChildPanel.Padding = new Padding(
                isCollapsed ? 0 : SubMenuIndent,
                0, 0, 0);

            bool showChildren =
                !isCollapsed &&
                group.IsExpanded &&
                parent.IsExpanded;

            parent.ChildPanel.Visible = showChildren;

            SidebarMenuViewNode parentViewNode =
                FindViewNodeByMenuId(parent.Node.MenuId);

            int childHeight =
                CalculateChildrenHeight(parentViewNode);

            foreach (IconButton child in parent.Children)
            {
                MenuItemData item =
                    child.Tag as MenuItemData;

                if (item == null)
                    continue;

                //child.BackColor = BackgroundColor;
                child.BackColor = MenuItemBackgroundColor;
                child.ForeColor = MenuTextColor;
                child.IconColor = MenuIconColor;

                child.Text = isCollapsed
                    ? string.Empty
                    : item.Text;

                child.TextAlign = isCollapsed
                    ? ContentAlignment.MiddleCenter
                    : ContentAlignment.MiddleLeft;

                child.TextImageRelation = isCollapsed
                    ? TextImageRelation.Overlay
                    : TextImageRelation.ImageBeforeText;

                child.ImageAlign = isCollapsed
                    ? ContentAlignment.MiddleCenter
                    : ContentAlignment.MiddleLeft;

                child.Padding = isCollapsed
                    ? new Padding(0)
                    : new Padding(4, 5, 6, 5);

                ApplyButtonColors(child);
                UpdateMenuItemHeight(child);


            }

            parent.Button.Location = new Point(0, 0);
            parent.Button.Size = new Size(
                parent.Container.ClientSize.Width,
                MenuItemMinHeight);

            parent.ChildPanel.Location = new Point(
                0,
                MenuItemMinHeight);

            parent.ChildPanel.Size = new Size(
                parent.Container.ClientSize.Width,
                showChildren ? childHeight : 0);

            ApplyButtonColors(parent.Button);
            UpdateMenuItemHeight(parent.Button);

        }

       
        private IconButton CreateIconButton(IconChar? icon)
        {
            IconButton button = new IconButton
            {
                IconChar = icon.HasValue ? icon.Value : IconChar.None,
                IconFont = IconFont.Auto,
                IconSize = 18,
                IconColor = MenuIconColor,
                Text = string.Empty,
                ForeColor = MenuTextColor,
                BackColor = MenuItemBackgroundColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false,
                ImageAlign = ContentAlignment.MiddleLeft
            };

            ApplyButtonColors(button);

            return button;
        }


        private IconChar GetGroupIcon(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return IconChar.Folder;

            switch (name.Trim().ToUpperInvariant())
            {
                case "DASHBOARD":
                    return IconChar.Home;

                case "MASTER":
                    return IconChar.Database;

                case "TRANSAKSI":
                    return IconChar.ExchangeAlt;

                case "MONITORING":
                    return IconChar.Eye;

                case "REPORT":
                    return IconChar.FileAlt;

                case "UTILITY":
                    return IconChar.Cog;

                case "WINDOWS":
                    return IconChar.WindowMaximize;

                case "APPLICATION":
                    return IconChar.Desktop;

                default:
                    return IconChar.Folder;
            }
        }

       

        private IconChar GetMenuIcon(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return IconChar.FolderOpen;

            string value = text.Trim().ToUpperInvariant();

            // =========================================
            // UTILITY - MANAJEMEN PENGGUNA
            // =========================================

            if (value.Contains("CREATE USER"))
                return IconChar.Users;

            if (value.Contains("CHANGE PASSWORD"))
                return IconChar.Cog;

            if (value.Contains("MONITOR LOGIN"))
                return IconChar.Eye;

            if (value.Contains("MANAJEMEN PENGGUNA"))
                return IconChar.Users;


            // =========================================
            // UTILITY - MANAJEMEN HAK AKSES
            // =========================================

            if (value.Contains("CREATE GROUP USER"))
                return IconChar.Users;

            if (value.Contains("MASTER SECURITY"))
                return IconChar.Lock;

            if (value.Contains("MANAJEMEN HAK AKSES"))
                return IconChar.ShieldAlt;


            // =========================================
            // UTILITY - PENGATURAN APLIKASI
            // =========================================

            if (value.Contains("PENGATURAN APLIKASI") ||
                value.Contains("APPLICATION SETTINGS"))
                return IconChar.Cog;


            // =========================================
            // MASTER DATA
            // =========================================

            if (value.Contains("TENAGA KERJA") ||
                value.Contains("KARYAWAN"))
                return IconChar.Users;

            if (value.Contains("PEKERJAAN"))
                return IconChar.Briefcase;

            if (value.Contains("PEMBORONG"))
                return IconChar.Handshake;

            if (value.Contains("PARAMETER UPAH"))
                return IconChar.Calculator;

            if (value.Contains("DEPARTEMEN") ||
                value.Contains("PERUSAHAAN"))
                return IconChar.Database;


            // =========================================
            // TRANSAKSI DAN MONITORING
            // =========================================

            if (value.Contains("ABSENSI"))
                return IconChar.CalendarAlt;

            if (value.Contains("KALKULASI"))
                return IconChar.Calculator;

            if (value.Contains("MONITORING"))
                return IconChar.Desktop;


            // =========================================
            // REPORT
            // =========================================

            if (value.Contains("LAPORAN") ||
                value.Contains("REPORT"))
                return IconChar.FileAlt;


            // =========================================
            // KALENDER / PERIODE
            // =========================================

            if (value.Contains("KALENDER") ||
                value.Contains("PERIODE"))
                return IconChar.CalendarAlt;


            // =========================================
            // APPLICATION
            // =========================================

            if (value.Contains("ABOUT") ||
                value.Contains("INFORMASI"))
                return IconChar.InfoCircle;

            if (value.Contains("DOKUMENTASI"))
                return IconChar.BookOpen;

            if (value.Contains("EXIT") ||
                value.Contains("KELUAR"))
                return IconChar.SignOutAlt;


            // =========================================
            // PENGATURAN UMUM
            // =========================================

            if (value.Contains("SETTING") ||
                value.Contains("PENGATURAN"))
                return IconChar.Cog;


            return IconChar.FolderOpen;
        }

        private void NotifyMenuSelected(string menuText)
        {
            if (string.IsNullOrWhiteSpace(menuText))
                return;

            MenuSelected?.Invoke(menuText);
        }

        // =====================================================
        // KLIK GRUP DAN HAMBURGER
        // =====================================================
        private void GroupHeader_Click(object sender, EventArgs e)
        {
            IconButton button = sender as IconButton;

            if (button == null ||
                !(button.Tag is MenuGroup group))
                return;

            // Perbarui judul pada header utama DUCKMENU.
            NotifyMenuSelected(group.Name);

            if (isCollapsed)
            {
                SetCollapsed(false);
                group.IsExpanded = true;
            }
            else
            {
                group.IsExpanded = !group.IsExpanded;
            }

            ApplyGroupState(group);
        }
        private void ToggleButton_Click(object sender, EventArgs e)
        {
            SetCollapsed(!isCollapsed);
        }

        public void SetCollapsed(bool collapsed)
        {
            if (isCollapsed == collapsed)
                return;

            isCollapsed = collapsed;

            ApplyCollapsedState();

            ToggleChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ApplyCollapsedState()
        {
            Width = isCollapsed
                ? CollapsedWidth
                : ExpandedWidth;

            brandPanel.Visible = !isCollapsed;

            toolTip.SetToolTip(
                toggleButton,
                isCollapsed
                    ? "Buka sidebar"
                    : "Tutup sidebar");

            foreach (MenuGroup group in groups.Values)
                ApplyGroupState(group);
        }

        // =====================================================
        // STATUS DAN TATA LETAK GRUP
        // =====================================================

        private void ApplyGroupState(MenuGroup group)
        {
            if (group == null)
                return;

            group.Container.BackColor = PanelBackgroundColor;
            group.SubMenu.BackColor = PanelBackgroundColor;

            group.Header.BackColor = MenuItemBackgroundColor;
            group.Header.ForeColor = MenuTextColor;
            group.Header.IconColor = MenuIconColor;
            group.Header.IconChar = group.Icon;
            group.Header.IconSize = 18;

            group.SubMenu.Padding = new Padding(
                isCollapsed ? 0 : SubMenuIndent,
                0, 0, 0);

            bool showSubMenu =
                !isCollapsed && group.IsExpanded;

            group.SubMenu.Visible = showSubMenu;

            bool hasSubMenu =
                group.Items.Count > 0 ||
                group.NestedMenus.Count > 0 ||
                (group.Name == "Windows" &&
                 windowsFormsPanel != null &&
                 windowsFormsPanel.Controls.Count > 0);

            group.Header.Text = isCollapsed
                ? string.Empty
                : group.Name +
                  (hasSubMenu
                      ? (group.IsExpanded ? "   ▾" : "   ▸")
                      : string.Empty);


            group.Header.TextAlign = isCollapsed
                ? ContentAlignment.MiddleCenter
                : ContentAlignment.MiddleLeft;

            group.Header.TextImageRelation = isCollapsed
                ? TextImageRelation.Overlay
                : TextImageRelation.ImageBeforeText;

            group.Header.ImageAlign = isCollapsed
                ? ContentAlignment.MiddleCenter
                : ContentAlignment.MiddleLeft;

            group.Header.Padding = isCollapsed
                ? new Padding(0)
                : new Padding(8, 0, 4, 0);

            toolTip.SetToolTip(group.Header, group.Name);

            ApplyButtonColors(group.Header);

            foreach (IconButton button in group.Items)
            {
                MenuItemData item =
                    button.Tag as MenuItemData;

                // Tombol About memakai NestedMenuData.
                // Pengaturannya dilakukan secara terpisah.
                if (item == null)
                    continue;


                button.Text = isCollapsed
                    ? string.Empty
                    : item.Text;

                button.TextImageRelation = isCollapsed
                    ? TextImageRelation.Overlay
                    : TextImageRelation.ImageBeforeText;

                button.TextAlign = isCollapsed
                    ? ContentAlignment.MiddleCenter
                    : ContentAlignment.MiddleLeft;

                button.ImageAlign = isCollapsed
                    ? ContentAlignment.MiddleCenter
                    : ContentAlignment.MiddleLeft;

                button.Padding = isCollapsed
                    ? new Padding(0)
                    : new Padding(4, 5, 6, 5);

                toolTip.SetToolTip(button, item.Text);

                ApplyButtonColors(button);
                UpdateMenuItemHeight(button);
            }

            foreach (NestedMenuData parent in group.NestedMenus)
            {
                ApplyNestedMenuState(group, parent);
            }

            UpdateGroupHeight(group);
        }

        private void UpdateMenuItemHeight(IconButton button)
        {
            if (button == null ||
                isCollapsed ||
                button.IsDisposed)
                return;

            int availableWidth =
                button.ClientSize.Width
                - button.Padding.Horizontal
                - button.IconSize
                - 16;

            if (availableWidth < 50)
                availableWidth = 50;

            Size measured = TextRenderer.MeasureText(
                button.Text,
                button.Font,
                new Size(availableWidth, 1000),
                TextFormatFlags.WordBreak |
                TextFormatFlags.TextBoxControl);

            int requiredHeight = Math.Max(
                MenuItemMinHeight,
                measured.Height +
                button.Padding.Vertical + 8);

            if (button.Height != requiredHeight)
                button.Height = requiredHeight;
        }

        private void UpdateGroupHeight(MenuGroup group)
        {
            if (group == null)
                return;

            int subMenuHeight = 0;

            foreach (IconButton button in group.Items)
            {
                if (button == null)
                    continue;

                // Pertahankan perhitungan submenu lama,
                // termasuk submenu About.
                NestedMenuData nestedMenu =
                    button.Tag as NestedMenuData;

                if (nestedMenu != null)
                {
                    nestedMenu.Container.Height =
                        nestedMenu.Button.Height +
                        (nestedMenu.IsExpanded
                            ? nestedMenu.ChildPanel.Height
                            : 0);

                    subMenuHeight += nestedMenu.Container.Height;
                    continue;
                }

                // Cari node tampilan yang sesuai dengan tombol.
                SidebarMenuViewNode viewNode = null;

                foreach (SidebarMenuViewNode candidate
                    in viewNodesByMenuIndex.Values)
                {
                    if (ReferenceEquals(candidate.Button, button))
                    {
                        viewNode = candidate;
                        break;
                    }
                }

                if (viewNode != null &&
                    viewNode.Container != null)
                {
                    // Gunakan tinggi wadah menu bertingkat,
                    // termasuk panel anak jika sedang terbuka.
                    subMenuHeight += viewNode.Container.Height;
                }
                else
                {
                    // Menu biasa tetap dihitung berdasarkan tombol.
                    subMenuHeight += button.Height;
                }
            }

            // Tambahkan tinggi daftar form dinamis pada grup Windows.
            if (group.Name == "Windows" &&
                windowsFormsPanel != null)
            {
                subMenuHeight += windowsFormsPanel.Height;
            }

            group.SubMenu.Height = subMenuHeight;

            group.Container.Height =
                GroupHeaderHeight +
                (!isCollapsed && group.IsExpanded
                    ? subMenuHeight
                    : 0);
        }

        private void ParentViewNode_Click(object sender, EventArgs e)
        {
            IconButton button = sender as IconButton;

            if (button == null)
                return;

            SidebarMenuViewNode viewNode = null;

            foreach (SidebarMenuViewNode candidate
                in viewNodesByMenuIndex.Values)
            {
                if (ReferenceEquals(candidate.Button, button))
                {
                    viewNode = candidate;
                    break;
                }
            }

            if (viewNode == null || viewNode.Children.Count == 0)
                return;

            if (isCollapsed)
                SetCollapsed(false);

            // Ubah status buka/tutup menu yang diklik.
            viewNode.IsExpanded = !viewNode.IsExpanded;

            // Cari root grup, misalnya Master atau Application.
            SidebarMenuViewNode groupRoot = viewNode;

            while (groupRoot.Parent != null)
                groupRoot = groupRoot.Parent;

            // Cari cabang menu paling atas di dalam grup.
            // Jangan menerapkan ApplyViewNodeState() ke root grup,
            // karena grup memiliki mekanisme buka/tutup tersendiri.
            SidebarMenuViewNode branchRoot = viewNode;

            while (branchRoot.Parent != null &&
                   !ReferenceEquals(branchRoot.Parent, groupRoot))
            {
                branchRoot = branchRoot.Parent;
            }

            // Terapkan tampilan hanya pada cabang menu terkait.
            ApplyViewNodeState(branchRoot, true);

            // Perbarui tinggi grup tanpa mengubah status buka/tutupnya.
            foreach (MenuGroup group in groups.Values)
            {
                if (groupNodes.TryGetValue(
                        group.Name,
                        out SidebarMenuNode groupNode) &&
                    ReferenceEquals(groupNode, groupRoot.Node))
                {
                    UpdateGroupHeight(group);
                    break;
                }
            }
        }
        private void ApplyViewNodeState(
            SidebarMenuViewNode viewNode,
            bool parentVisible)
        {
            if (viewNode == null || viewNode.Button == null)
                return;

            bool hasChildren = viewNode.Children.Count > 0;

            viewNode.Button.Visible = parentVisible;

            // Menu tanpa anak tidak memerlukan panel anak.
            if (!hasChildren)
                return;

            // Pertahankan nama menu dan tanda panah.
            viewNode.Button.Text =
                isCollapsed
                    ? string.Empty
                    : ((viewNode.Node.Text ?? string.Empty) +
                       (viewNode.IsExpanded ? "   ▾" : "   ▸"));

            Panel childrenPanel = EnsureChildrenPanel(viewNode);

            bool showChildren =
                parentVisible &&
                !isCollapsed &&
                viewNode.IsExpanded;

            childrenPanel.Visible = showChildren;

            // Perbarui semua anak sebelum menghitung tinggi.
            foreach (SidebarMenuViewNode child in viewNode.Children)
            {
                ApplyViewNodeState(child, showChildren);
            }

            int childHeight = showChildren
                ? CalculateViewNodeContentHeight(viewNode)
                : 0;

            childrenPanel.Location = new Point(
                0,
                viewNode.Button.Height);

            Control nodeContainer = viewNode.Button.Parent;

            int panelWidth = nodeContainer != null
                ? nodeContainer.ClientSize.Width
                : 0;

            childrenPanel.Size = new Size(
                Math.Max(0, panelWidth),
                childHeight);

            // Perbarui tinggi wadah khusus menu bertingkat baru.
            // Tidak mengubah wadah submenu lama AddNestedMenuItem().
            if (viewNode.Container != null)
            {
                viewNode.Container.Height =
                    viewNode.Button.Height + childHeight;
            }
        }



        private int CalculateChildrenHeight(
            SidebarMenuViewNode viewNode)
        {
            if (viewNode == null)
                return 0;

            int totalHeight = 0;

            foreach (SidebarMenuViewNode child in viewNode.Children)
            {
                if (child == null || child.Button == null)
                    continue;

                if (!child.Button.Visible)
                    continue;

                totalHeight += child.Button.Height;

                if (child.ChildrenPanel != null &&
                    child.ChildrenPanel.Visible)
                {
                    totalHeight += CalculateChildrenHeight(child);
                }
            }

            return totalHeight;
        }

        private int CalculateViewNodeContentHeight(
            SidebarMenuViewNode viewNode)
        {
            if (viewNode == null)
                return 0;

            int totalHeight = 0;

            foreach (SidebarMenuViewNode child in viewNode.Children)
            {
                if (child == null || child.Button == null)
                    continue;

                if (!child.Button.Visible)
                    continue;

                // Jika anak memiliki Container khusus,
                // gunakan tinggi Container sebagai satu kesatuan.
                if (child.Container != null)
                {
                    totalHeight += child.Container.Height;
                }
                else
                {
                    totalHeight += child.Button.Height;

                    if (child.ChildrenPanel != null &&
                        child.ChildrenPanel.Visible)
                    {
                        totalHeight +=
                            CalculateViewNodeContentHeight(child);
                    }
                }
            }

            return totalHeight;
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            if (isUpdatingLayout || groups == null)
                return;

            isUpdatingLayout = true;

            try
            {
                foreach (MenuGroup group in groups.Values)
                {
                    foreach (IconButton button in group.Items)
                        UpdateMenuItemHeight(button);

                    foreach (NestedMenuData parent in group.NestedMenus)
                    {
                        foreach (IconButton child in parent.Children)
                            UpdateMenuItemHeight(child);

                        ApplyNestedMenuState(group, parent);
                    }

                    UpdateGroupHeight(group);
                }
            }
            finally
            {
                isUpdatingLayout = false;
            }
        }

        // =====================================================
        // MODEL INTERNAL
        // =====================================================

        private sealed class MenuGroup
        {
            public string Name { get; set; }

            public IconChar Icon { get; set; }

            public bool IsExpanded { get; set; }

            public Panel Container { get; set; }

            public IconButton Header { get; set; }

            public Panel SubMenu { get; set; }

            public List<IconButton> Items { get; } =
                new List<IconButton>();

            public List<NestedMenuData> NestedMenus { get; } =
                new List<NestedMenuData>();
        }

        private sealed class NestedMenuData
        {
            public string Text { get; }
            public IconButton Button { get; }
            public Panel ChildPanel { get; }
            public Panel Container { get; }
            public bool IsExpanded { get; set; }

            public List<IconButton> Children { get; } =
                new List<IconButton>();

            public SidebarMenuNode Node { get; }

            public NestedMenuData(
                string text,
                IconButton button,
                Panel childPanel,
                Panel container,
                SidebarMenuNode node)
            {
                Text = text;
                Button = button;
                ChildPanel = childPanel;
                Container = container;
                Node = node;
            }
        }

        private sealed class MenuItemData
        {
            public string Text { get; }

            public string Icon { get; }

            public MenuItemData(string text, string icon)
            {
                Text = text;
                Icon = icon ?? string.Empty;
            }
        }

        public void UpdateWindowsForms(
            IEnumerable<Form> forms,
            Form activeForm)
        {
            if (!groups.ContainsKey("Windows"))
                return;

            MenuGroup group = groups["Windows"];

            if (windowsFormsPanel == null)
            {
                windowsFormsPanel = new Panel
                {
                    Name = "windowsFormsPanel",
                    Dock = DockStyle.Top,
                    Margin = new Padding(0),
                    Padding = new Padding(0),
                    //BackColor = BackgroundColor
                    BackColor = PanelBackgroundColor
                };

                group.SubMenu.Controls.Add(windowsFormsPanel);

                // Daftar form berada di atas perintah Windows.
                group.SubMenu.Controls.SetChildIndex(
                    windowsFormsPanel, 0);
            }

            windowsFormsPanel.SuspendLayout();

            try
            {
                windowsFormsPanel.Controls.Clear();

                Label header = new Label
                {
                    Name = "lblOpenForms",
                    Text = "OPEN FORMS",
                    BackColor = Color.Transparent,
                    ForeColor = SecondaryTextColor,
                    Dock = DockStyle.Top,
                    Height = 28,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(8, 0, 0, 0),
                    Font = new Font(
                        "Segoe UI Semibold",
                        8.0F,
                        FontStyle.Bold)
                };

                windowsFormsPanel.Controls.Add(header);
                windowsFormsPanel.Controls.SetChildIndex(header, 0);

                List<Form> openForms = new List<Form>();

                if (forms != null)
                {
                    foreach (Form form in forms)
                    {
                        if (form == null || form.IsDisposed)
                            continue;

                        openForms.Add(form);
                    }
                }

                // Dibalik agar urutan visual tetap sesuai urutan daftar.
                for (int i = openForms.Count - 1; i >= 0; i--)
                {
                    Form form = openForms[i];

                    string formText =
                        string.IsNullOrWhiteSpace(form.Text)
                            ? form.Name
                            : form.Text;


                    Button button = new Button
                    {
                        Name = "btnWindowsForm",
                        Text =
                        (form == activeForm ? "✓ " : "   ")
                        + formText,
                        Tag = form,
                        Dock = DockStyle.Top,
                        Height = MenuItemMinHeight,
                        MinimumSize = new Size(0, MenuItemMinHeight),

                        FlatStyle = FlatStyle.Flat,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Padding = new Padding(8, 0, 4, 0),
                        Margin = new Padding(0),
                        Font = new Font("Segoe UI", 9.0F),

                        BackColor = Color.Transparent,
                        ForeColor = MenuTextColor,

                        UseVisualStyleBackColor = false,
                        Cursor = Cursors.Hand
                    };

                    button.FlatAppearance.BorderSize = 0;

                    button.FlatAppearance.MouseOverBackColor =
                        MenuItemHoverColor;

                    button.FlatAppearance.MouseDownBackColor =
                        MenuItemHoverColor;

                    button.Click += WindowsFormButton_Click;

                    windowsFormsPanel.Controls.Add(button);
                    windowsFormsPanel.Controls.SetChildIndex(
                        button, 0);
                }

                windowsFormsPanel.Height =
                    28 + (openForms.Count * MenuItemMinHeight);
            }
            finally
            {
                windowsFormsPanel.ResumeLayout(true);
            }

            // Penting: tinggi grup harus menghitung panel dinamis.
            UpdateGroupHeight(group);

            ApplyGroupState(group);
        }

        private void WindowsFormButton_Click(
    object sender,
    EventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
                return;

            Form form = button.Tag as Form;

            if (form == null || form.IsDisposed)
                return;

            Action<Form> handler = WindowsFormSelected;

            if (handler != null)
                handler(form);
        }
        public void DebugMenuTree()
        {
            Debug.WriteLine("===== SIDEBAR MENU TREE =====");

            foreach (SidebarMenuNode node in menuTree.GetAllNodes())
            {
                int level = 0;
                SidebarMenuNode parent = node.Parent;

                while (parent != null)
                {
                    level++;
                    parent = parent.Parent;
                }

                string indent = new string(' ', level * 4);

                Debug.WriteLine(
                    indent +
                    "[Level " + level + "] " +
                    node.Text +
                    " | MenuIndex=" + node.MenuIndex +
                    " | MenuId=" + node.MenuId);
            }

            Debug.WriteLine("===== END SIDEBAR MENU TREE =====");
        }
        public void DebugMenuViewNodes()
        {
            Debug.WriteLine("===== SIDEBAR MENU VIEW NODES =====");

            foreach (SidebarMenuNode node in menuTree.GetAllNodes())
            {
                bool hasViewNode =
                    viewNodesByMenuIndex.ContainsKey(node.MenuIndex);

                Debug.WriteLine(
                    node.MenuId +
                    " | MenuIndex=" + node.MenuIndex +
                    " | HasViewNode=" + hasViewNode);
            }

            Debug.WriteLine("===== END SIDEBAR MENU VIEW NODES =====");
        }

        public void DebugMenuViewHierarchy()
        {
            Debug.WriteLine("===== SIDEBAR VIEW HIERARCHY =====");

            foreach (SidebarMenuNode node in menuTree.GetAllNodes())
            {
                SidebarMenuViewNode viewNode;

                if (!viewNodesByMenuIndex.TryGetValue(
                    node.MenuIndex,
                    out viewNode))
                {
                    Debug.WriteLine(
                        node.MenuId + " | ViewNode tidak ditemukan.");

                    continue;
                }

                int treeChildCount = node.Children.Count;
                int viewChildCount = viewNode.Children.Count;

                Debug.WriteLine(
                    node.MenuId +
                    " | TreeChildren=" + treeChildCount +
                    " | ViewChildren=" + viewChildCount +
                    " | Match=" +
                    (treeChildCount == viewChildCount));
            }

            Debug.WriteLine("===== END SIDEBAR VIEW HIERARCHY =====");
        }


        private SidebarMenuViewNode FindViewNodeByMenuId(string menuId)
        {
            if (string.IsNullOrWhiteSpace(menuId))
            {
                return null;
            }

            SidebarMenuNode node = menuTree.FindByMenuId(menuId);

            if (node == null)
            {
                return null;
            }

            SidebarMenuViewNode viewNode;

            if (!viewNodesByMenuIndex.TryGetValue(
                node.MenuIndex,
                out viewNode))
            {
                return null;
            }

            return viewNode;
        }

        public void SetMenuItemChecked(string menuId, bool isChecked)
        {
            SidebarMenuViewNode viewNode =
                FindViewNodeByMenuId(menuId);

            if (viewNode == null || viewNode.Button == null)
                return;

            string menuText = viewNode.Node.Text;

            // Hilangkan tanda checklist sebelumnya.
            if (menuText.StartsWith("☑ ") ||
                menuText.StartsWith("☐ "))
            {
                menuText = menuText.Substring(2);
            }

            // Perbarui teks pada node dan tombol.
            string checkMark = isChecked ? "☑ " : "☐ ";

            viewNode.Node.Text = checkMark + menuText;
            viewNode.Button.Text = checkMark + menuText;
        }



        private Panel EnsureChildrenPanel(
            SidebarMenuViewNode viewNode)
        {
            if (viewNode == null)
            {
                throw new ArgumentNullException(nameof(viewNode));
            }

            // Jika panel anak sudah tersedia, gunakan kembali.
            if (viewNode.ChildrenPanel != null)
            {
                return viewNode.ChildrenPanel;
            }

            IconButton parentButton = viewNode.Button;

            if (parentButton == null)
            {
                throw new InvalidOperationException(
                    "Tombol menu induk tidak ditemukan: " +
                    viewNode.Node.MenuId);
            }

            Panel parentPanel = parentButton.Parent as Panel;

            if (parentPanel == null)
            {
                throw new InvalidOperationException(
                    "Panel induk tombol menu tidak ditemukan: " +
                    viewNode.Node.MenuId);
            }

            int originalIndex =
                parentPanel.Controls.GetChildIndex(parentButton);

            int originalHeight = Math.Max(
                MenuItemMinHeight,
                parentButton.Height);

            // Buat wadah khusus untuk tombol induk dan anak-anaknya.
            Panel nodeContainer = new Panel
            {
                Dock = DockStyle.Top,
                Height = originalHeight,
                Margin = new Padding(0),
                Padding = new Padding(0),
                //BackColor = BackgroundColor
                BackColor = PanelBackgroundColor
            };

            // Pindahkan tombol ke wadah khusus.
            parentPanel.Controls.Remove(parentButton);
            parentPanel.Controls.Add(nodeContainer);

            parentPanel.Controls.SetChildIndex(
                nodeContainer,
                Math.Min(
                    originalIndex,
                    parentPanel.Controls.Count - 1));

            parentButton.Dock = DockStyle.None;
            parentButton.Location = new Point(0, 0);
            parentButton.Size = new Size(
                nodeContainer.ClientSize.Width,
                originalHeight);


            Panel childrenPanel = new Panel
            {
                Dock = DockStyle.None,
                Location = new Point(0, originalHeight),
                Size = new Size(
                    nodeContainer.ClientSize.Width,
                    0),
                Visible = false,
                Margin = new Padding(0),
                Padding = new Padding(0),
                //BackColor = BackgroundColor
                BackColor = PanelBackgroundColor
            };


            nodeContainer.Controls.Add(parentButton);
            nodeContainer.Controls.Add(childrenPanel);

            nodeContainer.Controls.SetChildIndex(parentButton, 0);
            nodeContainer.Controls.SetChildIndex(childrenPanel, 1);

            // Simpan kedua referensi secara eksplisit.
            viewNode.SetContainer(nodeContainer);
            viewNode.SetChildrenPanel(childrenPanel);


            return childrenPanel;

        }
        private sealed class GlassPanel : Panel
        {
            public bool DarkGlass { get; set; }

            protected override void OnPaintBackground(PaintEventArgs e)
            {
                if (!DarkGlass)
                {
                    base.OnPaintBackground(e);
                    return;
                }

                Rectangle bounds = ClientRectangle;

                if (bounds.Width <= 0 || bounds.Height <= 0)
                    return;

                using (LinearGradientBrush gradient =
                    new LinearGradientBrush(
                        bounds,
                        Color.FromArgb(48, 62, 80),
                        Color.FromArgb(19, 27, 40),
                        LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(gradient, bounds);
                }

                Rectangle highlight = new Rectangle(
                    0, 0, bounds.Width, Math.Min(120, bounds.Height));

                if (highlight.Height > 0)
                {
                    using (LinearGradientBrush shine =
                        new LinearGradientBrush(
                            highlight,
                            Color.FromArgb(35, 255, 255, 255),
                            Color.FromArgb(0, 255, 255, 255),
                            LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillRectangle(shine, highlight);
                    }
                }

                using (Pen border = new Pen(
                    Color.FromArgb(55, 220, 230, 245)))
                {
                    e.Graphics.DrawLine(
                        border,
                        0,
                        0,
                        bounds.Width - 1,
                        0);
                }
            }
        }

    }
}
