using System;
using System.Windows.Forms;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Text.Json.Nodes;
namespace dxf
{
    public partial class Form1 : Form
    {
        public Script script = new Script();
		public Form1()
        {
            InitializeComponent();
            script.OutputBox = OutputBox;
            btnExec.Click +=BtnExec_Click;
            btnClear.Click +=BtnClear_Click;   
            openMenu.Click += OpenMenu_Click;
            saveMenu.Click += SaveMenu_Click;
            execMenu.Click += BtnExec_Click;

		}
        protected override void OnLoad(EventArgs e)
        {
            PrefLoad();
            base.OnLoad(e);
        }

		protected override void OnFormClosing(FormClosingEventArgs e)
        {
            PrefSave();
            base.OnFormClosing(e);
		}
		public void PrefSave()
        {
            string filepath = JSApp.prefFileName;
            JsonObject prefs = new JsonObject();
            prefs["script"] = InputBox.Text;
            string json = prefs.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(filepath, json);
		}
        public void PrefLoad()
        {
            string filepath = JSApp.prefFileName;
            if (!File.Exists(filepath)) return;
            string json = File.ReadAllText(filepath);
            try
            {
                JsonObject? prefs = JsonNode.Parse(json)?.AsObject();
                if (prefs == null) return;
                InputBox.Text = prefs["script"]?.GetValue<string>() ?? "";
                InputBox.SelectionLength = 0;
                InputBox.SelectionStart = 0;
			}
            catch (Exception ex)
            {
                MessageBox.Show("Pref Load Error: " + ex.Message);
            }
        }
		private void OpenMenu_Click(object? sender, EventArgs e)
        {
            OpenFileDialog();
        }
        private void SaveMenu_Click(object? sender, EventArgs e)
        {
            SaveFileDialog();
        }
        private void Form1_Load(object? sender, EventArgs e)
        {
			//Execute();
		}
		public void Execute()
        {
			script.Execute(InputBox.Text);
			if (script.Error != "")
			{
				OutputBox.AppendText(script.Error + "\r\n");
			}
		}
		private void BtnExec_Click(object? sender, EventArgs e)
        {
           Execute();
		}
        private void BtnClear_Click(object? sender, EventArgs e)
        {
            OutputBox.Clear();
		}
        private string m_filepath="";
		public bool OpenFileDialog()
        {
            bool ret = false;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (m_filepath != "")
                {
                    ofd.InitialDirectory = Path.GetDirectoryName(m_filepath)??"";
                    ofd.FileName = Path.GetFileName(m_filepath);
				}
                ofd.Filter = "js Files (*.js;*.jsx)|*.js;*jsx|All Files (*.*)|*.*";
                ofd.FilterIndex = 1;
                ofd.Title = "Open JavaScript File";
                ofd.DefaultExt = "js";
				if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        m_filepath = ofd.FileName;
                        InputBox.Text = File.ReadAllText(ofd.FileName);
                        ret = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            return ret;
		}
		public bool SaveFileDialog()
		{
			bool ret = false;
			using (SaveFileDialog sfd = new SaveFileDialog())
			{
				if (m_filepath != "")
				{
					sfd.InitialDirectory = Path.GetDirectoryName(m_filepath) ?? "";
					sfd.FileName = Path.GetFileName(m_filepath);
				}
				sfd.Filter = "js Files (*.js;*.jsx)|*.js;*jsx|All Files (*.*)|*.*";
				sfd.FilterIndex = 1;
				sfd.Title = "Open JavaScript File";
				sfd.DefaultExt = "js";
				if (sfd.ShowDialog() == DialogResult.OK)
				{
					try
					{
						m_filepath = sfd.FileName;
                        File.WriteAllText(sfd.FileName, InputBox.Text);
						ret = true;
					}
					catch (Exception ex)
					{
						MessageBox.Show("Error: " + ex.Message);
					}
				}
			}
			return ret;
		}
	}
}
