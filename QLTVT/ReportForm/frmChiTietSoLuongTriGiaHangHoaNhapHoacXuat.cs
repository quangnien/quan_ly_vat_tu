using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLTVT.ReportForm
{
    public partial class frmChiTietSoLuongTriGiaHangHoaNhapHoacXuat : Form
    {
        private string chiNhanh = "";
        public frmChiTietSoLuongTriGiaHangHoaNhapHoacXuat()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string vaiTro = Program.role;
            string loaiPhieu = (cmbLoaiPhieu.SelectedItem.ToString() == "NHAP") ? "NHAP" : "XUAT";

            String hoTenNguoiLapPhieu = Program.staff;
            String ngayHienTai = DateTime.Today.Day.ToString();
            String thangHienTai = DateTime.Today.Month.ToString();
            String namHienTai = DateTime.Today.Year.ToString();

            DateTime fromDate = dteTuNgay.DateTime;
            DateTime toDate = dteToiNgay.DateTime;
            /*FIX LỖI fromDate không được lớn hơn toDate*/
            if (fromDate > toDate)
            {
                MessageBox.Show("fromDate không được lớn hơn toDate! Chọn lại toDate",
                "Thông báo", MessageBoxButtons.OK);
                return;
            }
            
            RP_ChiTietSoLuongTriGiaHangNhapHoacXuat report = new RP_ChiTietSoLuongTriGiaHangNhapHoacXuat(vaiTro,loaiPhieu,fromDate,toDate);
            /*GAN TEN CHI NHANH CHO BAO CAO*/
            /*GAN TEN CHI NHANH CHO BAO CAO*/
            if (cmbLoaiPhieu.SelectedItem.ToString().ToUpper() == "NHAP") report.txtLoaiPhieu.Text = "PHIẾU NHẬP";
            if (cmbLoaiPhieu.SelectedItem.ToString().ToUpper() == "XUAT") report.txtLoaiPhieu.Text = "PHIẾU XUẤT";
            //report.txtLoaiPhieu.Text = cmbLoaiPhieu.SelectedItem.ToString().ToUpper();
            report.txtTuNgay.Text = fromDate.ToString("dd-MM-yyyy");
            report.txtToiNgay.Text = toDate.ToString("dd-MM-yyyy");

            report.txtHoTen.Text = hoTenNguoiLapPhieu;
            report.txtNgay.Text = ngayHienTai;
            report.txtThang.Text = thangHienTai;
            report.txtNam.Text = namHienTai;

            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreviewDialog();
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string vaiTro = Program.role;
            string loaiPhieu = (cmbLoaiPhieu.SelectedItem.ToString() == "NHAP") ? "NHAP" : "XUAT";
            String hoTenNguoiLapPhieu = Program.staff;
            String ngayHienTai = DateTime.Today.Day.ToString();
            String thangHienTai = DateTime.Today.Month.ToString();
            String namHienTai = DateTime.Today.Year.ToString();
            try
            {

                DateTime fromDate = dteTuNgay.DateTime;
                DateTime toDate = dteToiNgay.DateTime;



                RP_ChiTietSoLuongTriGiaHangNhapHoacXuat report = new RP_ChiTietSoLuongTriGiaHangNhapHoacXuat(vaiTro, loaiPhieu, fromDate, toDate);


                /*GAN TEN CHI NHANH CHO BAO CAO*/
                if (cmbLoaiPhieu.SelectedItem.ToString().ToUpper() == "NHAP") report.txtLoaiPhieu.Text = "PHIẾU NHẬP";
                if (cmbLoaiPhieu.SelectedItem.ToString().ToUpper() == "XUAT") report.txtLoaiPhieu.Text = "PHIẾU XUẤT";
               // report.txtLoaiPhieu.Text = cmbLoaiPhieu.SelectedItem.ToString().ToUpper();
                report.txtTuNgay.Text = fromDate.ToString("dd-MM-yyyy");
                report.txtToiNgay.Text = toDate.ToString("dd-MM-yyyy");

                report.txtHoTen.Text = hoTenNguoiLapPhieu;
                report.txtNgay.Text = ngayHienTai;
                report.txtThang.Text = thangHienTai;
                report.txtNam.Text = namHienTai;



                if (File.Exists(@"E:\RP_ChiTietSoLuongTriGiaHangNhapHoacXuat.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File RP_ChiTietSoLuongTriGiaHangNhapHoacXuat.pdf tại ổ D đã có!\nBạn có muốn tạo lại?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        report.ExportToPdf(@"E:\RP_ChiTietSoLuongTriGiaHangNhapHoacXuat.pdf");
                        MessageBox.Show("File RP_ChiTietSoLuongTriGiaHangNhapHoacXuat.pdf đã được ghi thành công tại ổ D",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    report.ExportToPdf(@"E:\RP_ChiTietSoLuongTriGiaHangNhapHoacXuat.pdf");
                    MessageBox.Show("File RP_ChiTietSoLuongTriGiaHangNhapHoacXuat.pdf đã được ghi thành công tại ổ D",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Vui lòng đóng file RP_ChiTietSoLuongTriGiaHangNhapHoacXuat.pdf",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
        }

        private void FormChiTietSoLuongTriGiaHangHoaNhapXuat_Load(object sender, EventArgs e)
        {
            this.cmbLoaiPhieu.SelectedIndex = 1;
            this.dteTuNgay.EditValue = "01/01/2020";
            DateTime temp = DateTime.Today.Date;
            this.dteToiNgay.EditValue = temp;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
