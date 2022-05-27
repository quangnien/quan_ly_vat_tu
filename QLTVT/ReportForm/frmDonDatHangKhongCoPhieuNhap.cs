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
    public partial class frmDonDatHangKhongCoPhieuNhap : Form
    {
        private SqlConnection connPublisher = new SqlConnection();
        private string chiNhanh = "";

        private int KetNoiDatabaseGoc()
        {
            if (connPublisher != null && connPublisher.State == ConnectionState.Open)
                connPublisher.Close();
            try
            {
                connPublisher.ConnectionString = Program.connstrPublisher;
                connPublisher.Open();
                return 1;
            }

            catch (Exception e)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu.\nBạn xem lại user name và password.\n " + e.Message, "", MessageBoxButtons.OK);
                return 0;
            }
        }
        public frmDonDatHangKhongCoPhieuNhap()
        {
            InitializeComponent();
        }
        private void cmbCHINHANH_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
             /*Neu combobox khong co so lieu thi ket thuc luon*/
            if (cmbCHINHANH.SelectedValue.ToString() == "System.Data.DataRowView")
                return;

            Program.serverName = cmbCHINHANH.SelectedValue.ToString();

            /*Neu chon sang chi nhanh khac voi chi nhanh hien tai*/
            if (cmbCHINHANH.SelectedIndex != Program.brand)
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
        }
        private void FormDonHangKhongPhieuNhap_Load(object sender, EventArgs e)
        {

            /*Step 2*/
            cmbCHINHANH.DataSource = Program.bindingSource;/*sao chep bingding source tu form dang nhap*/
            cmbCHINHANH.DisplayMember = "TENCN";
            cmbCHINHANH.ValueMember = "TENSERVER";
            cmbCHINHANH.SelectedIndex = Program.brand;
            if (Program.role == "CONGTY")
            {
                cmbCHINHANH.Enabled = true;
            }
            else cmbCHINHANH.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String hoTenNguoiLapPhieu = Program.staff;
            String ngayHienTai = DateTime.Today.Day.ToString();
            String thangHienTai = DateTime.Today.Month.ToString();
            String namHienTai = DateTime.Today.Year.ToString();

            chiNhanh = cmbCHINHANH.SelectedValue.ToString().Contains("1") ? "CN1 - Quận 1" : "CN2 - Quận 9";
            RP_DonHangKhongPhieuNhap report = new RP_DonHangKhongPhieuNhap();
            report.txtChiNhanh.Text = chiNhanh.ToUpper();
            report.txtHoTen.Text = hoTenNguoiLapPhieu;
            report.txtNgay.Text = ngayHienTai;
            report.txtThang.Text = thangHienTai;
            report.txtNam.Text = namHienTai;
            /*GAN TEN CHI NHANH CHO BAO CAO*/
            report.txtChiNhanh.Text = chiNhanh.ToUpper();
            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreviewDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String hoTenNguoiLapPhieu = Program.staff;
            String ngayHienTai = DateTime.Today.Day.ToString();
            String thangHienTai = DateTime.Today.Month.ToString();
            String namHienTai = DateTime.Today.Year.ToString();
            try
            {
                RP_DonHangKhongPhieuNhap report = new RP_DonHangKhongPhieuNhap();
                /*GAN TEN CHI NHANH CHO BAO CAO*/
                report.txtChiNhanh.Text = chiNhanh.ToUpper();
                report.txtHoTen.Text = hoTenNguoiLapPhieu;
                report.txtNgay.Text = ngayHienTai;
                report.txtThang.Text = thangHienTai;
                report.txtNam.Text = namHienTai;
                if (File.Exists(@"E:\RP_DonHangKhongPhieuNhap.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File RP_DonHangKhongPhieuNhap.pdf tại ổ D đã có!\nBạn có muốn tạo lại?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        report.ExportToPdf(@"E:\RP_DonHangKhongPhieuNhap.pdf");
                        MessageBox.Show("File RP_DonHangKhongPhieuNhap.pdf đã được ghi thành công tại ổ D",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    report.ExportToPdf(@"E:\RP_DonHangKhongPhieuNhap.pdf");
                    MessageBox.Show("File RP_DonHangKhongPhieuNhap.pdf đã được ghi thành công tại ổ D",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Vui lòng đóng file RP_DonHangKhongPhieuNhap.pdf",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
        }

        private void btnTHOAT_Click(object sender, EventArgs e) { this.Close(); }
    }
}