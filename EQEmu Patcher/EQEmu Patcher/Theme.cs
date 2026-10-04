using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EQEmu_Patcher
{
    /// <summary>Magrathea colours: dark stone background with gold accents, to match the splash art.</summary>
    public static class Theme
    {
        public static readonly Color Background = Color.FromArgb(14, 14, 16);
        public static readonly Color Panel = Color.FromArgb(24, 22, 20);
        public static readonly Color Gold = Color.FromArgb(217, 164, 65);
        public static readonly Color GoldBright = Color.FromArgb(244, 200, 100);
        public static readonly Color GoldDark = Color.FromArgb(120, 88, 32);
        public static readonly Color Text = Color.FromArgb(232, 220, 192);

        public static void StyleButton(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.UseVisualStyleBackColor = false;
            b.FlatAppearance.BorderColor = Gold;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.MouseOverBackColor = GoldDark;
            b.FlatAppearance.MouseDownBackColor = Gold;
            b.Cursor = Cursors.Hand;
            SetButtonAttention(b, false);
        }

        // "Attention" = solid gold, used to say "something needs doing" (e.g. an update is available).
        public static void SetButtonAttention(Button b, bool attention)
        {
            if (attention)
            {
                b.BackColor = Gold;
                b.ForeColor = Background;
            }
            else
            {
                b.BackColor = Panel;
                b.ForeColor = Gold;
            }
        }

        public static void StyleCheckBox(CheckBox c)
        {
            c.FlatStyle = FlatStyle.Flat;
            c.UseVisualStyleBackColor = false;
            c.BackColor = Background;
            c.ForeColor = Text;
        }
    }

    /// <summary>A progress bar that draws itself, so it can be gold instead of the system green.</summary>
    public class GoldProgressBar : ProgressBar
    {
        public GoldProgressBar()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (SolidBrush back = new SolidBrush(Theme.Panel))
            {
                e.Graphics.FillRectangle(back, bounds);
            }

            double range = Maximum - Minimum;
            double fraction = range <= 0 ? 0 : (double)(Value - Minimum) / range;
            if (fraction < 0) fraction = 0;
            if (fraction > 1) fraction = 1;
            int fillWidth = (int)((bounds.Width - 2) * fraction);
            if (fillWidth > 0)
            {
                Rectangle fill = new Rectangle(1, 1, fillWidth, bounds.Height - 2);
                using (System.Drawing.Drawing2D.LinearGradientBrush brush =
                    new System.Drawing.Drawing2D.LinearGradientBrush(fill, Theme.GoldBright, Theme.GoldDark, System.Drawing.Drawing2D.LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, fill);
                }
            }

            using (Pen border = new Pen(Theme.Gold))
            {
                e.Graphics.DrawRectangle(border, bounds);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Everything is drawn in OnPaint; skipping this avoids flicker.
        }
    }
}
