using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;

namespace dietcal
{
    public partial class Form1 : Form
    {
#region 
        private string queryType = "select 方式 AS 条目 from mode"; //查询方式、食材及区域的SQL语句
        private string queryShicai = "select 地区 AS 条目 from area";
        private string queryQuyu = "select 食材 AS 条目 from material";
        //private DateTime begintime = new DateTime(1995, 1, 2);   //基准时间
        private DateTime begintime = new DateTime(2010,1,2);
        private DateTime today = DateTime.Now;                  //现在时间
        private DateTime timestart,timeend;                     //筛选的开始及结束时间
        private string searchRegion = "";                       //筛选区域
        private string searchCaiming = "";                      //筛选食材
        private string searchFangshi = "";                      //筛选方式
        private SqlConnection sqlconn;                          
        private SqlCommand sqlcmd;
        private DataTable cmb_dt_shicai, cmb_dt_type, cmb_dt_quyu;
        private DataTable dt_searchResult;
        private DataTable tmp_insert;        
        private string LoginUser, UserPasswd;                   //用于记录登录系统的用户名和密码
#endregion
        public Form1(string username,string userpwd,string connString)  //由登录界面传入用户名，密码及数据库连接字符串
        {
            InitializeComponent();
            this.Text = "欢迎" + username + "使用本管理系统";
            //设置时间选择器
            this.dateTimePicker_end.MaxDate = today;
            this.dateTimePicker_start.MaxDate = today;
            this.dateTimePicker_end.MinDate = begintime;
            this.dateTimePicker_start.MinDate = begintime;
            this.dateTimePicker_datainsert.MaxDate = today;
            timestart = this.dateTimePicker_start.Value;
            timeend = this.dateTimePicker_end.Value;
            this.chart_disp.Visible = false;
            //设置输入密码的显示方式
            this.textBox_oldPasswd.PasswordChar = '*';
            this.textBox_newPasswd.PasswordChar = '*';
            this.textBox_newConfirm.PasswordChar = '*';
            dt_searchResult = new DataTable();
            dt_searchResult.Clear();
            cmb_dt_shicai = new DataTable();
            cmb_dt_type = new DataTable();
            cmb_dt_quyu = new DataTable();
            cmb_dt_shicai.Clear();
            //cmb_dt_shicai.Columns.Add("条目",typeof(string));
            cmb_dt_type.Clear();
            //cmb_dt_type.Columns.Add("条目", typeof(string));
            cmb_dt_quyu.Clear();
            //cmb_dt_quyu.Columns.Add("条目", typeof(string));

            //用于缓存需要插入的数据
            tmp_insert = new DataTable();
            tmp_insert.Clear();
            tmp_insert.Columns.Add("日期",typeof(string));
            tmp_insert.Columns.Add("区域", typeof(string));
            tmp_insert.Columns.Add("方式", typeof(string));
            tmp_insert.Columns.Add("食材", typeof(string));
            tmp_insert.Columns.Add("数量", typeof(int));
            tmp_insert.Columns.Add("日", typeof(int));
            tmp_insert.Columns.Add("周", typeof(int));
            tmp_insert.Columns.Add("月", typeof(int));
            tmp_insert.Columns.Add("年", typeof(int));

            LoginUser = username;
            UserPasswd = userpwd;
            sqlconn = new SqlConnection(connString);
            sqlconn.Open();
            sqlcmd = new SqlCommand();
            sqlcmd.CommandType = CommandType.Text;
            sqlcmd.Connection = sqlconn;

            //读取方式、区域、食材数据并装入combobox中
            combdtInit();
            
            //显示表单初始化
            listviewInit(listView_caiming);
            listviewInit(listView_inseType);
            listviewInit(listView_quyu);
            //向显示列表中显示数据
            listviewInseData(listView_caiming, cmb_dt_shicai);
            listviewInseData(listView_inseType, cmb_dt_type);
            listviewInseData(listView_quyu, cmb_dt_quyu);

            //datagridview_tmpinsert初始化
            dataGridView_tmpinsert.ReadOnly = true;
            dataGridView_tmpinsert.AllowUserToAddRows = false;
            dataGridView_dispresult.ReadOnly = true;
            dataGridView_dispresult.AllowUserToAddRows = false;
        }


