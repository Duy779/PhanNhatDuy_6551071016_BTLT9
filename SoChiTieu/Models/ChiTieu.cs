namespace SoChiTieu.Models;

// Tương đương lớp do "dotnet ef dbcontext scaffold" sinh ra (NgayChi để DateTime cho tiện WinForms)
public partial class ChiTieu
{
    public int MaCT { get; set; }
    public DateTime NgayChi { get; set; }
    public string NoiDung { get; set; } = null!;
    public string LoaiChi { get; set; } = null!;
    public decimal SoTien { get; set; }
    public string? GhiChu { get; set; }
}
