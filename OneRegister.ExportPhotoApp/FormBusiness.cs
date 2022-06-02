using System;
using System.Drawing;
using System.Windows.Forms;

namespace OneRegister.ExportPhotoApp
{
    public class FormBusiness
    {
        private RichTextBox _display;

        public FormBusiness(RichTextBox result)
        {
            _display = result;
        }

        public void WriteVersion(string text)
        {
            _display.SelectionStart = _display.TextLength;
            _display.SelectionLength = 0;
            _display.SelectionColor = Color.Black;
            _display.SelectionBackColor = Color.Yellow;
            _display.AppendText(text);
            _display.SelectionColor = _display.ForeColor;
            _display.SelectionBackColor = _display.BackColor;
        }

        public void WriteInfo(string text)
        {
            _display.SelectionStart = _display.TextLength;
            _display.SelectionLength = 0;
            _display.SelectionColor = Color.Black;
            _display.AppendText(Environment.NewLine + text);
            _display.SelectionColor = _display.ForeColor;
        }

        public void WriteError(string text)
        {
            _display.SelectionStart = _display.TextLength;
            _display.SelectionLength = 0;
            _display.SelectionColor = Color.Red;
            _display.AppendText(Environment.NewLine + text);
            _display.SelectionColor = _display.ForeColor;
        }

        public void WriteWarning(string text)
        {
            _display.SelectionStart = _display.TextLength;
            _display.SelectionLength = 0;
            _display.SelectionColor = Color.Yellow;
            _display.AppendText(Environment.NewLine + text);
            _display.SelectionColor = _display.ForeColor;
        }

        public void WriteSuccess(string text)
        {
            _display.SelectionStart = _display.TextLength;
            _display.SelectionLength = 0;
            _display.SelectionColor = Color.Green;
            _display.AppendText(Environment.NewLine + text);
            _display.SelectionColor = _display.ForeColor;
        }
    }
}