        //用于从库中载入方式等数据
        private void combdtInit()
        {
            //获取数据信息
            querycmbItems("select 地区 as 条目 from area", cmb_dt_quyu);
            querycmbItems("select 食材 as 条目 from material", cmb_dt_shicai);
            querycmbItems("select 方式 as 条目 from mode", cmb_dt_type);
            //载入数据
            cmbUpdate(comboBox_dataquyu, cmb_dt_quyu);
            cmbUpdate(comboBox_Local, cmb_dt_quyu,true);
            cmbUpdate(comboBox_diet, cmb_dt_shicai,true);
            cmbUpdate(comboBox_datashicai, cmb_dt_shicai);
            cmbUpdate(comboBox_datatype, cmb_dt_type);
            cmbUpdate(comboBox_type, cmb_dt_type,true);
        }
        //用于初始化数据表，从数据库中提取条目
        private void querycmbItems(string cmdtext,DataTable dt)
        {
            SqlDataAdapter sda = new SqlDataAdapter(cmdtext,sqlconn);
            sda.Fill(dt);            
        }

        //用于数据库连接
        private bool sqlOpen()
        {
            try
            {
                sqlconn.Open();
            }
            catch (System.Exception ex)
            {
                return false;
            }
            return true;
        }
        
        
        //确认筛选按钮
        private void button_create_Click(object sender, EventArgs e)
        {
            int chtype;
            chtype = type_chart();
            string cmdString = searchtosql();
            //DataTable dt = new DataTable();
            if (sqlconn.State != ConnectionState.Open)
            {
                if (!sqlOpen())
                {
                    MessageBox.Show("数据库连接错误!");
                    return;
                }               
            }
            SqlDataAdapter sda = new SqlDataAdapter(cmdString, sqlconn);
            dt_searchResult.Clear();
            dt_searchResult.Columns.Clear();
            sda.Fill(dt_searchResult);
            dispInchart(dt_searchResult, chart_disp, "统计结果", chtype);
            dataGridView_dispresult.DataSource = dt_searchResult;          

        }

        //导出数据到excel中
        public static bool SaveDataTableToExcel(System.Data.DataTable dt, string filPath)   //dt中存放数据，filpath为存储路径
        {

            //Microsoft.Office.Interop.Excel.ApplicationClass app = new Microsoft.Office.Interop.Excel.ApplicationClass();
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            //MessageBox.Show(app.Version);
            int FormatNum;
            string fileType = System.IO.Path.GetExtension(filPath);
            //int verNum = Convert.ToInt32(app.Version);
            if (".xlsx" == fileType)     //07格式版本
            {
                FormatNum = 56;
            }
            else    //使用97-03格式版本
            {

                FormatNum = -4143;
            }
            try
            {

                app.Visible = false;
                Microsoft.Office.Interop.Excel.Workbook wBook = app.Workbooks.Add(true);
                Microsoft.Office.Interop.Excel.Worksheet wSheet = wBook.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
                if (dt.Rows.Count > 0)
                {
                    int row = 0;
                    row = dt.Rows.Count;
                    int col = dt.Columns.Count;
                    for (int i = 0; i < row; i++)
                    {
                        for (int j = 0; j < col; j++)
                        {
                            string str = dt.Rows[i][j].ToString();
                            wSheet.Cells[i + 2, j + 1] = str;
                        }
                    }
                }
                int size = dt.Columns.Count;
                for (int i = 0; i < size; i++)
                {
                    wSheet.Cells[1, 1 + i] = dt.Columns[i].ColumnName;
                }
                app.DisplayAlerts = false;
                app.AlertBeforeOverwriting = false;
                wBook.SaveAs(filPath, FormatNum);
                //wBook.Save();
                //app.ActiveWorkbook.SaveCopyAs(filPath);
                wBook.Close(false, System.Reflection.Missing.Value, System.Reflection.Missing.Value);

                app.SaveWorkspace(filPath);

                app.Quit();
                app = null;
                return true;
            }
            catch
            {
                return false;
            }

        }

        //确定所要绘制的图形类型
        private int type_chart()
        {
            //0:线图  1:饼图
            int i = 0;
            if (comboBox_diet.SelectedItem != "全部")
            {
                ++i;
            }
            if (comboBox_Local.SelectedItem != "全部")
            {
                ++i;
            }
            if (comboBox_type.SelectedItem != "全部")
            {
                ++i;
            }
            switch (i)
            {
                case 2:
                    return 1;
                default:
                    return 0;
            }
            
        }
        
