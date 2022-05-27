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
    public partial class FrmChonVatTu : Form
    {
        public FrmChonVatTu()
        {
            InitializeComponent();
        }

        private void vattuBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsVatTu.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet);

        }

        private void FormChonVatTu_Load(object sender, EventArgs e)
        {
            /*không kiểm tra khóa ngoại nữa*/
            dataSet.EnforceConstraints = false;
            this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
            this.vattuTableAdapter.Fill(this.dataSet.Vattu);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string maVatTu = ((DataRowView)bdsVatTu.Current)["MAVT"].ToString();
            int soLuongVatTu = int.Parse( ((DataRowView)bdsVatTu.Current)["SOLUONGTON"].ToString() );

            /*FIX LỖI CHỈ CHỌN ĐƯỢC MAVATTU ĐANG EDIT KHI EDIT CTPX P3*/
            if (Program.MaVatTuDangCoOCTPX.Trim() != maVatTu.Trim() &&
                Program.dangThemMoiPhieuXuat == false &&
                Program.MaVatTuDangCoOCTPX.Trim() != null &&
                Program.MaVatTuDangCoOCTPX.Trim() != "")
            {
                MessageBox.Show("Bạn phải chọn Vật tư có mã vật tư là :  " + Program.MaVatTuDangCoOCTPX, "Thông báo", MessageBoxButtons.OK);
                return;
            }

            if (Program.dangThemMoiPhieuXuat == true)
            {
                /*FIX LỖI : KIỂM TRA CHITIETPHIEUXUAT ĐÃ TỒN TẠI HAY CHƯA?*/
                String cauTruyVan =
                        "DECLARE @result int " +
                        "EXEC @result = SP_KiemTraChiTietPhieuNhapPhieuXuatDaTonTaiHayChua XUAT, @MAPHIEU = '" + Program.maPhieuXuatDuocChon + "' , @MAVT = '" + maVatTu + "' " +
                        "SELECT 'Value' = @result";
                SqlCommand sqlCommand = new SqlCommand(cauTruyVan, Program.conn);
                //MessageBox.Show("cauTruyVan : " + cauTruyVan, "Thông báo", MessageBoxButtons.OK);
                try
                {
                    Program.myReader = Program.ExecSqlDataReader(cauTruyVan);
                    /*khong co ket qua tra ve thi ket thuc luon*/
                    if (Program.myReader == null)
                    {
                        MessageBox.Show("ok, KO BÁO LỖI", "Thông báo", MessageBoxButtons.OK);
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
                //MessageBox.Show("result = " + result, "Thông báo",
                //            MessageBoxButtons.OK, MessageBoxIcon.Error);
                Program.myReader.Close();

                /*Step continue*/
                /* int viTriConTro = bdsPhieuNhap.Position; //đang nhập vô
                 int viTriMaPhieuNhap = bdsPhieuNhap.Find("MAPN", maPhieuNhap); //đã tồn tại*/

                /*Dang them moi phieu nhap*/
                if (result == 1)
                {
                    MessageBox.Show("Vật tư này đã được tạo trong Chi tiết phiếu xuất này rồi !", "Thông báo", MessageBoxButtons.OK);
                    // txtMaPhieuNhap.Focus();
                    return;
                }

            }



            Program.maVatTuDuocChon = maVatTu;
            Program.soLuongVatTu = soLuongVatTu;

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.maVatTuDuocChon = "";
            Program.soLuongVatTu = 0;
            Program.donGia = 0;
            this.Dispose();
        }

        private void vattuGridControl_Click(object sender, EventArgs e)
        {

        }
    }
}
