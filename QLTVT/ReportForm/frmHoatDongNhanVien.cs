using DevExpress.XtraReports.UI;
using QLTVT.SubForm;
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
    public partial class frmHoatDongNhanVien : Form
    {
        public frmHoatDongNhanVien()
        {
            InitializeComponent();
        }

        private void btnChonNhanVien_Click(object sender, EventArgs e)
        {
            FrmChonNhanVien form = new FrmChonNhanVien();
            form.ShowDialog();

            txtMaNhanVien.Text = Program.maNhanVienDuocChon;
            txtHoVaTen.Text = Program.hoTen;

            txtNgaySinh.Text = Program.ngaySinh;
            txtDiaChi.Text = Program.diaChi;

        }

        private void button1_Click(object sender, EventArgs e)
        {

            /*FIX LỖI PHƯA CHỌN MÃ NHÂN VIÊN */
            string maNhanVien = "";
            if(txtMaNhanVien.Text == "")
            {
                MessageBox.Show("Vui lòng chọn nhân viên!",
               "Thông báo", MessageBoxButtons.OK);
                return;
            }
            maNhanVien = txtMaNhanVien.Text;
            string loaiPhieu = (cmbLoaiPhieu.SelectedItem.ToString() == "NHAP") ? "NHAP" : "XUAT";
            DateTime fromDate = dteTuNgay.DateTime;
            DateTime toDate = dteToiNgay.DateTime;

            /*FIX LỖI fromDate không được lớn hơn toDate*/
            if (fromDate > toDate)
            {
                MessageBox.Show("fromDate không được lớn hơn toDate! Chọn lại toDate",
                "Thông báo", MessageBoxButtons.OK);
                return;
            }


            String hoTenNguoiLapPhieu = Program.staff;
            String ngayHienTai = DateTime.Today.Day.ToString();
            String thangHienTai = DateTime.Today.Month.ToString();
            String namHienTai = DateTime.Today.Year.ToString();

            

            /*
            int fromYear = dteTuNgay.DateTime.Year;
            int fromMonth = dteTuNgay.DateTime.Month;
            int toYear = dteToiNgay.DateTime.Year;
            int toMonth = dteToiNgay.DateTime.Month;
            */
            RP_HoatDongNhanVien report = new RP_HoatDongNhanVien(maNhanVien, loaiPhieu, fromDate, toDate);
            /*GAN TEN CHI NHANH CHO BAO CAO*/
            //report.txtLoaiPhieu.Text = cmbLoaiPhieu.SelectedItem.ToString().ToUpper();
            if (cmbLoaiPhieu.SelectedItem.ToString().ToUpper() == "NHAP") report.txtLoaiPhieu.Text = "PHIẾU NHẬP";
            if (cmbLoaiPhieu.SelectedItem.ToString().ToUpper() == "XUAT") report.txtLoaiPhieu.Text = "PHIẾU XUẤT";
            report.txtMaNhanVien.Text = Program.maNhanVienDuocChon;
            report.txtHoTen.Text = Program.hoTen;
            report.txtNgaySinh.Text = Program.ngaySinh;
            report.txtDiaChi.Text = Program.diaChi;
            //report.txtTuNgay.Text = dteTuNgay.EditValue.ToString();
            report.txtToiNgay.Text = dteToiNgay.EditValue.ToString();
            report.txtTuNgay.Text = fromDate.ToString("dd-MM-yyyy");
            report.txtToiNgay.Text = toDate.ToString("dd-MM-yyyy");

            report.xrLabel13.Text = hoTenNguoiLapPhieu;
            report.txtNgay.Text = ngayHienTai;
            report.txtThang.Text = thangHienTai;
            report.txtNam.Text = namHienTai;

            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreviewDialog();
        }

        private void FormHoatDongNhanVien_Load(object sender, EventArgs e)
        {
            cmbLoaiPhieu.SelectedIndex = 1;
            this.dteTuNgay.EditValue = "01-01-2020";
            DateTime temp = DateTime.Today.Date;
            this.dteToiNgay.EditValue = temp;
        }

        /***********************************************************
         * Step 1: lay cac du lieu can thiet
         * Step 2: gan cac du lieu nay vao bao cao
         * Step 3: xuat ban
         ***********************************************************/
        private void button2_Click(object sender, EventArgs e)
        {
            String hoTenNguoiLapPhieu = Program.staff;
            String ngayHienTai = DateTime.Today.Day.ToString();
            String thangHienTai = DateTime.Today.Month.ToString();
            String namHienTai = DateTime.Today.Year.ToString();
            try
            {
                string maNhanVien = txtMaNhanVien.Text;
                string loaiPhieu = (cmbLoaiPhieu.SelectedItem.ToString() == "NHAP") ? "NHAP" : "XUAT";

                DateTime fromDate = dteTuNgay.DateTime;
                DateTime toDate = dteToiNgay.DateTime;
                /*
                int fromYear = dteTuNgay.DateTime.Year;
                int fromMonth = dteTuNgay.DateTime.Month;
                int toYear = dteToiNgay.DateTime.Year;
                int toMonth = dteToiNgay.DateTime.Month;
                */
                RP_HoatDongNhanVien report = new RP_HoatDongNhanVien(maNhanVien, loaiPhieu, fromDate, toDate);

                report.txtLoaiPhieu.Text = cmbLoaiPhieu.SelectedItem.ToString().ToUpper();
                report.txtMaNhanVien.Text = Program.maNhanVienDuocChon;
                report.txtHoTen.Text = Program.hoTen;
                report.txtNgaySinh.Text = Program.ngaySinh;
                report.txtDiaChi.Text = Program.diaChi;
                report.txtTuNgay.Text = fromDate.ToString("dd-MM-yyyy");
                report.txtToiNgay.Text = toDate.ToString("dd-MM-yyyy");

                report.xrLabel13.Text = hoTenNguoiLapPhieu;
                report.txtNgay.Text = ngayHienTai;
                report.txtThang.Text = thangHienTai;
                report.txtNam.Text = namHienTai;

                if (File.Exists(@"E:\RP_HoatDongNhanVien.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File RP_HoatDongNhanVien.pdf tại ổ D đã có!\nBạn có muốn tạo lại?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        report.ExportToPdf(@"E:\RP_HoatDongNhanVien.pdf");
                        MessageBox.Show("File RP_HoatDongNhanVien.pdf đã được ghi thành công tại ổ D",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    report.ExportToPdf(@"E:\RP_HoatDongNhanVien.pdf");
                    MessageBox.Show("File RP_HoatDongNhanVien.pdf đã được ghi thành công tại ổ D",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Vui lòng đóng file RP_HoatDongNhanVien.pdf",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnTHOAT_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
