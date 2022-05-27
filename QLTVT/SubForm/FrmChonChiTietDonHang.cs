using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QLTVT.SubForm
{
    public partial class FrmChonChiTietDonHang : Form
    {
        public FrmChonChiTietDonHang()
        {
            InitializeComponent();
        }

        private void cTDDHBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsChiTietDonHang.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet);

        }

        private void FormChonChiTietDonHang_Load(object sender, EventArgs e)
        {
            dataSet.EnforceConstraints = false;
            this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTDDHTableAdapter.Fill(this.dataSet.CTDDH);

        }

        private void btnCHON_Click(object sender, EventArgs e)
        {

            DataRowView drv = ((DataRowView)(bdsChiTietDonHang.Current));
            string maDonHang = drv["MasoDDH"].ToString().Trim();
            string maVatTu = drv["MaVT"].ToString().Trim();
            int soLuong = int.Parse( drv["SOLUONG"].ToString().Trim() );
            int donGia = int.Parse( drv["DONGIA"].ToString().Trim());


            /*FIX LỖI CHỈ CHỌN ĐƯỢC MAVATTU ĐANG EDIT KHI EDIT CTPN P3*/
            if (Program.MaVatTuDangCoOCTPN.Trim() != maVatTu.Trim() &&
                Program.dangThemMoiPhieuNhap == false &&
                Program.MaVatTuDangCoOCTPN.Trim() != null &&
                Program.MaVatTuDangCoOCTPN.Trim() != "")
            {
                MessageBox.Show("Bạn phải chọn Vật tư có mã vật tư là :  " + Program.MaVatTuDangCoOCTPN, "Thông báo", MessageBoxButtons.OK);
                return;
            }

            /*Kiem tra xem ma don hang cua gcPhieuNhap co trung voi ma don hang duoc chon hay khong ?*/
            Program.maDonDatHangDuocChonChiTiet = maDonHang;
            if( Program.maDonDatHangDuocChon != Program.maDonDatHangDuocChonChiTiet)
            {
                MessageBox.Show("Bạn phải chọn chi tiết đơn hàng có mã đơn hàng là " + Program.maDonDatHangDuocChon, "Thông báo",MessageBoxButtons.OK);
                return;
            }

            if (Program.dangThemMoiPhieuNhap == true)
            {
                /*FIX LỖI : KIỂM TRA CHITIETPHIEUNHAP ĐÃ TỒN TẠI HAY CHƯA?*/
                String cauTruyVan =
                        "DECLARE @result int " +
                        "EXEC @result = SP_KiemTraChiTietPhieuNhapPhieuXuatDaTonTaiHayChua NHAP, @MAPHIEU = '" + Program.maPhieuNhapDuocChon + "' , @MAVT = '" + maVatTu + "' " + 
                        "SELECT 'Value' = @result";
                SqlCommand sqlCommand = new SqlCommand(cauTruyVan, Program.conn);
                try
                {
                    Program.myReader = Program.ExecSqlDataReader(cauTruyVan);
                    if (Program.myReader == null)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Thực thi database thất bại!\n\n" + ex.Message, "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(ex.Message);
                    return;
                }
                Program.myReader.Read();
                int result = int.Parse(Program.myReader.GetValue(0).ToString());
                Program.myReader.Close();

                if (result == 1)
                {
                    MessageBox.Show("Vật tư này đã được tạo trong Chi tiết phiếu nhập này rồi !", "Thông báo", MessageBoxButtons.OK);
                   // txtMaPhieuNhap.Focus();
                    return;
                }
            }

            Program.maVatTuDuocChon = maVatTu;
            Program.soLuongVatTu = soLuong;
            Program.donGia = donGia;
            this.Close();
        }

        private void btnTHOAT_Click(object sender, EventArgs e)
        {
            Program.maVatTuDuocChon = "";
            Program.soLuongVatTu = 0;
            Program.donGia = 0;
            this.Dispose();
        }

        private void cTDDHGridControl_Click(object sender, EventArgs e)
        {

        }
    }
}
