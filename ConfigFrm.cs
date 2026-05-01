﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using Core.Base;
using Core.Comm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Win
{
    public partial class ConfigFrm : Form
    {
        Win.WinInput winput = null;
        public ConfigFrm(Win.WinInput input)
        {
            this.winput=input;
            InitializeComponent();
            //this.Icon = new Icon(System.IO.Path.Combine(Application.StartupPath, "log32.ico"));
            // 设置ESC键关闭窗口
            this.CancelButton = this.btnClose;
            // 添加KeyDown事件处理器
            this.KeyDown += new KeyEventHandler(ConfigFrm_KeyDown);
        }

        private void ConfigFrm_KeyDown(object sender, KeyEventArgs e)
        {
            // 捕获ESC键按下事件
            if (e.KeyCode == Keys.Escape)
            {
                this.btnClose_Click(sender, e);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            winput.LoadSettting();
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            InputMode.OpenCould = this.ckOpenCould.Checked;
            InputMode.AutoRun = this.chkAutoRun.Checked;
            InputMode.AutoUpdate = this.ckAutoUpdate.Checked;
            InputMode.OpenLink = this.ckLink.Checked;
            InputMode.OpenAltSelect = this.ckalt.Checked;
            InputMode.SingleInput = this.SingleInput.Checked;
            InputMode.right3_out = this.ckright3out.Checked;
            winput.curTrac = this.tracchk.Checked;
            winput.curMouseTrac = this.mousetracchk.Checked;
            InputMode.PageSize = (int)this.selectnum.Value;
            InputMode.ViewType = this.chkVertical.Checked ? 1 : 0;
            InputMode.txtla = this.txtla.Text.Trim();
            InputMode.txtra = this.txtra.Text.Trim();
            InputMode.txtlas = this.txtlas.Text.Trim();
            InputMode.txtras = this.txtras.Text.Trim();
            InputMode.txtlra = this.txtlra.Text.Trim();
            InputMode.txtlras = this.txtlras.Text.Trim();
            InputMode.closebj = this.chclosebj.Checked;
            InputMode.autopos = this.ckautopos.Checked;
            InputMode.tautopos = this.cktautopos.Checked;
            InputMode.bjzckgsp = this.chkbjzckgsp.Checked;
            InputMode.omeno = this.chkomeno.Checked;
            InputMode.zsallmap = this.chkzsallmap.Checked;
            InputMode.zsmode1 = ((int)this.nuzsmode2.Value);
            InputMode.outtype = this.cmouttype.SelectedIndex;
            InputMode.datacf = this.chedatacf.Checked;
            InputMode.imghh = this.chimghh.Checked;
            InputMode.oneoutbj = this.choneoutbj.Checked;
            InputMode.ftfzxs = this.chftfzxs.Checked;
            InputMode.dcxz = this.chkdcxz.Checked;
            InputMode.iselect = this.chkiselect.Checked;
            InputMode.onesp = this.chkonesp.Checked;
            InputMode.select3 = this.chkselect3.Checked;
            InputMode.semicolonSelect = this.chksemicolonSelect.Checked;
            InputMode.spaceaout = this.cmspace.SelectedIndex;
            InputMode.autodata = this.chkautodata.Checked;
            InputMode.useregular = this.cheuseregular.Checked;
            InputMode.smautoadd = this.chksmautoadd.Checked;
            winput.SaveSetting();
            WinInput.InputStatus.bstring = new SolidBrush(InputMode.Skinbstring);
            WinInput.InputStatus.bcstring = new SolidBrush(InputMode.Skinbcstring);
            WinInput.InputStatus.fbcstring = new SolidBrush(InputMode.Skinfbcstring);
            WinInput.InputStatus.skinback = new SolidBrush(InputMode.SkinBack);
            if (InputMode.useregular && File.Exists(System.IO.Path.Combine(InputMode.AppPath, "dict", InputMode.CDPath, "setting.yaml")))
                WinInput.settingYaml = new YAMLHelp(System.IO.Path.Combine(InputMode.AppPath, "dict", InputMode.CDPath, "setting.yaml"));
            Comm.Function.RunWhenStart(InputMode.AutoRun);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ConfigFrm_Load(object sender, EventArgs e)
        {
            this.cmouttype.SelectedIndex = 0;
            this.cmspace.SelectedIndex = 0;
            this.ckOpenCould.Checked = InputMode.OpenCould;
            this.ckAutoUpdate.Checked = InputMode.AutoUpdate;
            this.chkAutoRun.Checked = InputMode.AutoRun;
            this.ckLink.Checked = InputMode.OpenLink;
            this.numSkinHeight.Value = InputMode.SkinHeith;
            this.selectnum.Value = InputMode.PageSize;
            this.chkVertical.Checked = (InputMode.ViewType == 1);
            this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
            this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
            this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            this.SkinBack.ForeColor = InputMode.SkinBack;
            this.btnSkinFontName.Font = new Font(InputMode.SkinFontName, InputMode.SkinFontSize);
            this.tracchk.Checked = winput.curTrac;
            this.mousetracchk.Checked = winput.curMouseTrac;
            this.ckalt.Checked = InputMode.OpenAltSelect;
            this.SingleInput.Checked = InputMode.SingleInput;
            this.txtla.Text = InputMode.txtla;
            this.txtra.Text = InputMode.txtra;
            this.txtlas.Text = InputMode.txtlas;
            this.txtras.Text = InputMode.txtras;
            this.txtlra.Text = InputMode.txtlra;
            this.txtlras.Text = InputMode.txtlras;
            this.ckright3out.Checked = InputMode.right3_out;
            this.chclosebj.Checked = InputMode.closebj;
            this.ckautopos.Checked = InputMode.autopos;
            this.cktautopos.Checked = InputMode.tautopos;
            this.chkbjzckgsp.Checked = InputMode.bjzckgsp;
            this.chkomeno.Checked = InputMode.omeno;
            this.chkzsallmap.Checked = InputMode.zsallmap;
            this.nuzsmode2.Value = InputMode.zsmode1;
            this.cmouttype.SelectedIndex = InputMode.outtype;
            this.cmspace.SelectedIndex = InputMode.spaceaout;
            this.groupBox2.BackColor = InputMode.SkinBack;
            
            // 临时禁用事件，避免设置索引时触发颜色重置
            this.comboBox1.SelectedIndexChanged -= new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBox1.SelectedIndex = InputMode.SkinIndex;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            
            this.chedatacf.Checked = InputMode.datacf;
            this.chimghh.Checked = InputMode.imghh;
            this.choneoutbj.Checked = InputMode.oneoutbj;
            this.chftfzxs.Checked = InputMode.ftfzxs;

            this.chkdcxz.Checked = InputMode.dcxz;
            this.chkiselect.Checked = InputMode.iselect;
            this.chkonesp.Checked = InputMode.onesp;
            this.chkselect3.Checked = InputMode.select3;
            this.chksemicolonSelect.Checked = InputMode.semicolonSelect;

            this.chkautodata.Checked = InputMode.autodata;

            this.cheuseregular.Checked = InputMode.useregular;
            this.chksmautoadd.Checked = InputMode.smautoadd;
            this.Text = "属性设置 ";
        }
 
        private void numSkinHeight_ValueChanged(object sender, EventArgs e)
        {
            InputMode.SkinHeith = int.Parse(this.numSkinHeight.Value.ToString());
        }

        private void btnSkinbstring_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = this.btnSkinbstring.ForeColor;
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.btnSkinbstring.ForeColor = this.colorDialog1.Color;
                InputMode.Skinbstring = this.btnSkinbstring.ForeColor;
            }
        }

        private void btnSkinbcstring_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = this.btnSkinbcstring.ForeColor;
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.btnSkinbcstring.ForeColor = this.colorDialog1.Color;
                InputMode.Skinbcstring = this.btnSkinbcstring.ForeColor;
            }
        }

        private void btnSkinfbcstring_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = this.btnSkinfbcstring.ForeColor;
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.btnSkinfbcstring.ForeColor = this.colorDialog1.Color;
                InputMode.Skinfbcstring = this.btnSkinfbcstring.ForeColor;
            }
        }

        private void btnSkinFontName_Click(object sender, EventArgs e)
        {
            this.fontDialog1.Font = new Font(InputMode.SkinFontName
                , InputMode.SkinFontSize);
            if (this.fontDialog1.ShowDialog() == DialogResult.OK)
            {
                this.btnSkinFontName.Font = this.fontDialog1.Font;
                InputMode.SkinFontName = this.btnSkinFontName.Font.Name;
                InputMode.SkinFontSize = (int)this.btnSkinFontName.Font.Size;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            InputMode.SkinIndex = this.comboBox1.SelectedIndex;
            if (this.comboBox1.SelectedIndex == 0)
            {
                // 默认 - 孤寺配色
                InputMode.SkinBack = Color.FromArgb(68, 68, 68);//背景色
                InputMode.Skinbordpen = Color.FromArgb(80, 80, 80);//边框色
                InputMode.Skinbstring = Color.FromArgb(232, 243, 246);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(130, 230, 202);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(200, 255, 220);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
    
            }
            else if (this.comboBox1.SelectedIndex == 1)
            {
                //清风 - 清新薄荷绿渐变
                InputMode.SkinBack = Color.FromArgb(240, 248, 250);//背景色
                InputMode.Skinbordpen = Color.FromArgb(100, 180, 160);//边框色
                InputMode.Skinbstring = Color.FromArgb(50, 80, 70);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(200, 100, 80);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(60, 150, 130);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
            else if (this.comboBox1.SelectedIndex == 2)
            {
                //安卓 - Material Design青绿色
                InputMode.SkinBack = Color.FromArgb(3, 169, 244);//背景色
                InputMode.Skinbordpen = Color.FromArgb(0, 150, 136);//边框色
                InputMode.Skinbstring = Color.FromArgb(255, 255, 255);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(255, 235, 59);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(255, 193, 7);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
            else if (this.comboBox1.SelectedIndex == 3)
            {
                //星际争霸 - 暗蓝科技风格
                InputMode.SkinBack = Color.FromArgb(10, 25, 45);//背景色
                InputMode.Skinbordpen = Color.FromArgb(100, 200, 255);//边框色
                InputMode.Skinbstring = Color.FromArgb(150, 220, 255);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(100, 255, 218);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(0, 255, 136);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
            else if (this.comboBox1.SelectedIndex == 4)
            {
                //小鹤 - 优雅紫色渐变
                InputMode.SkinBack = Color.FromArgb(250, 245, 255);//背景色
                InputMode.Skinbordpen = Color.FromArgb(156, 39, 176);//边框色
                InputMode.Skinbstring = Color.FromArgb(74, 20, 140);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(103, 58, 183);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(233, 30, 99);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
            else if (this.comboBox1.SelectedIndex == 5)
            {
                //暗堂 - Dark Temple
                InputMode.SkinBack = Color.FromArgb(34, 34, 34);//背景色
                InputMode.Skinbordpen = Color.FromArgb(34, 34, 34);//边框色
                InputMode.Skinbstring = Color.FromArgb(216, 227, 230);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(96, 108, 255);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(146, 246, 218);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
            else if (this.comboBox1.SelectedIndex == 6)
            {
                // Dota 2 - 暗红黑金属风格
                InputMode.SkinBack = Color.FromArgb(30, 15, 15);//背景色
                InputMode.Skinbordpen = Color.FromArgb(180, 60, 60);//边框色
                InputMode.Skinbstring = Color.FromArgb(240, 200, 200);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(255, 107, 107);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(255, 87, 34);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
            else if (this.comboBox1.SelectedIndex == 7)
            {
                // 谷歌/Google - 清新多彩
                InputMode.SkinBack = Color.FromArgb(255, 255, 255);//背景色
                InputMode.Skinbordpen = Color.FromArgb(219, 68, 55);//边框色
                InputMode.Skinbstring = Color.FromArgb(32, 33, 36);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(26, 115, 232);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(66, 133, 244);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
            else if (this.comboBox1.SelectedIndex == 8)
            {
                // 空明 - 深色模式
                InputMode.SkinBack = Color.FromArgb(18, 18, 18);//背景色
                InputMode.Skinbordpen = Color.FromArgb(60, 60, 60);//边框色
                InputMode.Skinbstring = Color.FromArgb(220, 220, 220);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(150, 200, 255);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(100, 255, 218);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
            else if (this.comboBox1.SelectedIndex == 9)
            {
                // 黑白 - 极简黑白
                InputMode.SkinBack = Color.FromArgb(250, 250, 250);//背景色
                InputMode.Skinbordpen = Color.FromArgb(180, 180, 180);//边框色
                InputMode.Skinbstring = Color.FromArgb(30, 30, 30);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(100, 100, 100);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(0, 0, 0);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LXFrm lxfrm = new LXFrm();
            lxfrm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("清除后将重新统计,确定清除吗?？", "清除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                WinInput.Input.mapkeys.ForEach(f =>
                {
                    f.keydown = 0;
                });
            }
             
        }

        private void SkinBack_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = this.SkinBack.ForeColor;
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.SkinBack.ForeColor = this.colorDialog1.Color;
                InputMode.SkinBack = this.SkinBack.ForeColor;
                this.groupBox2.BackColor = InputMode.SkinBack;
            }
        }
    }
}