        //获取筛选所需要的数据库语言
        private string searchtosql()
        {
            
            int startdiff = timediff(dateTimePicker_start.Value);
            int enddiff = timediff(dateTimePicker_end.Value);
            searchCaiming = this.comboBox_diet.SelectedItem.ToString();
            searchRegion = this.comboBox_Local.SelectedItem.ToString();
            searchFangshi = this.comboBox_type.SelectedItem.ToString();
            
            
            if (searchCaiming == "全部" && searchRegion == "全部" && searchFangshi == "全部")
            {
                
                return "select  日期 as 信息,sum(份数) as 结果 from tableAll where 日 between "+startdiff.ToString()+" and "+enddiff.ToString()+" GROUP BY 日期 ORDER BY 日期 ASC"; //显示所选时间内所有地区总销量的走势图
                //return "select 日期 as 信息,sum(份数) as 结果 from tableAll where 日 between 1 and 100 Group by 日期 order by 日期 asc";
            }
            if (searchRegion == "全部")   //查询各个地区的相关信息
            {
                if (searchCaiming == "全部")
                {
                    return "select 日期 as 信息,sum(份数) as 结果 from tableAll where 日 between "+startdiff.ToString()+" and "+enddiff.ToString()+" and 方式 ='"+searchFangshi+"' GROUP BY 日期 ORDER BY 日期 ASC";
                }
                if (searchFangshi == "全部")
                {
                    return "select 日期 as 信息,sum(份数) as 结果 from tableAll where 日 between " + startdiff.ToString() + " and " + enddiff.ToString() + " and 食材 ='" + searchCaiming + "' GROUP BY 日期 ORDER BY 日期 ASC";
                }
                return "select 地区 as 信息,sum(份数) as 结果 from tableAll where 日 between "+startdiff.ToString()+" and "+ enddiff.ToString()+" and 食材 ='"+searchCaiming+"' and  方式 = '"+searchFangshi+"' GROUP BY 地区 ";
                 
            }
            if (searchCaiming == "全部")  //查询特定区域的相关信息
            {
                if (searchFangshi == "全部")
                {
                    return "select 日期 as 信息,sum(份数) as 结果 from tableAll where 日 between " + startdiff.ToString() + " and " + enddiff.ToString() + " and 地区 ='" + searchRegion + "' GROUP BY 日期 ORDER BY 日期 ASC";
                }
                return "select 食材 as 信息,sum(份数) as 结果 from tableAll where 日 between " + startdiff.ToString() + " and " + enddiff.ToString() + " and 地区 ='" + searchRegion + "' and  方式 = '" + searchFangshi + "' GROUP BY 食材 ";
            }
            if (searchFangshi == "全部") 
            {
                return "select 方式 as 信息,sum(份数) as 结果 from tableAll where 日 between " + startdiff.ToString() + " and " + enddiff.ToString() + " and 食材 ='" + searchCaiming + "' and  地区 = '" + searchRegion + "' GROUP BY 方式 ";
            }

            return "select  日期 as 信息,sum(份数) as 结果 from tableAll where 日 between " + startdiff.ToString() + " and " + enddiff.ToString() + " and 食材 ='" + searchCaiming + "' and  地区 = '" + searchRegion + "' and 方式 ='"+searchFangshi+"' GROUP BY 日期 ORDER BY 日期 ASC";  //特定区域特定菜品特定方式的销售走势
        }

        //计算与基准时间相差的天数
        private int timediff(DateTime dt)   
        {
            TimeSpan ts = dt - begintime;
            return ts.Days;
        }

        //计算与基准时间相差的月数
        private int timediffmonth(DateTime dt)   //返回相差的月份
        {
            return (dt.Month - begintime.Month) + (dt.Year - begintime.Year) * 12;
        }


