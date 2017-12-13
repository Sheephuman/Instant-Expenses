namespace WindowsFormsApplication1
{
    partial class SettingForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.Topradio = new System.Windows.Forms.RadioButton();
            this.ButtomRadio = new System.Windows.Forms.RadioButton();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // Topradio
            // 
            this.Topradio.AutoSize = true;
            this.Topradio.Checked = true;
            this.Topradio.Location = new System.Drawing.Point(22, 27);
            this.Topradio.Name = "Topradio";
            this.Topradio.Size = new System.Drawing.Size(99, 16);
            this.Topradio.TabIndex = 0;
            this.Topradio.TabStop = true;
            this.Topradio.Text = "先頭に書き込む";
            this.Topradio.UseVisualStyleBackColor = true;
              // 
            // ButtomRadio
            // 
            this.ButtomRadio.AutoSize = true;
            this.ButtomRadio.Location = new System.Drawing.Point(22, 49);
            this.ButtomRadio.Name = "ButtomRadio";
            this.ButtomRadio.Size = new System.Drawing.Size(99, 16);
            this.ButtomRadio.TabIndex = 1;
            this.ButtomRadio.Text = "末尾に書き込む";
            this.ButtomRadio.UseVisualStyleBackColor = true;
              // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(22, 194);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(230, 45);
            this.trackBar1.TabIndex = 2;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(22, 5);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(124, 16);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "タイトルバーを非表示";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(22, 85);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(154, 16);
            this.checkBox2.TabIndex = 4;
            this.checkBox2.Text = "起動時に指定ファイルを開く";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(22, 107);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(143, 19);
            this.textBox1.TabIndex = 5;
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.ButtomRadio);
            this.Controls.Add(this.Topradio);
            this.Name = "SettingForm";
            this.Opacity = 0.1D;
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SettingForm_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RadioButton Topradio;
        public System.Windows.Forms.RadioButton ButtomRadio;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.TextBox textBox1;
    }
}