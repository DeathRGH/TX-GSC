using System.Drawing;
using System.Windows.Forms;

namespace T7_GSC
{
	public class GroupBoxX : GroupBox
	{
		private Color _borderColor;
		public Color borderColor
		{
			get { return _borderColor; }
			set { _borderColor = value; }
		}

		public GroupBoxX()
		{
			_borderColor = Color.FromArgb(63, 63, 70);
			Size = new Size(162, 68);
			Padding = new Padding(3, 3, 3, 3);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Size tSize = TextRenderer.MeasureText(Text, Font);
			Rectangle borderRect = e.ClipRectangle;
			borderRect.Y += tSize.Height / 2;
			borderRect.Height -= tSize.Height / 2;
			ControlPaint.DrawBorder(e.Graphics, borderRect, _borderColor, ButtonBorderStyle.Solid);
			Rectangle textRect = e.ClipRectangle;
			textRect.X += 6;
			textRect.Width = tSize.Width;
			textRect.Height = tSize.Height;
			e.Graphics.FillRectangle(new SolidBrush(BackColor), textRect);
			e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), textRect);
		}
	}

	public class ListBoxX : ListBox
	{
		public ListBoxX()
		{
			BackColor = Color.FromArgb(37, 37, 38);
			ForeColor = Color.FromArgb(224, 224, 224);
			TabStop = false;
			BorderStyle = BorderStyle.None;
			Padding = new Padding(3, 3, 3, 3);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle rect = e.ClipRectangle;
			rect.Size = new Size(rect.Size.Width + 10, rect.Size.Height + 10);
			e.Graphics.FillRectangle(new SolidBrush(BackColor), rect);
		}
	}

	public class ButtonX : Button
	{
		public ButtonX()
		{
			Size = new Size(75, 20);
			BackColor = Color.FromArgb(37, 37, 38);
			FlatAppearance.BorderColor = Color.FromArgb(63, 63, 70);
			FlatAppearance.BorderSize = 1;
			FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 122, 204);
			FlatAppearance.MouseOverBackColor = Color.FromArgb(84, 84, 92);
			FlatStyle = FlatStyle.Flat;
			ForeColor = Color.FromArgb(224, 224, 224);
			TabStop = false;
			UseMnemonic = false;
			UseVisualStyleBackColor = false;
			UseCompatibleTextRendering = true;
		}
	}

	public class TextBoxX : TextBox
	{
		public TextBoxX()
		{
			BackColor = Color.FromArgb(37, 37, 38);
			ForeColor = Color.FromArgb(224, 224, 224);
			TabStop = false;
			BorderStyle = BorderStyle.FixedSingle;
		}
	}
}
