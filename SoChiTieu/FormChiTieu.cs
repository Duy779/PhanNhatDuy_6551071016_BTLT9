using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SoChiTieu.Data;
using SoChiTieu.Models;

namespace SoChiTieu;

public partial class FormChiTieu : Form
{
    private static readonly CultureInfo Vi = new("vi-VN");
    private static readonly string[] DanhSachLoai = { "Ăn uống", "Đi lại", "Học tập", "Giải trí", "Khác" };
    private const string LoiCsdl = "Không thể truy cập cơ sở dữ liệu. Vui lòng kiểm tra kết nối (appsettings.json) rồi thử lại.";

    private int? _selectedId;   // MaCT đang được chọn trên lưới

    public FormChiTieu()
    {
        InitializeComponent();
        Load += FormChiTieu_Load;
    }

    private void FormChiTieu_Load(object? sender, EventArgs e)
    {
        cboLoai.Items.AddRange(DanhSachLoai);
        cboLoai.SelectedIndex = 0;

        cboLoaiLoc.Items.Add("Tất cả");
        cboLoaiLoc.Items.AddRange(DanhSachLoai);
        cboLoaiLoc.SelectedIndex = 0;

        ResetFilter();
        LoadData();
    }

    // ================== Tải dữ liệu + lọc + tổng chi ==================
    private void ResetFilter()
    {
        var today = DateTime.Today;
        txtSearch.Clear();
        cboLoaiLoc.SelectedIndex = 0;
        dtpTu.Value = new DateTime(today.Year, today.Month, 1);
        dtpDen.Value = today;
    }

    private void LoadData()
    {
        var tu = dtpTu.Value.Date;
        var den = dtpDen.Value.Date;
        if (tu > den)
        {
            MessageBox.Show("“Từ ngày” không được lớn hơn “Đến ngày”.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            using var db = Db.Create();
            var q = db.ChiTieus.AsNoTracking().Where(x => x.NgayChi >= tu && x.NgayChi <= den);

            var kw = txtSearch.Text.Trim();
            if (kw.Length > 0) q = q.Where(x => x.NoiDung.Contains(kw));

            if (cboLoaiLoc.SelectedIndex > 0)
            {
                var loai = cboLoaiLoc.SelectedItem!.ToString()!;
                q = q.Where(x => x.LoaiChi == loai);
            }

            var list = q.OrderBy(x => x.NgayChi).ThenBy(x => x.MaCT).ToList();
            dgv.DataSource = list;
            dgv.ClearSelection();
            lblTong.Text = $"Tổng chi: {list.Sum(x => x.SoTien).ToString("N0", Vi)} đ";
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Loi khi tai danh sach chi tieu");
            MessageBox.Show(LoiCsdl, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ================== Chọn dòng -> đổ lên control ==================
    private void dgv_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgv.Rows[e.RowIndex].DataBoundItem is not ChiTieu ct) return;

        _selectedId = ct.MaCT;
        dtpNgay.Value = ct.NgayChi;
        txtNoiDung.Text = ct.NoiDung;
        cboLoai.SelectedItem = ct.LoaiChi;
        if (cboLoai.SelectedIndex < 0) cboLoai.SelectedItem = "Khác";
        nudSoTien.Value = Math.Min(ct.SoTien, nudSoTien.Maximum);
        txtGhiChu.Text = ct.GhiChu ?? "";
    }

    private void ClearInput()
    {
        _selectedId = null;
        dtpNgay.Value = DateTime.Today;
        txtNoiDung.Clear();
        cboLoai.SelectedIndex = 0;
        nudSoTien.Value = 0;
        txtGhiChu.Clear();
        dgv.ClearSelection();
        txtNoiDung.Focus();
    }

    // ================== Kiểm tra hợp lệ ==================
    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(txtNoiDung.Text))
            return Fail("Nội dung không được để trống.", txtNoiDung);
        if (cboLoai.SelectedIndex < 0)
            return Fail("Vui lòng chọn loại chi.", cboLoai);
        if (nudSoTien.Value <= 0)
            return Fail("Số tiền phải lớn hơn 0.", nudSoTien);
        if (dtpNgay.Value.Date > DateTime.Today)
            return Fail("Ngày chi không được lớn hơn hôm nay.", dtpNgay);
        return true;
    }

    private static bool Fail(string msg, Control c)
    {
        MessageBox.Show(msg, "Dữ liệu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        c.Focus();
        return false;
    }

    private void Fill(ChiTieu ct)
    {
        ct.NgayChi = dtpNgay.Value.Date;
        ct.NoiDung = txtNoiDung.Text.Trim();
        ct.LoaiChi = cboLoai.SelectedItem!.ToString()!;
        ct.SoTien = nudSoTien.Value;
        ct.GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text.Trim();
    }

    // ================== Thêm / Sửa / Xóa / Làm mới / Tìm ==================
    private void btnThem_Click(object? sender, EventArgs e)
    {
        if (!ValidateInput()) return;
        try
        {
            using var db = Db.Create();
            var ct = new ChiTieu();
            Fill(ct);
            db.ChiTieus.Add(ct);
            db.SaveChanges();
            Log.Information("Them chi tieu MaCT={MaCT} NoiDung={NoiDung} SoTien={SoTien}", ct.MaCT, ct.NoiDung, ct.SoTien);
            ClearInput();
            LoadData();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Loi khi them chi tieu");
            MessageBox.Show(LoiCsdl, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnSua_Click(object? sender, EventArgs e)
    {
        if (_selectedId is null)
        {
            MessageBox.Show("Hãy chọn một dòng trên danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (!ValidateInput()) return;
        try
        {
            using var db = Db.Create();
            var ct = db.ChiTieus.Find(_selectedId.Value);
            if (ct is null)
            {
                MessageBox.Show("Khoản chi này không còn tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadData();
                return;
            }
            Fill(ct);
            db.SaveChanges();
            Log.Information("Sua chi tieu MaCT={MaCT}", ct.MaCT);
            ClearInput();
            LoadData();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Loi khi sua chi tieu MaCT={MaCT}", _selectedId);
            MessageBox.Show(LoiCsdl, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnXoa_Click(object? sender, EventArgs e)
    {
        if (_selectedId is null)
        {
            MessageBox.Show("Hãy chọn một dòng trên danh sách để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        var id = _selectedId.Value;
        if (MessageBox.Show($"Bạn có chắc muốn xóa khoản chi mã {id:000}?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try
        {
            using var db = Db.Create();
            var ct = db.ChiTieus.Find(id);
            if (ct is not null)
            {
                db.ChiTieus.Remove(ct);
                db.SaveChanges();
                Log.Information("Xoa chi tieu MaCT={MaCT}", id);
            }
            ClearInput();
            LoadData();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Loi khi xoa chi tieu MaCT={MaCT}", id);
            MessageBox.Show(LoiCsdl, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnLamMoi_Click(object? sender, EventArgs e)
    {
        ClearInput();
        ResetFilter();
        LoadData();
    }

    private void btnTim_Click(object? sender, EventArgs e) => LoadData();
}
