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

namespace QLTVT.SubForm
{
    public partial class FrmChuyenChiNhanh : DevExpress.XtraEditors.XtraForm
    {
        public FrmChuyenChiNhanh()
        {
            InitializeComponent();
        }

        private void FormChuyenChiNhanh_Load(object sender, EventArgs e)
        { 
            /*Lấy dữ liệu từ form đăng nhập đổ vào nhưng chỉ lấn đúng danh sách
             phân mảnh mà thôi*/
            cmbCHINHANH.DataSource  = Program.bindingSource.DataSource;
            /*sao chep bingding source tu form dang nhap*/
            cmbCHINHANH.DisplayMember = "tencn";
            cmbCHINHANH.ValueMember = "tenserver";
            cmbCHINHANH.SelectedIndex = Program.brand;

        }

        /************************************************************
         * tạo delegate - một cái biến mà khi được gọi, nó sẽ thực hiện 1 hàm(function) khác
         * Ví dụ: ở class formNhanVien, ta có hàm chuyển chi nhánh, hàm này cần 1 tham số, chính
         * là tên server được chọn ở formChuyenChiNhanh này. Để gọi được hàm chuyển chi nhánh ở formNHANVIEN
         * Chúng ta khai báo 1 delegate là branchTransfer để gọi hàm chuyển chi nhánh về form này
         *************************************************************/
        public delegate void MyDelegate(string chiNhanh);
        public MyDelegate branchTransfer;
        private void btnXACNHAN_Click(object sender, EventArgs e)
        {
            if (cmbCHINHANH.Text.Trim().Equals(""))
            {
                MessageBox.Show("Vui lòng chọn chi nhánh", "Thông báo", MessageBoxButtons.OK);
            }
            /*Step 2*/
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn chuyển nhân viên này đi ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if( dialogResult == DialogResult.OK)
            {
                branchTransfer(cmbCHINHANH.SelectedValue.ToString());
            }
                
            this.Dispose();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cCHINHANH_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbCHINHANH_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void btnTHOAT_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}