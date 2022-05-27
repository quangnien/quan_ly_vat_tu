using DevExpress.XtraGrid;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace QLTVT
{
    public partial class FormPhieuNhap : Form
    {
        /* vị trí của con trỏ trên grid view*/
        int viTri = 0;
        int viTriPhieuNhap = 0;
        int viTriCTPN = 0;

        bool dangThemMoi = false;

        Stack undoList = new Stack();

        BindingSource bds = null; // chứa những dữ liệu hiện tại đang làm việc
        GridControl gc = null;    // chứa grid view đang làm việc
        string type = "";

        //Program.dangThemMoiPhieuNhap = false;
        public string makho = "";
        string maChiNhanh = "";


        /* Tránh việc người dùng ấn vào 1 form đến 2 lần */
        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }

        public FormPhieuNhap()
        {
            InitializeComponent();
        }

        private void phieuNhapBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsPhieuNhap.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet);
        }

        private void FormPhieuNhap_Load(object sender, EventArgs e)
        {
            /* B1*/
            /*không kiểm tra khóa ngoại nữa*/
            /*Vì vd trong DATHANG có 2 khóa ngoại là MAKHO và MANV, thì khi mà ta tải 
             đơn DATHANG vô mà đã tải MANV vô trước rồi thì OK, còn MAKHO chưa có
             -> bão lỗi. Mà form này chỉ nhập NHANVIEN, đâu lq tới KHO làm gì chả nhẽ
             bh lại tải KHO nữa thì mất công => KO KIỀM TRA RÀNG BUỘC KHÓA NGOẠI NỮA.*/
            dataSet.EnforceConstraints = false;

            this.chiTietPhieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            //MessageBox.Show("Program.connstr : " + Program.connstr, "Thông báo", MessageBoxButtons.OK);
            this.chiTietPhieuNhapTableAdapter.Fill(this.dataSet.CTPN);

            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuNhapTableAdapter.Fill(this.dataSet.PhieuNhap);

            /* B2*/
            cmbCHINHANH.DataSource = Program.bindingSource;/*sao chep bingding source tu form dang nhap*/
            cmbCHINHANH.DisplayMember = "TENCN";
            cmbCHINHANH.ValueMember = "TENSERVER";
            cmbCHINHANH.SelectedIndex = Program.brand;             
        }

        private void groupBoxDonDatHang_Enter(object sender, EventArgs e)
        {

        }

        private void dteNgay_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void cmbCHINHANH_SelectedIndexChanged_1(object sender, EventArgs e)
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
                MessageBox.Show("Xảy ra lỗi kết nối với chi nhánh hiện tại!", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                this.chiTietPhieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                this.chiTietPhieuNhapTableAdapter.Fill(this.dataSet.CTPN);

                this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuNhapTableAdapter.Fill(this.dataSet.PhieuNhap);
            }
        }

        private void btnCheDoPhieuNhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /* B0 */
            btnMENU.Links[0].Caption = "Phiếu Nhập";

            /* B1*/
            bds = bdsPhieuNhap;
            gc = gcPhieuNhap;
            
            /* B2 */
            /*ON : PN */
            txtMaPhieuNhap.Enabled = false;
            dteNgay.Enabled = false;

            txtMaDonDatHang.Enabled = false;
            btnChonDonHang.Enabled = false;

            txtMaNhanVien.Enabled = false;
            txtMaKho.Enabled = false;

            /*OFF : CTPN*/
            btnChonChiTietDonHang.Enabled = false;

            txtMaVatChiTietPhieuNhap.Enabled = false;
            txtSoLuongChiTietPhieuNhap.Enabled = false;
            txtDonGiaChiTietPhieuNhap.Enabled = false;

            /*ON : grid control*/
            gcPhieuNhap.Enabled = true;
            gcChiTietPhieuNhap.Enabled = true;

            /* B3 */
            if (Program.role == "CONGTY") //read, chuyển CN
            {
                cmbCHINHANH.Enabled = true;

                this.btnTHEM.Enabled = false;
                this.btnXOA.Enabled = false;
                this.btnGHI.Enabled = false;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnMENU.Enabled = true;
                this.btnTHOAT.Enabled = true;

                this.groupBoxPhieuNhap.Enabled = false; //tải gc lên nhưng chỉ read được thôi
            }

            if (Program.role == "CHINHANH" || Program.role == "USER") //thao tác, ko cho chuyển CN
            {
                cmbCHINHANH.Enabled = false; 

                this.btnTHEM.Enabled = true;
                bool turnOn = (bdsPhieuNhap.Count > 0) ? true : false;
                this.btnXOA.Enabled = turnOn;
                this.btnGHI.Enabled = true;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnMENU.Enabled = true;
                this.btnTHOAT.Enabled = true;

                //this.txtMaDonDatHang.Enabled = false;

            }
        }

        private void btnCheDoChiTietPhieuNhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            /* B0 */
            btnMENU.Links[0].Caption = "Chi Tiết Phiếu Nhập";

            /* B1*/
            bds = bdsChiTietPhieuNhap;
            gc = gcPhieuNhap;

            /* B2 */
            /* ON : CTPN*/
            txtMaPhieuNhap.Enabled = false;
            dteNgay.Enabled = false;

            txtMaNhanVien.Enabled = false;

            txtMaKho.Enabled = false;
            btnChonDonHang.Enabled = false;

            /*ON : CTDDH*/
            txtMaDonDatHang.Enabled = false;
            btnChonChiTietDonHang.Enabled = true;

            txtMaVatTu.Enabled = false;
            /*FIX LỖI CHO EDIT */
            txtSoLuongChiTietPhieuNhap.Enabled = true;
            txtDonGiaChiTietPhieuNhap.Enabled = true;

            /* ON: grid control */
            gcPhieuNhap.Enabled = true;
            gcChiTietPhieuNhap.Enabled = true;

            /*Step 3*/
            if (Program.role == "CONGTY")
            {
                cmbCHINHANH.Enabled = true;

                this.btnTHEM.Enabled = false;
                this.btnXOA.Enabled = false;
                this.btnGHI.Enabled = false;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnMENU.Enabled = true;
                this.btnTHOAT.Enabled = true;
            }

            if (Program.role == "CHINHANH" || Program.role == "USER")
            {
                cmbCHINHANH.Enabled = false;

                this.btnTHEM.Enabled = true;
                this.btnXOA.Enabled = true;
                this.btnGHI.Enabled = true;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnMENU.Enabled = true;
                this.btnTHOAT.Enabled = true;
            }
        }

        private void btnChonDonHang_Click(object sender, EventArgs e)
        {
            FrmChonDonDatHang form = new FrmChonDonDatHang();
            form.ShowDialog();

            this.txtMaDonDatHang.Text = Program.maDonDatHangDuocChon;
            this.txtMaKho.Text = Program.maKhoDuocChon;
        }

        private void btnTHEM_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /* B1*/
            viTri = bds.Position;
            dangThemMoi = true;
            Program.dangThemMoiPhieuNhap = true;

            /* B2*/
            bds.AddNew();
            if (btnMENU.Links[0].Caption == "Phiếu Nhập")
            {
                this.txtMaPhieuNhap.Enabled = true;

                this.dteNgay.EditValue = DateTime.Now;
                this.dteNgay.Enabled = false;

                this.txtMaNhanVien.Text = Program.userName;
                this.txtMaKho.Text = Program.maKhoDuocChon;

                this.txtMaDonDatHang.Enabled = false;
                this.btnChonDonHang.Enabled = true;

                /*gán tự động*/
                ((DataRowView)(bdsPhieuNhap.Current))["NGAY"] = DateTime.Now;
                ((DataRowView)(bdsPhieuNhap.Current))["MasoDDH"] = Program.maDonDatHangDuocChon;
                ((DataRowView)(bdsPhieuNhap.Current))["MANV"] = Program.userName;
                ((DataRowView)(bdsPhieuNhap.Current))["MAKHO"] = Program.maKhoDuocChon;
            }

            if (btnMENU.Links[0].Caption == "Chi Tiết Phiếu Nhập")
            {
                DataRowView drv = ((DataRowView)bdsPhieuNhap[bdsPhieuNhap.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Bạn không thêm chi tiết phiếu nhập trên phiếu không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    bdsChiTietPhieuNhap.RemoveCurrent();
                    return;
                }

                /*Gán tự động*/
                ((DataRowView)(bdsChiTietPhieuNhap.Current))["MAPN"] = ((DataRowView)(bdsPhieuNhap.Current))["MAPN"];
                ((DataRowView)(bdsChiTietPhieuNhap.Current))["MAVT"] = Program.maVatTuDuocChon;
                ((DataRowView)(bdsChiTietPhieuNhap.Current))["SOLUONG"] = Program.soLuongVatTu;
                ((DataRowView)(bdsChiTietPhieuNhap.Current))["DONGIA"] = Program.donGia;

                /*THÊM ĐỂ FIX LỖI KIỂM TRA CHITIETPHIEUNHAP đã tồn tại hay chưa với [SP_KiemTraChiTietPhieuNhapDaTonTaiHayChua]*/
                Program.maPhieuNhapDuocChon = (String)((DataRowView)(bdsPhieuNhap.Current))["MAPN"];

                this.txtMaVatTu.Enabled = false;
                this.btnChonChiTietDonHang.Enabled = true;

                this.txtSoLuong.Enabled = true;
                this.txtSoLuong.EditValue = 1;

                this.txtDonGia.Enabled = true;
                this.txtDonGia.EditValue = 1;

                this.txtSoLuongChiTietPhieuNhap.Enabled = true;
                this.txtDonGiaChiTietPhieuNhap.Enabled = true;
            }

            /* B3 */
            this.btnTHEM.Enabled = false;
            this.btnXOA.Enabled = false;
            this.btnGHI.Enabled = true;

            this.btnHOANTAC.Enabled = true;
            this.btnLAMMOI.Enabled = false;
            this.btnMENU.Enabled = false; // mode 
            this.btnTHOAT.Enabled = false;

            gcPhieuNhap.Enabled = false;
            gcChiTietPhieuNhap.Enabled = false;

            /* FIX LỖI UPDATE TRẠNG THÁI THÊM MỚI CHO CHẮC :) */
        }

        private void btnTHOAT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Dispose();
        }

        private void btnLAMMOI_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.phieuNhapTableAdapter.Fill(this.dataSet.PhieuNhap);
                this.chiTietPhieuNhapTableAdapter.Fill(this.dataSet.CTPN);

                dangThemMoi = false;
                Program.dangThemMoiPhieuNhap = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi làm mới dữ liệu\n\n" + ex.Message,"Thông Báo",MessageBoxButtons.OK);
                Console.WriteLine(ex.Message);
                return;
            }
        }

        private void btnChonChiTietDonHang_Click(object sender, EventArgs e)
        {
            if(null != ((DataRowView)(bdsChiTietPhieuNhap.Current))){

                /*FIX LỖI CHỈ CHỌN ĐƯỢC MAVATTU ĐANG EDIT KHI EDIT CTPN P2*/
                Program.MaVatTuDangCoOCTPN = ((DataRowView)(bdsChiTietPhieuNhap.Current))["MaVT"].ToString().Trim();
                /*Lay MasoDDH hien tai cua gcPhieuNhap de so sanh voi MasoDDH se duoc chon*/
                Program.maDonDatHangDuocChon = ((DataRowView)(bdsPhieuNhap.Current))["MasoDDH"].ToString().Trim();
             
                FrmChonChiTietDonHang form = new FrmChonChiTietDonHang();
                form.ShowDialog();

                this.txtMaVatChiTietPhieuNhap.Text = Program.maVatTuDuocChon;
                this.txtSoLuongChiTietPhieuNhap.Value = Program.soLuongVatTu;
                this.txtDonGiaChiTietPhieuNhap.Value = Program.donGia;
            }
            else
            {
                MessageBox.Show("Chưa có chi tiết phiếu nhập, mời bạn chọn chức năng THÊM!", "Thông báo", MessageBoxButtons.OK);
                return;
            }
        }



        /* B0 : trường hợp đã ấn btnTHEM nhưng chưa ấn btnGHI
         * B1 : kiểm tra undoList có trông hay không
         * B2 : Neu undoList khong trống thì lấy ra khôi phục*/
        private void btnHOANTAC_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*FIX LỖI VỊ TRÍ CON TRỎ : THÊM PHẦN NÀY Ở btnXoa_itemclick NỮA*/
            if(btnMENU.Links[0].Caption == "Phiếu Nhập") {
                viTriPhieuNhap = bds.Position;
            }
            if (btnMENU.Links[0].Caption == "Chi Tiết Phiếu Nhập")
            {
                viTriPhieuNhap = bdsPhieuNhap.Position;
                viTriCTPN = bds.Position;
            }

            /*FIX LỖI ẨN MAPHIEUNHAP để ko EDIT được*/
            this.txtMaPhieuNhap.Enabled = false;

            /* B0: trường hợp đã ấn btnTHEM nhưng chưa ấn btnGHI */
            if (dangThemMoi == true && this.btnTHEM.Enabled == false)
            {
                dangThemMoi = false;
                Program.dangThemMoiPhieuNhap = false;

                if (btnMENU.Links[0].Caption == "Phiếu Nhập")
                {
                    this.txtMaDonDatHang.Enabled = false;
                    dteNgay.Enabled = false;

                    txtMaDonDatHang.Enabled = false;
                    txtMaKho.Enabled = false;

                    btnChonDonHang.Enabled = false;
                    txtMaDonDatHang.Enabled = false;
                }
                if (btnMENU.Links[0].Caption == "Chi Tiết Phiếu Nhập")
                {
                    this.txtMaDonDatHang.Enabled = false;
                    this.btnChonChiTietDonHang.Enabled = true;

                    this.txtMaVatChiTietPhieuNhap.Enabled = false;

                    txtSoLuongChiTietPhieuNhap.Enabled = true;
                    txtDonGiaChiTietPhieuNhap.Enabled = true;
                    /*this.txtSoLuongChiTietPhieuNhap.Enabled = false;
                    this.txtDonGiaChiTietPhieuNhap.Enabled = false;*/

                    this.btnXOA.Enabled = false;
                }

                this.btnTHEM.Enabled = true;
                this.btnXOA.Enabled = true;
                this.btnGHI.Enabled = true;

                //this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnMENU.Enabled = true;
                this.btnTHOAT.Enabled = true;

                this.gcPhieuNhap.Enabled = true;
                this.gcChiTietPhieuNhap.Enabled = true;

                bds.CancelEdit();
                /*LỖI KHI HOÀN TÁC THỰC HIỆN CHỨC NĂNG ADD CHITIETPHIEUNHAP => ĐÃ FIX
                 => KO FIX ĐƯỢC, VÌ THẾ MUỐN THÊM MỚI THÌ ẤN CON TRỎ VÔ 1 NV NÀO ĐÓ R MỚI THÊM MỚI*/
                if (btnMENU.Links[0].Caption == "Phiếu Nhập")
                {
                    bds.RemoveCurrent();
                }
                /* trở về lúc đầu con trỏ đang đứng*/
                bds.Position = viTri;
                return;
            }

            /* B1 : kiểm tra undoList có trông hay không ?*/
            if (undoList.Count == 0)
            {
                MessageBox.Show("Không còn thao tác nào để khôi phục", "Thông báo", MessageBoxButtons.OK);
                btnHOANTAC.Enabled = false;
                return;
            }

            /* B2:  Neu undoList khong trống thì lấy ra khôi phục*/
            bds.CancelEdit();
            String cauTruyVanHoanTac = undoList.Pop().ToString();

            Console.WriteLine(cauTruyVanHoanTac);
            Console.WriteLine("cauTruyVanHoanTac" + cauTruyVanHoanTac , "Thông báo", MessageBoxButtons.OK);
            int n = Program.ExecSqlNonQuery(cauTruyVanHoanTac);

            this.phieuNhapTableAdapter.Fill(this.dataSet.PhieuNhap);
            this.chiTietPhieuNhapTableAdapter.Fill(this.dataSet.CTPN);

            //FIX LỖI HOÀN TÁC CHỨC NĂNG SAU KHI UPDATE -> TRỎ ĐẾN ĐÚNG VỊ TRÍ 
            if (cauTruyVanHoanTac.Contains("DBO.CTPN")){
                bdsPhieuNhap.Position = viTriPhieuNhap;
                bds.Position = viTriCTPN;
                return;
            }

            //FIX LỖI HOÀN TÁC CHỨC NĂNG SAU KHI THÊM -> TRỎ ĐẾN ĐÚNG VỊ TRÍ 
            if (cauTruyVanHoanTac.Contains("DBO.PHIEUNHAP")){
                bds.Position = viTriPhieuNhap;
                //bdsPhieuNhap.Position = viTri;
                return;
            }
        }

        private void capNhatSoLuongVatTu(string mode, string maVatTu, int soLuong){
            string cauTruyVan = "EXEC sp_UpdateSoLuongTonCuaVatTu '" + mode + "', '" + maVatTu + "', " + soLuong;
            int n = Program.ExecSqlNonQuery(cauTruyVan);
            Console.WriteLine(cauTruyVan);
        }

        private String taoCauTruyVanHoanTac(String cheDo)
        {
            String cauTruyVan = "";
            DataRowView drv;

            /*TH1: dang sua phieu nhap - nhung ko co truong du lieu nao co the cho sua duoc ca*/
            if(cheDo == "Phiếu Nhập" && dangThemMoi == false)
            {
            }

            /*TH2: them moi phieu nhap*/
            if(cheDo == "Phiếu Nhập" && dangThemMoi == true)
            {
                // tao trong btnGHI thi hon
            }

            /*TH3: them moi chi tiet phieu nhap*/
            if (cheDo == "Chi Tiết Phiếu Nhập" && dangThemMoi == true)
            {
                // tao trong btnGHI thi hon
            }

            /*TH4: dang sua chi tiet phieu nhap*/
            if (cheDo == "Chi Tiết Phiếu Nhập" && dangThemMoi == false)
            {
                drv = ((DataRowView)(bdsChiTietPhieuNhap.Current));
                float donGia = float.Parse(drv["DONGIA"].ToString().Trim());
                String maPhieuNhap = drv["MAPN"].ToString().Trim();
                String maVatTu = drv["MAVT"].ToString().Trim();

                /*FIX LỖI UPDATE ĐƯỢC SỐ LƯỢNG TỒN KHI HOÀN UPDATE SỐ LƯỢNG*/
                string demsoluongcu = " SELECT SOLUONG FROM DBO.CTPN " +
                                      "WHERE MAPN = '" + maPhieuNhap + "' " +
                                      "AND MAVT = '" + maVatTu + "' ";
                SqlCommand sqlCommand = new SqlCommand(demsoluongcu, Program.conn);
                try{
                    Program.myReader = Program.ExecSqlDataReader(demsoluongcu);
                }
                catch (Exception ex){
                    MessageBox.Show("Thực thi database thất bại!\n\n" + ex.Message, "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Program.myReader.Read();
                int SOLUONGBANDAU = int.Parse(Program.myReader.GetValue(0).ToString());
                Program.myReader.Close();

                int soluongthaydoi = (int)txtSoLuongChiTietPhieuNhap.Value;
                int soluongchenhlenh = SOLUONGBANDAU - soluongthaydoi;
                cauTruyVan = "UPDATE DBO.CTPN " +
                    "SET " +
                    "SOLUONG = " + SOLUONGBANDAU + ", " +
                    "DONGIA = " + donGia + " " +
                    "WHERE MAPN = '" + maPhieuNhap + "' " +
                    "AND MAVT = '" + maVatTu + "' " +
                    " UPDATE DBO.Vattu " +
                    "SET SOLUONGTON = SOLUONGTON + " + soluongchenhlenh + 
                    " WHERE MAVT = '" + maVatTu + "'";
            }
            return cauTruyVan;
        }

        private bool kiemTraDuLieuDauVao(String cheDo)
        {
            if( cheDo == "Phiếu Nhập")
            {
                if( txtMaPhieuNhap.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã phiếu nhập !","Thông báo",MessageBoxButtons.OK);
                    txtMaPhieuNhap.Focus();
                    return false;
                }
                if (Regex.IsMatch(txtMaPhieuNhap.Text, @"^[a-zA-Z0-9]+$") == false)
                {
                    MessageBox.Show("Mã phiếu nhập chỉ chấp nhận số, chữ (A-Z, a-z), không chấp nhận khoảng trắng và ký tự đặc biệt!", "Thông báo", MessageBoxButtons.OK);
                    txtMaPhieuNhap.Focus();
                    return false;
                }
                if (txtMaPhieuNhap.Text.Length > 8)
                {
                    MessageBox.Show("Mã phiếu nhập không thể lớn hơn 8 kí tự", "Thông báo", MessageBoxButtons.OK);
                    txtMaPhieuNhap.Focus();
                    return false;
                }

                if (txtMaNhanVien.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã nhân viên !", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }

                if (txtMaKho.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã kho !", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }

                if (txtMaDonDatHang.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã đơn đặt hàng !", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
            }

            if (cheDo == "Chi Tiết Phiếu Nhập")
            {
                if (txtMaVatChiTietPhieuNhap.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã vật tư !", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }

                if ((txtSoLuongChiTietPhieuNhap.Value < 0 || 
                    txtSoLuongChiTietPhieuNhap.Value > Program.soLuongVatTu))
                {
                    MessageBox.Show("Số lượng vật tư không thể lớn hơn số lượng vật tư trong chi tiết đơn hàng !", "Thông báo", MessageBoxButtons.OK);
                    txtSoLuongChiTietPhieuNhap.Focus();
                    return false;
                }

                /*
                if (dangThemMoi == false){
                    MessageBox.Show("Mời bạn chọn chi tiết đơn hàng trước!", "Thông báo", MessageBoxButtons.OK);
                    txtSoLuongChiTietPhieuNhap.Focus();
                    return false;
                }*/

                if (txtDonGiaChiTietPhieuNhap.Value < 1 )
                {
                    MessageBox.Show("Đơn giá không thể nhỏ hơn 1 VND", "Thông báo", MessageBoxButtons.OK);
                    txtDonGiaChiTietPhieuNhap.Focus();
                    return false;
                }
            }

            return true;
        }

        private void btnGHI_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /* B1: xác định MODE*/
            String cheDo = (btnMENU.Links[0].Caption == "Phiếu Nhập") ? "Phiếu Nhập" : "Chi Tiết Phiếu Nhập";

            /* B2: Kiểm tra dữ liệu đầu vào*/
            bool ketQua = kiemTraDuLieuDauVao(cheDo);
            if (ketQua == false) return;

            /* B3 : Tạo truy vấn hoàn tác*/
            string cauTruyVanHoanTac = taoCauTruyVanHoanTac(cheDo);

            /* B4: Kiểm tra SP ?*/
            String maPhieuNhap = txtMaPhieuNhap.Text.Trim();
            //Console.WriteLine(maPhieuNhap);
            String cauTruyVan =
                    "DECLARE	@result int " +
                    "EXEC @result = SP_KiemTraMaPhieuNhapPhieuXuatDaTonTaiHayChua NHAP, '" +
                    maPhieuNhap + "' " +
                    "SELECT 'Value' = @result";
            SqlCommand sqlCommand = new SqlCommand(cauTruyVan, Program.conn);
            try
            {
                Program.myReader = Program.ExecSqlDataReader(cauTruyVan);
                if (Program.myReader == null) return;
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

            /* B5 : xử lý dữ liệu nếu có */
            int viTriConTro = bdsPhieuNhap.Position; //đang nhập vô
            int viTriMaPhieuNhap = bdsPhieuNhap.Find("MAPN", maPhieuNhap); //đã tồn tại

            /*Dang them moi phieu nhap*/
            if( result == 1 && cheDo == "Phiếu Nhập" && viTriMaPhieuNhap != viTriConTro)
            {
                MessageBox.Show("Mã phiếu nhập đã được sử dụng!", "Thông báo", MessageBoxButtons.OK);
                txtMaPhieuNhap.Focus();
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
                        /*TH1: them moi phieu nhap*/
                        if (cheDo == "Phiếu Nhập" && dangThemMoi == true)
                        {
                            cauTruyVanHoanTac =
                                "DELETE FROM DBO.PHIEUNHAP " +
                                "WHERE MAPN = '" + maPhieuNhap + "'";
                        }

                        /*TH2: them moi chi tiet PHIẾU NHẬP*/
                        if (cheDo == "Chi Tiết Phiếu Nhập" && dangThemMoi == true)
                        {
                            string maVatTu = txtMaVatChiTietPhieuNhap.Text.Trim();
                            int soLuong = (int)txtSoLuongChiTietPhieuNhap.Value;

                            cauTruyVanHoanTac =
                                "DELETE FROM DBO.CTPN " +
                                "WHERE MAPN = '" + maPhieuNhap + "' " +
                                "AND MAVT = '" + Program.maVatTuDuocChon + "'" +
                                " UPDATE DBO.Vattu " + //FIX LỖI THÊM HOÀN TÁC LẠI THÌ PHẢI UPDATE SỐ LƯỢNG TỒN
                                "SET SOLUONGTON = SOLUONGTON - " + soLuong +
                                "WHERE MAVT = '" + maVatTu + "'";

                            capNhatSoLuongVatTu("NHAPVATTU", maVatTu, soLuong);
                        }

                        /*TH3: chinh sua phieu nhap -> chang co gi co the chinh sua chang can xu ly*/

                        /*TH4: chinh sua chi tiet phieu nhap*/
                        /*FIX LỖI UPDATE ĐƯỢC SỐ LƯỢNG TỒN KHI UPDATE SỐ LƯỢNG*/
                        if (cheDo == "Chi Tiết Phiếu Nhập" && dangThemMoi == false)
                        {
                            DataRowView drv = ((DataRowView)(bdsChiTietPhieuNhap.Current));
                            String maVatTu = drv["MAVT"].ToString().Trim();

                            string demsoluongcu = " SELECT SOLUONG FROM DBO.CTPN " +
                                                  "WHERE MAPN = '" + maPhieuNhap + "' " +
                                                  "AND MAVT = '" + maVatTu + "' ";
                            sqlCommand = new SqlCommand(demsoluongcu, Program.conn);
                            try{
                                Program.myReader = Program.ExecSqlDataReader(demsoluongcu);
                            }
                            catch (Exception ex) {
                                MessageBox.Show("Thực thi database thất bại!\n\n" + ex.Message, "Thông báo",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            Program.myReader.Read();
                            int SOLUONGBANDAU = int.Parse(Program.myReader.GetValue(0).ToString());
                            Program.myReader.Close();

                            int soluongthaydoi = (int)txtSoLuongChiTietPhieuNhap.Value;
                            int soluongchenhlenh = SOLUONGBANDAU - soluongthaydoi;

                            capNhatSoLuongVatTu("UPDATENHAP", maVatTu, soluongchenhlenh);
                        }

                        /*-------------*/
                        undoList.Push(cauTruyVanHoanTac);
                        Console.WriteLine("cau truy van hoan tac");
                        Console.WriteLine(cauTruyVanHoanTac);

                        this.bdsPhieuNhap.EndEdit();
                        this.bdsChiTietPhieuNhap.EndEdit();
                        this.phieuNhapTableAdapter.Update(this.dataSet.PhieuNhap);
                        this.chiTietPhieuNhapTableAdapter.Update(this.dataSet.CTPN);

                        this.btnTHEM.Enabled = true;
                        this.btnXOA.Enabled = true;
                        this.btnGHI.Enabled = true;

                        this.btnHOANTAC.Enabled = true;
                        this.btnLAMMOI.Enabled = true;
                        this.btnMENU.Enabled = true;
                        this.btnTHOAT.Enabled = true;

                        this.gcPhieuNhap.Enabled = true;
                        this.gcChiTietPhieuNhap.Enabled = true;

                        this.txtSoLuongChiTietPhieuNhap.Enabled = true;
                        this.txtDonGiaChiTietPhieuNhap.Enabled = true;
                        /*cập nhật lại trạng thái thêm mới cho chắc*/
                        dangThemMoi = false;
                        Program.dangThemMoiPhieuNhap = false;
                        MessageBox.Show("Ghi thành công", "Thông báo", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        bds.RemoveCurrent();
                        MessageBox.Show("Da xay ra loi !\n\n" + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }    
            
        }

        private void btnXOA_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRowView drv;
            string cauTruyVanHoanTac = "";
            string cheDo = (btnMENU.Links[0].Caption == "Phiếu Nhập") ? "Phiếu Nhập" : "Chi Tiết Phiếu Nhập";


            if (btnMENU.Links[0].Caption == "Phiếu Nhập")
            {
                viTriPhieuNhap = bds.Position;
            }
            if (btnMENU.Links[0].Caption == "Chi Tiết Phiếu Nhập")
            {
                viTriPhieuNhap = bdsPhieuNhap.Position;
                viTriCTPN = bds.Position;
            }

            if (cheDo == "Phiếu Nhập")
            {
                drv = ((DataRowView)bdsPhieuNhap[bdsPhieuNhap.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Không xóa chi tiết phiếu xuất không phải do mình tạo!", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                if (bdsChiTietPhieuNhap.Count > 0)
                {
                    MessageBox.Show("Không thể xóa phiếu nhập vì có chi tiết phiếu nhập!", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                drv = ((DataRowView)bdsPhieuNhap[bdsPhieuNhap.Position]);
                DateTime ngay = ((DateTime)drv["NGAY"]);

                cauTruyVanHoanTac = "INSERT INTO DBO.PHIEUNHAP(MAPN, NGAY, MasoDDH, MANV, MAKHO) " +
                    "VALUES( '" + drv["MAPN"].ToString().Trim() + "', '"+
                    ngay.ToString("yyyy-MM-dd") + "', '" +
                    drv["MasoDDH"].ToString() + "', '" +
                    drv["MANV"].ToString() + "', '" +
                    drv["MAKHO"].ToString() + "')";
            }

            if(cheDo == "Chi Tiết Phiếu Nhập")
            {
                drv = ((DataRowView)bdsPhieuNhap[bdsPhieuNhap.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Bạn không xóa chi tiết phiếu nhập không phải do mình tạo!", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                /*FIX LỖI XÓA CTPX CUỐI CÙNG, VẪN ẤN THÊM NÚT XÓA*/
                try
                {
                    drv = ((DataRowView)bdsChiTietPhieuNhap[bdsChiTietPhieuNhap.Position]);
                    cauTruyVanHoanTac = "INSERT INTO DBO.CTPN (MAPN, MAVT, SOLUONG, DONGIA) " +
                        "VALUES('" + drv["MAPN"].ToString().Trim() + "', '" +
                        drv["MAVT"].ToString().Trim() + "', " +
                        drv["SOLUONG"].ToString().Trim() + ", " +
                        drv["DONGIA"].ToString().Trim() + ") " +
                        " UPDATE DBO.Vattu " +                    //FIX LỖI HOÀN TÁC SAU KHI XÓA LẠI THÌ PHẢI UPDATE SỐ LƯỢNG TỒN
                        "SET SOLUONGTON = SOLUONGTON + " + drv["SOLUONG"].ToString().Trim() +
                        "  WHERE MAVT = '" + drv["MAVT"].ToString().Trim() + "'";
                    MessageBox.Show("cauTruyVanHoanTac : " + cauTruyVanHoanTac, "Thông báo",
                     MessageBoxButtons.OKCancel);
                }
                catch
                {
                    return;
                }

                
            }

            undoList.Push(cauTruyVanHoanTac);

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không ?", "Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    viTri = bds.Position;
                    if (cheDo == "Phiếu Nhập")
                    {
                        bdsPhieuNhap.RemoveCurrent();
                    }
                    if (cheDo == "Chi Tiết Phiếu Nhập")
                    {
                        //FIX LỖI XÓA PN -> PHẢI XÓA LUÔN SL TỒN
                        drv = ((DataRowView)bdsChiTietPhieuNhap[bdsChiTietPhieuNhap.Position]);
                        capNhatSoLuongVatTu("DELETENHAP", drv["MAVT"].ToString().Trim(), int.Parse(drv["SOLUONG"].ToString().Trim()));

                        bdsChiTietPhieuNhap.RemoveCurrent();
                    }

                    this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.phieuNhapTableAdapter.Update(this.dataSet.PhieuNhap);

                    this.chiTietPhieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.chiTietPhieuNhapTableAdapter.Update(this.dataSet.CTPN);

                    dangThemMoi = false;
                    Program.dangThemMoiPhieuNhap = false;

                    MessageBox.Show("Xóa thành công ", "Thông báo", MessageBoxButtons.OK);
                    this.btnHOANTAC.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa phiếu nhập. Hãy thử lại\n" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.phieuNhapTableAdapter.Update(this.dataSet.PhieuNhap);

                    this.chiTietPhieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.chiTietPhieuNhapTableAdapter.Update(this.dataSet.CTPN);

                    bds.Position = viTri;
                    return;
                }
            }
            else
            {
                // xoa cau truy van hoan tac di
                undoList.Pop();
            }
        }

        private void txtMaVatChiTietPhieuNhap_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gcPhieuNhap_Click(object sender, EventArgs e)
        {

        }

        private void gcDDH_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtMaDonDatHang_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSoLuongChiTietPhieuNhap_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void gcChiTietPhieuNhap_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
