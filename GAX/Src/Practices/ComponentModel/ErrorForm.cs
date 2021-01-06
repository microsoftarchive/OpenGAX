using System;
using System.Windows.Forms;

namespace Microsoft.Practices.ComponentModel
{
	internal enum AlignButtons
    {
        AlignCenter,
        AlignLeft
    }
    internal sealed partial class ErrorForm : Form, IErrorInfo
    {
        #region Settings

        // private const int allHeight = 400; // scale will cause real height be different.
        private int cutHeight = 0;
        private int separationButton = 16;
        private AlignButtons align = AlignButtons.AlignCenter;

		#endregion

		int allHeight; // scale will cause real height be different.

		public ErrorForm(string caption, string text, string questiondetails, string yesString, string noString)
		{
			InitializeComponent();
			this.Text = caption;
			this.labelMessage.Text = text;
			this.buttons = MessageBoxButtons.YesNo;
			this.exceptionDetails.Text = questiondetails;
			this.exceptionDetails.Select(0, 0);
			this.pictureBox1.Image = Properties.Resources.Icon2.ToBitmap();
			this.showDetailsMessage = Properties.Resources.ErrorHelper_ShowDetails;
			this.hideDetailsMessage = Properties.Resources.ErrorHelper_HideDetails;
			this.toolTip1.SetToolTip(buttonException, showDetailsMessage);
			SetupButtons();
			this.buttonException.Visible = true;
			this.button1.Text = yesString;
			this.button2.Text = noString;

			this.allHeight = this.exceptionDetails.Location.Y + this.exceptionDetails.Height + separationButton;
		}

		public ErrorForm(string caption, string text, Exception exception, MessageBoxButtons buttons)
		{
			InitializeComponent();
			this.Text = caption;
			this.labelMessage.Text = text;
			this.exception = exception;
			this.buttons = buttons;
            if (exception != null)
            {
                this.exceptionDetails.Text = exception.ToString();
                this.exceptionDetails.Select(0, 0);
            }
            this.pictureBox1.Image = Properties.Resources.Icon3.ToBitmap();
			this.showDetailsMessage = Properties.Resources.ErrorHelper_ShowDetailsException;
			this.hideDetailsMessage = Properties.Resources.ErrorHelper_HideDetailsException;
			this.toolTip1.SetToolTip(buttonException, this.showDetailsMessage);
            SetupButtons();

			this.allHeight = this.exceptionDetails.Location.Y + this.exceptionDetails.Height + separationButton;
		}

        private void SetupButtons()
        {
            if (this.exception == null)
            {
                this.buttonException.Visible = false;
            }
            int nButtons = 0;

            switch (buttons)
            {
                case MessageBoxButtons.AbortRetryIgnore:
                    this.button1.Text = Properties.Resources.ErrorHelper_Abort;
                    this.button1.Click += new EventHandler(OnAbort);
                    this.button2.Text = Properties.Resources.ErrorHelper_Retry;
                    this.button2.Click += new EventHandler(OnRetry);
                    this.button3.Text = Properties.Resources.ErrorHelper_Ignore;
                    this.button3.Click += new EventHandler(OnIgnore);
                    this.CancelButton = this.button3;
                    this.AcceptButton = this.button2;
                    nButtons = 3;
                    break;
                case MessageBoxButtons.OK:
                    this.button1.Text = Properties.Resources.ErrorHelper_OK;
                    this.button1.Click += new EventHandler(OnOK);
                    this.AcceptButton = this.button1;
                    nButtons = 1;
                    break;
                case MessageBoxButtons.OKCancel:
                    this.button1.Text = Properties.Resources.ErrorHelper_OK;
                    this.button1.Click += new EventHandler(OnOK);
                    this.button2.Text = Properties.Resources.ErrorHelper_Cancel;
                    this.button2.Click += new EventHandler(OnCancel);
                    this.AcceptButton = this.button1;
                    this.CancelButton = this.button2;
                    nButtons = 2;
                    break;
                case MessageBoxButtons.RetryCancel:
                    this.button1.Text = Properties.Resources.ErrorHelper_Retry;
                    this.button1.Click += new EventHandler(OnRetry);
                    this.button2.Text = Properties.Resources.ErrorHelper_Cancel;
                    this.button2.Click += new EventHandler(OnCancel);
                    this.AcceptButton = this.button1;
                    this.CancelButton = this.button2;
                    nButtons = 2;
                    break;
                case MessageBoxButtons.YesNo:
                    this.button1.Text = Properties.Resources.ErrorHelper_Yes;
                    this.button1.Click += new EventHandler(OnYes);
                    this.button2.Text = Properties.Resources.ErrorHelper_No;
                    this.button2.Click += new EventHandler(OnNo);
                    this.AcceptButton = this.button1;
                    this.CancelButton = this.button2;
                    nButtons = 2;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    this.button1.Text = Properties.Resources.ErrorHelper_Yes;
                    this.button1.Click += new EventHandler(OnYes);
                    this.button2.Text = Properties.Resources.ErrorHelper_No;
                    this.button2.Click += new EventHandler(OnNo);
                    this.button3.Text = Properties.Resources.ErrorHelper_Cancel;
                    this.button3.Click += new EventHandler(OnCancel);
                    this.AcceptButton = this.button1;
                    this.CancelButton = this.button3;
                    nButtons = 3;
                    break;
            }
            int totalWidth = this.button1.Width * nButtons + separationButton * (nButtons - 1);
            int start;
            if (align == AlignButtons.AlignCenter)
            {
                start = (this.Width - totalWidth) / 2 + this.Left;
            }
            else //if (align == AlignButtons.AlignLeft)
            {
                start = this.Left + this.Width - totalWidth - (int)(separationButton * 1.5);
            }
            int step = this.button1.Width + separationButton;
            this.button1.Left = start;
            if (nButtons > 1)
            {
                start += step;
                this.button2.Left = start;
                if (nButtons > 2)
                {
                    start += step;
                    this.button3.Left = start;
                }
            }
            if (nButtons <= 2)
            {
                this.button3.Visible = false;
                if (nButtons <= 1)
                {
                    this.button2.Visible = false;
                }
            }
            this.button1.Focus();
        }

		#region Event Handlers

		void OnNo(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.No;
			Close();
		}

		void OnCancel(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			Close();
		}
		
		void OnYes(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Yes;
			Close();
		}

		void OnOK(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			Close();
		}

		void OnAbort(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Abort;
			Close();
		}

		void OnRetry(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Retry;
			Close();
		}

		void OnIgnore(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Ignore;
			Close();
		}

        private void buttonException_Click(object sender, EventArgs e)
        {
            if (this.Height != allHeight)
            {
                this.cutHeight = this.Height;
                this.Height = allHeight;
                buttonException.ImageIndex = 1;
                this.toolTip1.SetToolTip(buttonException, this.hideDetailsMessage);
            }
            else
            {
                this.Height = cutHeight;
                buttonException.ImageIndex = 0;
                this.toolTip1.SetToolTip(buttonException, this.showDetailsMessage);
            }
        }

        #endregion

        #region Error Members

        public string Caption
        {
            get { return Text; }
        }

        public string Message
        {
            get { return this.labelMessage.Text; }
        }

        public Exception Exception
        {
            get { return exception; }
        } Exception exception;

        public MessageBoxButtons MessageBoxButtons
        {
            get { return buttons; }
        } MessageBoxButtons buttons;

		private string showDetailsMessage;

		private string hideDetailsMessage;

        #endregion
    }
}