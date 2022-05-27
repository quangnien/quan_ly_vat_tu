using DevExpress.XtraEditors;
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

namespace QLTVT
{
    public partial class FormDangNhap : DevExpress.XtraEditors.XtraForm
    {
        private SqlConnection connPublisher = new SqlConnection();

        private void layDanhSachPhanManh(String cmd)
        {
            if (connPublisher.State == ConnectionState.Closed)
            {
                connPublisher.Open();
            }
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd, connPublisher); /*connPublisher lúc này ensure đã mở success*/
            da.Fill(dt); /*dt(data table) này chứa ds phân mảnh của ta gồm có nhiều dòng, 2 cột*/

            connPublisher.Close();
            Program.bindingSource.DataSource = dt; /*đưa số liệu từ dt vào trong bindingSource (đã chuẩn bị sẵn trong program.cs r)*/

            /*đưa số liệu, liên kết số liệu của cái bindingSource với cmbCHINHANH*/
            cmbCHINHANH.DataSource = Program.bindingSource;
            cmbCHINHANH.DisplayMember = "TENCN";/*chứa tên field mà khi ta click chuột vào thì nó hiện lên dl của field đó*/
            cmbCHINHANH.ValueMember = "TENSERVER";/*cũng là thuộc tính chứa tên field nhưng mà cái quan trọng là khi ta chọn
                                                   *chi nhánh nào trong TENCHINHANH thì nó tự động trả về giá trị SERVER tương 
                                                   *ứng của chi nhánh đó*/
        }
        public FormDangNhap()
        {
            InitializeComponent();
        }


