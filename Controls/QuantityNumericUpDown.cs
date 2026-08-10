namespace KabyliaTaste.Controls
{
    using System;
    using System.Globalization;
    using System.Drawing;
    using System.Windows.Forms;

    public class QuantityNumericUpDown : TextBox
    {
        private bool _updatingText;
        private decimal _value;

        public event EventHandler? ValueChanged;

        public int DecimalPlaces { get; set; } = 1;

        public decimal Minimum { get; set; } = 0m;

        public decimal Maximum { get; set; } = 1000000m;

        public decimal Increment { get; set; } = 0.1m;

        public bool ThousandsSeparator { get; set; }

        public decimal Value
        {
            get => _value;
            set
            {
                var clamped = Clamp(value);
                if (_value == clamped && Text == FormatValue(clamped))
                    return;

                _value = clamped;
                UpdateDisplayText();
                OnValueChanged(EventArgs.Empty);
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            UpdateDisplayText();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (_updatingText)
            {
                base.OnTextChanged(e);
                return;
            }

            var text = (Text ?? string.Empty).Trim();
            if (IsPartialInput(text))
            {
                base.OnTextChanged(e);
                return;
            }

            if (TryParseText(out var parsed))
            {
                var clamped = Clamp(parsed);
                if (_value != clamped)
                {
                    _value = clamped;
                    OnValueChanged(EventArgs.Empty);
                }

                UpdateDisplayText();
            }

            base.OnTextChanged(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            if (TryParseText(out var parsed))
                Value = parsed;
            else
                UpdateDisplayText();

            base.OnLeave(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == ',')
                e.KeyChar = '.';

            if (e.KeyChar == '.')
            {
                if (Text.Contains('.') || Text.Contains(','))
                {
                    e.Handled = true;
                    return;
                }
            }

            base.OnKeyPress(e);
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        private void UpdateDisplayText()
        {
            _updatingText = true;
            Text = FormatValue(_value);
            SelectionStart = Text.Length;
            SelectionLength = 0;
            _updatingText = false;
        }

        private bool TryParseText(out decimal value)
        {
            var text = (Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                value = Minimum;
                return true;
            }

            text = text.Replace(',', '.');
            return decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out value);
        }

        private static bool IsPartialInput(string text)
        {
            return text.EndsWith('.') || text.EndsWith(',') || text == "-" || text == "+";
        }

        private decimal Clamp(decimal value) => Math.Min(Maximum, Math.Max(Minimum, value));

        private string FormatValue(decimal value)
        {
            if (DecimalPlaces <= 0)
                return Math.Round(value, 0).ToString("0", CultureInfo.InvariantCulture);

            var decimals = new string('#', DecimalPlaces);
            var format = $"0.{decimals}";
            return value.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}