        //将数据显示于图中
        private void dispInchart(DataTable dt, Chart chartname, string chlabel,int chtype) //图标显示
        {
            chartname.Series.Clear();
            if (dt.Rows.Count <=0 )
            {
                return;
            }
            chartname.Visible = true;
            //0:走势图  1:饼图
            
            chartname.Series.Add(chlabel);
            //chartname.Series[chlabel].ChartType = SeriesChartType.Pie;
            switch (chtype)
            {
            case 1:
                    chartname.Series[chlabel].ChartType = SeriesChartType.Pie;
                    break;
            default:
                    chartname.Series[chlabel].ChartType = SeriesChartType.Line;
                    break;
            }
            chartname.Legends[0].Title = chlabel + "统计:";
            chartname.Legends[0].TitleAlignment = StringAlignment.Near;
           
            if (chtype != 0)
            {
                chartname.ChartAreas[0].Area3DStyle.Enable3D = true;
                chartname.ChartAreas[0].Area3DStyle.Rotation = 10;
                chartname.ChartAreas[0].Area3DStyle.Inclination = 30;
                chartname.ChartAreas[0].Area3DStyle.LightStyle = LightStyle.Realistic;
                chartname.Series[chlabel]["PieLineColor"] = "Red";
                chartname.Series[chlabel]["PieLabelStyle"] = "Outside";
                chartname.Series[chlabel].LegendText = "#VALX";
                chartname.Series[chlabel].IsValueShownAsLabel = false;
                
                chartname.Series[chlabel].Label = "#PERCENT";
            }
            else if (chtype == 0)
            {
                chartname.ChartAreas[0].Area3DStyle.Enable3D = false;
            }
            chartname.Series[chlabel].ToolTip = "#VALX\n#VALY";
            
            chartname.DataSource = dt;
            chartname.Series[chlabel].XValueMember = "信息";
            chartname.Series[chlabel].YValueMembers = "结果";
            chartname.DataBind();

        }

        //修改密码
        private void button_ok_Click(object sender, EventArgs e)
        {
            string passwdOld = this.textBox_oldPasswd.Text.Trim();
            string passwdNew = this.textBox_newPasswd.Text.Trim();
            string passwdNewConf = this.textBox_newConfirm.Text.Trim();
            if (passwdNew == string.Empty)
            {
                this.label_correPasswd.Text = "新密码不可为空";
                this.label_correPasswd.ForeColor = System.Drawing.Color.Red;
                this.textBox_newPasswd.Focus();
                this.label_correPasswd.Visible = true;
                return;
            }
            if (passwdNew != passwdNewConf)
            {
                this.label_correPasswd.Text = "新密码不一致";
                this.label_correPasswd.ForeColor = System.Drawing.Color.Red;
                this.label_correPasswd.Visible = true;
                this.textBox_newPasswd.Text = "";
                this.textBox_newConfirm.Text = "";
                this.textBox_newPasswd.Focus();
                return ;
            }
            if (UserPasswd == passwdOld)
            {
                if (ConnectionState.Open != sqlconn.State)
                {
                    if (!sqlOpen())
                    {
                        MessageBox.Show("数据库打开失败!");
                        return;
                    }
                }
                sqlcmd.Connection = sqlconn;
                sqlcmd.CommandText = "update user set userpasswd ='"+passwdNew+"' where username = '"+LoginUser+"'";
                sqlcmd.ExecuteNonQuery();
                UserPasswd = passwdNew;
            }
        }

        //修改密码的取消按钮操作
        private void button_cancle_Click(object sender, EventArgs e)
        {
            this.textBox_oldPasswd.Text = "";
            this.textBox_newPasswd.Text = "";
            this.textBox_newConfirm.Text = "";
            this.textBox_oldPasswd.Focus();
            this.label_correPasswd.Visible = false;
        }
        
        //listview组件初始化
        private void listviewInit(ListView lv)
        {
            lv.View = View.Details;
            lv.Items.Clear();
            lv.Columns.Add("条目", lv.Width );
            lv.FullRowSelect = true;            
            lv.GridLines = true;
            lv.Scrollable = true;            
        }

        //向listview组件中写入条目
        private void listviewInseData(ListView lv,DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++ )
            {
                lv.Items.Add(dt.Rows[i]["条目"].ToString());
            }
        }

        //更新combobox的条目
        private void cmbUpdate(ComboBox cmb,DataTable dt,bool dispall=false)
        {
            cmb.Items.Clear();
            if (dispall)
            {
                cmb.Items.Add("全部");
            }            
            string tmp;
            for (int i = 0; i < dt.Rows.Count;i++ )
            {
                tmp = dt.Rows[i]["条目"].ToString();
                cmb.Items.Add(dt.Rows[i]["条目"]);
            }
            if (cmb.Items.Count > 0)   //默认选择第一项
            {
                cmb.SelectedIndex = 0;//下拉框控件默认选择全部
            }
           
        }

