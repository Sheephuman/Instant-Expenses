using System;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System.Linq;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using System.Data.OleDb;
using System.Data;
using System.Drawing;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
     
        public Form1()
        {
            InitializeComponent();
            this.dateTimePicker1.MouseWheel
              += new MouseEventHandler(this.dateTimePicker1_MouseWheel);

            this.dataGridView1.MouseWheel += new MouseEventHandler(this.dataGridView1_MouseWheel);

            this.listBox1.MouseWheel += new MouseEventHandler(this.listBox1_MouseWheel);

        }




        string current = Directory.GetCurrentDirectory();

        void ファイルリスト取得()
        {
            comboBox1.Items.Clear();

            string[] files = Directory.GetFiles(
    current + "\\kakeibo\\", "*", System.IO.SearchOption.AllDirectories);


            foreach (string file in files)
            {

                comboBox1.Items.Add(Path.GetFileName(file));

                //  MessageBox.Show(Path.GetFileName(file));
            }


        }


        string LastLoad;
        private void Form1_Load(object sender, EventArgs e)
        {
            //toolTip1.SetToolTip(chart1 , "ツールチップです。");

            dataGridView1.Columns[0].HeaderText = "支出";
            dataGridView1.Columns[1].HeaderText = "内訳";
            dataGridView1.Columns[2].HeaderText = "日時";
            dataGridView1.Columns[3].HeaderText = "備考";


            ReWriteStripBtn.Enabled = false;

            ファイルリスト取得();


            dataGridView1.NotifyCurrentCellDirty(true);
            //  STF.Topradio.Checked = true;
            StreamReader sr = new StreamReader(
        current + "\\sort.txt", Encoding.GetEncoding("Shift_JIS"));
            while (sr.Peek() >= 0)
            {
                // ファイルを 1 行ずつ読み込む
                string stBuffer = sr.ReadLine();
                // 読み込んだものを追加で格納する
                listBox1.Items.Add(stBuffer);

                

            }
            sr.Close();



            listBox1.SelectedIndex = 0;

            LastLoad  =  Properties.Settings.Default.LastLoadFile;
          
            if (LastLoad != "")
            {
                

                CSV_to_DataGridView(LastLoad);


                comboBox1.Text = LastLoad.Substring(LastLoad.LastIndexOf("\\")+1);
                Text = "Instant Expenses -" + comboBox1.Text;
                // 自動でサイズを設定するのは、行や列を追加したり、セルに値を設定した後にする。

                 

                TotalCostCalc();
            }

           CommmonColor = chart1.Series[0].Color = Properties.Settings.Default.CommonColor;
        
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy年MM月dd日";
        }




        private void graph_chart_display()
        {



            chart1.Series.Clear();
            chart1.Series.Add("合計");



          



            chart1.Series[0].Color = CommmonColor;



            try
            {
                string current = Directory.GetCurrentDirectory();

                // Full path to the data source file

                string file = comboBox1.Text;
                string path = current + "\\kakeibo\\";

                // Create a connection string.
                string ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                path + ";Extended Properties=\"Text;HDR=No;FMT=Delimited\"";
                OleDbConnection myConnection = new OleDbConnection(ConStr);





                #region 試行錯誤
                // Create a database command on the connection using query
                //   string mySelectQuery = "Select F2, SUM(F1) AS F1S from" +"\\" + file + "GROUP BY F2 ORDER BY F2";

                //string mySelectQuery = "SELECT F2 FROM \\sample.csv GROUP BY F2 ORDER BY F2";


                //"D:\code\SheepMan Account Book\SheepMan Account Book\bin\Debug\kakeibo\2017年11月17日.csv"
                //string mySelectQuery = "SELECT F2,SUM(F1) AS F1S FROM"+ file+ "GROUP BY F2 ORDER BY F2";
                //  string mySelectQuery = "SELECT MIN(F3),SUM(F1) FROM  " +"\\" +file+ " GROUP BY F3";

                //  string mySelectQuery = "SELECT F3,Sum(F1) FROM " + "\\" + file +" Where F3 IN(Select F3 From " +"\\"+file+ " Group by F3 Order by f3)";
                // 集計関数の一部として指定された式 'F3' を含んでいないクエリを実行しようとしました。

                //string mySelectQuery = "SELECT F2, SUM(F1) AS F1S" + "WHERE F2 LIKE '%水道代%' GROUP BY F2 ORDER BY F2 FROM" +"\\" + file;

                #endregion


                //系列ごとに集計する場合
                if (checkBox2.Checked == true)
                {



                }


                else
                {
                    string mySelectQuery = "SELECT F3,Sum(F1) FROM " + "\\" + file + " Group by f3 Order by Format(F3,'MMDD')";
                    //Group by とOrder by を併用しないと通らなかった　集計関数の一部として指定された式 'F3' を含んでいないクエリを実行しようとしました。


                    string mySeriesData = "SELECT F4 FROM " + "\\" + file + " Group by f4 Order by F4";



                    chart1.Series[0].Label = "#VAL{C}";

                   
                    //chart1.Series[0].ToolTip = "#SERIESNAME";


                    OleDbCommand myCommand = new OleDbCommand(mySelectQuery, myConnection);
                    // Open the connection and create the reader
                    myCommand.Connection.Open();
                    OleDbDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);


                    chart1.ChartAreas[0].InnerPlotPosition.Width = 90;
                    chart1.ChartAreas[0].InnerPlotPosition.Height = 90;
                    chart1.ChartAreas[0].InnerPlotPosition.X = 8;
                    chart1.ChartAreas[0].InnerPlotPosition.Y = 0;

                    //chart1.ChartAreas[0].AxisX.TitleAlignment = ;
                    chart1.ChartAreas[0].IsSameFontSizeForAllAxes = true;


                    chart1.Series[0].Points.DataBindXY(myReader, "0", myReader, "1");

                    myReader.Close();
                    myConnection.Close();


                }




            }
            catch (OleDbException OleEx)
            { //Console.WriteLine(OleEx.ToString());

                MessageBox.Show(OleEx.Message+" \r +,-等の記号がファイル名に含まれています");
            }
        }


  
    


        private void listBox1_MouseWheel(object sender, MouseEventArgs e)
        {

            try
            {
               

                int wheel = e.Delta / 120;
                int Current = listBox1.SelectedIndex;
                listBox1.SetSelected(Current - wheel, true);
            }


            catch (ArgumentOutOfRangeException)
            {// MessageBox.Show(OutEx.Message); }

            }
        }
      

        private void dataGridView1_MouseWheel(object sender, MouseEventArgs e)
        {
            dataGridView1.MultiSelect = false;

            try
            {


                int wheel = e.Delta / 120;

                //if (CursorTrace.Checked == true)
                //{
                //    if (wheel == +1)
                //    {
                //        SendKeys.Send("{UP}");
                //        SendKeys.Send("{UP}");
                //    }

                //    else if (wheel == -1)
                //    {
                //        SendKeys.Send("{Down}");
                //        SendKeys.Send("{Down}");

                //    }
                //}

                int CurrentRow = dataGridView1.CurrentCell.RowIndex - wheel;


                int CurrentCell = dataGridView1.CurrentCell.ColumnIndex;
               
              dataGridView1.Rows[CurrentRow].Cells[CurrentCell].Selected = true;

                    //dataGridView1.Rows[CurrentRow].Frozen = false;
                
 
                    dataGridView1.Rows[CurrentRow].Cells[CurrentCell].Selected = true;

                    //dataGridView1.Rows[CurrentRow].Frozen = true;
                



            }

            catch (Exception exception)
            {
                if (exception is ArgumentOutOfRangeException || exception is ArgumentNullException)
                { }
            }
        }





        DateTime DT = new DateTime();


       
        private void dateTimePicker1_MouseWheel(object sender, MouseEventArgs e)
        {
            

            int DTHeight = dateTimePicker1.Bounds.Height;
            Point PickerRectLocation = dateTimePicker1.PointToClient(dateTimePicker1.Bounds.Location);



            Point WheelPoint = new Point(e.X, e.Y);

            // マウス座標をクライアント座標系へ変換
            Point mouseClientPos = dateTimePicker1.PointToClient(WheelPoint);

            //Rectangle YearRect;
            Rectangle YearRect = new Rectangle(PickerRectLocation.X,PickerRectLocation.Y + 3 ,23, DTHeight);

            Rectangle MonthRect = new Rectangle(PickerRectLocation.X+ 33,PickerRectLocation.Y + 3,23,DTHeight);

            Rectangle DayRect = new Rectangle(PickerRectLocation.X+ 57, PickerRectLocation.Y + 3, 23, DTHeight);


          

            if (YearRect.Contains(mouseClientPos) == true)
            {

                // スクロール量（方向）の表示
                DT = dateTimePicker1.Value.AddYears(e.Delta / 120);
                dateTimePicker1.Text = DT.ToShortDateString();

            }
            else if(MonthRect.Contains(mouseClientPos) == true)
            {

                DT = dateTimePicker1.Value.AddMonths(e.Delta / 120);
                dateTimePicker1.Text = DT.ToShortDateString();


            }
            else if (DayRect.Contains(mouseClientPos) == true)
            {

                DT = dateTimePicker1.Value.AddDays(e.Delta / 120);
                dateTimePicker1.Text = DT.ToShortDateString();


            }
        }








        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //private void SetTimeButton_Click(object sender, EventArgs e)
        //{
        //    DateTime DT = DateTime.Now;
        //    dateTimePicker1.Text = DT.ToString();
        //}


        SaveFileDialog sfd = new SaveFileDialog();
        public void WritingFileMethod(string FileName)
        {

            String strCsvData = string.Empty;
            //SaveFileDialogクラスのインスタンスを作成




            try
            {

                if (sfd.FileName != "" || Properties.Settings.Default.LastLoadFile != "")
                {
                    using (StreamWriter writer = new StreamWriter(FileName, false, Encoding.GetEncoding("shift_jis")))
                    {

                        int rowCount = dataGridView1.Rows.Count;



                        // ユーザによる行追加が許可されている場合は、最後に新規入力用の
                        // 1行分を差し引く
                        if (dataGridView1.AllowUserToAddRows == true)
                        {
                            rowCount = rowCount - 1;
                        }




                        // 行
                        for (int i = 0; i < rowCount; i++)
                        {
                            // リストの初期化
                            List<String> strList = new List<String>();

                            // 列
                            for (int j = 0; j < dataGridView1.Columns.Count; j++)
                            {
                                if (dataGridView1[j, i].Value != null)
                                {
                                    strList.Add(dataGridView1[j, i].Value.ToString());
                                }
                                else if (dataGridView1[j, i].Value == null)
                                {
                                    dataGridView1[j, i].Value = "";
                                    strList.Add(dataGridView1[j, i].Value.ToString());
                                }
                            }
                            // String[] strArray = strList.ToArray();  // 配列へ変換

                            // CSV 形式に変換
                            strCsvData = String.Join(",", strList);


                            writer.WriteLine(strCsvData);



                        }
                        writer.Close();
                    }

                }

             
                
            }
            //catch (ArgumentException AE)
            //{ MessageBox.Show(AE.Message); }

            catch (System.FormatException FE)
            { MessageBox.Show(FE.Message); }

        }


        private void SaveStripButton_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();

            //ダイアログを表示する
            sfd.DefaultExt = "*.csv";


            sfd.FileName = "";
            sfd.InitialDirectory = Directory.GetCurrentDirectory() + "\\kakeibo\\";

            sfd.Filter = "CSVファイル|*.csv|全てのファイル|*.*";
            if (checkBox1.Checked == true)
            {
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "yyyy年M月";
                sfd.FileName = dateTimePicker1.Text;  //.Text + ".csv";


                dateTimePicker1.Format = DateTimePickerFormat.Long;
                //カスタムフォーマットのリセット



            }

            if (sfd.ShowDialog() == DialogResult.OK)
            {

                WritingFileMethod(sfd.FileName);
            }



            ファイルリスト取得();

            string FileNameSt = sfd.FileName.Substring(sfd.FileName.LastIndexOf("\\")+1);

            Properties.Settings.Default.LastLoadFile = sfd.FileName;
            Text = "Instant Expenses -" + FileNameSt;

            if (dataGridView1.Rows[0].Cells[0].Value != null)
            {
                ReWriteStripBtn.Enabled = true;
            }
            else
            { ReWriteStripBtn.Enabled = false; }
        }











        public void TotalCostCalc()
        {
            try
            {
                List<int> TotalCalc = new List<int>();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    //if (i == 0) { label3.Text = (Convert.ToDecimal(dataGridView1.Rows[i].Cells[0].Value)).ToString(); }

                    if (i >= 0)
                    {
                        //if (i + 1 == dataGridView1.Rows.Count) break;
                        if (dataGridView1.Rows[i].Cells[0].Value != (object)"")
                        {
                            TotalCalc.Add(Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value));
                        }
                    }

                }
                label3.Text = TotalCalc.Sum().ToString("C");
                //"C"は通貨で使用される書式



            }

            catch (FormatException)
            { }



        }
        bool NewBool;
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

            textBox1.Text = re.Replace(textBox1.Text, myReplacer);
            dataGridView1.MultiSelect = false;


            // メインﾌｫｰﾑ情報をSTFに設定
            // Form1.STF.F1 = this;


            if (e.KeyCode == Keys.Enter && textBox1.Text != "")
            {
                if (NewBool == false)
                {
                    ReWriteStripBtn.Enabled = true;
                }


                #region 末尾から追加する処理
                if (BottomAdd.Checked == true)
                {

                    //int DataAbouveRow = dataGridView1.Rows.Count;
                    int EndData = dataGridView1.Rows.Count - 1;


                    if (textBox1.Text != "" && dataGridView1.Rows[EndData].Cells[0].Value == null)
                    {



                        dataGridView1.Rows.Add(1);
                        dataGridView1.Rows[EndData].Cells[0].Value = textBox1.Text;

                        dataGridView1.Rows[EndData].Cells[1].Value = listBox1.SelectedItem;

                        dataGridView1.Rows[EndData].Cells[2].Value = dateTimePicker1.Text.Substring(5, dateTimePicker1.Text.Length - 5);






                    }



                    dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;


                    dataGridView1.Rows[dataGridView1.Rows.Count - 2].Selected = true;


                    //  dataGridView1.UpdateCellValue(2,EndData);
                }
                //常に直近の入力データをフォーカス

                #endregion

                #region 先頭から追加する処理
                else if (BottomAdd.Checked == false)
                {
                    dataGridView1.Rows.Add(1);

                    

                    for (int i = dataGridView1.RowCount - 1; i >= 1; i--)
                    {
                        for (int j = 0; j <= dataGridView1.ColumnCount - 1; j++)
                        {
                            if (i == 1)
                            {
                                dataGridView1.Rows[i].Cells[j].Value = dataGridView1.Rows[i - 1].Cells[j].Value; }
                            //dataGridView1.Rows[i-2].Cells[j].Value;

                            else if (i > 1) {

                                dataGridView1.Rows[i - 1].Cells[j].Value = dataGridView1.Rows[i - 2].Cells[j].Value; }
                        }

                    }

                 

                    dataGridView1.Rows[0].Cells[0].Value = textBox1.Text;

                    dataGridView1.Rows[0].Cells[1].Value = listBox1.Text;

                    dataGridView1.Rows[0].Cells[2].Value = dateTimePicker1.Text.Substring(5, dateTimePicker1.Text.Length - 5);
                    dataGridView1.Rows[0].Cells[3].Value = "";

                    dataGridView1.FirstDisplayedScrollingRowIndex = 0;



                    dataGridView1.Rows[0].Selected = true;


                    //   dataGridView1.;
                }
                #endregion


                textBox1.Text = "";
            }



            TotalCostCalc();

            // dataGridView1.AllowUserToAddRows = false;

        }






        private SettingForm STF = new SettingForm();

        private void toolStripButton3_Click(object sender, EventArgs e)
        {




            if (STF.IsDisposed == false)
            {

                STF.Show();
                STF.Activate();
                STF.WindowState = FormWindowState.Normal;
            }
            else if (STF.IsDisposed == true)
            {

                STF.Activate();
            }


        }






        static string myReplacer(Match m)
        {
            return Strings.StrConv(m.Value, VbStrConv.Narrow, 0);


        }


        Regex re = new Regex("[０-９]");

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {


            // 数字(0-9)は入力可

            //   String str = textBox1.Text;

            if (char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
            {
                //textBox1.ImeMode != ImeMode.Hiragana ||

                //  textBox1.ImeMode = ImeMode.Off;


                //  SendKeys.Send(Keys.Back);
                e.Handled = false;
                return;


                //  MessageBox.Show(isZenkaku(str).ToString());







            }
            // 上記条件は入力不可
            e.Handled = true;






            // System.Threading.Thread.Sleep(1000);

        }

        public class NumericTextBox : TextBox
        {

            const int ES_NUMBER = 0x2000;
            protected override CreateParams CreateParams
            {
                [SecurityPermission(SecurityAction.Demand,
                    Flags = SecurityPermissionFlag.UnmanagedCode)]


                get
                {
                    CreateParams parms = base.CreateParams;
                    parms.Style |= ES_NUMBER;
                    return parms;
                }

            }
        }
private void CSV_to_DataGridView(string FileName)
        {
            try
            {
                dataGridView1.Rows.Clear();


                TextFieldParser parser = new TextFieldParser(FileName, Encoding.GetEncoding("Shift_JIS"));
                parser.TextFieldType = FieldType.Delimited;

                parser.SetDelimiters(","); // 区切り文字はコンマ


                while (!parser.EndOfData)
                {

                    string[] row = parser.ReadFields(); // 1行読み込み
                                                        // 読み込んだデータ(1行をDataGridViewに表示する)

                    dataGridView1.Rows.Add(row);
                }


                parser.Close();
            }

            catch (Exception)
            { }

        }

        String CommonPathString;
        OpenFileDialog ofd = new OpenFileDialog();

        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            NewBool = false;
             

            // ofd.FileName = dateTimePicker1.Text;
            CommonPathString = ofd.FileName;
            ofd.InitialDirectory = Directory.GetCurrentDirectory() + "\\kakeibo\\";


            ofd.Filter =
    "CSVファイル(*.csv)|*.csv|すべてのファイル(*.*)|*.*";
            ofd.FilterIndex = 1;

            ofd.Title = "開くファイルを選択してください";



            ofd.RestoreDirectory = true;



            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //OKボタンがクリックされたとき
                //選択されたファイル名を表示する



                CSV_to_DataGridView(ofd.FileName);

                TotalCostCalc();


                //  graph_chart_display();

                try
                {
                    dateTimePicker1.Text = ofd.SafeFileName.Replace(".csv", "");
                }

                catch (FormatException)
                {
                    // MessageBox.Show(FE.Message);
                }



            }

            if (ofd.SafeFileName != "")
            {
                comboBox1.Text = ofd.SafeFileName;
                Text = "Instant Expenses -" + ofd.SafeFileName;
            }
        }



        private void ReWriteFileButton_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();
            try
            {
                if (ofd.FileName != "")
                {
                    sfd.FileName = ofd.FileName;
                    WritingFileMethod(sfd.FileName);

                    MessageBox.Show("保存しました");
                }
                else if (Properties.Settings.Default.LastLoadFile != "")
                {
                    WritingFileMethod(Properties.Settings.Default.LastLoadFile);

                    MessageBox.Show("保存しました");
                }
              

                ReWriteStripBtn.Enabled = false;
            }


            catch (System.NullReferenceException)
            { MessageBox.Show("ファイルは開かれていません。既に開かれたファイルのみ上書き処理されます"); }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }






        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

            graph_chart_display();
        }


        Color CommmonColor;
        private void Chart_ColorChangeBtn_Click(object sender, EventArgs e)
        {
            //グラフ色変更
            try
            {
                Graphics g = this.CreateGraphics();
                ColorDialog cd = new ColorDialog();
                cd.Color = Color.FromKnownColor(KnownColor.DarkTurquoise);
                cd.FullOpen = true;
                cd.CustomColors = new int[] { ColorTranslator.ToWin32(Color.OrangeRed),
                ColorTranslator.ToWin32(Color.LawnGreen),
                ColorTranslator.ToWin32(Color.LightYellow),
                ColorTranslator.ToWin32(Color.AliceBlue),
                0x00880000,0x00008800,0x00000088,0x00ff8800 };
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    //  SolidBrush brush = new SolidBrush(cd.Color);

                    CommmonColor = cd.Color;
                    chart1.Series[0].Color = cd.Color;
                }

            }

            catch (ArgumentOutOfRangeException)
            { }
        }

        private void BottomAdd_CheckedChanged(object sender, EventArgs e)
        {
            if (BottomAdd.Checked == true)
            {
                BottomAdd.Text = "末尾に追加";

            }

            else if (BottomAdd.Checked == false)
            {
                BottomAdd.Text = "先頭に追加";
            }

        }

        private void 新規データ_Click(object sender, EventArgs e)
        {

            NewBool = true;
            ReWriteStripBtn.Enabled = false;

            dataGridView1.Rows.Clear();
          
            

           


            Text = "Instant Expencs";


            TotalCostCalc();



        }



        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            TotalCostCalc();
            //graph_chart_display();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Delete == e.KeyCode)
            {
                if (hti.RowIndex != -1 && hti.ColumnIndex > -1)
                {
                    dataGridView1.Rows[hti.RowIndex].Cells[hti.ColumnIndex].Value = "";
                }
            }
            TotalCostCalc();
        }
     
        DataGridView.HitTestInfo hti;
        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            hti = ((DataGridView)sender).HitTest(e.X, e.Y);

        }

        private void 選択行の削除ToolStripMenuItem_Click(object sender, EventArgs e)
        {         

            try
           {

            if (hti != null)
            {
                dataGridView1.Rows.RemoveAt(hti.RowIndex);
            }

            }
            catch(InvalidOperationException)
            {
                MessageBox.Show("新規行は削除出来ません");


            }
            ReWriteStripBtn.Enabled = true;

            TotalCostCalc();
        }

        private void dataGridView1_Validated(object sender, EventArgs e)
        {
            TotalCostCalc();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            TotalCostCalc();
           
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        { Properties.Settings.Default.CommonColor = CommmonColor = chart1.Series[0].Color;
            Properties.Settings.Default.BottomAdd_CheckState = BottomAdd.Checked;
          
            if (ofd != null && ofd.SafeFileName!= "")
            {
                LastLoad = ofd.FileName;
             
                Properties.Settings.Default.LastLoadFile = LastLoad;

            }

            Properties.Settings.Default.Save();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ReWriteStripBtn.Enabled = true;


            textBox1.Focus();        }
    }
}

