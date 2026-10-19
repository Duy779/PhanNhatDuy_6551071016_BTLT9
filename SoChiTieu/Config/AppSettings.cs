using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace SoChiTieu.Config;

// Lớp cấu hình có kiểu (strongly-typed), khớp cấu trúc appsettings.json
public class AppSettings
{
    public ConnectionStringsSection ConnectionStrings { get; set; } = new();
}

public class ConnectionStringsSection
{
    public string QLChiTieu { get; set; } = "";
}

public static class ConfigLoader
{
    public static AppSettings Load()
    {
        // Debug (F5) -> Development; chạy bản publish -> Production (không đọc file Development)
        var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                  ?? (Debugger.IsAttached ? "Development" : "Production");

        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)   // đọc cạnh file .exe, không phụ thuộc thư mục chạy
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: false) // ghi đè
            .Build();

        var settings = config.Get<AppSettings>() ?? new AppSettings();
        if (string.IsNullOrWhiteSpace(settings.ConnectionStrings.QLChiTieu))
            throw new InvalidOperationException("Thiếu ConnectionStrings:QLChiTieu trong appsettings.json");
        return settings;
    }
}
