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
using System.Data.SqlClient;// đã thêm
namespace QLTVT.ReportForm
{
    public partial class frmDanhSachVatTu : Form
    {



        private SqlConnection connPublisher = new SqlConnection(); // đã thêm
        public frmDanhSachVatTu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String hoTenNguoiLapPhieu = Program.staff;
            String ngayHienTai = DateTime.Today.Day.ToString();
            String thangHienTai = DateTime.Today.Month.ToString();
            String namHienTai = DateTime.Today.Year.ToString();

            RP_DanhSachVatTu report = new RP_DanhSachVatTu();
            report.txtHoTen.Text = hoTenNguoiLapPhieu;
            report.txtNgay.Text = ngayHienTai;
            report.txtThang.Text = thangHienTai;
            report.txtNam.Text = namHienTai;

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

                RP_DanhSachVatTu report = new RP_DanhSachVatTu();
                report.txtHoTen.Text = hoTenNguoiLapPhieu;
                report.txtNgay.Text = ngayHienTai;
                report.txtThang.Text = thangHienTai;
                report.txtNam.Text = namHienTai;

                if (File.Exists(@"E:\RP_DanhSachVatTu.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File RP_DanhSachVatTu.pdf tại ổ D đã có!\nBạn có muốn tạo lại?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        report.ExportToPdf(@"E:\RP_DanhSachVatTu.pdf");
                        MessageBox.Show("File RP_DanhSachVatTu.pdf đã được ghi thành công tại ổ D",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    report.ExportToPdf(@"E:\RP_DanhSachVatTu.pdf");
                    MessageBox.Show("File RP_DanhSachVatTu.pdf đã được ghi thành công tại ổ D",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Vui lòng đóng file RP_DanhSachVatTu.pdf",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
        }

        private void vattuBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.vattuBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet);

        }

        private void FormDanhSachVatTu_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet.Vattu' table. You can move, or remove it, as needed.
            //this.vattuTableAdapter.Fill(this.dataSet.Vattu);
            //fix lỗi k đổ dữ liệu vào table đc đã sửa thêm 2 dòng cuối
            this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
            this.vattuTableAdapter.Fill(this.dataSet.Vattu);



        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}