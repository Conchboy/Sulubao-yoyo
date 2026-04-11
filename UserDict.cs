using Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Core.Win
{
    public partial class UserDict : Form
    {
        InputMode input = null;
        WinInput winput = null;
        DataTable dt = new DataTable();
        
        private string originalValue = ""; // 保存完整的原始值（最多8字）
        private string initialValue = ""; // 保存初始显示的值（最多4字）
        private bool isEdited = false; // 标记字词是否被编辑过
        private bool isProgrammaticChange = false; // 标记是否是程序自动修改的文本
        
        // 单例模式
        private static UserDict _instance = null;
        public static UserDict Instance
        {
            get { return _instance; }
        }
        
        public UserDict(InputMode ninput, WinInput nwinput)
        {
            this.input = ninput;
            this.winput = nwinput;
            InitializeComponent();
            this.KeyPreview = true;
            _instance = this;
        }
        public UserDict(InputMode ninput, WinInput nwinput,string code,string va)
        {
            this.input = ninput;
            this.winput = nwinput;
            InitializeComponent();
            this.KeyPreview = true;
            _instance = this;
            
            // 保存完整的原始值（最多8字）
            this.originalValue = va.Trim();
            
            // 初始显示最多4字
            if (this.originalValue.Length > 4)
            {
                this.initialValue = this.originalValue.Substring(this.originalValue.Length - 4);
            }
            else
            {
                this.initialValue = this.originalValue;
            }
            
            this.txtValue.Text = this.initialValue;
            // 使用我们自己的编码生成逻辑，而不是传入的编码
        }
        
        protected override void OnClosed(EventArgs e)
        {
            _instance = null;
            base.OnClosed(e);
        }
        private void UserDict_Load(object sender, EventArgs e)
        {
            dt.Columns.Add("词库");
            Query("", "");
            
            // 绑定事件
            this.txtValue.KeyDown += new KeyEventHandler(txtValue_KeyDown);
            this.txtCode.KeyDown += new KeyEventHandler(txtCode_KeyDown);
            this.txtValue.TextChanged += new EventHandler(txtValue_TextChanged);
            
            // 如果有原始值，初始化编码建议
            if (!string.IsNullOrEmpty(this.txtValue.Text))
            {
                UpdateCodeSuggestion(this.txtValue.Text);
            }
        }

        // 处理编码输入框的按键事件
        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                // 在编码输入框按向下键，扩展字词长度
                if (!this.isEdited)
                {
                    string currentValue = this.txtValue.Text;
                    if (currentValue.Length < 8 && this.originalValue.Length > currentValue.Length)
                    {
                        int startIndex = this.originalValue.Length - currentValue.Length - 1;
                        if (startIndex >= 0)
                        {
                            string newValue = this.originalValue.Substring(startIndex);
                            this.isProgrammaticChange = true;
                            this.txtValue.Text = newValue;
                            this.isProgrammaticChange = false;
                            UpdateCodeSuggestion(newValue);
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }
                    }
                }
            }
        }

        // 重写 ProcessCmdKey 以确保 Esc 键或 Alt+Q 能正常工作
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape || 
                keyData == (Keys.Alt | Keys.D0) || 
                keyData == (Keys.Alt | Keys.NumPad0) ||
                keyData == (Keys.Alt | Keys.X))
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // 处理字词输入框的文本变化事件
        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            // 如果是程序自动修改的，不标记为已编辑
            if (this.isProgrammaticChange)
            {
                return;
            }
            
            // 如果文本与完整原始值或初始值不同，标记为已编辑
            this.isEdited = (this.txtValue.Text != this.originalValue) && (this.txtValue.Text != this.initialValue);
            
            // 无论是否编辑过，都更新编码
            UpdateCodeSuggestion(this.txtValue.Text);
        }

        // 处理字词输入框的按键事件
        private void txtValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.isEdited) return; // 如果已编辑过，不处理上下键
            
            string currentValue = this.txtValue.Text;
            if (string.IsNullOrEmpty(currentValue)) return;
            
            if (e.KeyCode == Keys.Up)
            {
                // 向上键：缩减一字（最少两字）
                if (currentValue.Length > 2)
                {
                    string newValue = currentValue.Substring(1); // 去掉第一个字，保留最后一个字为锚点
                    this.isProgrammaticChange = true;
                    this.txtValue.Text = newValue;
                    this.isProgrammaticChange = false;
                    UpdateCodeSuggestion(newValue);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                // 向下键：扩展一字（最多8字）
                if (currentValue.Length < 8)
                {
                    // 尝试从原始值中扩展
                    if (this.originalValue.Length > currentValue.Length)
                    {
                        int startIndex = this.originalValue.Length - currentValue.Length - 1;
                        if (startIndex >= 0)
                        {
                            string newValue = this.originalValue.Substring(startIndex);
                            this.isProgrammaticChange = true;
                            this.txtValue.Text = newValue;
                            this.isProgrammaticChange = false;
                            UpdateCodeSuggestion(newValue);
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }
                    }
                }
            }
        }

        // 更新编码建议 - 无论是否编辑过都提供编码
        private void UpdateCodeSuggestion(string word)
        {
            string code = GetYoyoCode(word);
            if (!string.IsNullOrEmpty(code))
            {
                this.txtCode.Text = code;
            }
            else
            {
                this.txtCode.Text = "";
            }
        }

        // 获取yoyo编码（按照规则生成）
        private string GetYoyoCode(string word)
        {
            if (string.IsNullOrEmpty(word) || word.Length < 2) return "";
            
            int len = word.Length;
            StringBuilder codeBuilder = new StringBuilder();
            
            try
            {
                if (len == 2)
                {
                    // 两字词：第一字前两码 + 第二字前两码
                    string code1 = input.GetCodeForChar(word.Substring(0, 1));
                    string code2 = input.GetCodeForChar(word.Substring(1, 1));
                    if (!string.IsNullOrEmpty(code1) && code1.Length >= 2)
                        codeBuilder.Append(code1.Substring(0, 2));
                    else
                        codeBuilder.Append(code1);
                    
                    if (!string.IsNullOrEmpty(code2) && code2.Length >= 2)
                        codeBuilder.Append(code2.Substring(0, 2));
                    else
                        codeBuilder.Append(code2);
                }
                else if (len == 3)
                {
                    // 三字词：第一字第一码 + 第二字第一码 + 第三字前两码
                    string code1 = input.GetCodeForChar(word.Substring(0, 1));
                    string code2 = input.GetCodeForChar(word.Substring(1, 1));
                    string code3 = input.GetCodeForChar(word.Substring(2, 1));
                    
                    if (!string.IsNullOrEmpty(code1) && code1.Length >= 1)
                        codeBuilder.Append(code1.Substring(0, 1));
                    
                    if (!string.IsNullOrEmpty(code2) && code2.Length >= 1)
                        codeBuilder.Append(code2.Substring(0, 1));
                    
                    if (!string.IsNullOrEmpty(code3) && code3.Length >= 2)
                        codeBuilder.Append(code3.Substring(0, 2));
                    else
                        codeBuilder.Append(code3);
                }
                else
                {
                    // 四字及以上词：第一字第一码 + 第二字第一码 + 第三字第一码 + 最后一字第一码
                    string code1 = input.GetCodeForChar(word.Substring(0, 1));
                    string code2 = input.GetCodeForChar(word.Substring(1, 1));
                    string code3 = input.GetCodeForChar(word.Substring(2, 1));
                    string codeLast = input.GetCodeForChar(word.Substring(len - 1, 1));
                    
                    if (!string.IsNullOrEmpty(code1) && code1.Length >= 1)
                        codeBuilder.Append(code1.Substring(0, 1));
                    
                    if (!string.IsNullOrEmpty(code2) && code2.Length >= 1)
                        codeBuilder.Append(code2.Substring(0, 1));
                    
                    if (!string.IsNullOrEmpty(code3) && code3.Length >= 1)
                        codeBuilder.Append(code3.Substring(0, 1));
                    
                    if (!string.IsNullOrEmpty(codeLast) && codeLast.Length >= 1)
                        codeBuilder.Append(codeLast.Substring(0, 1));
                }
            }
            catch
            {
                return "";
            }
            
            return codeBuilder.ToString();
        }

        private void Query(string c, string v)
        {
            int count = 0;
            dt.Clear();
            foreach (var s in input.UserDit)
            {
                bool view = false;
                if (c.Length > 0 && v.Length > 0)
                {
                    if (s.IndexOf(c) >= 0 && s.IndexOf(v) > 0)
                        view = true;
                }
                else if (c.Length > 0 && v.Length == 0)
                {
                    if (s.IndexOf(c) == 0)
                        view = true;
                }
                else if (c.Length == 0 && v.Length > 0)
                {
                    if (s.IndexOf(v) > 0)
                        view = true;
                }
                else if (c.Length == 0 && v.Length == 0)
                    view = true;
                if (view)
                {
                    if (count > 50) break;
                    var rw = dt.NewRow();
                    rw[0] = s;
                    dt.Rows.Add(rw);
                    count++;
                }
            }
            this.dataGridView1.DataSource = dt;
        }

        private void AddDict(string c, string v)
        {
            var tu = input.UserDit.ToList();
            if (tu.Find(f => f == c + " " + v) == null)
            {
                tu.Add(c + " " + v);
                input.UserDit = tu.ToArray();

                winput.SaveUserDict();
                btnQuery_Click(null, null);
            }
            else
                MessageBox.Show("已存在该词库!");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
 
            if (this.txtCode.Text.Trim().Length > 0 && this.txtValue.Text.Trim().Length > 0)
            {
                AddDict(this.txtCode.Text.Trim(), this.txtValue.Text.Trim());
            }
            else
                MessageBox.Show("编码或字词不能为空!");
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            Query(this.txtCode.Text.Trim(), this.txtValue.Text.Trim());
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
 
            if (this.dataGridView1.SelectedRows.Count <= 0)
            {
                MessageBox.Show("未选中要删除的词条!");
            }
            else
            {
                var tu = input.UserDit.ToList();
                bool haveupdate = false;
                for (int i = 0; i < this.dataGridView1.SelectedRows.Count; i++)
                {
                    if (this.dataGridView1.SelectedRows[i].Cells[0].Value == null) continue;
                    string selects = this.dataGridView1.SelectedRows[i].Cells[0].Value.ToString();
                    if (!string.IsNullOrEmpty(selects))
                    {
                        //delete one
                         
                          if (tu.Find(f => f == selects) != null)
                          {
                              tu.Remove(selects);
                              haveupdate = true;
                          }
                    }
                }
                if (haveupdate)
                {
                    input.UserDit = tu.ToArray();
                    winput.SaveUserDict();
                }
                btnQuery_Click(null, null);
            }
        }
    }
}
