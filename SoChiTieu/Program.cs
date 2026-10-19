using Serilog;
using SoChiTieu.Config;
using SoChiTieu.Data;

namespace SoChiTieu;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Logs/log-yyyyMMdd.txt cạnh file .exe
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(
                Path.Combine(AppContext.BaseDirectory, "Logs", "log-.txt"),
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                encoding: System.Text.Encoding.UTF8)
            .CreateLogger();

        try
        {
            Db.Settings = ConfigLoader.Load();
            Log.Information("Ung dung khoi dong");
            Application.Run(new FormChiTieu());
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Loi khoi dong ung dung");
            MessageBox.Show("Không thể khởi động ứng dụng. Hãy kiểm tra file appsettings.json.",
                "Sổ chi tiêu cá nhân", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
