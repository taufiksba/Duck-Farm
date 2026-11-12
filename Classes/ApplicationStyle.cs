using DuckFarm.Classes.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace DuckFarm.Classes
{
    public static class ApplicationStyle
    {
        // =========================================================
        // FONT
        // =========================================================

        public static readonly Font DefaultFont =
            new Font("Segoe UI", 9F, FontStyle.Regular);

        public static readonly Font SmallFont =
            new Font("Segoe UI", 8F, FontStyle.Regular);

        public static readonly Font LabelFont =
            new Font("Segoe UI", 9F, FontStyle.Regular);

        public static readonly Font LabelBoldFont =
            new Font("Segoe UI", 9F, FontStyle.Bold);

        public static readonly Font HeaderFont =
            new Font("Segoe UI", 16F, FontStyle.Bold);

        public static readonly Font SubHeaderFont =
            new Font("Segoe UI", 10F, FontStyle.Regular);

        public static readonly Font ButtonFont =
            new Font("Segoe UI", 9F, FontStyle.Bold);


        // =========================================================
        // WARNA FORM
        // =========================================================

        public static readonly Color FormBackColor =
            Color.FromArgb(245, 247, 250);

        public static readonly Color PanelBackColor =
            Color.White;

        public static readonly Color CardBackColor =
            Color.White;


        // =========================================================
        // WARNA HEADER
        // =========================================================

        public static readonly Color HeaderBackColor =
            Color.White;

        public static readonly Color HeaderForeColor =
            Color.FromArgb(17, 24, 39);

        public static readonly Color HeaderSecondaryColor =
            Color.FromArgb(100, 116, 139);


        // =========================================================
        // WARNA TEXT
        // =========================================================

        public static readonly Color TextColor =
            Color.FromArgb(31, 41, 55);

        public static readonly Color SecondaryTextColor =
            Color.FromArgb(100, 116, 139);

        public static readonly Color DisabledTextColor =
            Color.FromArgb(156, 163, 175);


        // =========================================================
        // WARNA BORDER
        // =========================================================

        public static readonly Color BorderColor =
            Color.FromArgb(203, 213, 225);

        public static readonly Color LightBorderColor =
            Color.FromArgb(226, 232, 240);


        // =========================================================
        // WARNA INPUT
        // =========================================================

        public static readonly Color InputBackColor =
            Color.White;

        public static readonly Color InputForeColor =
            Color.FromArgb(30, 41, 59);

        public static readonly Color InputFocusColor =
            Color.FromArgb(37, 99, 235);


        // =========================================================
        // WARNA BUTTON
        // =========================================================

        public static readonly Color PrimaryButtonColor =
            Color.FromArgb(37, 99, 235);

        public static readonly Color PrimaryButtonHoverColor =
            Color.FromArgb(29, 78, 216);

        public static readonly Color PrimaryButtonPressedColor =
            Color.FromArgb(30, 64, 175);


        public static readonly Color SecondaryButtonColor =
            Color.FromArgb(100, 116, 139);

        public static readonly Color SecondaryButtonHoverColor =
            Color.FromArgb(71, 85, 105);

        public static readonly Color SecondaryButtonPressedColor =
            Color.FromArgb(51, 65, 85);


        public static readonly Color SuccessButtonColor =
            Color.FromArgb(22, 163, 74);

        public static readonly Color SuccessButtonHoverColor =
            Color.FromArgb(21, 128, 61);

        public static readonly Color SuccessButtonPressedColor =
            Color.FromArgb(20, 83, 45);


        public static readonly Color DangerButtonColor =
            Color.FromArgb(220, 38, 38);

        public static readonly Color DangerButtonHoverColor =
            Color.FromArgb(185, 28, 28);

        public static readonly Color DangerButtonPressedColor =
            Color.FromArgb(153, 27, 27);


        public static readonly Color WarningButtonColor =
            Color.FromArgb(245, 158, 11);

        public static readonly Color WarningButtonHoverColor =
            Color.FromArgb(217, 119, 6);

        public static readonly Color WarningButtonPressedColor =
            Color.FromArgb(180, 83, 9);


        // =========================================================
        // DATAGRID
        // =========================================================

        public static readonly Color GridHeaderColor =
            Color.FromArgb(241, 245, 249);

        public static readonly Color GridHeaderTextColor =
            Color.FromArgb(30, 41, 59);

        public static readonly Color GridRowColor =
            Color.White;

        public static readonly Color GridAlternateRowColor =
            Color.FromArgb(248, 250, 252);

        public static readonly Color GridSelectionColor =
            Color.FromArgb(219, 234, 254);

        public static readonly Color GridSelectionTextColor =
            Color.FromArgb(30, 41, 59);

        public static readonly Color GridLineColor =
            Color.FromArgb(226, 232, 240);


        // =========================================================
        // APPLY FORM
        // =========================================================

        public static void Apply(Form form)
        {
            if (form == null)
                return;

            form.BackColor =
                FormBackColor;

            form.ForeColor =
                TextColor;

            form.Font =
                DefaultFont;

            form.AutoScaleMode =
                AutoScaleMode.Font;

            form.AutoScroll =
                false;

            ApplyControls(form);
        }


        // =========================================================
        // APPLY SEMUA CONTROL
        // =========================================================

        private static void ApplyControls(Control parent)
        {
            if (parent == null)
                return;

            foreach (Control control in parent.Controls)
            {
                ApplyControl(control);

                if (control.HasChildren)
                {
                    ApplyControls(control);
                }
            }
        }


        // =========================================================
        // APPLY CONTROL
        // =========================================================

        private static void ApplyControl(Control control)
        {
            if (control == null)
                return;


            // =====================================================
            // APP TEXTBOX
            // =====================================================

            if (control is AppTextBox appTextBox)
            {
                ApplyAppTextBox(appTextBox);
                return;
            }


            // =====================================================
            // APP BUTTON
            // =====================================================

            if (control is AppButton appButton)
            {
                ApplyAppButton(appButton);
                return;
            }


            // =====================================================
            // APP PANEL
            // =====================================================

            if (control is AppPanel appPanel)
            {
                ApplyAppPanel(appPanel);
                return;
            }


            // =====================================================
            // APP LABEL
            // =====================================================

            if (control is AppLabel appLabel)
            {
                ApplyAppLabel(appLabel);
                return;
            }


            // =====================================================
            // APP COMBOBOX
            // =====================================================

            if (control is AppComboBox appComboBox)
            {
                ApplyAppComboBox(appComboBox);
                return;
            }


            // =====================================================
            // APP DATE PICKER
            // =====================================================

            if (control is AppDatePicker appDatePicker)
            {
                ApplyAppDatePicker(appDatePicker);
                return;
            }


            // =====================================================
            // LABEL
            // =====================================================

            if (control is Label label)
            {
                ApplyLabel(label);
                return;
            }


            // =====================================================
            // TEXTBOX STANDARD
            // =====================================================

            if (control is TextBox textBox)
            {
                ApplyTextBox(textBox);
                return;
            }


            // =====================================================
            // COMBOBOX STANDARD
            // =====================================================

            if (control is ComboBox comboBox)
            {
                ApplyComboBox(comboBox);
                return;
            }


            // =====================================================
            // BUTTON STANDARD
            // =====================================================

            if (control is Button button)
            {
                ApplyButton(button);
                return;
            }


            // =====================================================
            // PANEL STANDARD
            // =====================================================

            if (control is Panel panel)
            {
                ApplyPanel(panel);
                return;
            }


            // =====================================================
            // GROUPBOX
            // =====================================================

            if (control is GroupBox groupBox)
            {
                ApplyGroupBox(groupBox);
                return;
            }


            // =====================================================
            // DATAGRIDVIEW
            // =====================================================

            if (control is DataGridView grid)
            {
                ApplyDataGridView(grid);
                return;
            }


            // =====================================================
            // CHECKBOX
            // =====================================================

            if (control is CheckBox checkBox)
            {
                ApplyCheckBox(checkBox);
                return;
            }


            // =====================================================
            // RADIOBUTTON
            // =====================================================

            if (control is RadioButton radioButton)
            {
                ApplyRadioButton(radioButton);
                return;
            }


            // =====================================================
            // DATETIMEPICKER
            // =====================================================

            if (control is DateTimePicker dateTimePicker)
            {
                ApplyDateTimePicker(dateTimePicker);
                return;
            }


            // =====================================================
            // NUMERIC UP DOWN
            // =====================================================

            if (control is NumericUpDown numericUpDown)
            {
                ApplyNumericUpDown(numericUpDown);
                return;
            }


            // =====================================================
            // TAB CONTROL
            // =====================================================

            if (control is TabControl tabControl)
            {
                ApplyTabControl(tabControl);
                return;
            }


            // =====================================================
            // STATUS STRIP
            // =====================================================

            if (control is StatusStrip statusStrip)
            {
                ApplyStatusStrip(statusStrip);
                return;
            }
        }


        // =========================================================
        // APP LABEL
        // =========================================================

        public static void ApplyAppLabel(
            AppLabel label)
        {
            if (label == null)
                return;

            label.Font =
                LabelFont;

            label.ForeColor =
                TextColor;

            label.BackColor =
                Color.Transparent;
        }


        // =========================================================
        // LABEL
        // =========================================================

        public static void ApplyLabel(
            Label label)
        {
            if (label == null)
                return;

            label.Font =
                LabelFont;

            label.ForeColor =
                TextColor;

            label.BackColor =
                Color.Transparent;
        }


        // =========================================================
        // APP TEXTBOX
        // =========================================================

        public static void ApplyAppTextBox(
            AppTextBox textBox)
        {
            if (textBox == null)
                return;

            textBox.Font =
                DefaultFont;

            textBox.BackColor =
                InputBackColor;

            textBox.ForeColor =
                InputForeColor;

            // Jangan ubah:
            // BorderStyle
            // BorderRadius
            // BorderColor
            // FocusBorderColor
            //
            // Semua tersebut menjadi tanggung jawab
            // AppTextBox.
        }


        // =========================================================
        // TEXTBOX STANDARD
        // =========================================================

        public static void ApplyTextBox(
            TextBox textBox)
        {
            if (textBox == null)
                return;

            textBox.Font =
                DefaultFont;

            textBox.BackColor =
                InputBackColor;

            textBox.ForeColor =
                InputForeColor;

            textBox.BorderStyle =
                BorderStyle.FixedSingle;
        }


        // =========================================================
        // APP COMBOBOX
        // =========================================================

        public static void ApplyAppComboBox(
            AppComboBox comboBox)
        {
            if (comboBox == null)
                return;

            comboBox.Font =
                DefaultFont;

            comboBox.BackColor =
                InputBackColor;

            comboBox.ForeColor =
                InputForeColor;

            // Jangan merusak OwnerDraw
            // dan property custom AppComboBox.
        }


        // =========================================================
        // COMBOBOX STANDARD
        // =========================================================

        public static void ApplyComboBox(
            ComboBox comboBox)
        {
            if (comboBox == null)
                return;

            comboBox.Font =
                DefaultFont;

            comboBox.BackColor =
                InputBackColor;

            comboBox.ForeColor =
                InputForeColor;

            comboBox.FlatStyle =
                FlatStyle.Flat;
        }


        // =========================================================
        // APP DATE PICKER
        // =========================================================

        public static void ApplyAppDatePicker(
            AppDatePicker datePicker)
        {
            if (datePicker == null)
                return;

            datePicker.Font =
                DefaultFont;

            datePicker.BackColor =
                InputBackColor;

            datePicker.ForeColor =
                InputForeColor;

            // Property custom milik AppDatePicker
            // tidak disentuh.
        }


        // =========================================================
        // DATE TIME PICKER STANDARD
        // =========================================================

        public static void ApplyDateTimePicker(
            DateTimePicker dateTimePicker)
        {
            if (dateTimePicker == null)
                return;

            dateTimePicker.Font =
                DefaultFont;

            dateTimePicker.BackColor =
                InputBackColor;

            dateTimePicker.ForeColor =
                InputForeColor;

            dateTimePicker.Format =
                DateTimePickerFormat.Custom;

            dateTimePicker.CustomFormat =
                "dd-MM-yyyy";
        }


        // =========================================================
        // APP BUTTON
        // =========================================================

        public static void ApplyAppButton(
            AppButton button)
        {
            if (button == null)
                return;

            button.Font =
                ButtonFont;

            button.ForeColor =
                Color.White;

            button.Cursor =
                Cursors.Hand;

            button.UseVisualStyleBackColor =
                false;

            button.Height =
                36;

            button.Padding =
                new Padding(12, 0, 12, 0);

            // Custom property AppButton
            button.BorderRadius =
                8;

            button.BorderSize =
                0;

            button.BorderColor =
                Color.Transparent;

            button.HoverColor =
                PrimaryButtonHoverColor;

            button.PressedColor =
                PrimaryButtonPressedColor;

            button.BackColor =
                PrimaryButtonColor;
        }


        // =========================================================
        // BUTTON STANDARD
        // =========================================================

        public static void ApplyButton(
            Button button)
        {
            if (button == null)
                return;

            button.Font =
                ButtonFont;

            button.ForeColor =
                Color.White;

            button.BackColor =
                PrimaryButtonColor;

            button.FlatStyle =
                FlatStyle.Flat;

            button.FlatAppearance.BorderSize =
                0;

            button.FlatAppearance.MouseOverBackColor =
                PrimaryButtonHoverColor;

            button.FlatAppearance.MouseDownBackColor =
                PrimaryButtonPressedColor;

            button.Cursor =
                Cursors.Hand;

            button.Height =
                36;

            button.Padding =
                new Padding(12, 0, 12, 0);

            button.UseVisualStyleBackColor =
                false;
        }


        // =========================================================
        // PRIMARY BUTTON
        // =========================================================

        public static void SetPrimaryButton(
            Button button)
        {
            if (button == null)
                return;

            if (button is AppButton appButton)
            {
                ApplyAppButton(appButton);

                appButton.BackColor =
                    PrimaryButtonColor;

                appButton.HoverColor =
                    PrimaryButtonHoverColor;

                appButton.PressedColor =
                    PrimaryButtonPressedColor;

                return;
            }

            ApplyButton(button);

            button.BackColor =
                PrimaryButtonColor;

            button.FlatAppearance.MouseOverBackColor =
                PrimaryButtonHoverColor;

            button.FlatAppearance.MouseDownBackColor =
                PrimaryButtonPressedColor;
        }


        // =========================================================
        // SUCCESS BUTTON
        // =========================================================

        public static void SetSuccessButton(
            Button button)
        {
            if (button == null)
                return;

            if (button is AppButton appButton)
            {
                ApplyAppButton(appButton);

                appButton.BackColor =
                    SuccessButtonColor;

                appButton.HoverColor =
                    SuccessButtonHoverColor;

                appButton.PressedColor =
                    SuccessButtonPressedColor;

                return;
            }

            ApplyButton(button);

            button.BackColor =
                SuccessButtonColor;

            button.FlatAppearance.MouseOverBackColor =
                SuccessButtonHoverColor;

            button.FlatAppearance.MouseDownBackColor =
                SuccessButtonPressedColor;
        }


        // =========================================================
        // DANGER BUTTON
        // =========================================================

        public static void SetDangerButton(
            Button button)
        {
            if (button == null)
                return;

            if (button is AppButton appButton)
            {
                ApplyAppButton(appButton);

                appButton.BackColor =
                    DangerButtonColor;

                appButton.HoverColor =
                    DangerButtonHoverColor;

                appButton.PressedColor =
                    DangerButtonPressedColor;

                return;
            }

            ApplyButton(button);

            button.BackColor =
                DangerButtonColor;

            button.FlatAppearance.MouseOverBackColor =
                DangerButtonHoverColor;

            button.FlatAppearance.MouseDownBackColor =
                DangerButtonPressedColor;
        }


        // =========================================================
        // SECONDARY BUTTON
        // =========================================================

        public static void SetSecondaryButton(
            Button button)
        {
            if (button == null)
                return;

            if (button is AppButton appButton)
            {
                ApplyAppButton(appButton);

                appButton.BackColor =
                    SecondaryButtonColor;

                appButton.HoverColor =
                    SecondaryButtonHoverColor;

                appButton.PressedColor =
                    SecondaryButtonPressedColor;

                return;
            }

            ApplyButton(button);

            button.BackColor =
                SecondaryButtonColor;

            button.FlatAppearance.MouseOverBackColor =
                SecondaryButtonHoverColor;

            button.FlatAppearance.MouseDownBackColor =
                SecondaryButtonPressedColor;
        }


        // =========================================================
        // WARNING BUTTON
        // =========================================================

        public static void SetWarningButton(
            Button button)
        {
            if (button == null)
                return;

            if (button is AppButton appButton)
            {
                ApplyAppButton(appButton);

                appButton.BackColor =
                    WarningButtonColor;

                appButton.HoverColor =
                    WarningButtonHoverColor;

                appButton.PressedColor =
                    WarningButtonPressedColor;

                return;
            }

            ApplyButton(button);

            button.BackColor =
                WarningButtonColor;

            button.FlatAppearance.MouseOverBackColor =
                WarningButtonHoverColor;

            button.FlatAppearance.MouseDownBackColor =
                WarningButtonPressedColor;
        }


        // =========================================================
        // APP PANEL
        // =========================================================

        public static void ApplyAppPanel(
            AppPanel panel)
        {
            if (panel == null)
                return;

            panel.PanelBackColor =
                CardBackColor;

            panel.BorderColor =
                LightBorderColor;

            panel.BorderSize =
                1;

            panel.BorderRadius =
                10;

            panel.Padding =
                new Padding(16);
        }


        // =========================================================
        // PANEL STANDARD
        // =========================================================

        public static void ApplyPanel(
            Panel panel)
        {
            if (panel == null)
                return;

            panel.BackColor =
                PanelBackColor;
        }


        // =========================================================
        // CARD
        // =========================================================

        public static void SetCard(
            Panel panel)
        {
            if (panel == null)
                return;

            if (panel is AppPanel appPanel)
            {
                ApplyAppPanel(appPanel);
                return;
            }

            panel.BackColor =
                CardBackColor;

            panel.Padding =
                new Padding(16);
        }


        // =========================================================
        // HEADER PANEL
        // =========================================================

        public static void SetHeaderPanel(
            Panel panel)
        {
            if (panel == null)
                return;

            panel.BackColor =
                HeaderBackColor;
        }


        // =========================================================
        // FORM HEADER
        // =========================================================

        public static void SetFormHeader(
            Panel panel,
            Label title)
        {
            if (panel != null)
            {
                panel.BackColor =
                    HeaderBackColor;

                panel.Height =
                    70;

                panel.Padding =
                    new Padding(
                        20,
                        10,
                        20,
                        10);
            }

            if (title != null)
            {
                title.Font =
                    HeaderFont;

                title.ForeColor =
                    HeaderForeColor;

                title.BackColor =
                    Color.Transparent;

                title.AutoSize =
                    true;

                title.TextAlign =
                    ContentAlignment.MiddleLeft;
            }
        }


        // =========================================================
        // GROUPBOX
        // =========================================================

        public static void ApplyGroupBox(
            GroupBox groupBox)
        {
            if (groupBox == null)
                return;

            groupBox.Font =
                LabelBoldFont;

            groupBox.ForeColor =
                TextColor;

            groupBox.BackColor =
                PanelBackColor;
        }


        // =========================================================
        // CHECKBOX
        // =========================================================

        public static void ApplyCheckBox(
            CheckBox checkBox)
        {
            if (checkBox == null)
                return;

            checkBox.Font =
                DefaultFont;

            checkBox.ForeColor =
                TextColor;

            checkBox.BackColor =
                Color.Transparent;

            checkBox.Cursor =
                Cursors.Hand;
        }


        // =========================================================
        // RADIOBUTTON
        // =========================================================

        public static void ApplyRadioButton(
            RadioButton radioButton)
        {
            if (radioButton == null)
                return;

            radioButton.Font =
                DefaultFont;

            radioButton.ForeColor =
                TextColor;

            radioButton.BackColor =
                Color.Transparent;

            radioButton.Cursor =
                Cursors.Hand;
        }


        // =========================================================
        // NUMERIC UP DOWN
        // =========================================================

        public static void ApplyNumericUpDown(
            NumericUpDown numericUpDown)
        {
            if (numericUpDown == null)
                return;

            numericUpDown.Font =
                DefaultFont;

            numericUpDown.BackColor =
                InputBackColor;

            numericUpDown.ForeColor =
                InputForeColor;
        }


        // =========================================================
        // TAB CONTROL
        // =========================================================

        public static void ApplyTabControl(
            TabControl tabControl)
        {
            if (tabControl == null)
                return;

            tabControl.Font =
                DefaultFont;

            tabControl.BackColor =
                FormBackColor;

            tabControl.ForeColor =
                TextColor;
        }


        // =========================================================
        // STATUS STRIP
        // =========================================================

        public static void ApplyStatusStrip(
            StatusStrip statusStrip)
        {
            if (statusStrip == null)
                return;

            statusStrip.BackColor =
                Color.FromArgb(
                    248,
                    250,
                    252);

            statusStrip.ForeColor =
                SecondaryTextColor;

            statusStrip.Font =
                SmallFont;
        }


        // =========================================================
        // DATAGRIDVIEW
        // =========================================================

        public static void ApplyDataGridView(
            DataGridView grid)
        {
            if (grid == null)
                return;


            // -----------------------------------------------------
            // BASIC
            // -----------------------------------------------------

            grid.BackgroundColor =
                GridRowColor;

            grid.BorderStyle =
                BorderStyle.None;

            grid.Font =
                DefaultFont;

            grid.ForeColor =
                TextColor;

            grid.EnableHeadersVisualStyles =
                false;

            grid.AllowUserToAddRows =
                false;

            grid.AllowUserToResizeRows =
                false;

            grid.RowHeadersVisible =
                false;

            grid.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            grid.MultiSelect =
                false;


            // -----------------------------------------------------
            // HEADER
            // -----------------------------------------------------

            grid.ColumnHeadersDefaultCellStyle.BackColor =
                GridHeaderColor;

            grid.ColumnHeadersDefaultCellStyle.ForeColor =
                GridHeaderTextColor;

            grid.ColumnHeadersDefaultCellStyle.Font =
                LabelBoldFont;

            grid.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                GridHeaderColor;

            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                GridHeaderTextColor;

            grid.ColumnHeadersHeight =
                38;

            grid.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;


            // -----------------------------------------------------
            // ROW
            // -----------------------------------------------------

            grid.DefaultCellStyle.Font =
                DefaultFont;

            grid.DefaultCellStyle.ForeColor =
                TextColor;

            grid.DefaultCellStyle.BackColor =
                GridRowColor;

            grid.DefaultCellStyle.SelectionBackColor =
                GridSelectionColor;

            grid.DefaultCellStyle.SelectionForeColor =
                GridSelectionTextColor;

            grid.DefaultCellStyle.Padding =
                new Padding(
                    5,
                    0,
                    5,
                    0);


            // -----------------------------------------------------
            // ALTERNATE ROW
            // -----------------------------------------------------

            grid.AlternatingRowsDefaultCellStyle.BackColor =
                GridAlternateRowColor;


            // -----------------------------------------------------
            // ROW HEIGHT
            // -----------------------------------------------------

            grid.RowTemplate.Height =
                32;


            // -----------------------------------------------------
            // GRID LINE
            // -----------------------------------------------------

            grid.GridColor =
                GridLineColor;

            grid.CellBorderStyle =
                DataGridViewCellBorderStyle.SingleHorizontal;


            // -----------------------------------------------------
            // HEADER BORDER
            // -----------------------------------------------------

            grid.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;

            grid.ColumnHeadersDefaultCellStyle.Padding =
                new Padding(
                    5,
                    0,
                    5,
                    0);
        }


        // =========================================================
        // GRID HEADER LEFT
        // =========================================================

        public static void SetGridHeaderLeft(
            DataGridView grid)
        {
            if (grid == null)
                return;

            grid.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleLeft;
        }


        // =========================================================
        // GRID HEADER CENTER
        // =========================================================

        public static void SetGridHeaderCenter(
            DataGridView grid)
        {
            if (grid == null)
                return;

            grid.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
        }


        // =========================================================
        // GRID HEADER RIGHT
        // =========================================================

        public static void SetGridHeaderRight(
            DataGridView grid)
        {
            if (grid == null)
                return;

            grid.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight;
        }


        // =========================================================
        // TITLE LABEL
        // =========================================================

        public static void SetTitleLabel(
            Label label)
        {
            if (label == null)
                return;

            label.Font =
                HeaderFont;

            label.ForeColor =
                TextColor;

            label.BackColor =
                Color.Transparent;
        }


        // =========================================================
        // SUBTITLE LABEL
        // =========================================================

        public static void SetSubtitleLabel(
            Label label)
        {
            if (label == null)
                return;

            label.Font =
                SubHeaderFont;

            label.ForeColor =
                SecondaryTextColor;

            label.BackColor =
                Color.Transparent;
        }


        // =========================================================
        // SECTION LABEL
        // =========================================================

        public static void SetSectionLabel(
            Label label)
        {
            if (label == null)
                return;

            label.Font =
                LabelBoldFont;

            label.ForeColor =
                TextColor;

            label.BackColor =
                Color.Transparent;
        }


        // =========================================================
        // SEPARATOR
        // =========================================================

        public static void SetSeparator(
            Panel panel)
        {
            if (panel == null)
                return;

            panel.BackColor =
                LightBorderColor;

            panel.Height =
                1;
        }
    }
}
