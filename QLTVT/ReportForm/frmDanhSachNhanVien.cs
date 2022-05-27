using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace QLTVT.ReportForm
{
    public partial class frmDanhSachNhanVien : Form
    {

        private string chiNhanh = "";

        public frmDanhSachNhanVien()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String hoTenNguoiLapPhieu = Program.staff;
            String ngayHienTai = DateTime.Today.Day.ToString();
            String thangHienTai = DateTime.Today.Month.ToString();
            String namHienTai = DateTime.Today.Year.ToString();

            chiNhanh = cmbCHINHANH.SelectedValue.ToString().Contains("1") ? "CN1 - Quận 1" : "CN2 - Quận 9";
            RP_DanhSachNhanVien report = new RP_DanhSachNhanVien();
            /*GAN TEN CHI NHANH CHO BAO CAO*/
            report.txtChiNhanh.Text = chiNhanh.ToUpper();

            report.txtHoTen.Text = hoTenNguoiLapPhieu;
            report.txtNgay.Text = ngayHienTai;
            report.txtThang.Text = thangHienTai;
            report.txtNam.Text = namHienTai;


            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreviewDialog();
        }
        // BUTON XUẤT BẢN
        private void button2_Click(object sender, EventArgs e)
        {
            String hoTenNguoiLapPhieu = Program.staff;
            String ngayHienTai = DateTime.Today.Day.ToString();
            String thangHienTai = DateTime.Today.Month.ToString();
            String namHienTai = DateTime.Today.Year.ToString();
            try
            {
                RP_DanhSachNhanVien report = new RP_DanhSachNhanVien();
                /*GAN TEN CHI NHANH CHO BAO CAO*/
                report.txtChiNhanh.Text = chiNhanh.ToUpper();
                report.txtHoTen.Text = hoTenNguoiLapPhieu;
                report.txtNgay.Text = ngayHienTai;
                report.txtThang.Text = thangHienTai;
                report.txtNam.Text = namHienTai;
                if (File.Exists(@"E:\RP_DanhSachNhanVien.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File RP_DanhSachNhanVien.pdf tại ổ D đã có!\nBạn có muốn tạo lại?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        report.ExportToPdf(@"E:\RP_DanhSachNhanVien.pdf");
                        MessageBox.Show("File RP_DanhSachNhanVien.pdf đã được ghi thành công tại ổ D",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    report.ExportToPdf(@"E:\RP_DanhSachNhanVien.pdf");
                    MessageBox.Show("File RP_DanhSachNhanVien.pdf đã được ghi thành công tại ổ D",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Vui lòng đóng file RP_DanhSachNhanVien.pdf",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
        }
        private void FormDanhSachNhanVien_Load(object sender, EventArgs e)
        {
            dataSet.EnforceConstraints = false;
            this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.nhanVienTableAdapter.Fill(this.dataSet.NhanVien);


            /*Step 2*/
            cmbCHINHANH.DataSource = Program.bindingSource;
            cmbCHINHANH.DisplayMember = "TENCN";
            cmbCHINHANH.ValueMember = "TENSERVER";
            cmbCHINHANH.SelectedIndex = Program.brand;
            if (Program.role == "CONGTY")
            {
                cmbCHINHANH.Enabled = true;
            }
            else cmbCHINHANH.Enabled = false;
        }
        private void cmbCHINHANH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCHINHANH.SelectedValue.ToString() == "System.Data.DataRowView")
                return;
            Program.serverName = cmbCHINHANH.SelectedValue.ToString();
            if (cmbCHINHANH.SelectedIndex != Program.brand) //brand tên chi nhánh đang đăng nhập
            {
                Program.loginName = Program.remoteLogin;
                Program.loginPassword = Program.remotePassword;
            }
            /*Neu chon trung voi chi nhanh dang dang nhap o formDangNhap*/
            else
            {
                Program.loginName = Program.currentLogin;
                Program.loginPassword = Program.currentPassword;
            }
            if (Program.KetNoi() == 0)
            {
                MessageBox.Show("Xảy ra lỗi kết nối với chi nhánh hiện tại", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                /*Do du lieu tu dataSet vao grid Control*/
                this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                this.nhanVienTableAdapter.Fill(this.dataSet.NhanVien);
            }
            if (Program.KetNoi() == 0)
            {
                MessageBox.Show("Xảy ra lỗi kết nối với chi nhánh hiện tại", "Thông báo", MessageBoxButtons.OK);
            }
        }
        private void nhanVienBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.nhanVienBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet);
        }
        private void btnTHOAT_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void nhanVienGridControl_Click(object sender, EventArgs e)
        {

        }
    }
}