using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Vip.ComboBox
{
    /// <summary>
    ///     A dropdown window for combos.
    /// </summary>
    [ToolboxItem(false)]
    public class DropDownControl : ToolStripDropDown, IMessageFilter
    {
        #region Properties

        /// <summary>
        ///     Gets the content of the pop-up.
        /// </summary>
        public Control Content { get; private set; }

        #endregion

        #region Fields

        /// <summary>The control for which we display this dropdown</summary>
        private Control _opener;

        #endregion

        #region Construtor

        public DropDownControl(Control content)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            Content = content;
            Content.Location = Point.Empty;
            var host = new ToolStripControlHost(Content);
            // NB: AutoClose must be set to false, because otherwise the ToolStripManager would steal keyboard events
            AutoClose = false;
            // we do ourselves the sizing
            AutoSize = false;
            DoubleBuffered = true;
            ResizeRedraw = false;
            Padding = Margin = host.Padding = host.Margin = Padding.Empty;
            // we adjust the size according to the contents
            MinimumSize = Content.MinimumSize;
            content.MinimumSize = Content.Size;
            MaximumSize = Content.MaximumSize;
            content.MaximumSize = Content.Size;
            Size = Content.Size;
            TabStop = Content.TabStop = true;
            // set up the content
            Items.Add(host);
            // we must listen to mouse events for "emulating" AutoClose
            Application.AddMessageFilter(this);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Display the dropdown and adjust its size and location
        /// </summary>
        public void Show(Control opener, Size preferredSize = new Size())
        {
            _opener = opener ?? throw new ArgumentNullException(nameof(opener));

            int w = preferredSize.Width == 0 ? ClientRectangle.Width : preferredSize.Width;
            int h = preferredSize.Height == 0 ? Content.Height : preferredSize.Height;
            h += Padding.Size.Height + Content.Margin.Size.Height;

            Rectangle screen = Screen.FromControl(_opener).WorkingArea;

            // let's try first to place it below the opener control
            Rectangle loc = _opener.RectangleToScreen(
                new Rectangle(_opener.ClientRectangle.Left, _opener.ClientRectangle.Bottom,
                    _opener.ClientRectangle.Left + w, _opener.ClientRectangle.Bottom + h));
            Point cloc = new Point(_opener.ClientRectangle.Left, _opener.ClientRectangle.Bottom);
            if (!screen.Contains(loc))
            {
                // let's try above the opener control
                loc = _opener.RectangleToScreen(new Rectangle(_opener.ClientRectangle.Left, _opener.ClientRectangle.Top - h,
                    _opener.ClientRectangle.Left + w, _opener.ClientRectangle.Top));
                if (screen.Contains(loc)) cloc = new Point(_opener.ClientRectangle.Left, _opener.ClientRectangle.Top - h);
            }

            Width = w;
            Height = h;
            Show(_opener, cloc, ToolStripDropDownDirection.BelowRight);
        }

        #endregion

        #region internals

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Content != null)
                {
                    Content.Dispose();
                    Content = null;
                }

                Application.RemoveMessageFilter(this);
            }

            base.Dispose(disposing);
        }

        /// <summary>
        ///     On resizes, resize the contents
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            if (Content != null)
            {
                Content.MinimumSize = Size;
                Content.MaximumSize = Size;
                Content.Size = Size;
                Content.Location = Point.Empty;
            }

            base.OnSizeChanged(e);
        }

        #endregion

        #region IMessageFilter implementation

        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_MBUTTONDOWN = 0x0207;
        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int WM_NCRBUTTONDOWN = 0x00A4;
        private const int WM_NCMBUTTONDOWN = 0x00A7;

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In] [Out] ref Point pt, int cPoints);

        /// <summary>
        ///     We monitor all messages in order to detect when the users clicks outside the dropdown and the combo
        ///     If this happens, we close the dropdown (as AutoClose is false)
        /// </summary>
        public bool PreFilterMessage(ref Message m)
        {
            if (Visible)
                switch (m.Msg)
                {
                    case WM_LBUTTONDOWN:
                    case WM_RBUTTONDOWN:
                    case WM_MBUTTONDOWN:
                    case WM_NCLBUTTONDOWN:
                    case WM_NCRBUTTONDOWN:
                    case WM_NCMBUTTONDOWN:
                        int i = unchecked((int) (long) m.LParam);
                        short x = (short) (i & 0xFFFF);
                        short y = (short) ((i >> 16) & 0xffff);
                        Point pt = new Point(x, y);
                        IntPtr srcWnd =
                            // client area: x, y are relative to the client area of the windows
                            m.Msg == WM_LBUTTONDOWN || m.Msg == WM_RBUTTONDOWN || m.Msg == WM_MBUTTONDOWN
                                ? m.HWnd
                                :
                                // non-client area: x, y are relative to the desktop
                                IntPtr.Zero;

                        MapWindowPoints(srcWnd, Handle, ref pt, 1);
                        if (!ClientRectangle.Contains(pt))
                        {
                            // the user has clicked outside the dropdown
                            pt = new Point(x, y);
                            MapWindowPoints(srcWnd, _opener.Handle, ref pt, 1);
                            if (!_opener.ClientRectangle.Contains(pt))
                                // the user has clicked outside the opener control
                                Close();
                        }

                        break;
                }

            return false;
        }

        #endregion
    }
}