using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace QLTVT.ReportForm
{
    public partial class RP_DonHangKhongPhieuNhap : DevExpress.XtraReports.UI.XtraReport
    {
        public RP_DonHangKhongPhieuNhap()
        {
            InitializeComponent();
            this.sqlDataSource1.Connection.ConnectionString = Program.connstr;
            this.sqlDataSource1.Fill();
        }

    }
}
