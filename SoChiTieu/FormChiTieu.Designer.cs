namespace SoChiTieu;

partial class FormChiTieu
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    // Control
    private GroupBox grpInfo = null!, grpSearch = null!;
    private DateTimePicker dtpNgay = null!, dtpTu = null!, dtpDen = null!;
    private TextBox txtNoiDung = null!, txtGhiChu = null!, txtSearch = null!;
    private ComboBox cboLoai = null!, cboLoaiLoc = null!;
    private NumericUpDown nudSoTien = null!;
    private Button btnThem = null!, btnSua = null!, btnXoa = null!, btnLamMoi = null!, btnTim = null!;
    private DataGridView dgv = null!;
    private Label lblTong = null!;

    private static Label Lbl(string text, int x, int y) =>
        new() { Text = text, Left = x, Top = y, AutoSize = true };

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        SuspendLayout();

        // ---- Thông tin khoản chi ----
        grpInfo = new GroupBox { Text = "Thông tin khoản chi", Left = 10, Top = 5, Width = 415, Height = 175 };
        dtpNgay = new DateTimePicker { Left = 80, Top = 24, Width = 130, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" };
        txtNoiDung = new TextBox { Left = 80, Top = 56, Width = 320, MaxLength = 100 };
        cboLoai = new ComboBox { Left = 80, Top = 88, Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
        nudSoTien = new NumericUpDown
        {
            Left = 290, Top = 24, Width = 110, Minimum = 0, Maximum = 1_000_000_000,
            Increment = 1000, ThousandsSeparator = true
        };
        txtGhiChu = new TextBox { Left = 80, Top = 120, Width = 320, Height = 45, Multiline = true, ScrollBars = ScrollBars.Vertical, MaxLength = 200 };
        grpInfo.Controls.AddRange(new Control[]
        {
            Lbl("Ngày chi", 12, 28), dtpNgay, Lbl("Số tiền", 235, 28), nudSoTien,
            Lbl("Nội dung", 12, 60), txtNoiDung, Lbl("Loại chi", 12, 92), cboLoai,
            Lbl("Ghi chú", 12, 124), txtGhiChu
        });

        // ---- Tìm kiếm ----
        grpSearch = new GroupBox { Text = "Tìm kiếm", Left = 440, Top = 5, Width = 335, Height = 175 };
        txtSearch = new TextBox { Left = 80, Top = 24, Width = 240, PlaceholderText = "Nhập nội dung..." };
        cboLoaiLoc = new ComboBox { Left = 80, Top = 56, Width = 240, DropDownStyle = ComboBoxStyle.DropDownList };
        dtpTu = new DateTimePicker { Left = 80, Top = 90, Width = 120, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" };
        dtpDen = new DateTimePicker { Left = 80, Top = 124, Width = 120, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" };
        btnTim = new Button { Text = "Tìm", Left = 230, Top = 122, Width = 90, Height = 28 };
        grpSearch.Controls.AddRange(new Control[]
        {
            Lbl("Nội dung", 12, 28), txtSearch, Lbl("Loại chi", 12, 60), cboLoaiLoc,
            Lbl("Từ ngày", 12, 94), dtpTu, Lbl("Đến ngày", 12, 128), dtpDen, btnTim
        });

        // ---- Nút chức năng ----
        btnThem = new Button { Text = "Thêm", Left = 95, Top = 190, Width = 80, Height = 30 };
        btnSua = new Button { Text = "Sửa", Left = 185, Top = 190, Width = 80, Height = 30 };
        btnXoa = new Button { Text = "Xóa", Left = 275, Top = 190, Width = 80, Height = 30 };
        btnLamMoi = new Button { Text = "Làm mới", Left = 365, Top = 190, Width = 80, Height = 30 };

        // ---- Lưới ----
        dgv = new DataGridView
        {
            Left = 10, Top = 230, Width = 765, Height = 240,
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            AutoGenerateColumns = false, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
            ReadOnly = true, MultiSelect = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            RowHeadersVisible = true, BackgroundColor = Color.White,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        };
        dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaCT", HeaderText = "Mã", Width = 60, DefaultCellStyle = { Format = "000" } });
        dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NgayChi", HeaderText = "Ngày chi", Width = 100, DefaultCellStyle = { Format = "dd/MM/yyyy" } });
        dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NoiDung", HeaderText = "Nội dung", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 40 });
        dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "LoaiChi", HeaderText = "Loại chi", Width = 100 });
        dgv.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "SoTien", HeaderText = "Số tiền", Width = 100,
            DefaultCellStyle = { Format = "N0", FormatProvider = new System.Globalization.CultureInfo("vi-VN"), Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "GhiChu", HeaderText = "Ghi chú", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 60 });

        // ---- Tổng chi ----
        lblTong = new Label
        {
            Text = "Tổng chi: 0 đ", AutoSize = false, TextAlign = ContentAlignment.MiddleRight,
            Left = 400, Top = 475, Width = 375, Height = 28,
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.Red
        };

        // ---- Sự kiện ----
        btnThem.Click += btnThem_Click;
        btnSua.Click += btnSua_Click;
        btnXoa.Click += btnXoa_Click;
        btnLamMoi.Click += btnLamMoi_Click;
        btnTim.Click += btnTim_Click;
        dgv.CellClick += dgv_CellClick;
        txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; LoadData(); } };

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(790, 510);
        Controls.AddRange(new Control[] { grpInfo, grpSearch, btnThem, btnSua, btnXoa, btnLamMoi, dgv, lblTong });
        Font = new Font("Segoe UI", 9F);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Sổ chi tiêu cá nhân";
        ResumeLayout(false);
    }
}