        //插入菜名操作
        private void button_insertcaiming_Click(object sender, EventArgs e)
        {
            string NewShicai = textBox_insercaiming.Text.Trim();
            if (DialogResult.OK != MessageBox.Show("确认添加"+NewShicai+"？","提示",MessageBoxButtons.OKCancel,MessageBoxIcon.Question))
            {
                textBox_insercaiming.Text = "";
                return;
            }
            if (NewShicai == string.Empty)
            {
                return;
            }
            if (null == listView_caiming.FindItemWithText(NewShicai))  //表中无此项则插入
            {
                if (ConnectionState.Open != sqlconn.State)
                {
                    if (!sqlOpen())
                    {
                        MessageBox.Show("数据库打开失败!");
                        return;
                    }
                }
                sqlcmd.Connection = sqlconn;
                sqlcmd.CommandText = "insert into material([食材]) values('"+NewShicai+"')";   //将新添加的数据加入到数据库中
                sqlcmd.ExecuteNonQuery();
                DataRow dr = cmb_dt_shicai.NewRow();
                dr["条目"] = NewShicai;
                cmb_dt_shicai.Rows.Add(dr);
                
                //更新使用食材数据的选择框信息
                cmbUpdate(comboBox_diet, cmb_dt_shicai,true);
                cmbUpdate(comboBox_datashicai, cmb_dt_shicai);
                listView_caiming.Items.Add(NewShicai);                             
                textBox_insercaiming.Text = "";
            }
            
        }

        //插入方式操作
        private void button_inseType_Click(object sender, EventArgs e)
        {
            string NewType = textBox_inseType.Text.Trim();
            if (DialogResult.OK != MessageBox.Show("确认添加" + NewType + "？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            {
                textBox_inseType.Text = "";
                return;
            }
            if (NewType == string.Empty)
            {
                return;
            }
            if (null == listView_inseType.FindItemWithText(NewType)) 
            {
                if (ConnectionState.Open != sqlconn.State)
                {
                    if (!sqlOpen())
                    {
                        MessageBox.Show("数据库打开失败!");
                        return;
                    }
                }
                sqlcmd.Connection = sqlconn;
                sqlcmd.CommandText = "insert into mode([方式]) values('"+NewType+"')";
                sqlcmd.ExecuteNonQuery();
                DataRow dr = cmb_dt_type.NewRow();
                dr["条目"] = NewType;
                cmb_dt_type.Rows.Add(dr);

                //更新使用方式信息的选择框
                cmbUpdate(comboBox_type, cmb_dt_type,true);
                cmbUpdate(comboBox_datatype, cmb_dt_type);
                listView_inseType.Items.Add(NewType);
                
                textBox_inseType.Text = "";
            }
        }

        //插入区域操作
        private void button_insertquyu_Click(object sender, EventArgs e)
        {
            string NewQuyu = textBox_insequyu.Text.Trim();
            if (DialogResult.OK != MessageBox.Show("确认添加" + NewQuyu + "？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            {
                textBox_insequyu.Text = "";
                return;
            }
            if (NewQuyu == string.Empty)
            {
                return;
            }
            if (null == listView_quyu.FindItemWithText(NewQuyu))
            {
                if (ConnectionState.Open != sqlconn.State)
                {
                    if (!sqlOpen())
                    {
                        MessageBox.Show("数据库打开失败!");
                        return;
                    }
                }
                sqlcmd.Connection = sqlconn;
                sqlcmd.CommandText = "insert into area([地区]) values('"+NewQuyu+"')";
                sqlcmd.ExecuteNonQuery();
                DataRow dr = cmb_dt_quyu.NewRow();
                dr["条目"] = NewQuyu;
                cmb_dt_quyu.Rows.Add(dr);
                //更新使用区域信息的选择框
                cmbUpdate(comboBox_dataquyu, cmb_dt_quyu);
                cmbUpdate(comboBox_Local, cmb_dt_quyu,true);
                listView_quyu.Items.Add(NewQuyu);
                
                textBox_insequyu.Text = "";
            }
        }


        //插入一条数据操作
        private void button_insertTmp_Click(object sender, EventArgs e)
        {
            DataRow dr = tmp_insert.NewRow();
            DateTime dtime= dateTimePicker_datainsert.Value;
            
            int daydiff=timediff(dtime);
            dr["日期"] = dtime.ToString();
            dr["区域"] = comboBox_dataquyu.SelectedItem;
            dr["方式"] = comboBox_datatype.SelectedItem;
            dr["食材"] = comboBox_datashicai.SelectedItem;
            if (textBox_datashuliang.Text.Trim() == string.Empty)
            {
                dr["数量"] = "0";
                textBox_datashuliang.Text = "0";
            }
            else
            {
                dr["数量"] = textBox_datashuliang.Text.Trim();
            }
            dr["日"] = daydiff;
            dr["周"] = daydiff/7;
            dr["月"] = timediffmonth(dtime);
            dr["年"] = dtime.Year;
            
            //将数据插入视图中并加入到缓冲数据表中
            dataGridView_tmpinsert.Rows.Add(dtime.ToLongDateString(),comboBox_dataquyu.SelectedItem,comboBox_datatype.SelectedItem,comboBox_datashicai.SelectedItem,textBox_datashuliang.Text);
            tmp_insert.Rows.Add(dr);           
        }

        
        //鼠标右键删除数据信息
        private void dataGridView_tmpinsert_MouseClick(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView_tmpinsert.CurrentCell != null)
                {
                    if (DialogResult.OK == MessageBox.Show("确认删除数据","提示",MessageBoxButtons.OKCancel,MessageBoxIcon.Question))
                    {
                        int i = dataGridView_tmpinsert.CurrentCell.RowIndex;
                        dataGridView_tmpinsert.Rows.RemoveAt(i);
                        tmp_insert.Rows.RemoveAt(i);
                    }
                    
                }
            }
            
        }

