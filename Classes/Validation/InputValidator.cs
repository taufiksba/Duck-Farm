using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace DuckFarm.Classes.Validation
{
    /// <summary>
    /// Central validation helper untuk aplikasi DuckFarm.
    ///
    /// Standar angka Indonesia:
    ///
    /// Input user:
    ///     147730,77
    ///
    /// Display:
    ///     147.730,77
    ///
    /// Nilai C#:
    ///     147730.77m
    ///
    /// Database:
    ///     Gunakan decimal/int sebagai parameter.
    ///
    /// Class ini dapat dikembangkan untuk validasi:
    ///     - TextBox
    ///     - AppTextBox
    ///     - ComboBox
    ///     - DateTimePicker
    ///     - CheckBox
    ///     - dan control lainnya
    /// </summary>
    public static class InputValidator
    {
        // ============================================================
        // CULTURE
        // ============================================================

        private static readonly CultureInfo IndonesianCulture =
            CultureInfo.GetCultureInfo("id-ID");

        private const char DecimalSeparator = ',';


        // ============================================================
        // ERROR PROVIDER MANAGEMENT
        // ============================================================

        private static readonly Dictionary<Form, ErrorProvider>
            ErrorProviders =
                new Dictionary<Form, ErrorProvider>();


        // ============================================================
        // SET TEXT
        // ============================================================

        /// <summary>
        /// TextBox biasa.
        ///
        /// required = true:
        /// TextBox wajib diisi.
        /// </summary>
        public static void SetText(
            TextBox textBox,
            bool required = false,
            bool selectAllOnFocus = true)
        {
            if (textBox == null)
                throw new ArgumentNullException(
                    nameof(textBox));

            RemoveHandlers(textBox);

            ValidationSettings settings =
                new ValidationSettings
                {
                    Type = InputType.Text,
                    DecimalPlaces = 0,
                    Required = required,
                    AllowNegative = false,
                    SelectAllOnFocus = selectAllOnFocus
                };

            textBox.Tag = settings;

            textBox.Enter += TextBox_Enter;
            textBox.Leave += TextBox_Leave;
            textBox.Validating += TextBox_Validating;

            textBox.CausesValidation = true;
        }


        // ============================================================
        // SET INTEGER
        // ============================================================

        /// <summary>
        /// TextBox hanya menerima bilangan bulat.
        ///
        /// Contoh:
        /// 100
        /// 1000
        /// 150000
        ///
        /// Tidak menerima:
        /// 10,5
        /// ABC
        /// </summary>
        public static void SetInteger(
            TextBox textBox,
            bool required = false,
            bool allowNegative = false,
            bool selectAllOnFocus = true)
        {
            if (textBox == null)
                throw new ArgumentNullException(
                    nameof(textBox));

            RemoveHandlers(textBox);

            ValidationSettings settings =
                new ValidationSettings
                {
                    Type = InputType.Integer,
                    DecimalPlaces = 0,
                    Required = required,
                    AllowNegative = allowNegative,
                    SelectAllOnFocus = selectAllOnFocus
                };

            textBox.Tag = settings;

            textBox.Enter += TextBox_Enter;
            textBox.KeyPress += NumericTextBox_KeyPress;
            textBox.KeyDown += NumericTextBox_KeyDown;
            textBox.Leave += NumericTextBox_Leave;
            textBox.Validating += NumericTextBox_Validating;

            textBox.CausesValidation = true;
        }


        // ============================================================
        // SET DECIMAL
        // ============================================================

        /// <summary>
        /// TextBox menerima angka decimal dengan format Indonesia.
        ///
        /// Contoh:
        ///
        /// SetDecimal(txtHarga, 2);
        ///
        /// Input:
        /// 147730,77
        ///
        /// Display setelah Leave:
        /// 147.730,77
        /// </summary>
        public static void SetDecimal(
            TextBox textBox,
            int decimalPlaces = 2,
            bool required = false,
            bool allowNegative = false,
            bool selectAllOnFocus = true)
        {
            if (textBox == null)
                throw new ArgumentNullException(
                    nameof(textBox));

            if (decimalPlaces < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(decimalPlaces),
                    "DecimalPlaces tidak boleh kurang dari 0.");

            RemoveHandlers(textBox);

            ValidationSettings settings =
                new ValidationSettings
                {
                    Type = InputType.Decimal,
                    DecimalPlaces = decimalPlaces,
                    Required = required,
                    AllowNegative = allowNegative,
                    SelectAllOnFocus = selectAllOnFocus
                };

            textBox.Tag = settings;

            textBox.Enter += TextBox_Enter;
            textBox.KeyPress += NumericTextBox_KeyPress;
            textBox.KeyDown += NumericTextBox_KeyDown;
            textBox.Leave += NumericTextBox_Leave;
            textBox.Validating += NumericTextBox_Validating;

            textBox.CausesValidation = true;
        }


        // ============================================================
        // SET REQUIRED
        // ============================================================

        /// <summary>
        /// Menentukan apakah control TextBox wajib diisi.
        ///
        /// Bisa dipanggil setelah SetText, SetInteger,
        /// atau SetDecimal.
        /// </summary>
        public static void SetRequired(
            TextBox textBox,
            bool required = true)
        {
            if (textBox == null)
                throw new ArgumentNullException(
                    nameof(textBox));

            ValidationSettings settings =
                GetSettings(textBox);

            if (settings == null)
            {
                SetText(
                    textBox,
                    required);

                return;
            }

            settings.Required = required;
        }


        // ============================================================
        // SET DECIMAL PLACES
        // ============================================================

        /// <summary>
        /// Mengubah jumlah maksimum decimal.
        /// </summary>
        public static void SetDecimalPlaces(
            TextBox textBox,
            int decimalPlaces)
        {
            if (textBox == null)
                throw new ArgumentNullException(
                    nameof(textBox));

            if (decimalPlaces < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(decimalPlaces));

            ValidationSettings settings =
                GetSettings(textBox);

            if (settings == null)
                return;

            if (settings.Type != InputType.Decimal)
                return;

            settings.DecimalPlaces =
                decimalPlaces;
        }


        // ============================================================
        // ENTER
        // ============================================================

        private static void TextBox_Enter(
            object sender,
            EventArgs e)
        {
            TextBox textBox =
                sender as TextBox;

            if (textBox == null)
                return;

            ValidationSettings settings =
                GetSettings(textBox);

            if (settings == null)
                return;

            if (!settings.SelectAllOnFocus)
                return;

            textBox.BeginInvoke(
                new Action(() =>
                {
                    if (!textBox.IsDisposed &&
                        textBox.CanFocus)
                    {
                        textBox.SelectAll();
                    }
                }));
        }


        // ============================================================
        // KEY PRESS NUMERIC
        // ============================================================

        private static void NumericTextBox_KeyPress(
            object sender,
            KeyPressEventArgs e)
        {
            TextBox textBox =
                sender as TextBox;

            if (textBox == null)
                return;

            ValidationSettings settings =
                GetSettings(textBox);

            if (settings == null)
                return;


            // --------------------------------------------------------
            // CONTROL CHARACTER
            // --------------------------------------------------------

            // Backspace, Delete, dll.
            if (char.IsControl(e.KeyChar))
                return;


            // --------------------------------------------------------
            // DIGIT
            // --------------------------------------------------------

            if (char.IsDigit(e.KeyChar))
            {
                e.Handled =
                    !CanInsertDigit(
                        textBox,
                        settings,
                        e.KeyChar);

                return;
            }


            // --------------------------------------------------------
            // DECIMAL SEPARATOR
            // --------------------------------------------------------

            if (e.KeyChar == DecimalSeparator)
            {
                if (settings.Type !=
                    InputType.Decimal)
                {
                    e.Handled = true;
                    return;
                }

                // Tidak boleh dua koma.
                if (CountCharacter(
                    textBox.Text,
                    DecimalSeparator) > 0)
                {
                    e.Handled = true;
                    return;
                }

                // Jangan izinkan comma di awal
                // jika tidak ada angka.
                if (textBox.SelectionStart == 0 &&
                    textBox.SelectionLength == 0)
                {
                    e.Handled = true;
                    return;
                }

                e.Handled = false;

                return;
            }


            // --------------------------------------------------------
            // NEGATIVE
            // --------------------------------------------------------

            if (e.KeyChar == '-')
            {
                if (!settings.AllowNegative)
                {
                    e.Handled = true;
                    return;
                }

                // Minus harus di posisi pertama.
                if (textBox.SelectionStart != 0)
                {
                    e.Handled = true;
                    return;
                }

                // Tidak boleh minus kedua.
                if (textBox.Text.Contains("-"))
                {
                    e.Handled = true;
                    return;
                }

                e.Handled = false;

                return;
            }


            // --------------------------------------------------------
            // CHARACTER LAIN
            // --------------------------------------------------------

            // Semua huruf A-Z / a-z dan symbol lain ditolak.
            e.Handled = true;
        }


        // ============================================================
        // CAN INSERT DIGIT
        // ============================================================

        private static bool CanInsertDigit(
            TextBox textBox,
            ValidationSettings settings,
            char digit)
        {
            // Integer selalu boleh digit.
            if (settings.Type ==
                InputType.Integer)
            {
                return true;
            }


            if (settings.Type !=
                InputType.Decimal)
            {
                return false;
            }


            string currentText =
                textBox.Text;

            int selectionStart =
                textBox.SelectionStart;

            int selectionLength =
                textBox.SelectionLength;


            // Simulasi TextBox setelah digit dimasukkan.
            string newText =
                currentText.Remove(
                    selectionStart,
                    selectionLength)
                .Insert(
                    selectionStart,
                    digit.ToString());


            int decimalIndex =
                newText.IndexOf(
                    DecimalSeparator);


            // Belum ada comma.
            if (decimalIndex < 0)
                return true;


            int decimalCount =
                newText.Length -
                decimalIndex -
                1;


            return decimalCount <=
                   settings.DecimalPlaces;
        }


        // ============================================================
        // KEY DOWN
        // ============================================================

        private static void NumericTextBox_KeyDown(
            object sender,
            KeyEventArgs e)
        {
            TextBox textBox =
                sender as TextBox;

            if (textBox == null)
                return;


            // Ctrl + A
            if (e.Control &&
                e.KeyCode == Keys.A)
            {
                textBox.SelectAll();

                e.SuppressKeyPress = true;

                return;
            }


            // Ctrl + V
            if (e.Control &&
                e.KeyCode == Keys.V)
            {
                e.SuppressKeyPress = true;

                PasteNumericValue(textBox);

                return;
            }


            // Ctrl + C
            if (e.Control &&
                e.KeyCode == Keys.C)
            {
                return;
            }


            // Ctrl + X
            if (e.Control &&
                e.KeyCode == Keys.X)
            {
                return;
            }
        }


        // ============================================================
        // PASTE
        // ============================================================

        private static void PasteNumericValue(
            TextBox textBox)
        {
            if (!Clipboard.ContainsText())
                return;

            string clipboardText =
                Clipboard.GetText();

            if (string.IsNullOrWhiteSpace(
                clipboardText))
            {
                return;
            }

            clipboardText =
                clipboardText.Trim();


            ValidationSettings settings =
                GetSettings(textBox);

            if (settings == null)
                return;


            // --------------------------------------------------------
            // SIMULASI PASTE
            // --------------------------------------------------------

            string newText =
                textBox.Text.Remove(
                    textBox.SelectionStart,
                    textBox.SelectionLength)
                .Insert(
                    textBox.SelectionStart,
                    clipboardText);


            // --------------------------------------------------------
            // VALIDASI
            // --------------------------------------------------------

            if (!IsValidNumericInput(
                newText,
                settings,
                true))
            {
                System.Media.SystemSounds.Beep.Play();

                return;
            }


            // --------------------------------------------------------
            // MASUKKAN
            // --------------------------------------------------------

            int newPosition =
                textBox.SelectionStart +
                clipboardText.Length;

            textBox.Text =
                newText;

            textBox.SelectionStart =
                Math.Min(
                    newPosition,
                    textBox.Text.Length);
        }


        // ============================================================
        // LEAVE NUMERIC
        // ============================================================

        private static void NumericTextBox_Leave(
            object sender,
            EventArgs e)
        {
            TextBox textBox =
                sender as TextBox;

            if (textBox == null)
                return;


            ValidationSettings settings =
                GetSettings(textBox);

            if (settings == null)
                return;


            textBox.Text =
                textBox.Text.Trim();


            if (string.IsNullOrWhiteSpace(
                textBox.Text))
            {
                ClearError(textBox);

                return;
            }


            if (!IsValidNumericInput(
                textBox.Text,
                settings,
                false))
            {
                return;
            }


            FormatNumericText(
                textBox,
                settings);
        }


        // ============================================================
        // VALIDATING NUMERIC
        // ============================================================

        private static void NumericTextBox_Validating(
            object sender,
            System.ComponentModel.CancelEventArgs e)
        {
            TextBox textBox =
                sender as TextBox;

            if (textBox == null)
                return;


            ValidationSettings settings =
                GetSettings(textBox);

            if (settings == null)
                return;


            textBox.Text =
                textBox.Text.Trim();


            // --------------------------------------------------------
            // REQUIRED
            // --------------------------------------------------------

            if (settings.Required &&
                string.IsNullOrWhiteSpace(
                    textBox.Text))
            {
                ShowError(
                    textBox,
                    "Field ini wajib diisi.");

                e.Cancel = true;

                return;
            }


            // --------------------------------------------------------
            // EMPTY OPTIONAL
            // --------------------------------------------------------

            if (string.IsNullOrWhiteSpace(
                textBox.Text))
            {
                ClearError(textBox);

                return;
            }


            // --------------------------------------------------------
            // VALIDATE
            // --------------------------------------------------------

            if (!IsValidNumericInput(
                textBox.Text,
                settings,
                false))
            {
                ShowError(
                    textBox,
                    GetInvalidNumberMessage(
                        settings));

                e.Cancel = true;

                return;
            }


            // --------------------------------------------------------
            // FORMAT
            // --------------------------------------------------------

            FormatNumericText(
                textBox,
                settings);

            ClearError(textBox);
        }


        // ============================================================
        // VALIDATE NUMERIC INPUT
        // ============================================================

        private static bool IsValidNumericInput(
            string value,
            ValidationSettings settings,
            bool fromPaste)
        {
            if (string.IsNullOrWhiteSpace(
                value))
            {
                return false;
            }


            value =
                value.Trim();


            // ========================================================
            // INTEGER
            // ========================================================

            if (settings.Type ==
                InputType.Integer)
            {
                // Integer tidak boleh comma.
                if (value.Contains(","))
                    return false;


                // Input manual/paste tidak boleh
                // menggunakan separator ribuan.
                if (value.Contains("."))
                    return false;


                if (value == "-")
                    return false;


                if (value.StartsWith("-") &&
                    !settings.AllowNegative)
                {
                    return false;
                }


                int integerValue;


                return int.TryParse(
                    value,
                    NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture,
                    out integerValue);
            }


            // ========================================================
            // DECIMAL
            // ========================================================

            if (settings.Type ==
                InputType.Decimal)
            {
                // ----------------------------------------------------
                // PASTE
                //
                // Kita izinkan display format Indonesia:
                //
                // 147.730,77
                //
                // ----------------------------------------------------

                if (fromPaste &&
                    value.Contains("."))
                {
                    decimal pastedValue;

                    return decimal.TryParse(
                        value,
                        NumberStyles.Number |
                        NumberStyles.AllowLeadingSign,
                        IndonesianCulture,
                        out pastedValue);
                }


                // Input normal tidak boleh titik.
                if (value.Contains("."))
                    return false;


                // Hanya boleh satu comma.
                if (CountCharacter(
                    value,
                    DecimalSeparator) > 1)
                {
                    return false;
                }


                if (value == ",")
                    return false;


                if (value == "-")
                    return false;


                if (value == "-,")
                    return false;


                if (value.StartsWith("-") &&
                    !settings.AllowNegative)
                {
                    return false;
                }


                int decimalIndex =
                    value.IndexOf(
                        DecimalSeparator);


                if (decimalIndex >= 0)
                {
                    int decimalCount =
                        value.Length -
                        decimalIndex -
                        1;


                    if (decimalCount >
                        settings.DecimalPlaces)
                    {
                        return false;
                    }


                    // Harus ada angka setelah comma.
                    if (decimalCount == 0)
                        return false;
                }


                decimal decimalValue;


                return decimal.TryParse(
                    value,
                    NumberStyles.AllowLeadingSign |
                    NumberStyles.AllowDecimalPoint,
                    IndonesianCulture,
                    out decimalValue);
            }


            return true;
        }


        // ============================================================
        // FORMAT NUMERIC
        // ============================================================

        private static void FormatNumericText(
            TextBox textBox,
            ValidationSettings settings)
        {
            if (string.IsNullOrWhiteSpace(
                textBox.Text))
            {
                return;
            }


            // ========================================================
            // INTEGER
            // ========================================================

            if (settings.Type ==
                InputType.Integer)
            {
                int value;


                if (!int.TryParse(
                    textBox.Text,
                    NumberStyles.Number |
                    NumberStyles.AllowLeadingSign,
                    IndonesianCulture,
                    out value))
                {
                    return;
                }


                textBox.Text =
                    value.ToString(
                        "#,##0",
                        IndonesianCulture);

                return;
            }


            // ========================================================
            // DECIMAL
            // ========================================================

            if (settings.Type ==
                InputType.Decimal)
            {
                decimal value;


                if (!decimal.TryParse(
                    textBox.Text,
                    NumberStyles.Number |
                    NumberStyles.AllowLeadingSign,
                    IndonesianCulture,
                    out value))
                {
                    return;
                }


                string format;


                if (settings.DecimalPlaces <= 0)
                {
                    format = "#,##0";
                }
                else
                {
                    // # = decimal optional.
                    //
                    // 1000
                    //       -> 1.000
                    //
                    // 1000,5
                    //       -> 1.000,5
                    //
                    // 1000,50
                    //       -> 1.000,5
                    //
                    // 1000,55
                    //       -> 1.000,55

                    format =
                        "#,##0." +
                        new string(
                            '#',
                            settings.DecimalPlaces);
                }


                textBox.Text =
                    value.ToString(
                        format,
                        IndonesianCulture);
            }
        }


        // ============================================================
        // NORMAL TEXTBOX LEAVE
        // ============================================================

        private static void TextBox_Leave(
            object sender,
            EventArgs e)
        {
            TextBox textBox =
                sender as TextBox;

            if (textBox == null)
                return;


            textBox.Text =
                textBox.Text.Trim();
        }


        // ============================================================
        // NORMAL TEXTBOX VALIDATING
        // ============================================================

        private static void TextBox_Validating(
            object sender,
            System.ComponentModel.CancelEventArgs e)
        {
            TextBox textBox =
                sender as TextBox;

            if (textBox == null)
                return;


            ValidationSettings settings =
                GetSettings(textBox);

            if (settings == null)
                return;


            textBox.Text =
                textBox.Text.Trim();


            if (settings.Required &&
                string.IsNullOrWhiteSpace(
                    textBox.Text))
            {
                ShowError(
                    textBox,
                    "Field ini wajib diisi.");

                e.Cancel = true;

                return;
            }


            ClearError(textBox);
        }


        // ============================================================
        // PUBLIC VALIDATE
        // ============================================================

        /// <summary>
        /// Validasi satu TextBox secara manual.
        ///
        /// Sangat berguna ketika tombol Save ditekan.
        /// </summary>
        public static bool Validate(
            TextBox textBox)
        {
            if (textBox == null)
                return false;


            ValidationSettings settings =
                GetSettings(textBox);


            if (settings == null)
                return true;


            textBox.Text =
                textBox.Text.Trim();


            // ========================================================
            // REQUIRED
            // ========================================================

            if (settings.Required &&
                string.IsNullOrWhiteSpace(
                    textBox.Text))
            {
                ShowError(
                    textBox,
                    "Field ini wajib diisi.");

                textBox.Focus();

                return false;
            }


            // ========================================================
            // OPTIONAL EMPTY
            // ========================================================

            if (string.IsNullOrWhiteSpace(
                textBox.Text))
            {
                ClearError(textBox);

                return true;
            }


            // ========================================================
            // NUMERIC
            // ========================================================

            if (settings.Type ==
                    InputType.Integer ||
                settings.Type ==
                    InputType.Decimal)
            {
                if (!IsValidNumericInput(
                    textBox.Text,
                    settings,
                    false))
                {
                    ShowError(
                        textBox,
                        GetInvalidNumberMessage(
                            settings));

                    textBox.Focus();

                    return false;
                }


                FormatNumericText(
                    textBox,
                    settings);
            }


            ClearError(textBox);

            return true;
        }


        // ============================================================
        // VALIDATE ALL
        // ============================================================

        /// <summary>
        /// Validasi beberapa TextBox.
        ///
        /// Validasi berhenti pada TextBox pertama
        /// yang tidak valid.
        /// </summary>
        public static bool ValidateAll(
            params TextBox[] textBoxes)
        {
            if (textBoxes == null)
                return true;


            foreach (TextBox textBox in textBoxes)
            {
                if (textBox == null)
                    continue;


                if (!Validate(textBox))
                {
                    return false;
                }
            }


            return true;
        }


        // ============================================================
        // TRY GET DECIMAL
        // ============================================================

        /// <summary>
        /// Mengambil nilai TextBox menjadi decimal.
        ///
        /// Contoh:
        ///
        /// TextBox:
        /// 147.730,77
        ///
        /// Result:
        /// 147730.77m
        /// </summary>
        public static bool TryGetDecimal(
            TextBox textBox,
            out decimal value)
        {
            value = 0m;


            if (textBox == null)
                return false;


            string text =
                textBox.Text.Trim();


            if (string.IsNullOrWhiteSpace(
                text))
            {
                return false;
            }


            return decimal.TryParse(
                text,
                NumberStyles.Number |
                NumberStyles.AllowLeadingSign,
                IndonesianCulture,
                out value);
        }


        // ============================================================
        // TRY GET INTEGER
        // ============================================================

        /// <summary>
        /// Mengambil nilai TextBox menjadi integer.
        /// </summary>
        public static bool TryGetInteger(
            TextBox textBox,
            out int value)
        {
            value = 0;


            if (textBox == null)
                return false;


            string text =
                textBox.Text.Trim();


            if (string.IsNullOrWhiteSpace(
                text))
            {
                return false;
            }


            return int.TryParse(
                text,
                NumberStyles.Number |
                NumberStyles.AllowLeadingSign,
                IndonesianCulture,
                out value);
        }


        // ============================================================
        // GET DECIMAL
        // ============================================================

        /// <summary>
        /// Mengambil nilai decimal.
        ///
        /// Jika gagal/kosong:
        /// return 0.
        /// </summary>
        public static decimal GetDecimal(
            TextBox textBox)
        {
            decimal value;


            if (TryGetDecimal(
                textBox,
                out value))
            {
                return value;
            }


            return 0m;
        }


        // ============================================================
        // GET INTEGER
        // ============================================================

        public static int GetInteger(
            TextBox textBox)
        {
            int value;


            if (TryGetInteger(
                textBox,
                out value))
            {
                return value;
            }


            return 0;
        }


        // ============================================================
        // FORMAT DECIMAL
        // ============================================================

        /// <summary>
        /// Format decimal untuk tampilan Indonesia.
        ///
        /// Contoh:
        ///
        /// 147730.77
        /// ->
        /// 147.730,77
        /// </summary>
        public static string FormatDecimal(
            decimal value,
            int decimalPlaces = 2)
        {
            if (decimalPlaces < 0)
                decimalPlaces = 0;


            string format;


            if (decimalPlaces == 0)
            {
                format = "#,##0";
            }
            else
            {
                format =
                    "#,##0." +
                    new string(
                        '#',
                        decimalPlaces);
            }


            return value.ToString(
                format,
                IndonesianCulture);
        }


        // ============================================================
        // FORMAT INTEGER
        // ============================================================

        public static string FormatInteger(
            int value)
        {
            return value.ToString(
                "#,##0",
                IndonesianCulture);
        }


        // ============================================================
        // INVALID MESSAGE
        // ============================================================

        private static string GetInvalidNumberMessage(
            ValidationSettings settings)
        {
            if (settings.Type ==
                InputType.Integer)
            {
                if (settings.AllowNegative)
                {
                    return
                        "Masukkan bilangan bulat yang valid.";
                }


                return
                    "Masukkan bilangan bulat positif.";
            }


            if (settings.Type ==
                InputType.Decimal)
            {
                if (settings.AllowNegative)
                {
                    return
                        "Masukkan angka decimal yang valid " +
                        $"dengan maksimal {settings.DecimalPlaces} " +
                        "digit decimal.";
                }


                return
                    "Masukkan angka positif yang valid " +
                    $"dengan maksimal {settings.DecimalPlaces} " +
                    "digit decimal.";
            }


            return "Nilai tidak valid.";
        }


        // ============================================================
        // SHOW ERROR
        // ============================================================

        private static void ShowError(
            TextBox textBox,
            string message)
        {
            ErrorProvider provider =
                GetErrorProvider(textBox);


            provider.SetError(
                textBox,
                message);
        }


        // ============================================================
        // CLEAR ERROR
        // ============================================================

        private static void ClearError(
            TextBox textBox)
        {
            ErrorProvider provider =
                GetErrorProvider(textBox);


            provider.SetError(
                textBox,
                string.Empty);
        }


        // ============================================================
        // GET ERROR PROVIDER
        // ============================================================

        private static ErrorProvider GetErrorProvider(
            Control control)
        {
            Form form =
                control.FindForm();


            if (form == null)
            {
                return new ErrorProvider();
            }


            ErrorProvider provider;


            if (!ErrorProviders.TryGetValue(
                form,
                out provider))
            {
                provider =
                    new ErrorProvider(form);


                provider.BlinkStyle =
                    ErrorBlinkStyle.NeverBlink;


                ErrorProviders.Add(
                    form,
                    provider);
            }


            return provider;
        }


        // ============================================================
        // GET SETTINGS
        // ============================================================

        private static ValidationSettings GetSettings(
            TextBox textBox)
        {
            if (textBox == null)
                return null;


            return textBox.Tag
                as ValidationSettings;
        }


        // ============================================================
        // REMOVE HANDLERS
        // ============================================================

        private static void RemoveHandlers(
            TextBox textBox)
        {
            textBox.Enter -=
                TextBox_Enter;

            textBox.KeyPress -=
                NumericTextBox_KeyPress;

            textBox.KeyDown -=
                NumericTextBox_KeyDown;

            textBox.Leave -=
                NumericTextBox_Leave;

            textBox.Leave -=
                TextBox_Leave;

            textBox.Validating -=
                NumericTextBox_Validating;

            textBox.Validating -=
                TextBox_Validating;
        }


        // ============================================================
        // COUNT CHARACTER
        // ============================================================

        private static int CountCharacter(
            string value,
            char character)
        {
            int count = 0;


            foreach (char c in value)
            {
                if (c == character)
                    count++;
            }


            return count;
        }


        // ============================================================
        // INPUT TYPE
        // ============================================================

        private enum InputType
        {
            Text,
            Integer,
            Decimal
        }


        // ============================================================
        // VALIDATION SETTINGS
        // ============================================================

        private sealed class ValidationSettings
        {
            public InputType Type
            {
                get;
                set;
            }


            public int DecimalPlaces
            {
                get;
                set;
            }


            public bool Required
            {
                get;
                set;
            }


            public bool AllowNegative
            {
                get;
                set;
            }


            public bool SelectAllOnFocus
            {
                get;
                set;
            }
        }
    }
}
