using DevExpress.Skins;
using DevExpress.UserSkins;
using QLTVT.ReportForm;
using QLTVT.SubForm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace QLTVT
{
    static class Program
    {
        /*FIX LỖI CHỈ CHỌN ĐƯỢC MAVATTU ĐANG EDIT KHI EDIT CTPN P1*/
        public static string MaVatTuDangCoOCTPN = "";
        /*FIX LỖI CHỈ CHỌN ĐƯỢC MAVATTU ĐANG EDIT KHI EDIT CTPX P1*/
        public static string MaVatTuDangCoOCTPX = "";
        /*ĐỂ BẮT LỖI GIỮA THÊM VÀ EDIT CHITIETPHIEUNHAP*/
        public static bool dangThemMoiPhieuNhap = false;
        /*ĐỂ BẮT LỖI GIỮA THÊM VÀ EDIT CHITIETPHIEUXUAT*/
        public static bool dangThemMoiPhieuXuat = false;
        /*THÊM BIẾN ĐỂ FIX LỖI KIỂM TRA CHITIETPHIEUNHAP đã tồn tại hay chưa với [SP_KiemTraChiTietPhieuNhapDaTonTaiHayChua]*/
        public static string maPhieuNhapDuocChon = "";
        /*THÊM BIẾN ĐỂ FIX LỖI KIỂM TRA CHITIETPHIEUXUAT đã tồn tại hay chưa với [SP_KiemTraChiTietPhieuXuatDaTonTaiHayChua]*/
        public static string maPhieuXuatDuocChon = "";

        public static SqlConnection conn = new SqlConnection();  // biến để kết nối tới cơ sở dữ liệu
        public static String connstr = "";                       // chuỗi kết nối động
        public static String connstrPublisher = "Data Source=DESKTOP-405626K\\MAYCHU;Initial Catalog=QLVT;Integrated Security=true";
        public static SqlDataReader myReader;

        public static String serverName = "";
        public static String serverNameLeft = "";
        public static String userName = "";

        public static String loginName = "";
        public static String loginPassword = "";

        public static String database = "QLVT";

        public static String remoteLogin = "HTKN";
        public static String remotePassword = "123456";

        public static String currentLogin = "";
        public static String currentPassword = "";

        /*đang login */
        public static String role = "";
        public static String staff = "";
        public static int brand = 0;

        /* tạo mới đơn đặt hàng */
        public static string maKhoDuocChon = "";
        public static string maVatTuDuocChon = "";

        /* bao cao hoat dong nhan vien */
        public static int soLuongVatTu = 0;
        public static string maDonDatHangDuocChon = "";
        public static string maDonDatHangDuocChonChiTiet = "";
        public static int donGia = 0;

        /* HOAT DONG NHAN VIEN */
        public static string maNhanVienDuocChon = "";
        public static string hoTen = "";
        public static string diaChi = "";
        public static string ngaySinh = "";

        /*BindingSource -> liên kết dữ liệu từ bảng dữ liệu vào chương trình*/
        /*có 2 cột: TENCN, TENSERVER*/
        /* tồn tại : (login->end)*/
        public static BindingSource bindingSource = new BindingSource();

        public static FormDangNhap formDangNhap;
        public static Formmain formChinh; /*Đây mới chỉ là con trỏ, chưa phải object, về sau sẽ là object của formMain*/
        public static FormNhanVien formNhanVien;

        public static FrmChuyenChiNhanh formChuyenChiNhanh;
        public static FormVatTu formVatTu;
        public static FormKho formKho;

        public static FormDonDatHang formDonDatHang;
        public static FrmChonKhoHang formChonKhoHang;
        public static FormPhieuNhap formPhieuNhap;

        public static FrmChonDonDatHang formChonDonDatHang;
        public static FrmChonChiTietDonHang formChonChiTietDonHang;
        public static FormPhieuXuat formPhieuXuat;

        public static frmDanhSachNhanVien formDanhSachNhanVien;
        public static frmDanhSachVatTu formDanhSachVatTu;
        public static frmDonDatHangKhongCoPhieuNhap formDonHangKhongPhieuNhap;

        public static frmChiTietSoLuongTriGiaHangHoaNhapHoacXuat formChiTietSoLuongTriGiaHangHoaNhapXuat;
        public static frmHoatDongNhanVien formHoatDongNhanVien;
        public static frmTongHopNhapXuat formTongHopNhapXuat;
        
        public static int KetNoi()
        {
            if (Program.conn != null && Program.conn.State == ConnectionState.Open) 
                Program.conn.Close();
            try
            {
                /*có 4 attribute giống y như bên csdl gốc chỉ # là USERID, password*/
                /*nếu báo sai thì chỉ có thể sai ở userName và password
                ko thể sai : Program.serverName được vì đâu có gõ tay đâu mà sai, chọn thôi mà
                và Program.database cũng ko thể sai được vì đã gán trực tiếp biến toàn cục là 
                1 csdl duy nhất xuyên suốt trên toàn dự án của ta*/
                Program.connstr = "Data Source=" + Program.serverName + ";Initial Catalog=" +
                       Program.database + ";User ID=" +
                       Program.loginName + ";password=" + Program.loginPassword;
                Program.conn.ConnectionString = Program.connstr;

                Program.conn.Open();
                return 1;
            }

            catch (Exception e)
            {
                MessageBox.Show("Kiểm tra lại tài khoản và mật khẩu!\nError : " + e.Message, "", MessageBoxButtons.OK);
                return 0;
            }
        }

        public static SqlDataReader ExecSqlDataReader(String strLenh)
        {
            SqlDataReader myreader;
            SqlCommand sqlcmd = new SqlCommand(strLenh, Program.conn);
            sqlcmd.CommandType = CommandType.Text; /*luôn luôn là chuỗi lệnh => dùng dạng TEXT*/
            if (Program.conn.State == ConnectionState.Closed)
                Program.conn.Open();
            try
            {
                myreader = sqlcmd.ExecuteReader(); return myreader;
            }
            catch (SqlException ex)
            {
                Program.conn.Close();
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        /* Cách #: tạo dt nữa là sẽ tải về dưới dạng là 1 dataReader trước 
           sau đó load dl đó vào dt (dùng LoadData) (ko cần dùng SqlDataAdapter nữa) */
        public static DataTable ExecSqlDataTable(String cmd)
        {
            DataTable dt = new DataTable();
            if (Program.conn.State == ConnectionState.Closed) Program.conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd, conn);
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        public static int ExecSqlNonQuery(String strlenh)
        {
            SqlCommand Sqlcmd = new SqlCommand(strlenh, conn);
            Sqlcmd.CommandType = CommandType.Text;
            Sqlcmd.CommandTimeout = 600;/* //10 phut   /* default: 60s => lớn -> ko đủ */
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                Sqlcmd.ExecuteNonQuery(); 
                conn.Close();
                return 0; //success
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
                return ex.State; //Trạng thái lỗi gửi từ RAISERROR trong SQL Server qua 
                                 //Chuỗi thông báo từ server gửi đến client thông qua cái ex này.
            }
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Program.formChinh = new Formmain();
            Application.Run(formChinh);
        }
    }
}
