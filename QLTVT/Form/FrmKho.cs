using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLTVT
{
    public partial class FormKho : Form
    {
        int viTri = 0;
        bool dangThemMoi = false;

        String maChiNhanh = "";
        Stack undoList = new Stack(); // chứa các thao tác để hoàn tác

        public FormKho()
        {
            InitializeComponent();
        }

        private void FormKho_Load(object sender, EventArgs e)
        {
            /*B1*/
            /*không kiểm tra khóa ngoại nữa*/
            /*Vì vd trong DATHANG có 2 khóa ngoại là MAKHO và MANV, thì khi mà ta tải 
             đơn DATHANG vô mà đã tải MANV vô trước rồi thì OK, còn MAKHO chưa có
             -> bão lỗi. Mà form này chỉ nhập NHANVIEN, đâu lq tới KHO làm gì chả nhẽ
             bh lại tải KHO nữa thì mất công => KO KIỀM TRA RÀNG BUỘC KHÓA NGOẠI NỮA.*/
            dataSet.EnforceConstraints = false;

            this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
            this.datHangTableAdapter.Fill(this.dataSet.DatHang);

            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuNhapTableAdapter.Fill(this.dataSet.PhieuNhap);

            this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuXuatTableAdapter.Fill(this.dataSet.PhieuXuat);

            this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
            this.khoTableAdapter.Fill(this.dataSet.Kho);

            maChiNhanh = ((DataRowView)bdsKho[0])["MACN"].ToString();

            cmbCHINHANH.DataSource = Program.bindingSource;
            cmbCHINHANH.DisplayMember = "TENCN";
            cmbCHINHANH.ValueMember = "TENSERVER";
            cmbCHINHANH.SelectedIndex = Program.brand;

            /*B2*/
            if (Program.role == "CONGTY")
            {
                cmbCHINHANH.Enabled = true;

                this.btnTHEM.Enabled = false;
                this.btnXOA.Enabled = false;
                this.btnGHI.Enabled = false;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnCHUYENCHINHANH.Enabled = true;
                this.btnTHOAT.Enabled = true;

                this.panelNhapLieu.Enabled = false;
            }

            if (Program.role == "CHINHANH" || Program.role == "USER")
            {
                cmbCHINHANH.Enabled = false;

                this.btnTHEM.Enabled = true;
                this.btnXOA.Enabled = true;
                this.btnGHI.Enabled = true;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnTHOAT.Enabled = true;

                this.panelNhapLieu.Enabled = true;
                this.txtMAKHO.Enabled = false;
            }

        }

        private void cmbCHINHANH_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*Neu combobox khong co so lieu thi ket thuc luon*/
            if (cmbCHINHANH.SelectedValue.ToString() == "System.Data.DataRowView")
                return;

            Program.serverName = cmbCHINHANH.SelectedValue.ToString();

            if (cmbCHINHANH.SelectedIndex != Program.brand)
            {
                Program.loginName = Program.remoteLogin;
                Program.loginPassword = Program.remotePassword;
            }
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
                this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
                this.datHangTableAdapter.Fill(this.dataSet.DatHang);

                this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuNhapTableAdapter.Fill(this.dataSet.PhieuNhap);

                this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuXuatTableAdapter.Fill(this.dataSet.PhieuXuat);

                this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
                this.khoTableAdapter.Fill(this.dataSet.Kho);
            }
        }

        private void btnTHEM_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            viTri = bdsKho.Position;
            this.panelNhapLieu.Enabled = true;
            dangThemMoi = true;

            bdsKho.AddNew();
            txtMACHINHANH.Text = maChiNhanh;

            this.txtMAKHO.Enabled = true;
            this.btnTHEM.Enabled = false;
            this.btnXOA.Enabled = false;
            this.btnGHI.Enabled = true;

            this.btnHOANTAC.Enabled = true;
            this.btnLAMMOI.Enabled = false;
            this.btnTHOAT.Enabled = false;

            this.gcKHO.Enabled = false;
            this.panelNhapLieu.Enabled = true;
        }

        private void btnLAMMOI_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.khoTableAdapter.Fill(this.dataSet.Kho);
                this.gcKHO.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Làm mới" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                return;
            }
        }

        private void btnTHOAT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private bool kiemTraDuLieuDauVao()
        {
            if (txtMAKHO.Text == "")
            {
                MessageBox.Show("Không bỏ trống mã kho hàng", "Thông báo", MessageBoxButtons.OK);
                txtMAKHO.Focus();
                return false;
            }
            if (Regex.IsMatch(txtMAKHO.Text.Trim(), @"^[a-zA-Z0-9]+$") == false)

            {
                MessageBox.Show("Mã kho chỉ chấp nhận chữ và số", "Thông báo", MessageBoxButtons.OK);
                txtMAKHO.Focus();
                return false;
            }

            if (txtMAKHO.Text.Length > 4)
            {
                MessageBox.Show("Mã kho không thể lớn hơn 4 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtMAKHO.Focus();
                return false;
            }
            if (txtTENKHO.Text == "")
            {
                MessageBox.Show("Không bỏ trống tên kho hàng", "Thông báo", MessageBoxButtons.OK);
                txtTENKHO.Focus();
                return false;
            }

            if (Regex.IsMatch(txtTENKHO.Text.Trim(), @"^[a-zA-Z0-9áàạảãâấầậẩẫăắằặẳẵÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴéèẹẻẽêếềệểễÉÈẸẺẼÊẾỀỆỂỄóòọỏõôốồộổỗơớờợởỡÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠúùụủũưứừựửữÚÙỤỦŨƯỨỪỰỬỮíìịỉĩÍÌỊỈĨđĐýỳỵỷỹÝỲỴỶỸ ]+$") == false)
            {
                MessageBox.Show("Mã kho chỉ chấp nhận chữ cái, số và khoảng trắng", "Thông báo", MessageBoxButtons.OK);
                txtTENKHO.Focus();
                return false;
            }

            if (txtTENKHO.Text.Length > 30)
            {
                MessageBox.Show("Tên kho không thể quá 30 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtTENKHO.Focus();
                return false;
            }
            if (txtDIACHI.Text == "")
            {
                MessageBox.Show("Không bỏ trống địa chỉ kho hàng", "Thông báo", MessageBoxButtons.OK);
                txtDIACHI.Focus();
                return false;
            }

            if (Regex.IsMatch(txtDIACHI.Text.Trim(), @"^[a-zA-Z0-9áàạảãâấầậẩẫăắằặẳẵÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴéèẹẻẽêếềệểễÉÈẸẺẼÊẾỀỆỂỄóòọỏõôốồộổỗơớờợởỡÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠúùụủũưứừựửữÚÙỤỦŨƯỨỪỰỬỮíìịỉĩÍÌỊỈĨđĐýỳỵỷỹÝỲỴỶỸ ]+$") == false)
            {
                MessageBox.Show("Địa chỉ chỉ gồm chữ cái, số và khoảng trắng", "Thông báo", MessageBoxButtons.OK);
                txtDIACHI.Focus();
                return false;
            }

            if (txtDIACHI.Text.Length > 100)
            {
                MessageBox.Show("Địa chỉ không quá 100 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtDIACHI.Focus();
                return false;
            }
            return true;
        }

        private void btnGHI_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bool ketQua = kiemTraDuLieuDauVao();
            if (ketQua == false)
                return;

            String maKhoHang = txtMAKHO.Text.Trim();
            DataRowView drv = ((DataRowView)bdsKho[bdsKho.Position]);
            String tenKhoHang = drv["TENKHO"].ToString();
            String diaChi = drv["DIACHI"].ToString();

            String cauTruyVan =
                    "DECLARE	@result int " +
                    "EXEC @result = sp_TimMaKho '" +
                    maKhoHang + "' " +
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
                MessageBox.Show("Thực thi Stored Procedure thất bại!\n\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
                return;
            }
            Program.myReader.Read();
            int result = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();

            int viTriConTro = bdsKho.Position;
            int viTriMaKhoHang = bdsKho.Find("MAKHO", txtMAKHO.Text);

            if (result == 1 && viTriConTro != viTriMaKhoHang)
            {
                MessageBox.Show("Mã kho hàng này đã được sử dụng !", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            else
            {
                DialogResult dr = MessageBox.Show("Bạn có chắc muốn ghi dữ liệu vào cơ sở dữ liệu ?", "Thông báo",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        btnTHEM.Enabled = true;
                        btnXOA.Enabled = true;
                        btnGHI.Enabled = true;
                        btnHOANTAC.Enabled = true;

                        btnLAMMOI.Enabled = true;
                        btnCHUYENCHINHANH.Enabled = true;
                        btnTHOAT.Enabled = true;

                        this.txtMAKHO.Enabled = false;
                        this.gcKHO.Enabled = true;

                        String cauTruyVanHoanTac = "";
                        if (dangThemMoi == true)
                        {
                            cauTruyVanHoanTac = "" +
                                "DELETE DBO.KHO " +
                                "WHERE MAKHO = '" + txtMAKHO.Text.Trim() + "'";
                        }
                        else
                        {
                            cauTruyVanHoanTac =
                                "UPDATE DBO.KHO " +
                                "SET " +
                                "TENKHO = N'" + txtTENKHO + "', " +
                                "DIACHI = N'" + txtDIACHI + "' " +
                                "WHERE MAKHO = '" + maKhoHang + "'";
                        }
                        undoList.Push(cauTruyVanHoanTac);

                        this.bdsKho.EndEdit();
                        this.khoTableAdapter.Update(this.dataSet.Kho);
                        dangThemMoi = false;
                        MessageBox.Show("Ghi thành công", "Thông báo", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        bdsKho.RemoveCurrent();
                        MessageBox.Show("Tên kho có thể đã được dùng !\n\n" + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void btnHOANTAC_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /* Step 0 */
            if (dangThemMoi == true && this.btnTHEM.Enabled == false)
            {
                dangThemMoi = false;

                this.txtMAKHO.Enabled = false;
                this.btnTHEM.Enabled = true;
                this.btnXOA.Enabled = true;
                this.btnGHI.Enabled = true;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnTHOAT.Enabled = true;


                this.gcKHO.Enabled = true;
                this.panelNhapLieu.Enabled = true;

                bdsKho.CancelEdit();
                /*xoa dong hien tai*/
                bdsKho.RemoveCurrent();
                this.khoTableAdapter.Fill(this.dataSet.Kho); //đã thêm
                /* trở về lúc đầu con trỏ đang đứng*/
                bdsKho.Position = viTri;
                return;
            }

            /*Step 1*/
            if (undoList.Count == 0)
            {
                MessageBox.Show("Không còn thao tác nào để khôi phục", "Thông báo", MessageBoxButtons.OK);
                btnHOANTAC.Enabled = false;
                return;
            }

            /*Step 2*/
            bdsKho.CancelEdit();
            String cauTruyVanHoanTac = undoList.Pop().ToString();
            Console.WriteLine(cauTruyVanHoanTac);
            int n = Program.ExecSqlNonQuery(cauTruyVanHoanTac);
            bdsKho.Position = viTri;
            this.khoTableAdapter.Fill(this.dataSet.Kho);
        }

        private void btnXOA_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 1*/
            if (bdsKho.Count == 0)
            {
                btnXOA.Enabled = false;
            }

            if (bdsDATHANG.Count > 0)
            {
                MessageBox.Show("Không thể xóa kho hàng này vì đã lập đơn đặt hàng", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            if (bdsPHIEUNHAP.Count > 0)
            {
                MessageBox.Show("Không thể xóa kho hàng này vì đã lập phiếu nhập", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            if (bdsPHIEUXUAT.Count > 0)
            {
                MessageBox.Show("Không thể xóa kho hàng này vì đã lập phiếu xuất", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            string cauTruyVanHoanTac =
            "INSERT INTO DBO.KHO( MAKHO,TENKHO,DIACHI,MACN) " +
            " VALUES( '" + txtMAKHO.Text + "', N'" +
                        txtTENKHO.Text + "', N'" +
                        txtDIACHI.Text + "', N'" +
                        txtMACHINHANH.Text.Trim() + "' ) ";

            Console.WriteLine(cauTruyVanHoanTac);
            undoList.Push(cauTruyVanHoanTac);

            /*Step 2*/
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không ?", "Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    /*Step 3*/
                    viTri = bdsKho.Position;
                    bdsKho.RemoveCurrent();

                    this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.khoTableAdapter.Update(this.dataSet.Kho);

                    // niên fix 

                    /*FIX LỖI XÓA XONG, KO ẤN HOÀN TÁC, THÊM MỚI, BỊ LỖI TRÙNG MÃ NV*/
                    this.khoTableAdapter.Fill(this.dataSet.Kho); /*tải dl từ csdl sqlserver -> dùng SQL SERVER*/
                    this.gcKHO.Enabled = true;


                    MessageBox.Show("Xóa thành công ", "Thông báo", MessageBoxButtons.OK);
                    this.btnHOANTAC.Enabled = true;
                }
                catch (Exception ex)
                {
                    /*Step 4*/
                    MessageBox.Show("Lỗi xóa nhân viên. Hãy thử lại\n" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    this.khoTableAdapter.Fill(this.dataSet.Kho);
                    bdsKho.Position = viTri;
                    return;
                }
            }
            else
            {
                undoList.Pop();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtMAKHO_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gcKHO_Click(object sender, EventArgs e)
        {

        }
    }
}