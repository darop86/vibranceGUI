using vibrance.GUI.Properties;

namespace vibrance.GUI.common
{
    partial class GraphicsDriverSelectionUI
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphicsDriverSelectionUI));
            this.cBoxSelectionStrategy = new System.Windows.Forms.ComboBox();
            this.labelCBox = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelInfoText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cBoxSelectionStrategy
            // 
            this.cBoxSelectionStrategy.FormattingEnabled = true;
            this.cBoxSelectionStrategy.Location = new System.Drawing.Point(98, 63);
            this.cBoxSelectionStrategy.Name = "cBoxSelectionStrategy";
            this.cBoxSelectionStrategy.Size = new System.Drawing.Size(234, 21);
            this.cBoxSelectionStrategy.TabIndex = 18;
            // 
            // labelCBox
            // 
            this.labelCBox.AutoSize = true;
            this.labelCBox.Location = new System.Drawing.Point(12, 66);
            this.labelCBox.MaximumSize = new System.Drawing.Size(150, 0);
            this.labelCBox.Name = "labelCBox";
            this.labelCBox.Size = new System.Drawing.Size(80, 13);
            this.labelCBox.TabIndex = 19;
            this.labelCBox.Text = "Graphics Driver";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(144, 95);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 20;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // labelInfoText
            // 
            this.labelInfoText.AutoSize = true;
            this.labelInfoText.Location = new System.Drawing.Point(12, 9);
            this.labelInfoText.Name = "labelInfoText";
            this.labelInfoText.Size = new System.Drawing.Size(63, 13);
            this.labelInfoText.TabIndex = 21;
            this.labelInfoText.Text = "Placeholder";
            // 
            // GraphicsDriverSelectionUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 140);
            this.Controls.Add(this.labelInfoText);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.labelCBox);
            this.Controls.Add(this.cBoxSelectionStrategy);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GraphicsDriverSelectionUI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cBoxSelectionStrategy;
        private System.Windows.Forms.Label labelCBox;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label labelInfoText;
    }
}
