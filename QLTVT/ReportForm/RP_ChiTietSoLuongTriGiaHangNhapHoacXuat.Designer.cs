
namespace QLTVT.ReportForm
{
    partial class RP_ChiTietSoLuongTriGiaHangNhapHoacXuat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery1 = new DevExpress.DataAccess.Sql.StoredProcQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter2 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter3 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter4 = new DevExpress.DataAccess.Sql.QueryParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RP_ChiTietSoLuongTriGiaHangNhapHoacXuat));
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailCaption1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData3_Odd = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.pageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.pageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.txtToiNgay = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.txtTuNgay = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.txtLoaiPhieu = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.label1 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.table1 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.table2 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.txtNam = new DevExpress.XtraReports.UI.XRLabel();
            this.txtThang = new DevExpress.XtraReports.UI.XRLabel();
            this.txtNgay = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.txtHoTen = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.calculatedField1 = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "QLTVT.Properties.Settings.QLVTConnectionString";
            this.sqlDataSource1.Name = "sqlDataSource1";
            storedProcQuery1.Name = "SP_BaoCaoChiTietSoLuongTriGiaHangHoaNhapHoacXuat";
            queryParameter1.Name = "@ROLE";
            queryParameter1.Type = typeof(string);
            queryParameter2.Name = "@TYPE";
            queryParameter2.Type = typeof(string);
            queryParameter3.Name = "@DATEFROM";
            queryParameter3.Type = typeof(System.DateTime);
            queryParameter3.ValueInfo = "1753-01-01";
            queryParameter4.Name = "@DATETO";
            queryParameter4.Type = typeof(System.DateTime);
            queryParameter4.ValueInfo = "1753-01-01";
            storedProcQuery1.Parameters.Add(queryParameter1);
            storedProcQuery1.Parameters.Add(queryParameter2);
            storedProcQuery1.Parameters.Add(queryParameter3);
            storedProcQuery1.Parameters.Add(queryParameter4);
            storedProcQuery1.StoredProcName = "SP_BaoCaoChiTietSoLuongTriGiaHangHoaNhapHoacXuat";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // Title
            // 
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.BorderColor = System.Drawing.Color.Black;
            this.Title.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Title.BorderWidth = 1F;
            this.Title.Font = new System.Drawing.Font("Arial", 14.25F);
            this.Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(70)))), ((int)(((byte)(80)))));
            this.Title.Name = "Title";
            // 
            // DetailCaption1
            // 
            this.DetailCaption1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(69)))), ((int)(((byte)(168)))));
            this.DetailCaption1.BorderColor = System.Drawing.Color.White;
            this.DetailCaption1.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.DetailCaption1.BorderWidth = 2F;
            this.DetailCaption1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.DetailCaption1.ForeColor = System.Drawing.Color.White;
            this.DetailCaption1.Name = "DetailCaption1";
            this.DetailCaption1.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailCaption1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailData1
            // 
            this.DetailData1.BorderColor = System.Drawing.Color.Transparent;
            this.DetailData1.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.DetailData1.BorderWidth = 2F;
            this.DetailData1.Font = new System.Drawing.Font("Arial", 8.25F);
            this.DetailData1.ForeColor = System.Drawing.Color.Black;
            this.DetailData1.Name = "DetailData1";
            this.DetailData1.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailData1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailData3_Odd
            // 
            this.DetailData3_Odd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(245)))), ((int)(((byte)(248)))));
            this.DetailData3_Odd.BorderColor = System.Drawing.Color.Transparent;
            this.DetailData3_Odd.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.DetailData3_Odd.BorderWidth = 1F;
            this.DetailData3_Odd.Font = new System.Drawing.Font("Arial", 8.25F);
            this.DetailData3_Odd.ForeColor = System.Drawing.Color.Black;
            this.DetailData3_Odd.Name = "DetailData3_Odd";
            this.DetailData3_Odd.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailData3_Odd.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // PageInfo
            // 
            this.PageInfo.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.PageInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(70)))), ((int)(((byte)(80)))));
            this.PageInfo.Name = "PageInfo";
            this.PageInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            // 
            // TopMargin
            // 
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.pageInfo1,
            this.pageInfo2});
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.StylePriority.UseBorderDashStyle = false;
            // 
            // pageInfo1
            // 
            this.pageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(5F, 5F);
            this.pageInfo1.Name = "pageInfo1";
            this.pageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.pageInfo1.SizeF = new System.Drawing.SizeF(315F, 23F);
            this.pageInfo1.StyleName = "PageInfo";
            // 
            // pageInfo2
            // 
            this.pageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(330F, 5F);
            this.pageInfo2.Name = "pageInfo2";
            this.pageInfo2.SizeF = new System.Drawing.SizeF(315F, 23F);
            this.pageInfo2.StyleName = "PageInfo";
            this.pageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.pageInfo2.TextFormatString = "Page {0} of {1}";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.txtToiNgay,
            this.xrLabel5,
            this.txtTuNgay,
            this.xrLabel4,
            this.txtLoaiPhieu,
            this.xrLabel1,
            this.label1});
            this.ReportHeader.HeightF = 140.7692F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // txtToiNgay
            // 
            this.txtToiNgay.BorderColor = System.Drawing.Color.Transparent;
            this.txtToiNgay.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtToiNgay.LocationFloat = new DevExpress.Utils.PointFloat(506.3821F, 86.1923F);
            this.txtToiNgay.Multiline = true;
            this.txtToiNgay.Name = "txtToiNgay";
            this.txtToiNgay.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.txtToiNgay.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.txtToiNgay.StylePriority.UseBorderColor = false;
            this.txtToiNgay.StylePriority.UseFont = false;
            this.txtToiNgay.StylePriority.UseTextAlignment = false;
            this.txtToiNgay.Text = "...";
            this.txtToiNgay.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel5
            // 
            this.xrLabel5.BorderColor = System.Drawing.Color.Transparent;
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(357.2846F, 86.1923F);
            this.xrLabel5.Multiline = true;
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel5.StylePriority.UseBorderColor = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "Tới ngày";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // txtTuNgay
            // 
            this.txtTuNgay.BorderColor = System.Drawing.Color.Transparent;
            this.txtTuNgay.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtTuNgay.LocationFloat = new DevExpress.Utils.PointFloat(180.9975F, 86.1923F);
            this.txtTuNgay.Multiline = true;
            this.txtTuNgay.Name = "txtTuNgay";
            this.txtTuNgay.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.txtTuNgay.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.txtTuNgay.StylePriority.UseBorderColor = false;
            this.txtTuNgay.StylePriority.UseFont = false;
            this.txtTuNgay.StylePriority.UseTextAlignment = false;
            this.txtTuNgay.Text = "...";
            this.txtTuNgay.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel4
            // 
            this.xrLabel4.BorderColor = System.Drawing.Color.Transparent;
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(36.15385F, 86.1923F);
            this.xrLabel4.Multiline = true;
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel4.StylePriority.UseBorderColor = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Từ ngày";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // txtLoaiPhieu
            // 
            this.txtLoaiPhieu.BorderColor = System.Drawing.Color.Transparent;
            this.txtLoaiPhieu.LocationFloat = new DevExpress.Utils.PointFloat(357.2846F, 45.42307F);
            this.txtLoaiPhieu.Multiline = true;
            this.txtLoaiPhieu.Name = "txtLoaiPhieu";
            this.txtLoaiPhieu.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.txtLoaiPhieu.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.txtLoaiPhieu.StylePriority.UseBorderColor = false;
            this.txtLoaiPhieu.StylePriority.UseTextAlignment = false;
            this.txtLoaiPhieu.Text = "....";
            this.txtLoaiPhieu.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel1
            // 
            this.xrLabel1.BorderColor = System.Drawing.Color.Transparent;
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(220F, 45.42307F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel1.StylePriority.UseBorderColor = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "LOẠI PHIẾU";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.LocationFloat = new DevExpress.Utils.PointFloat(5F, 5F);
            this.label1.Name = "label1";
            this.label1.SizeF = new System.Drawing.SizeF(640F, 24.19433F);
            this.label1.StyleName = "Title";
            this.label1.StylePriority.UseFont = false;
            this.label1.StylePriority.UseForeColor = false;
            this.label1.StylePriority.UseTextAlignment = false;
            this.label1.Text = "BẢNG KÊ CHI TIẾT SỐ LƯỢNG - TRỊ GIÁ HÀNG NHẬP HOẶC XUẤT";
            this.label1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table1});
            this.GroupHeader1.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WithFirstDetail;
            this.GroupHeader1.HeightF = 28F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // table1
            // 
            this.table1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.table1.Name = "table1";
            this.table1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow1});
            this.table1.SizeF = new System.Drawing.SizeF(650F, 28F);
            // 
            // tableRow1
            // 
            this.tableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell1,
            this.tableCell2,
            this.tableCell3,
            this.tableCell4});
            this.tableRow1.Name = "tableRow1";
            this.tableRow1.Weight = 1D;
            // 
            // tableCell1
            // 
            this.tableCell1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(148)))), ((int)(((byte)(130)))));
            this.tableCell1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.tableCell1.Name = "tableCell1";
            this.tableCell1.StyleName = "DetailCaption1";
            this.tableCell1.StylePriority.UseBackColor = false;
            this.tableCell1.StylePriority.UseBorders = false;
            this.tableCell1.StylePriority.UseTextAlignment = false;
            this.tableCell1.Text = "THỜI GIAN";
            this.tableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell1.Weight = 0.27845768855168268D;
            // 
            // tableCell2
            // 
            this.tableCell2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(148)))), ((int)(((byte)(130)))));
            this.tableCell2.Name = "tableCell2";
            this.tableCell2.StyleName = "DetailCaption1";
            this.tableCell2.StylePriority.UseBackColor = false;
            this.tableCell2.StylePriority.UseTextAlignment = false;
            this.tableCell2.Text = "TÊN VẬT TƯ";
            this.tableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell2.Weight = 0.17771971482496995D;
            // 
            // tableCell3
            // 
            this.tableCell3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(148)))), ((int)(((byte)(130)))));
            this.tableCell3.Name = "tableCell3";
            this.tableCell3.StyleName = "DetailCaption1";
            this.tableCell3.StylePriority.UseBackColor = false;
            this.tableCell3.StylePriority.UseTextAlignment = false;
            this.tableCell3.Text = "SỐ LƯỢNG";
            this.tableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell3.Weight = 0.36173213078425481D;
            // 
            // tableCell4
            // 
            this.tableCell4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(148)))), ((int)(((byte)(130)))));
            this.tableCell4.Name = "tableCell4";
            this.tableCell4.StyleName = "DetailCaption1";
            this.tableCell4.StylePriority.UseBackColor = false;
            this.tableCell4.StylePriority.UseTextAlignment = false;
            this.tableCell4.Text = "TRỊ GIÁ (VNĐ)";
            this.tableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell4.Weight = 0.18209040715144231D;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table2});
            this.Detail.HeightF = 25F;
            this.Detail.Name = "Detail";
            // 
            // table2
            // 
            this.table2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.table2.Name = "table2";
            this.table2.OddStyleName = "DetailData3_Odd";
            this.table2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow2});
            this.table2.SizeF = new System.Drawing.SizeF(650F, 25F);
            // 
            // tableRow2
            // 
            this.tableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell5,
            this.tableCell6,
            this.tableCell7,
            this.tableCell8});
            this.tableRow2.Name = "tableRow2";
            this.tableRow2.Weight = 11.5D;
            // 
            // tableCell5
            // 
            this.tableCell5.BorderColor = System.Drawing.Color.Black;
            this.tableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[THANGNAM]")});
            this.tableCell5.Name = "tableCell5";
            this.tableCell5.NullValueText = " ";
            this.tableCell5.ProcessDuplicatesMode = DevExpress.XtraReports.UI.ProcessDuplicatesMode.Merge;
            this.tableCell5.StyleName = "DetailData1";
            this.tableCell5.StylePriority.UseBorderColor = false;
            this.tableCell5.StylePriority.UseBorders = false;
            this.tableCell5.StylePriority.UseTextAlignment = false;
            this.tableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell5.Weight = 0.2784577120267428D;
            // 
            // tableCell6
            // 
            this.tableCell6.BorderColor = System.Drawing.Color.Black;
            this.tableCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TENVT]")});
            this.tableCell6.Name = "tableCell6";
            this.tableCell6.NullValueText = " ";
            this.tableCell6.StyleName = "DetailData1";
            this.tableCell6.StylePriority.UseBorderColor = false;
            this.tableCell6.StylePriority.UseBorders = false;
            this.tableCell6.Weight = 0.1777197265625D;
            // 
            // tableCell7
            // 
            this.tableCell7.BorderColor = System.Drawing.Color.Black;
            this.tableCell7.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TONGSOLUONG]")});
            this.tableCell7.Name = "tableCell7";
            this.tableCell7.NullValueText = " ";
            this.tableCell7.StyleName = "DetailData1";
            this.tableCell7.StylePriority.UseBorderColor = false;
            this.tableCell7.StylePriority.UseBorders = false;
            this.tableCell7.StylePriority.UseTextAlignment = false;
            this.tableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell7.TextFormatString = "{0:#,#}";
            this.tableCell7.Weight = 0.36173215425931493D;
            // 
            // tableCell8
            // 
            this.tableCell8.BorderColor = System.Drawing.Color.Black;
            this.tableCell8.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TRIGIA]")});
            this.tableCell8.Name = "tableCell8";
            this.tableCell8.NullValueText = " ";
            this.tableCell8.StyleName = "DetailData1";
            this.tableCell8.StylePriority.UseBorderColor = false;
            this.tableCell8.StylePriority.UseBorders = false;
            this.tableCell8.StylePriority.UseTextAlignment = false;
            this.tableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell8.TextFormatString = "{0:#,#}";
            this.tableCell8.Weight = 0.18209040715144231D;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.txtNam,
            this.txtThang,
            this.txtNgay,
            this.xrLabel10,
            this.xrLabel9,
            this.xrLabel8,
            this.txtHoTen,
            this.xrLabel6,
            this.xrLabel3,
            this.xrLabel2});
            this.ReportFooter.HeightF = 299.9616F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // txtNam
            // 
            this.txtNam.BorderColor = System.Drawing.Color.Transparent;
            this.txtNam.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtNam.LocationFloat = new DevExpress.Utils.PointFloat(586.087F, 78.89732F);
            this.txtNam.Multiline = true;
            this.txtNam.Name = "txtNam";
            this.txtNam.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.txtNam.SizeF = new System.Drawing.SizeF(40.57965F, 23.00002F);
            this.txtNam.StylePriority.UseBorderColor = false;
            this.txtNam.StylePriority.UseFont = false;
            this.txtNam.StylePriority.UseTextAlignment = false;
            this.txtNam.Text = "...";
            this.txtNam.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // txtThang
            // 
            this.txtThang.BorderColor = System.Drawing.Color.Transparent;
            this.txtThang.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtThang.LocationFloat = new DevExpress.Utils.PointFloat(519.2308F, 78.89732F);
            this.txtThang.Multiline = true;
            this.txtThang.Name = "txtThang";
            this.txtThang.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.txtThang.SizeF = new System.Drawing.SizeF(18.35876F, 23.00002F);
            this.txtThang.StylePriority.UseBorderColor = false;
            this.txtThang.StylePriority.UseFont = false;
            this.txtThang.StylePriority.UseTextAlignment = false;
            this.txtThang.Text = "...";
            this.txtThang.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // txtNgay
            // 
            this.txtNgay.BorderColor = System.Drawing.Color.Transparent;
            this.txtNgay.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtNgay.LocationFloat = new DevExpress.Utils.PointFloat(432.3741F, 78.89732F);
            this.txtNgay.Multiline = true;
            this.txtNgay.Name = "txtNgay";
            this.txtNgay.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.txtNgay.SizeF = new System.Drawing.SizeF(24.14127F, 23F);
            this.txtNgay.StylePriority.UseBorderColor = false;
            this.txtNgay.StylePriority.UseFont = false;
            this.txtNgay.StylePriority.UseTextAlignment = false;
            this.txtNgay.Text = "...";
            this.txtNgay.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel10
            // 
            this.xrLabel10.BorderColor = System.Drawing.Color.Transparent;
            this.xrLabel10.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(537.5895F, 78.89732F);
            this.xrLabel10.Multiline = true;
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(48.4975F, 23F);
            this.xrLabel10.StylePriority.UseBorderColor = false;
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = ", Năm";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel9
            // 
            this.xrLabel9.BorderColor = System.Drawing.Color.Transparent;
            this.xrLabel9.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(459.7437F, 78.89732F);
            this.xrLabel9.Multiline = true;
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(59.48709F, 23F);
            this.xrLabel9.StylePriority.UseBorderColor = false;
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = ", Tháng";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel8
            // 
            this.xrLabel8.BorderColor = System.Drawing.Color.Transparent;
            this.xrLabel8.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(373.7203F, 78.89732F);
            this.xrLabel8.Multiline = true;
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(58.65378F, 23F);
            this.xrLabel8.StylePriority.UseBorderColor = false;
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = " Ngày";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // txtHoTen
            // 
            this.txtHoTen.BorderColor = System.Drawing.Color.Transparent;
            this.txtHoTen.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.txtHoTen.LocationFloat = new DevExpress.Utils.PointFloat(437.5896F, 167.7949F);
            this.txtHoTen.Multiline = true;
            this.txtHoTen.Name = "txtHoTen";
            this.txtHoTen.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.txtHoTen.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.txtHoTen.StylePriority.UseBorderColor = false;
            this.txtHoTen.StylePriority.UseFont = false;
            this.txtHoTen.StylePriority.UseTextAlignment = false;
            this.txtHoTen.Text = "...";
            this.txtHoTen.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel6
            // 
            this.xrLabel6.BorderColor = System.Drawing.Color.Transparent;
            this.xrLabel6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(392.0536F, 113.8973F);
            this.xrLabel6.Multiline = true;
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(179.4871F, 23F);
            this.xrLabel6.StylePriority.UseBorderColor = false;
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Người lập hóa đơn";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel3
            // 
            this.xrLabel3.BorderColor = System.Drawing.Color.Transparent;
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(403.0769F, 24.73069F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(116.1538F, 23F);
            this.xrLabel3.StylePriority.UseBorderColor = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "TỔNG TRỊ GIÁ";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel2
            // 
            this.xrLabel2.BorderColor = System.Drawing.Color.Transparent;
            this.xrLabel2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Sum([TRIGIA])\n")});
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(550F, 24.73069F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel2.StylePriority.UseBorderColor = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "xrLabel2";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrLabel2.TextFormatString = "{0:#,#}";
            // 
            // calculatedField1
            // 
            this.calculatedField1.DataMember = "SP_BaoCaoChiTietSoLuongTriGiaHangHoaNhapHoacXuat";
            this.calculatedField1.Name = "calculatedField1";
            // 
            // RP_ChiTietSoLuongTriGiaHangNhapHoacXuat
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.GroupHeader1,
            this.Detail,
            this.ReportFooter});
            this.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.calculatedField1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "SP_BaoCaoChiTietSoLuongTriGiaHangHoaNhapHoacXuat";
            this.DataSource = this.sqlDataSource1;
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.DetailCaption1,
            this.DetailData1,
            this.DetailData3_Odd,
            this.PageInfo});
            this.Version = "19.2";
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.UI.XRControlStyle Title;
        private DevExpress.XtraReports.UI.XRControlStyle DetailCaption1;
        private DevExpress.XtraReports.UI.XRControlStyle DetailData1;
        private DevExpress.XtraReports.UI.XRControlStyle DetailData3_Odd;
        private DevExpress.XtraReports.UI.XRControlStyle PageInfo;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRPageInfo pageInfo1;
        private DevExpress.XtraReports.UI.XRPageInfo pageInfo2;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRLabel label1;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRTable table1;
        private DevExpress.XtraReports.UI.XRTableRow tableRow1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell2;
        private DevExpress.XtraReports.UI.XRTableCell tableCell3;
        private DevExpress.XtraReports.UI.XRTableCell tableCell4;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRTable table2;
        private DevExpress.XtraReports.UI.XRTableRow tableRow2;
        private DevExpress.XtraReports.UI.XRTableCell tableCell5;
        private DevExpress.XtraReports.UI.XRTableCell tableCell6;
        private DevExpress.XtraReports.UI.XRTableCell tableCell7;
        private DevExpress.XtraReports.UI.XRTableCell tableCell8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        public DevExpress.XtraReports.UI.XRLabel txtLoaiPhieu;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.CalculatedField calculatedField1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        public DevExpress.XtraReports.UI.XRLabel txtTuNgay;
        public DevExpress.XtraReports.UI.XRLabel txtToiNgay;
        public DevExpress.XtraReports.UI.XRLabel txtHoTen;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        public DevExpress.XtraReports.UI.XRLabel txtNam;
        public DevExpress.XtraReports.UI.XRLabel txtThang;
        public DevExpress.XtraReports.UI.XRLabel txtNgay;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
    }
}
