using QLTVT.SubForm;
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
    public partial class FormNhanVien : Form
    {
        /* vị trí của con trỏ trên grid view*/
        int viTri = 0;
        
        bool dangThemMoi = false;
                    
        String maChiNhanh = "";

        Stack undoList = new Stack();

        private static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;
            return age;
        }

        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }

        public FormNhanVien()
        {
            InitializeComponent();
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void FormNhanVien_Load(object sender, EventArgs e)
        {
            /*Step 1*/
            /*không kiểm tra khóa ngoại nữa*/
            /*Vì vd trong DATHANG có 2 khóa ngoại là MAKHO và MANV, thì khi mà ta tải 
             đơn DATHANG vô mà đã tải MANV vô trước rồi thì OK, còn MAKHO chưa có
             -> bão lỗi. Mà form này chỉ nhập NHANVIEN, đâu lq tới KHO làm gì chả nhẽ
             bh lại tải KHO nữa thì mất công => KO KIỀM TRA RÀNG BUỘC KHÓA NGOẠI NỮA.*/
            dataSet.EnforceConstraints = false;


            /*theo thứ tự lần lượt xuất hiện, vd: DATHANG có trước, PHIEUNHAP có sau*/
            /*Giả sử quên đoạn code ở dưới thì vẫn chạy bt, nhưng đến 1 lúc nào đó sẽ
             bị lỗi ko chạy vì nếu ko có lệnh đó thì khi chạy ct nó sẽ lấy cái tài khoản
             mà khi tạo cái dataTable của cái ta vừa login (vd : TT) để kết nối. Về sau cái 
             tk login (TT) đó đổi password thì báo lỗi liền, mặc dù login thành công dưới 
             cái tk mới (vs : LT) thì ở đây nó vẫn lấy tk login cũ (TT) để kết nối.
             => login bằng tk nào, pass nào thì phải lấy infor của tk đó để kết nối.*/
            this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.nhanVienTableAdapter.Fill(this.dataSet.NhanVien);
            
            this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
            this.datHangTableAdapter.Fill(this.dataSet.DatHang);
            
            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuNhapTableAdapter.Fill(this.dataSet.PhieuNhap);
           
            this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuXuatTableAdapter.Fill(this.dataSet.PhieuXuat);

            maChiNhanh = ((DataRowView)bdsNhanVien[0])["MACN"].ToString();

            /*Step 2*/
            cmbCHINHANH.DataSource = Program.bindingSource;/*sao chep bingding source tu form dang nhap*/
            cmbCHINHANH.DisplayMember = "TENCN";
            cmbCHINHANH.ValueMember = "TENSERVER";
            cmbCHINHANH.SelectedIndex = Program.brand;
            
            /*Step 3*/
            if( Program.role == "CONGTY")
            {
                cmbCHINHANH.Enabled = true;

                this.btnTHEM.Enabled = false;
                this.btnXOA.Enabled = false;
                this.btnGHI.Enabled = false;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnCHUYENCHINHANH.Enabled = false;
                this.btnTHOAT.Enabled = true;

                this.panelNhapLieu.Enabled = false;
            }

            if( Program.role == "CHINHANH" || Program.role == "USER")
            {
                cmbCHINHANH.Enabled = false;

                this.btnTHEM.Enabled = true;
                this.btnXOA.Enabled = true;
                this.btnGHI.Enabled = true;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnCHUYENCHINHANH.Enabled = true;
                this.btnTHOAT.Enabled = true;

                this.panelNhapLieu.Enabled = true;
                this.txtMANV.Enabled = false;
            }

        }

        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnTHEM_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 1*/
            viTri = bdsNhanVien.Position;
            this.panelNhapLieu.Enabled = true;
            dangThemMoi = true;


            /*Step 2*/
            bdsNhanVien.AddNew();
            txtMACN.Text = maChiNhanh; /*chinhanh ban đầu là khóa lại, để khi ta addNew thì sẽ auto lấy machinhanh ở trên gán xuống*/
            dteNGAYSINH.EditValue = "2000-05-01"; 
            txtLUONG.Value = 4000000;

            


            /*Step 3*/
            this.txtMANV.Enabled = true;
            this.btnTHEM.Enabled = false;
            this.btnXOA.Enabled = false;
            this.btnGHI.Enabled = true;

            this.btnHOANTAC.Enabled = true;
            this.btnLAMMOI.Enabled = false;
            this.btnCHUYENCHINHANH.Enabled = false;
            this.btnTHOAT.Enabled = false;
            this.trangThaiXoaCheckBox.Checked = false;

            /*nói thêm: để bảo đảm an toàn thì cho gcNhanVien là false
            1 cái sẽ có 2 object là gridControl (dữ liệu) và gridView (giao diện) */
            this.gcNhanVien.Enabled = false;
            this.panelNhapLieu.Enabled = true;
        }

        private void btnHOANTAC_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {   /*btn này dùng trong 2TH : thêm và sửa => bắt code*/
            /*Step 0 - trường hợp đã ấn btnTHEM nhưng chưa ấn btnGHI*/
            /*FIX LỖI HOÀN TÁC CHUYỂN CHI NHÁNH VẪN GIỮ NGUYÊN VỊ TRÍ CON TRỎ P1*/
            int viTri = bdsNhanVien.Position;

            if ( dangThemMoi == true && this.btnTHEM.Enabled == false)
            {
                dangThemMoi = false;

                this.txtMANV.Enabled = false;
                this.btnTHEM.Enabled = true;
                this.btnXOA.Enabled = true;
                this.btnGHI.Enabled = true;

                this.btnHOANTAC.Enabled = false; /*đã HOANTAC r thì ko thể HOANTAC nữa*/
                this.btnLAMMOI.Enabled = true;
                this.btnCHUYENCHINHANH.Enabled = true;
                this.btnTHOAT.Enabled = true;
                this.trangThaiXoaCheckBox.Checked = false;

                this.gcNhanVien.Enabled = true; 
                this.panelNhapLieu.Enabled = true; 

                bdsNhanVien.CancelEdit(); 
                bdsNhanVien.RemoveCurrent();

                this.nhanVienTableAdapter.Fill(this.dataSet.NhanVien);

                bdsNhanVien.Position = viTri;
                return;
            }


            /*Step 1: kiểm tra undoList có trông hay không ?*/
            if ( undoList.Count == 0)
            {
                MessageBox.Show("Không còn thao tác nào để khôi phục", "Thông báo", MessageBoxButtons.OK);
                btnHOANTAC.Enabled = false;
                return;
            }

            /*Step 2: Neu undoList khong trống thì lấy ra khôi phục*/
            bdsNhanVien.CancelEdit();
            String cauTruyVanHoanTac = undoList.Pop().ToString();

            /*Step 2.1*/
            if ( cauTruyVanHoanTac.Contains("sp_ChuyenChiNhanh") )
            {
                try
                {
                    String chiNhanhHienTai = Program.serverName;
                    if (Program.KetNoi() == 0)
                    {
                        return;
                    }

                    Console.WriteLine("cauTruyVanHoanTac : " + cauTruyVanHoanTac);
                    int n = Program.ExecSqlNonQuery(cauTruyVanHoanTac);

                    MessageBox.Show("Chuyển nhân viên trở lại thành công", "Thông báo", MessageBoxButtons.OK);
                    Program.serverName = chiNhanhHienTai;
                    Program.loginName = Program.currentLogin;
                    Program.loginPassword = Program.currentPassword;
                }
                catch( Exception ex)
                {
                    MessageBox.Show("Chuyển nhân viên thất bại \n"+ex.Message, "Thông báo", MessageBoxButtons.OK);
                    return;
                }
                
            }
            /*Step 2.2*/
            else
            {
                if (Program.KetNoi() == 0)
                {
                    return;
                }
                int n = Program.ExecSqlNonQuery(cauTruyVanHoanTac);
                
            }

            /*FIX LỖI HOÀN TÁC CHUYỂN CHI NHÁNH VẪN GIỮ NGUYÊN VỊ TRÍ CON TRỎ P2*/
            this.nhanVienTableAdapter.Fill(this.dataSet.NhanVien);
            bdsNhanVien.Position = viTri;
        }

        private void btnLAMMOI_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           try
           {
                this.nhanVienTableAdapter.Fill(this.dataSet.NhanVien);
                this.gcNhanVien.Enabled = true;
           }
           catch(Exception ex)
           {
                MessageBox.Show("Lỗi Làm mới" + ex.Message,"Thông báo", MessageBoxButtons.OK);
                return;
           }
        }

        private void btnXOA_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String tenNV = ((DataRowView)bdsNhanVien[bdsNhanVien.Position])["MANV"].ToString();

            /*B1*/
            if(tenNV == Program.userName)
            {
                MessageBox.Show("Không thể xóa chính tài khoản đang đăng nhập", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            if ( bdsNhanVien.Count == 0) /*TH ko còn gì để xóa thì phải unable btnXoa*/
            {
                btnXOA.Enabled = false;
            }

            if( bdsDatHang.Count > 0)
            {
                MessageBox.Show("Không thể xóa nhân viên này vì đã lập đơn đặt hàng", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            if (bdsPhieuNhap.Count > 0)
            {
                MessageBox.Show("Không thể xóa nhân viên này vì đã lập phiếu nhập", "Thông báo", MessageBoxButtons.OK);
                return;
            }


            if (bdsPhieuXuat.Count > 0)
            {
                MessageBox.Show("Không thể xóa nhân viên này vì đã lập phiếu xuất", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            int trangThai = (trangThaiXoaCheckBox.Checked == true) ? 1 : 0;
            DateTime NGAYSINH = (DateTime)((DataRowView)bdsNhanVien[bdsNhanVien.Position])["NGAYSINH"];

            string cauTruyVanHoanTac =
                string.Format("INSERT INTO DBO.NHANVIEN( MANV,HO,TEN,DIACHI,NGAYSINH,LUONG,MACN)" +
            "VALUES({0},N'{1}',N'{2}',N'{3}',CAST({4} AS DATETIME), {5},'{6}')", txtMANV.Text, txtHO.Text, txtTEN.Text, txtDIACHI.Text, NGAYSINH.ToString("yyyy-MM-dd"), txtLUONG.Value, txtMACN.Text.Trim());

            Console.WriteLine(cauTruyVanHoanTac);
            undoList.Push(cauTruyVanHoanTac);

            /*B2*/
            if ( MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này không ?","Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    /*B3*/
                    viTri = bdsNhanVien.Position;
                    bdsNhanVien.RemoveCurrent(); 

                    this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.nhanVienTableAdapter.Update(this.dataSet.NhanVien);/*ghi hđ xóa về csdl, nếu có lỗi => CATCH*/

                    /*FIX LỖI XÓA XONG, KO ẤN HOÀN TÁC, THÊM MỚI, BỊ LỖI TRÙNG MÃ NV*/
                    this.nhanVienTableAdapter.Fill(this.dataSet.NhanVien); /*tải dl từ csdl sqlserver -> dùng SQL SERVER*/
                    this.gcNhanVien.Enabled = true;

                    MessageBox.Show("Xóa thành công ", "Thông báo", MessageBoxButtons.OK);
                    this.btnHOANTAC.Enabled = true;
                }
                catch(Exception ex) /*đôi khi xóa r, đến đoạn update thì 1 số TH bị báo lỗi, 
                                     * ko rõ nên phải bắt Try-Catch */
                {
                    /*B4*/
                    MessageBox.Show("Lỗi xóa nhân viên. Hãy thử lại\n" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    this.nhanVienTableAdapter.Fill(this.dataSet.NhanVien); /*vì remove trên màn hình r nhưng csdl lại
                                                                            ko remove do đến đoạn "Update" ở trên
                                                                            mới nhảy vào catch => tải lại dl*/
                    bdsNhanVien.Position = viTri;
                    return;
                }
            }
            else
            {
                undoList.Pop();
            }    
        }

        private void cmbCHINHANH_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*Neu combobox khong co so lieu thi ket thuc luon*/
            if (cmbCHINHANH.SelectedValue.ToString() == "System.Data.DataRowView")
                return;

            /*1 chuỗi kết nối gồm 4 thông tin: tên csdl, tên server, username, password
             ở đây là lấy được thông tin tên server r
             tên csdl thì là cố định r 
             => đi lấy username và password*/
            Program.serverName = cmbCHINHANH.SelectedValue.ToString();

            if( cmbCHINHANH.SelectedIndex != Program.brand )
            {
                Program.loginName = Program.remoteLogin;
                Program.loginPassword = Program.remotePassword;
            }
            else
            {
                Program.loginName = Program.currentLogin;
                Program.loginPassword = Program.currentPassword;
            }

            if( Program.KetNoi() == 0)
            {
                MessageBox.Show("Xảy ra lỗi kết nối với chi nhánh hiện tại","Thông báo",MessageBoxButtons.OK);
            }
            else
            {
                this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                this.nhanVienTableAdapter.Fill(this.dataSet.NhanVien);

                this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
                this.datHangTableAdapter.Fill(this.dataSet.DatHang);

                this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuNhapTableAdapter.Fill(this.dataSet.PhieuNhap);

                this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuXuatTableAdapter.Fill(this.dataSet.PhieuXuat);
            }
        }

        private void bdsPhieuNhap_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void dteNGAYSINH_EditValueChanged(object sender, EventArgs e)
        {
            //dteNGAYSINH.ShowPopup();
        }

        private void trangThaiXoaCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private bool kiemTraDuLieuDauVao()
        {
            if (txtMANV.Text == "")
            {
                MessageBox.Show("Không bỏ trống mã nhân viên", "Thông báo", MessageBoxButtons.OK);
                txtMANV.Focus(); /*bão lối, vẫn giữ nguyên dấu nháy ở chỗ có lỗi*/
                return false;
            }

            if (Regex.IsMatch(txtMANV.Text, @"^[0-9]+$") == false)
            {
                MessageBox.Show("Mã nhân viên chỉ chấp nhận số", "Thông báo", MessageBoxButtons.OK);
                txtMANV.Focus();
                return false;
            }
            if (txtHO.Text == "")
            {
                MessageBox.Show("Không bỏ trống họ và tên", "Thông báo", MessageBoxButtons.OK);
                txtHO.Focus();
                return false;
            }
            if (Regex.IsMatch(txtHO.Text.Trim(), @"^[a-zA-Z0-9áàạảãâấầậẩẫăắằặẳẵÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴéèẹẻẽêếềệểễÉÈẸẺẼÊẾỀỆỂỄóòọỏõôốồộổỗơớờợởỡÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠúùụủũưứừựửữÚÙỤỦŨƯỨỪỰỬỮíìịỉĩÍÌỊỈĨđĐýỳỵỷỹÝỲỴỶỸ ]+$") == false)
            {
                MessageBox.Show("Họ của người chỉ có chữ cái và khoảng trắng", "Thông báo", MessageBoxButtons.OK);
                txtHO.Focus();
                return false;
            }
            if (txtHO.Text.Length > 40)
            {
                MessageBox.Show("Họ không thể lớn hơn 40 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtHO.Focus();
                return false;
            }
            if (txtTEN.Text == "")
            {
                MessageBox.Show("Không bỏ trống họ và tên", "Thông báo", MessageBoxButtons.OK);
                txtTEN.Focus();
                return false;
            }

            if (Regex.IsMatch(txtTEN.Text.Trim(), @"^[a-zA-Z0-9áàạảãâấầậẩẫăắằặẳẵÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴéèẹẻẽêếềệểễÉÈẸẺẼÊẾỀỆỂỄóòọỏõôốồộổỗơớờợởỡÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠúùụủũưứừựửữÚÙỤỦŨƯỨỪỰỬỮíìịỉĩÍÌỊỈĨđĐýỳỵỷỹÝỲỴỶỸ ]+$") == false)
            {
                MessageBox.Show("Tên người chỉ có chữ cái và khoảng trắng", "Thông báo", MessageBoxButtons.OK);
                txtTEN.Focus();
                return false;
            }

            if (txtTEN.Text.Length > 10)
            {
                MessageBox.Show("Tên không thể lớn hơn 10 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtTEN.Focus();
                return false;
            }
            if (txtDIACHI.Text == "")
            {
                MessageBox.Show("Không bỏ trống địa chỉ", "Thông báo", MessageBoxButtons.OK);
                txtDIACHI.Focus();
                return false;
            }

            if (Regex.IsMatch(txtDIACHI.Text.Trim(), @"^[a-zA-Z0-9áàạảãâấầậẩẫăắằặẳẵÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴéèẹẻẽêếềệểễÉÈẸẺẼÊẾỀỆỂỄóòọỏõôốồộổỗơớờợởỡÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠúùụủũưứừựửữÚÙỤỦŨƯỨỪỰỬỮíìịỉĩÍÌỊỈĨđĐýỳỵỷỹÝỲỴỶỸ ]+$") == false)
            {
                MessageBox.Show("Địa chỉ chỉ chấp nhận chữ cái, số và khoảng trắng", "Thông báo", MessageBoxButtons.OK);
                txtDIACHI.Focus();
                return false;
            }

            if (txtDIACHI.Text.Length > 100)
            {
                MessageBox.Show("Không bỏ trống địa chỉ", "Thông báo", MessageBoxButtons.OK);
                txtDIACHI.Focus();
                return false;
            }
            if (CalculateAge(dteNGAYSINH.DateTime) < 18)
            {
                MessageBox.Show("Nhân viên chưa đủ 18 tuổi", "Thông báo", MessageBoxButtons.OK);
                dteNGAYSINH.Focus();
                return false;
            }

            if (txtLUONG.Value < 4000000 || txtLUONG.Value == 0) /*Lương thỏa miền giá trị của đề tài đã cho*/
            {
                MessageBox.Show("Mức lương không thể bỏ trống & tối thiểu 4.000.000 đồng", "Thông báo", MessageBoxButtons.OK);
                txtLUONG.Focus();
                return false;
            }
            return true;
        }

        private void btnGHI_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /* B1 */
            bool ketQua = kiemTraDuLieuDauVao();
            if (ketQua == false)
                return;

            /* B2 */
            String maNhanVien = txtMANV.Text.Trim();
            DataRowView drv = ((DataRowView)bdsNhanVien[bdsNhanVien.Position]);
            String ho = drv["HO"].ToString();
            String ten = drv["TEN"].ToString();

            String diaChi = drv["DIACHI"].ToString();

            //FIX LỖI BÁO LỖI THIẾU NGÀY VÀ LƯƠNG
            //DateTime ngaySinh = ((DateTime)drv["NGAYSINH"]);
            DateTime ngaySinh = dteNGAYSINH.DateTime;

            //int luong = int.Parse(drv["LUONG"].ToString());
            int luong = ((int)txtLUONG.Value);
            String maChiNhanh = drv["MACN"].ToString();

            /*FIX LỖI ẤN HOÀN TÁC, TRẠNG THÁI XÓA KO ĂN P1*/
            int TRANGTHAIXOABANDAU = 0;
            if (null != (drv["TrangThaiXoa"]))
            {
                bool TTXBANDAU = (bool)(drv["TrangThaiXoa"]);
                if (TTXBANDAU == false) TRANGTHAIXOABANDAU = 0;
                if (TTXBANDAU == true) TRANGTHAIXOABANDAU = 1;
            }

            /*FIX LỖI : LỖI 1 NV CÓ Ở 2 SITE THÌ KO CHO EDIT TRẠNG THÁI XÓA*/
            String CauTruyVanKiemTra =
            "DECLARE	@result int " +
            "exec @result = sp_KiemTraXemNhanVienDaTungChuyenChiNhanhHayChua '" +
            maNhanVien + "' " + ", '" + maChiNhanh + "'" + 
            "SELECT 'Value' = @result";
                SqlCommand sqlCommand1 = new SqlCommand(CauTruyVanKiemTra, Program.conn);
                try
                {
                    Program.myReader = Program.ExecSqlDataReader(CauTruyVanKiemTra);
                    /*khong co ket qua tra ve thi ket thuc luon*/
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
            int result1 = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();

            int trangThai = 0;

            if (result1 == 1) // tức là chỉ tồn tại ở 1 site
            {
                trangThai = (trangThaiXoaCheckBox.Checked == true) ? 1 : 0;
            }
            if (result1 == 0) //tức là tồn tại ở 2 site 
            {
                if ((bool)(drv["TrangThaiXoa"]) == false) // mặc dù ở 2 site, nhưng site đang đứng nó đang enable
                {
                }

                if ((bool)(drv["TrangThaiXoa"]) == true) // tồn tại ở 2 site, site đang đứng là disable
                {
                    MessageBox.Show("Nhân viên này đã được chuyển chi nhánh, bạn không thể thay đổi TRANGTHAIXOA thành false!')", "Thông báo",
                                       MessageBoxButtons.OK);
                    trangThai = 1; // phục vụ cho cautruyvanhoantac
                    trangThaiXoaCheckBox.Checked = true;
                    return;
                }    
            }

            String cauTruyVan =
                    "DECLARE	@result int " +
                    "EXEC @result = [dbo].[sp_TraCuu_KiemTraMaNhanVien] '" +
                    maNhanVien + "' " +
                    "SELECT 'Value' = @result";
            SqlCommand sqlCommand = new SqlCommand(cauTruyVan, Program.conn);
            try
            {
                Program.myReader = Program.ExecSqlDataReader(cauTruyVan);
                /*khong co ket qua tra ve thi ket thuc luon*/
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



            /*B2*/
            int viTriConTro = bdsNhanVien.Position;
            int viTriMaNhanVien = bdsNhanVien.Find("MANV", txtMANV.Text);
            
            if ( result == 1 && viTriConTro != viTriMaNhanVien)
            {
                MessageBox.Show("Mã nhân viên này đã được sử dụng !", "Thông báo", MessageBoxButtons.OK);
                return; 
            }
            else/*them moi | sua nhan vien*/
            {
                DialogResult dr = MessageBox.Show("Bạn có chắc muốn ghi dữ liệu vào cơ sở dữ liệu ?", "Thông báo",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if ( dr == DialogResult.OK)
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

                        this.txtMANV.Enabled = false;
                        this.bdsNhanVien.EndEdit(); 
                        this.nhanVienTableAdapter.Update(this.dataSet.NhanVien); 
                        this.gcNhanVien.Enabled = true;

                        String cauTruyVanHoanTac = "";
                        if( dangThemMoi == true)
                        {
                            cauTruyVanHoanTac = "" +
                                "DELETE DBO.NHANVIEN " +
                                "WHERE MANV = " + txtMANV.Text.Trim();
                        }
                        else
                        {
                            cauTruyVanHoanTac = 
                                "UPDATE DBO.NhanVien "+
                                "SET " +
                                "HO = N'" + ho + "'," +
                                "TEN = N'" + ten + "'," +
                                "DIACHI = N'" + diaChi + "'," +
                                "NGAYSINH = CAST('" + ngaySinh.ToString("yyyy-MM-dd") + "' AS DATETIME)," +
                                "LUONG = '" + luong + "',"+
                                // "TrangThaiXoa = " + trangThai + " " + 
                                "TrangThaiXoa = " + TRANGTHAIXOABANDAU + " " + ///*FIX LỖI ẤN HOÀN TÁC, TRẠNG THÁI XÓA KO ĂN P2*/
                                "WHERE MANV = '" + maNhanVien + "'";
                        }
                        Console.WriteLine("cauTruyVanHoanTac : " + cauTruyVanHoanTac);
                        undoList.Push(cauTruyVanHoanTac);
                        dangThemMoi = false;
                        MessageBox.Show("Ghi thành công", "Thông báo", MessageBoxButtons.OK);
                    }
                    catch(Exception ex)
                    {

                        bdsNhanVien.RemoveCurrent();
                        MessageBox.Show("Thất bại. Vui lòng kiểm tra lại!\n" + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void dteNGAYSINH_Validating(object sender, CancelEventArgs e)
        {
        }

        public void chuyenChiNhanh(String chiNhanh )
        {
            /*B1 : kiêm tra xem có nằm trên cùng chi nhánh không*/
            if ( Program.serverName == chiNhanh)
            {
                MessageBox.Show("Hãy chọn chi nhánh khác chi nhánh bạn đang đăng nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            /*B2 : chuẩn bị các biến để lưu tên chi nhánh hiện tại và chi nhánh chuyển tới, tên nhân viên được chuyển*/
            String maChiNhanhHienTai = "";
            String maChiNhanhMoi = "";
            int viTriHienTai = bdsNhanVien.Position;
            String maNhanVien = ((DataRowView)bdsNhanVien[viTriHienTai])["MANV"].ToString();

            if (chiNhanh.Contains("1"))
            {
                maChiNhanhHienTai = "CN2";
                maChiNhanhMoi = "CN1";
            }
            else if( chiNhanh.Contains("2"))
            {
                maChiNhanhHienTai = "CN1";
                maChiNhanhMoi = "CN2";
            }
            else
            {
                MessageBox.Show("Mã chi nhánh không hợp lệ","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            Console.WriteLine("Ma chi nhanh hien tai : " + maChiNhanhHienTai);
            Console.WriteLine("Ma chi nhanh Moi : " + maChiNhanhMoi);

            /*B3: trước khi thực hiện, lưu sẵn câu lệnh hoàn tác vào undoList + tên chi nhánh tới*/
            String cauTruyVanHoanTac = "EXEC sp_ChuyenChiNhanh "+maNhanVien+",'"+maChiNhanhHienTai+"'";
            undoList.Push(cauTruyVanHoanTac);

            Program.serverNameLeft = chiNhanh; /*Lấy tên chi nhánh tới để làm tính năng hoàn tác*/
            Console.WriteLine("Ten server con lai" + Program.serverNameLeft);

            /*B4: thực hiện chuyển chi nhánh với sp_ChuyenChiNhanh*/
            String cauTruyVan = "EXEC sp_ChuyenChiNhanh " + maNhanVien + ",'" + maChiNhanhMoi + "'";
            Console.WriteLine("Cau Truy Van: " + cauTruyVan);
            Console.WriteLine("Cau Truy Van Hoan Tac: " + cauTruyVanHoanTac);
            Console.WriteLine("cauTruyVan : " + cauTruyVan);

            SqlCommand sqlcommand = new SqlCommand(cauTruyVan, Program.conn);
            try
            {
                Program.myReader = Program.ExecSqlDataReader(cauTruyVan);
                MessageBox.Show("Chuyển chi nhánh thành công", "thông báo", MessageBoxButtons.OK);

                /*FIX LỖI CHUYÊN CHỈ NHÁNH VẪN GIỮ NGUYÊN VỊ TRÍ CON TRỎ MÀ VẪN THAY ĐỔI DỮ LIỆU*/
                this.nhanVienTableAdapter.Fill(this.dataSet.NhanVien);
                bdsNhanVien.Position = viTriHienTai;

                if (Program.myReader == null)
                {
                    return;/*khong co ket qua tra ve thi ket thuc luon*/
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("thực thi database thất bại!\n\n" + ex.Message, "thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
                return;
            }
            this.nhanVienTableAdapter.Update(this.dataSet.NhanVien);
        }

        private void btnCHUYENCHINHANH_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int viTriHienTai = bdsNhanVien.Position;
            int trangThaiXoa = 0;
            string testtrangThaiXoa = ((DataRowView)(bdsNhanVien[viTriHienTai]))["TrangThaiXoa"].ToString();
            if (testtrangThaiXoa == "False") trangThaiXoa = 0;
            if (testtrangThaiXoa == "True") trangThaiXoa = 1;

            string maNhanVien = ((DataRowView)(bdsNhanVien[viTriHienTai]))["MANV"].ToString();

            if( maNhanVien == Program.userName)
            {
                MessageBox.Show("Không thể chuyển chính người đang đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }    

            /*B1*/
            if ( trangThaiXoa == 1 )
            {
                MessageBox.Show("Nhân viên này không có ở chi nhánh này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            /*B2*/
            Form f = this.CheckExists(typeof(FrmChuyenChiNhanh));
            if (f != null)
            {
                f.Activate();
            }
            FrmChuyenChiNhanh form = new FrmChuyenChiNhanh();
            form.Show();

            /*B3*/
            /*đóng gói hàm chuyenChiNhanh từ formNHANVIEN đem về formChuyenChiNhanh để làm việc*/
            form.branchTransfer = new FrmChuyenChiNhanh.MyDelegate(chuyenChiNhanh);
            
            /*B4*/
            this.btnHOANTAC.Enabled = true;
        }

        private void txtMANV_TextChanged(object sender, EventArgs e)
        {

        }

        private void panelNhapLieu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lUONGLabel_Click(object sender, EventArgs e)
        {

        }

        private void gcNhanVien_Click(object sender, EventArgs e)
        {

        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtLUONG_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txtHO_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