        /* Để tránh việc người dùng ấn vào 1 form đến 2 lần */
        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }

        /* mở kết nối tới server */
        private int KetNoiDatabaseGoc()
        {
            /*connPublisher : biến cục bộ của formDangNhap đã khai báo ở trên*/
            if (connPublisher != null && connPublisher.State == ConnectionState.Open) /*vì khi 1 connection đang mở, mà mở nữa sẽ báo lỗi*/
                /*Câu hỏi: nếu đã mở r thì ko mở nữa, thành ra khởi mất thời gian mở lại?
                 => Sai vì : Thực tế DOTE.NET lại có 1 vấn đề là khi mở kết nối tới db là tải dl về 
                xong r thì trong vòng từ 5-10s là sẽ tự động đóng, khác với các phiên bản
                DOTE.NET cũ. Thành ra có khả năng là cái thời điểm nó đang mở, 1 lúc sau nó đóng
                thì mình tải dl về nó sẽ báo lỗi. Cho nên cẩn thận cứ làm vậy cho chắc ăn.*/
                connPublisher.Close();
            try
            {
                connPublisher.ConnectionString = Program.connstrPublisher;
                connPublisher.Open();
                return 1;
            }

            catch (Exception e)
            {
                MessageBox.Show("Kiểm tra lại tên tài khoản và mật khẩu!\n" + e.Message, "", MessageBoxButtons.OK);
                return 0;
            }
        }

        private void FormDangNhap_Load(object sender, EventArgs e)
        {
            txtTAIKHOAN.Text = "TT";
            txtMATKHAU.Text = "123456";
            if (KetNoiDatabaseGoc() == 0)
                return;
            layDanhSachPhanManh("SELECT TOP 2 * FROM view_DanhSachPhanManh");
            cmbCHINHANH.SelectedIndex = 0;
            cmbCHINHANH.SelectedIndex = 1;
        }

        private void btnDANGNHAP_Click(object sender, EventArgs e)
        {
            /* B1 */
            if (txtTAIKHOAN.Text.Trim() == "" || txtMATKHAU.Text.Trim() == "")
            {
                MessageBox.Show("Tài khoản & mật khẩu không được bỏ trống!", "Thông Báo", MessageBoxButtons.OK);
                return;
            }

            /* B2 */
            Program.loginName = txtTAIKHOAN.Text.Trim();
            Program.loginPassword = txtMATKHAU.Text.Trim();
            if (Program.KetNoi() == 0) return;

            /*B3 : gán vô biến toàn cục để sài cho những form sau này (nhanvien, lapphieunhap,...)*/
            Program.brand = cmbCHINHANH.SelectedIndex;
            Program.currentLogin = Program.loginName;
            Program.currentPassword = Program.loginPassword;
            
            /* Khi mà ta thực thi những câu lệnh truy vấn những SP ở trên CSDL thì ta có 3 TH xảy ra
            - TH1 : tải dl về mà dl đó chỉ cho reader thôi, ko cho phép edit => return dưới dạng Datareader
            - TH2 : gọi và trả về dataTable(cùng là nhiều dòng, nhiều cột) nhưng cho phép
                    xem, xóa, sửa, thêm, đi lên đi xuống thỏa mái
            - TH3 : thực thi câu lệnh update trên SP đó và ko trả về giá trị

            Vậy thì trả về 1 cái bảng dưới dạng dataReader và dataTable thì cái nào nhanh hơn?
            => datareader nhanh hơn nhưng mà cái dl tải về thì ko thể sửa đc, chỉ có thể đi xuống, 
            ko đi ngược lại.
             */

            /* B4 */
            String statement = "EXEC sp_DangNhap '" + Program.loginName + "'";
            Program.myReader = Program.ExecSqlDataReader(statement); /*Viết hàm thực thi chung, chỉ cần truyền query gọi SP vô thôi => gọn*/
            if (Program.myReader == null) return;
            // đọc một dòng của myReader - điều này là hiển nhiên vì kết quả chỉ có 1 dùng duy nhất
            Program.myReader.Read(); // Nếu có nhiều kq trả về thì chỗ này phải viết 1 vòng lặp.

            /* B5: gán info vô form chính*/
            Program.userName = Program.myReader.GetString(0);
            if (Convert.IsDBNull(Program.userName))
            {
                MessageBox.Show("Tài khoản này không có quyền truy cập \n Hãy thử tài khoản khác", "Thông Báo", MessageBoxButtons.OK);
            }

            Program.staff = Program.myReader.GetString(1);
            Program.role = Program.myReader.GetString(2);

            Program.myReader.Close();
            Program.conn.Close();


            /*FIX LỖI TÀI KHOẢN Ở TRẠNG THÁI XÓA THÌ KHÔNG CHO ĐĂNG NHẬP, MẶC DÙ ĐÃ CÓ LOGINNAME*/
            /*TRẠNG THÁI XÓA = TRUE CÓ 2 DẠNG
             - DẠNG 1 : CHUYỂN CHI NHÁNH -> XÓA LOGIN LUÔN
             - DẠNG 2 : NV HĐ Ở 1 SITE, NGHỈ LÀM -> KO XÓA LOGIN VÌ ĐẶT TRƯỜNG HỢP GIẢ SỬ NV ĐI LÀM LẠI!*/
            String statementKiemTraTTX =
                "DECLARE @result int " +
                " exec @result =  sp_KiemTraTrangThaiXoa '" + Program.userName + "'" +
                " SELECT 'Value' = @result";
            SqlCommand sqlCommand1 = new SqlCommand(statementKiemTraTTX, Program.conn);
            /*  MessageBox.Show("Program.conn : " + Program.conn, "Thông báo",
                          MessageBoxButtons.OK);*/
            try
            {
                Program.myReader = Program.ExecSqlDataReader(statementKiemTraTTX);
                if (Program.myReader == null) return;
            }
            catch (Exception ex){
                MessageBox.Show("Thực thi database thất bại!\n\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK);
                return;
            }
            Program.myReader.Read();
            int result1 = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();
            if (result1 == 1){
                MessageBox.Show("Tài khoản này không tồn tại!\nHãy thử tài khoản khác", "Thông Báo", MessageBoxButtons.OK);
                foreach (Form f2 in this.MdiChildren)
                    f2.Dispose();

                Form f = this.CheckExists(typeof(FormDangNhap));
                if (f != null)
                {
                    f.Activate();
                }
                else
                {
                    FormDangNhap form = new FormDangNhap();
                    form.Show();
                }

                Program.formChinh.MANHANVIEN.Text = "MÃ NHÂN VIÊN:";
                Program.formChinh.HOTEN.Text = "HỌ TÊN:";
                Program.formChinh.NHOM.Text = "VAI TRÒ:";
                this.Close();
                return;
            }

            Program.formChinh.MANHANVIEN.Text = "MÃ NHÂN VIÊN: " + Program.userName;
            Program.formChinh.HOTEN.Text = "HỌ TÊN: " + Program.staff;
            Program.formChinh.NHOM.Text = "VAI TRÒ: " + Program.role;

            /* B6 */
            this.Visible = false;
            Program.formChinh.enableButtons();
        }

        private void btnTHOAT_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cmbCHINHANH_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*CÂU HỎI VẤN ĐÁP: Commobox có những thuộc tính và biến cố nào qtrg thường dùng? giải thích?
             * => 3 : DataSource, DisplayMember, ValueMember*/
            try
            {
                Program.serverName = cmbCHINHANH.SelectedValue.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void txtMATKHAU_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTAIKHOAN_TextChanged(object sender, EventArgs e)
        {

        }
    }
}