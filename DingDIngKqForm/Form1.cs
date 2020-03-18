using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DdApi;
using DdApi.Sys;
using DdApi.Util;
using AttDbContext;
using DingDingKq.Model;
using DingDingKq;
using SG.Utilities;
using SG.Utilities.FToS;
using kq;
using DdApi.Att;
using DdApi.Model;
namespace DingDIngKqForm
{
    public partial class Form1 : Form
    {
        DdOperator ddoper = DdOperator.GetDdOperator();
        DingDingKqContext dkc = new DingDingKqContext();
        DeptProvider dpr;
        DinDingCheckInOutOper ddcio { get; set; }
        AttRecordOper aro { get; set; }
        List<AttRecord> attRecords { get; set; } = new List<AttRecord>();
        List<KqUser> NowUserList { get; set; } = new List<KqUser>();
        List<DdApi.Model.User> DdUserList { get; set; } = new List<DdApi.Model.User>();
        DateTime dtBegin, dtEnd;

        public Form1()
        {
            InitializeComponent();

            //ddoper.CorpId = "ding9936f1c1a7d506d9acaaa37764f94726";
            //ddoper.CorpSecret = "FJi_OKqaRYt3t5s8Z5R7FtuflLyEGzZXD9yMfL2aSRZBTarrKs43Ub5oorVEEn5l";
            ddoper.AgentID = ConfigHelper.AppSettings("AgentID");
            ddoper.AppKey = ConfigHelper.AppSettings("AppKey");
            ddoper.AppSecret = ConfigHelper.AppSettings("AppSecret");
            dpr = new DeptProvider(ddoper);
            ddoper.SetDept(dpr.GetDepts());
            ddcio = new DinDingCheckInOutOper(ddoper, dkc, new AttContext());
            aro = new AttRecordOper(ddoper);
            ddcio.GetCheckInOutRecord = aro.GetCheckInOutRecord;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            LoadUserList();
            datetimeBegin.Value = DateTime.Parse(now.Year + "-" + now.Month + "-" + "01");
            //设置自动换行  
            dgvFailRecord.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //设置自动调整高度  
            dgvFailRecord.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            //设置自动换行  
            dgvSyncRecord.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //设置自动调整高度  
            dgvSyncRecord.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

        }

        /// <summary>
        /// 同步人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, EventArgs e)
        {
            DingDingKqUserOper ddkuo = new DingDingKqUserOper(ddoper, dkc, new AttContext());
            ddkuo.EmpSync();
            NowUserList = dkc.KqUsers.ToList();
            MessageBox.Show("同步完成");
        }
        /// <summary>
        /// 载入员工信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            LoadUserList();
        }

        private void LoadUserList()
        {
            new Task(() =>
            {

                NowUserList = dkc.KqUsers.ToList();
                DingDingKqUserOper ddkuo = new DingDingKqUserOper(ddoper, dkc, new AttContext());
                DdUserList = ddkuo.GetDdUserList();
                this.Invoke(new Action(() =>
                {
                    dgvNowEmp.DataSource = NowUserList;
                    dgvDdEmp.DataSource = DdUserList;
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                }));

            }).Start();

        }
        /// <summary>
        /// 删除考勤系统未录入人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            dkc.KqUsers.ToList().ForEach(p =>
            {
                if (p.AttUserId == 0) dkc.KqUsers.Remove(p);
            });
            dkc.SaveChanges();
            MessageBox.Show("删除完成");
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                dgvNowEmp.DataSource = NowUserList.Where(p => p.AttUserId == 0).ToList();
            }
            else
            {
                dgvNowEmp.DataSource = NowUserList;
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                dgvDdEmp.DataSource = DdUserList.Where(p => NowUserList.Count(n => n.DdId == p.DdId) == 0).ToList();
            }
            else
            {
                dgvDdEmp.DataSource = DdUserList;
            }
        }
        bool haveLoadUser = false;
        bool haveLoadSyncRecord = false;
        /// <summary>
        /// 改变选项卡载入钉钉系统人员数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedIndex == 1 && !haveLoadUser)
            {
                new Task(() =>
                 {
                     haveLoadUser = true;
                     DingDingKqUserOper ddkuo = new DingDingKqUserOper(ddoper, dkc, new AttContext());
                     DdUserList = ddkuo.GetDdUserList();
                     this.Invoke(new Action(() =>
                     {
                         dgvEmp.DataSource = DdUserList;
                     }));
                 }).Start();

            }
            else if (tabControl1.SelectedIndex == 2 && !haveLoadSyncRecord)
            {

                haveLoadSyncRecord = true;
                LoadSyncRecore();
            }
        }
        List<FailRecord> failRecords = new List<FailRecord>();
        /// <summary>
        /// 载入同步数据
        /// </summary>
        private void LoadSyncRecore()
        {
            dgvSyncRecord.DataSource = dkc.AttSyncRecords.ToList();
            failRecords = dkc.FailRecords.ToList();
            failRecordBindingSource.DataSource = failRecords;
        }

        /// <summary>
        /// 选择人员显示读取的考勤数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvEmp_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dgvEmp.CurrentRow != null)
            {

                User re = (User)dgvEmp.CurrentRow.DataBoundItem;
                dgvAtt.DataSource = attRecords.Where(p => p.DdId == re.DdId).ToList();


            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            LoadSyncRecore();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            dkc.SaveChanges();
            MessageBox.Show("保存成功!");
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            dkc.FailRecords.Remove((FailRecord)dgvFailRecord.CurrentRow.DataBoundItem);
            failRecordBindingSource.Remove(dgvFailRecord.CurrentRow.DataBoundItem);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (failRecords.Count > 0)
            {
                ddcio.SyncByFailRecord(failRecords);
            }
            MessageBox.Show("同步完成");
        }
        /// <summary>
        /// 从钉钉系统读取考勤系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            List<string> emps = new List<string>();
            DdUserList.ForEach(p =>
            {
                emps.Add(p.DdId);
            });
            dtBegin = datetimeBegin.Value;
            dtEnd = datetimeEnd.Value;
            attRecords = new List<AttRecord>();
            attRecords = aro.GetCheckInOutRecord(emps, dtBegin, dtEnd).OrderBy(p => p.BaseCheckTime).ToList();
            dgvAtt.DataSource = attRecords;

        }

        private void 显示全部ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           failRecords=dkc.FailRecords.ToList();
           failRecordBindingSource.DataSource = failRecords;
        }

        private void 显示失败ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            failRecords = dkc.FailRecords.Where(p=>!p.IsSucess).ToList();
            failRecordBindingSource.DataSource = failRecords;

        }

        /// <summary>
        /// 将钉钉数据写入考勤系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {

            ddcio.WriteToAttSystem(attRecords, dtBegin, dtEnd);
            MessageBox.Show("写入数据库完成");
        }


    }
}
