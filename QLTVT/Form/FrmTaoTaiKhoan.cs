using DevExpress.XtraEditors;
using QLTVT.SubForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace QLTVT
{
    public partial class FormTaoTaiKhoan : DevExpress.XtraEditors.XtraForm
    {
        private string taiKhoan = "";
        private string matKhau = "";
        private string maNhanVien = "";
        private string vaiTro = "";
        public FormTaoTaiKhoan()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnChonNhanVien_Click(object sender, EventArgs e)
        {
            FrmChonNhanVien form = new FrmChonNhanVien();
            form.ShowDialog();

            txtMaNhanVien.Text = Program.maNhanVienDuocChon;
        }

        private bool kiemTraDuLieuDauVao()
        {
            if( txtMaNhanVien.Text == "")
            {
                MessageBox.Show("Thiếu mã nhân viên!","Thông báo", MessageBoxButtons.OK);
                return false;
            }

            if( txtMatKhau.Text == "" )
            {
                MessageBox.Show("Thiếu mật khẩu!", "Thông báo", MessageBoxButtons.OK);
                return false;
            }

            if (txtXacNhanMatKhau.Text == "")
            {
                MessageBox.Show("Thiếu mật khẩu xác nhận!", "Thông báo", MessageBoxButtons.OK);
                return false;
            }

            if( txtMatKhau.Text != txtXacNhanMatKhau.Text)
            {
                MessageBox.Show("Mật khẩu không khớp với mật khẩu xác nhận!", "Thông báo", MessageBoxButtons.OK);
                return false;
            }

            /*FIX LỖI LOGINNAME KHÔNG NHẬN KHOẲNG TRẮNG*/
            if (textTenLogin.Text == "")
            {
                MessageBox.Show("Không được bỏ trống tên LoginName!", "Thông Báo", MessageBoxButtons.OK);
                return false;
            }
            /*FIX LỖI CHỈ ĐƯỢC NHẬP SỐ, CHỮ, KHÔNG NHẬP KHOẢNG TRẮNG*/
            if (Regex.IsMatch(textTenLogin.Text, @"^[a-zA-Z0-9]+$") == false)
            {
                MessageBox.Show("LoginName chỉ nhận chữ cái, số và không nhận khoảng trắng!", "Thông Báo", MessageBoxButtons.OK);
                return false;
            }

            return true;
        } 

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            bool ketQua = kiemTraDuLieuDauVao();
            if (ketQua == false) return;

            /*FIX LỖI THÊM LOGIN NAME*/
            //taiKhoan = Program.hoTen;
            taiKhoan = textTenLogin.Text;
            matKhau = txtMatKhau.Text;
            maNhanVien = Program.maNhanVienDuocChon;

            if (Program.role != "CONGTY")
            {
                vaiTro = (rdChiNhanh.Checked == true) ? "CHINHANH" : "USER";
            }

            if (Program.role == "CONGTY")
            {
                vaiTro = "CONGTY";
                /*FIX LỖI : Không được tạo tài khoản thuộc nhóm CONGTY ở CHINHANH khác!*/
                if (Program.loginName == Program.remoteLogin && Program.loginPassword == Program.remotePassword)
                {
                    MessageBox.Show("Không được tạo tài khoản thuộc nhóm CONGTY ở CHINHANH khác!", "Thông báo",
                        MessageBoxButtons.OK);
                    return;
                }
            }

            Console.WriteLine(taiKhoan);
            Console.WriteLine(matKhau);
            Console.WriteLine(maNhanVien);
            Console.WriteLine("ROLE : " + vaiTro);

            String cauTruyVan =
                    "EXEC sp_TaoTaiKhoan '" + taiKhoan + "' , '" + matKhau + "', '"
                    + maNhanVien + "', '" + vaiTro + "'";
            Console.WriteLine("cauTruyVan : " + cauTruyVan);

            SqlCommand sqlCommand = new SqlCommand(cauTruyVan, Program.conn);
            try
            {
                Program.myReader = Program.ExecSqlDataReader(cauTruyVan);
                if (Program.myReader == null) return;

                MessageBox.Show("Đăng kí tài khoản thành công!\n______________________\nTài khoản: "+taiKhoan+"\nMật khẩu: " + matKhau + "\nMã Nhân Viên: " + maNhanVien + "\nVai Trò: " + vaiTro,"Thông Báo",MessageBoxButtons.OK);
                this.Close();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thực thi database thất bại!\n\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
                return;
            }
        }

        private void cmbCHINHANH_SelectedIndexChanged(object sender, EventArgs e) {}

        private void FormTaoTaiKhoan_Load(object sender, EventArgs e)
        {
            if( Program.role == "CONGTY")
            {
                vaiTro = "CONGTY";
                rdChiNhanh.Enabled = false;
                rdUser.Enabled = false;

                /*add by QuangNien
                 vì CONGTY chỉ được tạo login có quyền là CONGTY */
                rdChiNhanh.Hide();
                rdUser.Hide();
                lableVaiTro.Hide();
            }
            rdChiNhanh.Enabled = true;
            rdUser.Enabled = true;
        }

        private void textTenLogin_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtMatKhau_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void txtXacNhanMatKhau_EditValueChanged(object sender, EventArgs e)
        {
        }
    }
}