using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace QLTVT.ReportForm
{
    public partial class frmTongHopNhapXuat : Form
    {
        public frmTongHopNhapXuat()
        {
            InitializeComponent();
        }

        private void FormTongHopNhapXuat_Load(object sender, EventArgs e)
        {
            /*Step 2*/
            cmbCHINHANH.DataSource = Program.bindingSource;/*sao chep bingding source tu form dang nhap*/
            cmbCHINHANH.DisplayMember = "TENCN";
            cmbCHINHANH.ValueMember = "TENSERVER";
            cmbCHINHANH.SelectedIndex = Program.brand;

            this.dteTuNgay.EditValue = "01-01-2020";
            DateTime temp = DateTime.Today.Date;
            this.dteToiNgay.EditValue = temp;

            if (Program.role == "CONGTY")
            {
                cmbCHINHANH.Enabled = true;
            }    
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

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime fromDate = (DateTime)dteTuNgay.DateTime;
            DateTime toDate = (DateTime)dteToiNgay.DateTime;
            string chiNhanh = cmbCHINHANH.SelectedValue.ToString().Contains("1") ? "CN1 - Quận 1" : "CN2 - Quận 9";

            String hoTenNguoiLapPhieu = Program.staff;
            String ngayHienTai = DateTime.Today.Day.ToString();
            String thangHienTai = DateTime.Today.Month.ToString();
            String namHienTai = DateTime.Today.Year.ToString();


            if (fromDate > toDate)
            {
                MessageBox.Show("fromDate không được lớn hơn toDate! Chọn lại toDate",
                "Thông báo", MessageBoxButtons.OK);
                return;
            }


            RP_TongHopNhapXuat report = new RP_TongHopNhapXuat(fromDate, toDate);
            report.txtTuNgay.Text = fromDate.ToString("dd-MM-yyyy");
            report.txtToiNgay.Text = toDate.ToString("dd-MM-yyyy");
            report.txtChiNhanh.Text = chiNhanh;

            report.txtHoTen.Text = hoTenNguoiLapPhieu;
            report.txtNgay.Text = ngayHienTai;
            report.txtThang.Text = thangHienTai;
            report.txtNam.Text = namHienTai;

            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreviewDialog();
        }

        private void btnTHOAT_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fromDate = (DateTime)dteTuNgay.DateTime;
                DateTime toDate = (DateTime)dteToiNgay.DateTime;
                string chiNhanh = cmbCHINHANH.SelectedValue.ToString().Contains("1") ? "CN1 - Quận 1" : "CN2 - Quận 9";

                String hoTenNguoiLapPhieu = Program.staff;
                String ngayHienTai = DateTime.Today.Day.ToString();
                String thangHienTai = DateTime.Today.Month.ToString();
                String namHienTai = DateTime.Today.Year.ToString();


                if (fromDate > toDate)
                {
                    MessageBox.Show("fromDate không được lớn hơn toDate! Chọn lại toDate",
                    "Thông báo", MessageBoxButtons.OK);
                    return;
                }


                RP_TongHopNhapXuat report = new RP_TongHopNhapXuat(fromDate, toDate);
                report.txtTuNgay.Text = fromDate.ToString("dd-MM-yyyy");
                report.txtToiNgay.Text = toDate.ToString("dd-MM-yyyy");
                report.txtChiNhanh.Text = chiNhanh;

                report.txtHoTen.Text = hoTenNguoiLapPhieu;
                report.txtNgay.Text = ngayHienTai;
                report.txtThang.Text = thangHienTai;
                report.txtNam.Text = namHienTai;


                if (File.Exists(@"E:\RP_TongHopNhapXuat.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File RP_TongHopNhapXuat.pdf tại ổ E đã có!\nBạn có muốn tạo lại?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        report.ExportToPdf(@"E:\RP_TongHopNhapXuat.pdf");
                        MessageBox.Show("File RP_TongHopNhapXuat.pdf đã được ghi thành công tại ổ E",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    report.ExportToPdf(@"E:\RP_TongHopNhapXuat.pdf");
                    MessageBox.Show("File RP_TongHopNhapXuat.pdf đã được ghi thành công tại ổ E",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Vui lòng đóng file RP_TongHopNhapXuat.pdf",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}
