namespace WinFormsTest
{
    partial class SshTerminal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            sendButton = new Button();
            inputBox = new TextBox();
            terminalOutput = new RichTextBox();
            SuspendLayout();
            // 
            // sendButton
            // 
            sendButton.Dock = DockStyle.Bottom;
            sendButton.Location = new Point(0, 538);
            sendButton.Name = "sendButton";
            sendButton.Size = new Size(784, 23);
            sendButton.TabIndex = 2;
            sendButton.Text = "Send";
            sendButton.UseVisualStyleBackColor = true;
            // 
            // inputBox
            // 
            inputBox.Dock = DockStyle.Bottom;
            inputBox.Location = new Point(0, 515);
            inputBox.Name = "inputBox";
            inputBox.Size = new Size(784, 23);
            inputBox.TabIndex = 3;
            // 
            // terminalOutput
            // 
            terminalOutput.BackColor = Color.Black;
            terminalOutput.Dock = DockStyle.Fill;
            terminalOutput.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            terminalOutput.Location = new Point(0, 0);
            terminalOutput.Name = "terminalOutput";
            terminalOutput.ReadOnly = true;
            terminalOutput.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            terminalOutput.Size = new Size(784, 515);
            terminalOutput.TabIndex = 4;
            terminalOutput.Text = "";
            // 
            // SshTerminal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(terminalOutput);
            Controls.Add(inputBox);
            Controls.Add(sendButton);
            Name = "SshTerminal";
            Text = "SSH Terminal Emulator";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button sendButton;
        private TextBox inputBox;
        private RichTextBox terminalOutput;
    }
}
