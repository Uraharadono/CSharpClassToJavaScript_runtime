namespace CSharpToJavascriptRuntimeConverter
{
    partial class Main
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SelectFileButton = new System.Windows.Forms.Button();
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.codeTextEditor = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.camelCaseCheckBox = new System.Windows.Forms.CheckBox();
            this.includeMergeFunctionCheckBox = new System.Windows.Forms.CheckBox();
            this.respectDataMemberAttributeCheckBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.asdasd = new System.Windows.Forms.Label();
            this.respectDefaultValueAttributeCheckBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.generateTypesDropdown = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.treatEnumsAsStringsCheckBox = new System.Windows.Forms.CheckBox();
            this.namespaceInput = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.isLoadingCheckBox = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.unmapFunctionCheckBox = new System.Windows.Forms.CheckBox();
            this.includeHeadersCheckBox = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.includeEqualsFunctionCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // SelectFileButton
            // 
            this.SelectFileButton.FlatAppearance.BorderSize = 2;
            this.SelectFileButton.Location = new System.Drawing.Point(203, 24);
            this.SelectFileButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SelectFileButton.Name = "SelectFileButton";
            this.SelectFileButton.Size = new System.Drawing.Size(115, 29);
            this.SelectFileButton.TabIndex = 0;
            this.SelectFileButton.Text = "Select File";
            this.SelectFileButton.UseVisualStyleBackColor = true;
            this.SelectFileButton.Click += new System.EventHandler(this.SelectFileButton_Click);
            // 
            // fileNameLabel
            // 
            this.fileNameLabel.AutoSize = true;
            this.fileNameLabel.Location = new System.Drawing.Point(97, 56);
            this.fileNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(0, 13);
            this.fileNameLabel.TabIndex = 1;
            // 
            // btnGenerate
            // 
            this.btnGenerate.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnGenerate.Location = new System.Drawing.Point(183, 426);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(136, 32);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // codeTextEditor
            // 
            this.codeTextEditor.Dock = System.Windows.Forms.DockStyle.Right;
            this.codeTextEditor.Location = new System.Drawing.Point(344, 0);
            this.codeTextEditor.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.codeTextEditor.Name = "codeTextEditor";
            this.codeTextEditor.Size = new System.Drawing.Size(618, 509);
            this.codeTextEditor.TabIndex = 3;
            this.codeTextEditor.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "File:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 146);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Camel case:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 175);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Include merge function:";
            // 
            // camelCaseCheckBox
            // 
            this.camelCaseCheckBox.AutoSize = true;
            this.camelCaseCheckBox.Location = new System.Drawing.Point(203, 146);
            this.camelCaseCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.camelCaseCheckBox.Name = "camelCaseCheckBox";
            this.camelCaseCheckBox.Size = new System.Drawing.Size(45, 17);
            this.camelCaseCheckBox.TabIndex = 7;
            this.camelCaseCheckBox.Text = "Yea";
            this.camelCaseCheckBox.UseVisualStyleBackColor = true;
            // 
            // includeMergeFunctionCheckBox
            // 
            this.includeMergeFunctionCheckBox.AutoSize = true;
            this.includeMergeFunctionCheckBox.Location = new System.Drawing.Point(203, 175);
            this.includeMergeFunctionCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.includeMergeFunctionCheckBox.Name = "includeMergeFunctionCheckBox";
            this.includeMergeFunctionCheckBox.Size = new System.Drawing.Size(45, 17);
            this.includeMergeFunctionCheckBox.TabIndex = 8;
            this.includeMergeFunctionCheckBox.Text = "Yea";
            this.includeMergeFunctionCheckBox.UseVisualStyleBackColor = true;
            // 
            // respectDataMemberAttributeCheckBox
            // 
            this.respectDataMemberAttributeCheckBox.AutoSize = true;
            this.respectDataMemberAttributeCheckBox.Location = new System.Drawing.Point(203, 232);
            this.respectDataMemberAttributeCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.respectDataMemberAttributeCheckBox.Name = "respectDataMemberAttributeCheckBox";
            this.respectDataMemberAttributeCheckBox.Size = new System.Drawing.Size(45, 17);
            this.respectDataMemberAttributeCheckBox.TabIndex = 9;
            this.respectDataMemberAttributeCheckBox.Text = "Yea";
            this.respectDataMemberAttributeCheckBox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 232);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Respect data member attribute";
            // 
            // asdasd
            // 
            this.asdasd.AutoSize = true;
            this.asdasd.Location = new System.Drawing.Point(11, 258);
            this.asdasd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.asdasd.Name = "asdasd";
            this.asdasd.Size = new System.Drawing.Size(155, 13);
            this.asdasd.TabIndex = 11;
            this.asdasd.Text = "Respect default value attribute:";
            // 
            // respectDefaultValueAttributeCheckBox
            // 
            this.respectDefaultValueAttributeCheckBox.AutoSize = true;
            this.respectDefaultValueAttributeCheckBox.Location = new System.Drawing.Point(203, 258);
            this.respectDefaultValueAttributeCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.respectDefaultValueAttributeCheckBox.Name = "respectDefaultValueAttributeCheckBox";
            this.respectDefaultValueAttributeCheckBox.Size = new System.Drawing.Size(45, 17);
            this.respectDefaultValueAttributeCheckBox.TabIndex = 12;
            this.respectDefaultValueAttributeCheckBox.Text = "Yea";
            this.respectDefaultValueAttributeCheckBox.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 84);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Type:";
            // 
            // generateTypesDropdown
            // 
            this.generateTypesDropdown.FormattingEnabled = true;
            this.generateTypesDropdown.Location = new System.Drawing.Point(203, 77);
            this.generateTypesDropdown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.generateTypesDropdown.Name = "generateTypesDropdown";
            this.generateTypesDropdown.Size = new System.Drawing.Size(116, 21);
            this.generateTypesDropdown.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 289);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Treat enums as strings:";
            // 
            // treatEnumsAsStringsCheckBox
            // 
            this.treatEnumsAsStringsCheckBox.AutoSize = true;
            this.treatEnumsAsStringsCheckBox.Location = new System.Drawing.Point(203, 288);
            this.treatEnumsAsStringsCheckBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.treatEnumsAsStringsCheckBox.Name = "treatEnumsAsStringsCheckBox";
            this.treatEnumsAsStringsCheckBox.Size = new System.Drawing.Size(45, 17);
            this.treatEnumsAsStringsCheckBox.TabIndex = 17;
            this.treatEnumsAsStringsCheckBox.Text = "Yea";
            this.treatEnumsAsStringsCheckBox.UseVisualStyleBackColor = true;
            // 
            // namespaceInput
            // 
            this.namespaceInput.Location = new System.Drawing.Point(203, 115);
            this.namespaceInput.Name = "namespaceInput";
            this.namespaceInput.Size = new System.Drawing.Size(115, 20);
            this.namespaceInput.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 118);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Namespace";
            // 
            // isLoadingCheckBox
            // 
            this.isLoadingCheckBox.AutoSize = true;
            this.isLoadingCheckBox.Location = new System.Drawing.Point(203, 364);
            this.isLoadingCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.isLoadingCheckBox.Name = "isLoadingCheckBox";
            this.isLoadingCheckBox.Size = new System.Drawing.Size(45, 17);
            this.isLoadingCheckBox.TabIndex = 27;
            this.isLoadingCheckBox.Text = "Yea";
            this.isLoadingCheckBox.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 368);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Add IsLoading:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 343);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "Unmap funct:";
            // 
            // unmapFunctionCheckBox
            // 
            this.unmapFunctionCheckBox.AutoSize = true;
            this.unmapFunctionCheckBox.Location = new System.Drawing.Point(203, 339);
            this.unmapFunctionCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.unmapFunctionCheckBox.Name = "unmapFunctionCheckBox";
            this.unmapFunctionCheckBox.Size = new System.Drawing.Size(45, 17);
            this.unmapFunctionCheckBox.TabIndex = 24;
            this.unmapFunctionCheckBox.Text = "Yea";
            this.unmapFunctionCheckBox.UseVisualStyleBackColor = true;
            // 
            // includeHeadersCheckBox
            // 
            this.includeHeadersCheckBox.AutoSize = true;
            this.includeHeadersCheckBox.Location = new System.Drawing.Point(203, 313);
            this.includeHeadersCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.includeHeadersCheckBox.Name = "includeHeadersCheckBox";
            this.includeHeadersCheckBox.Size = new System.Drawing.Size(45, 17);
            this.includeHeadersCheckBox.TabIndex = 22;
            this.includeHeadersCheckBox.Text = "Yea";
            this.includeHeadersCheckBox.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(11, 317);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(86, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Include headers:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 202);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(120, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Include equals function:";
            // 
            // includeEqualsFunctionCheckBox
            // 
            this.includeEqualsFunctionCheckBox.AutoSize = true;
            this.includeEqualsFunctionCheckBox.Location = new System.Drawing.Point(203, 202);
            this.includeEqualsFunctionCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.includeEqualsFunctionCheckBox.Name = "includeEqualsFunctionCheckBox";
            this.includeEqualsFunctionCheckBox.Size = new System.Drawing.Size(45, 17);
            this.includeEqualsFunctionCheckBox.TabIndex = 8;
            this.includeEqualsFunctionCheckBox.Text = "Yea";
            this.includeEqualsFunctionCheckBox.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(962, 509);
            this.Controls.Add(this.isLoadingCheckBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.unmapFunctionCheckBox);
            this.Controls.Add(this.includeHeadersCheckBox);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.namespaceInput);
            this.Controls.Add(this.treatEnumsAsStringsCheckBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.generateTypesDropdown);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.respectDefaultValueAttributeCheckBox);
            this.Controls.Add(this.asdasd);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.respectDataMemberAttributeCheckBox);
            this.Controls.Add(this.includeEqualsFunctionCheckBox);
            this.Controls.Add(this.includeMergeFunctionCheckBox);
            this.Controls.Add(this.camelCaseCheckBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.codeTextEditor);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.fileNameLabel);
            this.Controls.Add(this.SelectFileButton);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Main";
            this.Text = "C# class runtime generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button SelectFileButton;
        private System.Windows.Forms.Label fileNameLabel;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.RichTextBox codeTextEditor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox camelCaseCheckBox;
        private System.Windows.Forms.CheckBox includeMergeFunctionCheckBox;
        private System.Windows.Forms.CheckBox respectDataMemberAttributeCheckBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label asdasd;
        private System.Windows.Forms.CheckBox respectDefaultValueAttributeCheckBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox generateTypesDropdown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox treatEnumsAsStringsCheckBox;
        private System.Windows.Forms.TextBox namespaceInput;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox isLoadingCheckBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox unmapFunctionCheckBox;
        private System.Windows.Forms.CheckBox includeHeadersCheckBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox includeEqualsFunctionCheckBox;
    }
}

