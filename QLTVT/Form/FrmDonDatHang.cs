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

namespace QLTVT
{

    public partial class FormDonDatHang : Form
    {
        int viTri = 0;
        int vitriddh = 0;
        int vitrictdh = 0;
        bool dangThemMoi = false;
        public string makho = "";
        string maChiNhanh = "";

        Stack undoList = new Stack();

        BindingSource bds = null;
        GridControl gc = null;
        string type = "";

        public FormDonDatHang()
        {
            InitializeComponent();
        }

        private void datHangBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsDonDatHang.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet);

        }

        private void FormDonDatHang_Load(object sender, EventArgs e)
        {

            /*Step 1*/
            dataSet.EnforceConstraints = false; // tắt kích hoặc các ràng buộc

            this.chiTietDonDatHangTableAdapter.Connection.ConnectionString = Program.connstr;
            this.chiTietDonDatHangTableAdapter.Fill(this.dataSet.CTDDH);

            this.donDatHangTableAdapter.Connection.ConnectionString = Program.connstr;
            this.donDatHangTableAdapter.Fill(this.dataSet.DatHang);

            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuNhapTableAdapter.Fill(this.dataSet.PhieuNhap);

            /*Step 2*/
            cmbCHINHANH.DataSource = Program.bindingSource;/*sao chep bingding source tu form dang nhap*/
            cmbCHINHANH.DisplayMember = "TENCN";
            cmbCHINHANH.ValueMember = "TENSERVER";
            cmbCHINHANH.SelectedIndex = Program.brand;

            bds = bdsDonDatHang;
            gc = gcDonDatHang;
        }

        private void sOLUONGSpinEdit_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void btnCheDoDonDatHang_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 0*/
            btnMENU.Links[0].Caption = "Đơn Đặt Hàng";

            /*Step 1*/
            bds = bdsDonDatHang;
            gc = gcDonDatHang;

            /*Step 2*/
            txtMaDonDatHang.Enabled = false;
            dteNGAY.Enabled = false;

            txtNhaCungCap.Enabled = true;
            txtMaNhanVien.Enabled = false;

            txtMaKho.Enabled = false;
            btnChonKhoHang.Enabled = true;

            /*Tat chuc nang cua chi tiet don hang*/
            txtMaVatTu.Enabled = false;
            btnChonVatTu.Enabled = false;
            txtSoLuong.Enabled = false;
            txtDonGia.Enabled = false;

            /*Bat cac grid control len*/
            gcDonDatHang.Enabled = true;
            gcChiTietDonDatHang.Enabled = true;


            /*Step 3*/
            if (Program.role == "CONGTY")
            {
                cmbCHINHANH.Enabled = true;

                this.btnTHEM.Enabled = false;
                this.btnXOA.Enabled = false;
                this.btnGHI.Enabled = false;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnMENU.Enabled = false;
                this.btnTHOAT.Enabled = true;

                this.groupBoxDonDatHang.Enabled = false;
            }

            if (Program.role == "CHINHANH" || Program.role == "USER")
            {
                cmbCHINHANH.Enabled = false;

                this.btnTHEM.Enabled = true;
                bool turnOn = (bdsDonDatHang.Count > 0) ? true : false;
                this.btnXOA.Enabled = turnOn;
                this.btnGHI.Enabled = true;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnMENU.Enabled = true;
                this.btnTHOAT.Enabled = true;

                this.txtMaDonDatHang.Enabled = false;
            }
        }

        private void btnCheDoChiTietDonDatHang_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 0*/
            btnMENU.Links[0].Caption = "Chi Tiết Đơn Đặt Hàng";

            /*Step 1*/
            bds = bdsChiTietDonDatHang;
            gc = gcChiTietDonDatHang; // chỗ này bên chi tiết phiếu nhập mà gc = gcPhieuNhap

            /*Step 2*/
            txtMaDonDatHang.Enabled = false;
            dteNGAY.Enabled = false;

            txtNhaCungCap.Enabled = false;
            txtMaNhanVien.Enabled = false;

            txtMaKho.Enabled = false;
            btnChonKhoHang.Enabled = false;

            /*Bat chuc nang cua chi tiet don hang*/
            txtMaVatTu.Enabled = false;
            btnChonVatTu.Enabled = false;
            txtSoLuong.Enabled = true;
            txtDonGia.Enabled = true;

            gcDonDatHang.Enabled = true;
            gcChiTietDonDatHang.Enabled = true;

            /*Step 3*/
            if (Program.role == "CONGTY")
            {
                cmbCHINHANH.Enabled = true;

                this.btnTHEM.Enabled = false;
                this.btnXOA.Enabled = false;
                this.btnGHI.Enabled = false;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnMENU.Enabled = false;
                this.btnTHOAT.Enabled = true;

                this.groupBoxDonDatHang.Enabled = false;
            }

            if (Program.role == "CHINHANH" || Program.role == "USER")
            {
                cmbCHINHANH.Enabled = false;

                this.btnTHEM.Enabled = true;
                bool turnOn = (bdsChiTietDonDatHang.Count > 0) ? true : false;
                this.btnXOA.Enabled = turnOn;

                this.btnGHI.Enabled = true;

                this.btnHOANTAC.Enabled = false;
                this.btnLAMMOI.Enabled = true;
                this.btnMENU.Enabled = true;
                this.btnTHOAT.Enabled = true;

                this.txtMaDonDatHang.Enabled = false;
            }
        }

        private void btnTHOAT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Dispose();
        }

        private void btnTHEM_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 1*/
            viTri = bds.Position;
            dangThemMoi = true;

            /*Step 2*/
            if (btnMENU.Links[0].Caption == "Đơn Đặt Hàng")
            {
                bdsDonDatHang.AddNew(); // đã thêm
                this.txtMaDonDatHang.Enabled = true;
                //this.txtMaKho.Text = "";
                this.dteNGAY.EditValue = DateTime.Now;
                this.dteNGAY.Enabled = false;
                this.txtNhaCungCap.Enabled = true;
                this.txtMaNhanVien.Text = Program.userName;
                this.btnChonKhoHang.Enabled = true;

                /*Gan tu dong may truong du lieu nay*/
                ((DataRowView)(bdsDonDatHang.Current))["MANV"] = Program.userName;
                ((DataRowView)(bdsDonDatHang.Current))["NGAY"] = DateTime.Now;
            }

            if (btnMENU.Links[0].Caption == "Chi Tiết Đơn Đặt Hàng")
            {
                bdsChiTietDonDatHang.AddNew();
                DataRowView drv = ((DataRowView)bdsDonDatHang[bdsDonDatHang.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien) 
                {
                    MessageBox.Show("Bạn không thêm chi tiết đơn hàng trên phiếu không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    bdsChiTietDonDatHang.RemoveCurrent();
                    return;
                }

                this.txtMaVatTu.Enabled = false;
                this.btnChonVatTu.Enabled = true;

                this.txtSoLuong.Enabled = true;
                this.txtSoLuong.EditValue = 1;

                this.txtDonGia.Enabled = true;
                this.txtDonGia.EditValue = 1;
            }

            /*Step 3*/
            this.btnTHEM.Enabled = false;
            this.btnXOA.Enabled = false;
            this.btnGHI.Enabled = true;

            this.btnHOANTAC.Enabled = true;
            this.btnLAMMOI.Enabled = false;
            this.btnMENU.Enabled = false;
            this.btnTHOAT.Enabled = false;
        }

        private bool kiemTraDuLieuDauVao(String cheDo)
        {
            if (cheDo == "Đơn Đặt Hàng")
            {
                if (txtMaDonDatHang.Text == "")
                {
                    MessageBox.Show("Không thể bỏ trống mã đơn hàng", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
                if (txtMaDonDatHang.Text.Length > 8)
                {
                    MessageBox.Show("Mã đơn đặt hàng không quá 8 kí tự", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
                if (txtMaNhanVien.Text == "")
                {
                    MessageBox.Show("Không thể bỏ trống mã nhân viên", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
                if (txtNhaCungCap.Text == "")
                {
                    MessageBox.Show("Không thể bỏ trống nhà cung cấp", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
                if (txtNhaCungCap.Text.Length > 100)
                {
                    MessageBox.Show("Tên nhà cung cấp không quá 100 kí tự", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
                if (txtMaKho.Text == "")
                {
                    MessageBox.Show("Không thể bỏ trống mã kho", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
            }

            if (cheDo == "Chi Tiết Đơn Đặt Hàng")
            {
                if (txtMaVatTu.Text == "")
                {
                    MessageBox.Show("Không thể bỏ trống mã vật tư", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
                if (txtSoLuong.Value <= 0 )
                {
                    MessageBox.Show("Số lượng không thể nhỏ hơn 1", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }

                if ( txtDonGia.Value <= 0)
                {
                    MessageBox.Show("Đơn giá không thể nhỏ hơn 1", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
                /*l
                if( txtSoLuong.Value > Program.soLuongVatTu)
                {
                    MessageBox.Show("Sô lượng đặt mua lớn hơn số lượng vật tư hiện có", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }*/

            }
            return true;
        }

        private String taoCauTruyVanHoanTac(String cheDo)
        {
            String cauTruyVan = "";
            DataRowView drv;

            if (cheDo == "Đơn Đặt Hàng" && dangThemMoi == false)
            {
                drv = ((DataRowView)bdsDonDatHang[bdsDonDatHang.Position]);
                vitriddh = bdsDonDatHang.Position; 

                /*Ngày cần được xử lý đặc biệt*/
                DateTime ngay = ((DateTime)drv["NGAY"]);
                // SỬA THÌ UPDATE
                cauTruyVan = "UPDATE DBO.DATHANG " +
                    "SET " +
                    "NGAY = CAST('" + ngay.ToString("yyyy-MM-dd") + "' AS DATETIME), " +
                    "NhaCC = '" + drv["NhaCC"].ToString().Trim() + "', " +
                    "MANV = '" + drv["MANV"].ToString().Trim() + "', " +
                    "MAKHO = '" + drv["MAKHO"].ToString().Trim() + "' " +
                    "WHERE MasoDDH = '" + drv["MasoDDH"].ToString().Trim() + "'";
            }
            if (cheDo == "Đơn Đặt Hàng" && dangThemMoi == true)
            {
                drv = ((DataRowView)bdsDonDatHang[bdsDonDatHang.Position]);
                DateTime ngay = ((DateTime)drv["NGAY"]);
                cauTruyVan = "INSERT INTO DBO.DATHANG(MasoDDH, NGAY, NhaCC, MaNV, MaKho) " +
                    "VALUES('" + drv["MasoDDH"] + "', '" +
                    ngay.ToString("yyyy-MM-dd") + "', '" +
                    drv["NhaCC"].ToString() + "', '" +
                    drv["MaNV"].ToString() + "', '" +
                    drv["MaKho"].ToString() + "' )";
            }

            if (cheDo == "Chi Tiết Đơn Đặt Hàng" && dangThemMoi == false)
            {
                drv = ((DataRowView)bdsChiTietDonDatHang[bdsChiTietDonDatHang.Position]);
                vitrictdh = bdsChiTietDonDatHang.Position; //
                cauTruyVan = "UPDATE DBO.CTDDH " +
                    "SET " +
                    "SOLUONG = " + drv["SOLUONG"].ToString() + " , " +
                    "DONGIA = " + drv["DONGIA"].ToString() + " " +
                    "WHERE MasoDDH = '" + drv["MasoDDH"].ToString().Trim() + "'" +
                    " AND MAVT = '" + drv["MAVT"].ToString().Trim() + "'";

            }
            if (cheDo == "Chi Tiết Đơn Đặt Hàng" && dangThemMoi == true)
            {
                drv = ((DataRowView)bdsChiTietDonDatHang[bdsChiTietDonDatHang.Position]);
                //      DateTime ngay = ((DateTime)drv["NGAY"]);
                cauTruyVan = "INSERT INTO DBO.CTDDH(MasoDDH, MAVT, SOLUONG, DONGIA)" +
                    "VALUES('" + drv["MasoDDH"] + "', '" +
                    drv["MAVT"].ToString() + "', '" +
                    drv["SOLUONG"].ToString() + "', '" +
                    drv["DONGIA"].ToString() + "' )";
            }
            return cauTruyVan;
        }
        private void btnGHI_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitriddh = bdsDonDatHang.Position;
            vitrictdh = bdsChiTietDonDatHang.Position;

            /*Step 1*/
            DataRowView drv = ((DataRowView)bdsDonDatHang[bdsDonDatHang.Position]);
            String maNhanVien = drv["MANV"].ToString();
            String maDonDatHang = drv["MasoDDH"].ToString().Trim();

            if (Program.userName != maNhanVien && dangThemMoi == false)
            {
                MessageBox.Show("Bạn không thể sửa phiếu do người khác lập", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            if (bdsChiTietDonDatHang.Count == 0 && btnMENU.Links[0].Caption == "Chi Tiết Đơn Đặt Hàng")
            {
                MessageBox.Show("Không có chi tiết đơn hàng nào để sửa", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            /*Step 2*/
            String cheDo = (btnMENU.Links[0].Caption == "Đơn Đặt Hàng") ? "Đơn Đặt Hàng" : "Chi Tiết Đơn Đặt Hàng";

            bool ketQua = kiemTraDuLieuDauVao(cheDo);
            if (ketQua == false) return;

            String cauTruyVanHoanTac = taoCauTruyVanHoanTac(cheDo);

            /*Step 3*/
            String maDonDatHangMoi = txtMaDonDatHang.Text;
            String cauTruyVan =
                   "DECLARE	@result int " +
                    "EXEC @result = sp_TimMaSoDonDangHang '" +
                    maDonDatHangMoi + "' " +
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

            String maVatTuMoi = txtMaVatTu.Text;
            String masoDDHMoi = txtMaDonDatHang.Text;
            String cauTruyVanVattu =
                   "DECLARE	@ketqua int " +
                    "EXEC @ketqua = sp_KiemTraMaVatTuOCTDD  '" + masoDDHMoi + "' , '" + maVatTuMoi + "'" +
                    "SELECT 'Value' = @ketqua";

            SqlCommand sqlCommand1 = new SqlCommand(cauTruyVanVattu, Program.conn);

            try
            {
                Program.myReader = Program.ExecSqlDataReader(cauTruyVanVattu);
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
            int ketqua = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();

            /*Step 4*/
            int viTriHienTai = bds.Position;
            int viTriMaDonDatHang = bdsDonDatHang.Find("MasoDDH", txtMaDonDatHang.Text);

            if (result == 1 && cheDo == "Đơn Đặt Hàng" && viTriHienTai != viTriMaDonDatHang)
            {
                MessageBox.Show("Mã đơn hàng này đã được sử dụng !\n\n", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (ketqua == 1 && cheDo == "Chi Tiết Đơn Đặt Hàng" && dangThemMoi == true)
            {
                MessageBox.Show("Không thể thêm trùng mã vật tư!\n\n", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        if (cheDo == "Đơn Đặt Hàng" && dangThemMoi == true)
                        {
                            cauTruyVanHoanTac =
                                "DELETE FROM DBO.DATHANG " +
                                "WHERE MasoDDH = '" + maDonDatHang + "'";
                        }
                        if (cheDo == "Chi Tiết Đơn Đặt Hàng" && dangThemMoi == true)
                        {
                            // gán auto
                            ((DataRowView)(bdsChiTietDonDatHang.Current))["MasoDDH"] = ((DataRowView)(bdsDonDatHang.Current))["MasoDDH"];
                            ((DataRowView)(bdsChiTietDonDatHang.Current))["MAVT"] = Program.maVatTuDuocChon;
                            ((DataRowView)(bdsChiTietDonDatHang.Current))["SOLUONG"] =
                                txtSoLuong.Value;
                            ((DataRowView)(bdsChiTietDonDatHang.Current))["DONGIA"] =
                                (int)txtDonGia.Value;

                            cauTruyVanHoanTac =
                                "DELETE FROM DBO.CTDDH " +
                                "WHERE MasoDDH = '" + maDonDatHang + "' " +
                                "AND MAVT = '" + txtMaVatTu.Text.Trim() + "'";
                        }

                        /*TH3: chinh sua don hang */

                        /*TH4: chinh sua chi tiet don hang - > thi chi can may dong lenh duoi la xong*/

                        undoList.Push(cauTruyVanHoanTac);

                        this.bdsDonDatHang.EndEdit();
                        this.bdsChiTietDonDatHang.EndEdit();
                        this.donDatHangTableAdapter.Update(this.dataSet.DatHang);
                        this.chiTietDonDatHangTableAdapter.Update(this.dataSet.CTDDH);

                        this.btnTHEM.Enabled = true;
                        this.btnXOA.Enabled = true;
                        this.btnGHI.Enabled = true;

                        this.btnHOANTAC.Enabled = true;
                        this.btnLAMMOI.Enabled = true;
                        this.btnMENU.Enabled = true;
                        this.btnTHOAT.Enabled = true;

                        dangThemMoi = false;
                        this.btnChonVatTu.Enabled = false;

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

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnHOANTAC_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /* Step 0 đã ấn thêm nhưng chưa ấn ghi  */
            if (dangThemMoi == true && this.btnTHEM.Enabled == false)
            {
                dangThemMoi = false;

                if (btnMENU.Links[0].Caption == "Đơn Đặt Hàng")
                {
                    this.txtMaDonDatHang.Enabled = false;
                    this.dteNGAY.Enabled = false;
                    this.txtNhaCungCap.Enabled = true;
                    this.btnChonKhoHang.Enabled = true;

                    // đã thêm
                    this.btnTHEM.Enabled = true;
                    this.btnXOA.Enabled = true;
                    this.btnGHI.Enabled = true;

                    this.btnLAMMOI.Enabled = true;
                    this.btnMENU.Enabled = true;
                    this.btnTHOAT.Enabled = true;

                    bdsDonDatHang.CancelEdit();
                    bdsDonDatHang.RemoveCurrent();
                    this.donDatHangTableAdapter.Fill(this.dataSet.DatHang);
                    bdsDonDatHang.Position = vitriddh;
                    return;
                }
                if (btnMENU.Links[0].Caption == "Chi Tiết Đơn Đặt Hàng")
                {
                    this.txtMaVatTu.Enabled = false;
                    this.btnChonVatTu.Enabled = false;

                    this.txtSoLuong.Enabled = true;
                    this.txtSoLuong.EditValue = 1;

                    this.txtDonGia.Enabled = true;
                    this.txtDonGia.EditValue = 1;

                    this.btnTHEM.Enabled = true;
                    this.btnXOA.Enabled = true;
                    this.btnGHI.Enabled = true;

                    this.btnLAMMOI.Enabled = true;
                    this.btnMENU.Enabled = true;
                    this.btnTHOAT.Enabled = true;

                    bdsChiTietDonDatHang.CancelEdit();

                    this.chiTietDonDatHangTableAdapter.Fill(this.dataSet.CTDDH); // đã thêm
                    bdsChiTietDonDatHang.Position = vitrictdh;
                    return;
                }
            }

            if (btnMENU.Links[0].Caption == "Đơn Đặt Hàng" && dangThemMoi == false)
            {
                bds.CancelEdit();
                this.donDatHangTableAdapter.Fill(this.dataSet.DatHang);
                this.chiTietDonDatHangTableAdapter.Fill(this.dataSet.CTDDH);
                bdsDonDatHang.Position = vitriddh;
            }

            if (btnMENU.Links[0].Caption == "Chi Tiết Đơn Đặt Hàng" && dangThemMoi == false)
            {
                bds.CancelEdit();
                this.donDatHangTableAdapter.Fill(this.dataSet.DatHang);
                this.chiTietDonDatHangTableAdapter.Fill(this.dataSet.CTDDH);
                bdsChiTietDonDatHang.Position = vitrictdh;
            }

            /*Step 1*/
            if (undoList.Count == 0)
            {
                MessageBox.Show("Không còn thao tác nào để khôi phục", "Thông báo", MessageBoxButtons.OK);
                btnHOANTAC.Enabled = false;
                return;
            }

            /*Step 2*/
            bds.CancelEdit();

            String cauTruyVanHoanTac = undoList.Pop().ToString();

            Console.WriteLine(cauTruyVanHoanTac);
            int n = Program.ExecSqlNonQuery(cauTruyVanHoanTac);

            this.donDatHangTableAdapter.Fill(this.dataSet.DatHang);
            this.chiTietDonDatHangTableAdapter.Fill(this.dataSet.CTDDH);

            this.btnChonVatTu.Enabled = false;

            bdsDonDatHang.Position = vitriddh;
            bdsChiTietDonDatHang.Position = vitrictdh;
        }

        private void btnLAMMOI_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.donDatHangTableAdapter.Fill(this.dataSet.DatHang);
                this.chiTietDonDatHangTableAdapter.Fill(this.dataSet.CTDDH);

                this.gcDonDatHang.Enabled = true;
                this.gcChiTietDonDatHang.Enabled = true;

                bdsDonDatHang.Position = viTri;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Làm mới" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                return;
            }
        }

        private void btnChonKhoHang_Click(object sender, EventArgs e)
        {
            FrmChonKhoHang form = new FrmChonKhoHang();
            form.ShowDialog();

            this.txtMaKho.Text = Program.maKhoDuocChon;
        }

        private void btnChonVatTu_Click(object sender, EventArgs e)
        {
            FrmChonVatTu form = new FrmChonVatTu();
            form.ShowDialog();
            this.txtMaVatTu.Text = Program.maVatTuDuocChon;
        }

        private void btnXOA_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string cauTruyVan = "";
            string cheDo = (btnMENU.Links[0].Caption == "Đơn Đặt Hàng") ? "Đơn Đặt Hàng" : "Chi Tiết Đơn Đặt Hàng";

            dangThemMoi = true;// bật cái này lên để ứng với điều kiện tạo câu truy vấn

            if (cheDo == "Đơn Đặt Hàng")
            {
                /*Cái bdsChiTietDonHangHang là đại diện cho binding source riêng biệt của CTDDH
                 *Còn cTDDHBindingSource là lấy ngay từ trong data source DATHANG
                 */
                if (bdsChiTietDonDatHang.Count > 0)
                {
                    MessageBox.Show("Không thể xóa đơn đặt hàng này vì có chi tiết đơn đặt hàng", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                if (bdsPhieuNhap.Count > 0)
                {
                    MessageBox.Show("Không thể xóa đơn đặt hàng này vì có phiếu nhập", "Thông báo", MessageBoxButtons.OK);
                    return;
                }


            }
            if (cheDo == "Chi Tiết Đơn Đặt Hàng")
            {
                DataRowView drv = ((DataRowView)bdsDonDatHang[bdsDonDatHang.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Bạn không xóa chi tiết đơn hàng trên phiếu không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    return;
                }
                if (bdsChiTietDonDatHang.Count == 0)
                {
                    MessageBox.Show("Không còn chi tiết đơn đặt hàng nào để xóa", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                /*FIX LỖI ĐƠN ĐH MÀ ĐƯỢC LẬP TRONG PHIẾU NHẬP THÌ KO CHO XÓA */
                DataRowView drvCT = ((DataRowView)bdsChiTietDonDatHang[bdsChiTietDonDatHang.Position]);
                String maDDH = drvCT["MasoDDH"].ToString();
               
                String maVT = drvCT["MaVT"].ToString();

                String cauTruyVantemp =
               "DECLARE	@result int " +
                "EXEC @result = sp_KiemTraRangBuocCTDDHDaDuocSuDungTrongPNHayChua '" +
                maDDH + "', '" + maVT + "'" +
                "SELECT 'Value' = @result";
                SqlCommand sqlCommand = new SqlCommand(cauTruyVantemp, Program.conn);

                try
                {
                    Program.myReader = Program.ExecSqlDataReader(cauTruyVantemp);
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

                if(result ==1)
                {
                    MessageBox.Show("Không thể xóa CTDDH vì đã tồn tại trong Phiếu nhập!", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                /*END FIX LỖI ĐƠN ĐH MÀ ĐƯỢC LẬP TRONG PHIẾU NHẬP THÌ KO CHO XÓA */
            }


            cauTruyVan = taoCauTruyVanHoanTac(cheDo);
            undoList.Push(cauTruyVan);

            /*Step 2*/
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không ?", "Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    /*Step 3*/
                    vitriddh = bdsDonDatHang.Position; 

                    if (cheDo == "Đơn Đặt Hàng")
                    {
                        bdsDonDatHang.RemoveCurrent();
                    }
                    vitrictdh = bdsChiTietDonDatHang.Position;
                    if (cheDo == "Chi Tiết Đơn Đặt Hàng")
                    {
                        bdsChiTietDonDatHang.RemoveCurrent();
                    }

                    this.donDatHangTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.donDatHangTableAdapter.Update(this.dataSet.DatHang);

                    this.chiTietDonDatHangTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.chiTietDonDatHangTableAdapter.Update(this.dataSet.CTDDH);

                    dangThemMoi = false;
                    MessageBox.Show("Xóa thành công ", "Thông báo", MessageBoxButtons.OK);
                    this.btnHOANTAC.Enabled = true;
                }
                catch (Exception ex)
                {
                    /*Step 4*/
                    MessageBox.Show("Lỗi xóa nhân viên. Hãy thử lại\n" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    this.donDatHangTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.donDatHangTableAdapter.Update(this.dataSet.DatHang);

                    this.chiTietDonDatHangTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.chiTietDonDatHangTableAdapter.Update(this.dataSet.CTDDH);
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
        private void cmbCHINHANH_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
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
                this.chiTietDonDatHangTableAdapter.Connection.ConnectionString = Program.connstr;
                this.chiTietDonDatHangTableAdapter.Fill(this.dataSet.CTDDH);

                this.donDatHangTableAdapter.Connection.ConnectionString = Program.connstr;
                this.donDatHangTableAdapter.Fill(this.dataSet.DatHang);

                this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuNhapTableAdapter.Fill(this.dataSet.PhieuNhap);
            }
        }

        private void gcDonDatHang_Click(object sender, EventArgs e)
        {

        }

        private void bdsChiTietDonDatHang_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}