        //清空数据按钮操作
        private void button_dataCancle_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("确定要清空数据？","提示",MessageBoxButtons.OKCancel,MessageBoxIcon.Question))
            {
                dataGridView_tmpinsert.Rows.Clear();
                tmp_insert.Rows.Clear();
            }
        }

        //将缓存数据插入数据库中
        private void button__datainsert_Click(object sender, EventArgs e)
        {
            sqlcmd.Connection = sqlconn;
            int i = 0;
            if (tmp_insert.Rows.Count > 0)
            {
                for (; i < tmp_insert.Rows.Count; i++)
                {
                    sqlcmd.CommandText = "insert into tableAll([日期],[地区],[食材],[方式],[份数],[日],[周],[月],[年]) values('" +tmp_insert.Rows[i]["日期"]+"','"+tmp_insert.Rows[i]["区域"]+"','"
                                            + tmp_insert.Rows[i]["食材"] + "','" + tmp_insert.Rows[i]["方式"] + "'," + tmp_insert.Rows[i]["数量"] + "," + tmp_insert.Rows[i]["日"] + "," + tmp_insert.Rows[i]["周"] + ","
                                            + tmp_insert.Rows[i]["月"] + "," + tmp_insert.Rows[i]["年"]  + ")";
                    sqlcmd.ExecuteNonQuery();

                }
            }  
          
            //清空显示数据，清空缓存数据
            dataGridView_tmpinsert.Rows.Clear();
            tmp_insert.Rows.Clear();

        }

        //listview删除操作
        private void deletelistItem(string tablename,string columname,string delitem,ListView lv)
        {
            sqlcmd.Connection = sqlconn;
            sqlcmd.CommandText = "delete from "+tablename+" where "+columname +" = '"+delitem+"'";
            sqlcmd.ExecuteNonQuery();            
            //lv.Items.Remove(delitem));
        }

        //控制数量文本框仅可显示数字
        private void textBox_datashuliang_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || e.KeyChar == '\b'))
            {
                e.Handled = true;
            }
        }

        private void button_export_MouseClick(object sender, MouseEventArgs e)
        {
            if (dt_searchResult.Rows.Count <= 0)
            {
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel 2003 格式文件(*.xls)|*.xls|Excel 2007 格式文件(*.xlsx)|*.xlsx";
            sfd.FilterIndex = 0;
            

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                SaveDataTableToExcel(dt_searchResult, sfd.FileName.ToString());
            }
        }

        private void button_dbBak_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = today.ToString("yyyyMMdd")+".bak";
            sfd.Filter = "备份文件(*.bak)|*.bak";
            sfd.FilterIndex = 0;
            if (DialogResult.OK == sfd.ShowDialog())
            {
                if (sqlconn.State != ConnectionState.Open)
                {
                    if (!sqlOpen())
                    {
                        MessageBox.Show("数据库连接错误!");
                        return;
                    }
                }
                sqlcmd.Connection = sqlconn;
                sqlcmd.CommandText = "backup database test to disk='"+sfd.FileName+"'";
                sqlcmd.ExecuteNonQuery();
            }

        }


        


    }
}
