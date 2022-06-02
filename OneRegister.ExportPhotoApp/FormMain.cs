using OneRegister.ExportPhotoApp.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace OneRegister.ExportPhotoApp
{
    public partial class FormMain : Form
    {
        private FormBusiness _form;
        private readonly ExportBusiness _business;
        private List<StudentModel> _students;
        private const int LIMIT = 1000;
        private const string C_APIURL = "ApiUrl";
        private const string C_DMSURL = "DMSUrl";
        private const string C_UserName = "User";

        public FormMain()
        {
            InitializeComponent();
            _form = new FormBusiness(rtbxResult);
            _business = new ExportBusiness(this);
            _students = new List<StudentModel>();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            HideLoading();

            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(2);

            _form.WriteVersion($"## Welcome to Export Photo Application Version {version}");
            btnBrowse.Enabled = false;
            btnExport.Enabled = false;
            lblVersion.Text = version;
            LoadConfig();
        }

        private void LoadConfig()
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings[C_APIURL] == null)
                {
                    config.AppSettings.Settings.Add(C_APIURL, "https://uat.onepay.com.my:65108/portal/api");
                }
                if (config.AppSettings.Settings[C_DMSURL] == null)
                {
                    config.AppSettings.Settings.Add(C_DMSURL, "https://uat.onepay.com.my:65104");
                }
                if (config.AppSettings.Settings[C_UserName] == null)
                {
                    config.AppSettings.Settings.Add(C_UserName, "[UserName]");
                }
                config.Save();
                ConfigurationManager.RefreshSection("appSettings");

                tbxUrl.Text = ConfigurationManager.AppSettings[C_APIURL];
                tbxDmsUrl.Text = ConfigurationManager.AppSettings[C_DMSURL];
                tbxUser.Text = ConfigurationManager.AppSettings[C_UserName];
            }
            catch (Exception ex)
            {
                _form.WriteError("Error On Loading Config File");
                _form.WriteError(ex.Message);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxUrl.Text))
            {
                _form.WriteWarning("URL should be provided");
                return;
            }
            try
            {
                ShowLoading();
                LoginModel loginModel = new() { UserName = tbxUser.Text.Trim(), Password = tbxPass.Text.Trim() };
                if (_business.IsAuthorized(loginModel))
                {
                    SaveCurrentConfiguration();
                    SetAfterLoginAppearance();
                    _form.WriteSuccess("Welcome " + loginModel.UserName + "!");
                }
                else
                {
                    _form.WriteError("Authentication failed");
                }
            }
            catch (Exception ex)
            {
                HideLoading();
                _form.WriteError(ex.Message);
            }
            finally
            {
                HideLoading();
            }
        }

        private void SaveCurrentConfiguration()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[C_APIURL].Value = tbxUrl.Text;
            config.AppSettings.Settings[C_DMSURL].Value = tbxDmsUrl.Text;
            config.AppSettings.Settings[C_UserName].Value = tbxUser.Text;

            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                ShowLoading();
                using var fbd = new FolderBrowserDialog();
                var result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    tbxPath.Text = fbd.SelectedPath;
                    bool hasRightPermission = ExportBusiness.HasPermissionToPath(fbd.SelectedPath);
                    if (hasRightPermission)
                    {
                        var model = _business.GetSchoolYears();
                        cboSchool.DataSource = model.SchoolsList;
                        cboSchool.DisplayMember = "Text";
                        cboSchool.ValueMember = "Value";
                        cboYear.DataSource = model.YearsList;
                        cboYear.DisplayMember = "Text";
                        cboYear.ValueMember = "Value";
                    }
                    else
                    {
                        _form.WriteError("you don't have grant permission to write in selected path");
                    }
                }
            }
            catch (Exception ex)
            {
                HideLoading();
                _form.WriteError(ex.Message);
            }
            finally
            {
                HideLoading();
            }
        }

        private void btnGetClass_Click(object sender, EventArgs e)
        {
            try
            {
                ShowLoading();
                if (cboSchool.SelectedItem == null || cboYear.SelectedItem == null) return;
                var model = _business.GetClassAndHomerooms(((TextValueItem)cboSchool.SelectedItem).Value, ((TextValueItem)cboYear.SelectedItem).Value);
                cboClass.DataSource = model.Classes;
                cboClass.DisplayMember = "Text";
                cboClass.ValueMember = "Value";
                cboHomeroom.DataSource = model.HomeRooms;
                cboHomeroom.DisplayMember = "Text";
                cboHomeroom.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                HideLoading();
                _form.WriteError(ex.Message);
            }
            finally
            {
                HideLoading();
            }
        }

        private void btnSetFilter_Click(object sender, EventArgs e)
        {
            try
            {
                ShowLoading();
                if (string.IsNullOrEmpty(tbxPath.Text))
                {
                    _form.WriteInfo("Please provide a folder to save photos");
                    return;
                }
                progressBarExport.Value = 0;
                var requestModel = new StudentListRequestModel
                {
                    SchoolId = ((TextValueItem)cboSchool.SelectedItem)?.Value,
                    ClassId = ((TextValueItem)cboClass.SelectedItem)?.Value,
                    HomeRoomId = ((TextValueItem)cboHomeroom.SelectedItem)?.Value
                };
                if (string.IsNullOrEmpty(requestModel.SchoolId))
                {
                    throw new ApplicationException("You need to choose at least one School to proceed");
                }
                _students = _business.GetStudentList(requestModel);

                _form.WriteInfo($"matched records: {_students.Count}");
                if (_students.Count > LIMIT)
                {
                    _form.WriteInfo($"you can't export records more than {LIMIT}");
                    return;
                }
                if (_students.Count > 0)
                {
                    btnExport.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                HideLoading();
                _form.WriteError(ex.Message);
            }
            finally
            {
                HideLoading();
            }
        }

        private void btnFilterById_Click(object sender, EventArgs e)
        {
            try
            {
                ShowLoading();
                if (string.IsNullOrEmpty(tbxPath.Text))
                {
                    _form.WriteInfo("Please provide a folder to save photos");
                    return;
                }
                progressBarExport.Value = 0;
                _students.Clear();
                var student = _business.GetStudent(tbxIcNumber.Text);
                if (student == null)
                {
                    _form.WriteInfo("IC number does not exist or you don't have access to this School");
                }
                else
                {
                    _students.Add(student);
                    btnExport.Enabled = true;
                    _form.WriteInfo($"matched records: {_students.Count}");
                }
            }
            catch (Exception ex)
            {
                HideLoading();
                _form.WriteError(ex.Message);
            }
            finally
            {
                HideLoading();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            progressBarExport.Minimum = 0;
            progressBarExport.Maximum = _students.Count;
            int counter = 1;
            foreach (var student in _students)
            {
                try
                {
                    var fileByte = _business.GetPhoto(tbxDmsUrl.Text, student.PhotoId);
                    if (fileByte != null)
                    {
                        using var image = Image.FromStream(new MemoryStream(fileByte));
                        image.Save($"{tbxPath.Text}\\{student.Ic}.jpg", ImageFormat.Jpeg);
                    }
                    _form.WriteInfo($"--> {student.Ic}.jpg");
                    progressBarExport.Value = counter;
                    counter++;
                }
                catch (Exception ex)
                {
                    _form.WriteError($"Error On saving the photo with IC number: {student.Ic} {ex.Message}");
                }
            }
            _form.WriteSuccess("Done.");
            Process.Start("explorer.exe", tbxPath.Text);
        }

        private void rtbxResult_TextChanged(object sender, EventArgs e)
        {
            rtbxResult.SelectionStart = rtbxResult.Text.Length;
            rtbxResult.ScrollToCaret();
        }
        internal void SetAfterLoginAppearance()
        {

            tbxUser.Enabled = false;
            tbxPass.Enabled = false;
            btnLogin.Enabled = false;
            tbxUrl.Enabled = false;
            tbxDmsUrl.Enabled = false;
            btnBrowse.Enabled = true;
        }
        internal void SetBeforeLoginAppearance()
        {
            tbxUser.Enabled = true;
            tbxPass.Enabled = true;
            btnLogin.Enabled = true;
        }
        internal string GetBaseUrl()
        {
            return tbxUrl.Text;
        }
        internal void HideLoading()
        {
            pbLoading.Visible = false;
        }
        internal void ShowLoading()
        {
            pbLoading.Visible = true;
        }
    }
}