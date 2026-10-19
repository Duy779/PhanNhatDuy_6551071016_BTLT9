using Microsoft.EntityFrameworkCore;
using SoChiTieu.Config;
using SoChiTieu.Models;

namespace SoChiTieu.Data;

public static class Db
{
    public static AppSettings Settings { get; set; } = new();

    // Mỗi thao tác dùng một DbContext ngắn hạn
    public static QlChiTieuContext Create()
    {
        var options = new DbContextOptionsBuilder<QlChiTieuContext>()
            .UseSqlServer(Settings.ConnectionStrings.QLChiTieu)
            .Options;
        return new QlChiTieuContext(options);
    }
}
