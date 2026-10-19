# Sổ chi tiêu cá nhân (WinForms + EF Core Database First)

## 1. Tạo CSDL
Mở `QL_ChiTieu.sql` trong SSMS → Execute (tạo DB `QL_ChiTieu`, bảng `ChiTieu`, 12 dòng mẫu, tổng 1.250.000).

## 2. Scaffold (Database First)
Project này đã kèm sẵn Model/DbContext tương đương. Muốn tự sinh lại:
```
dotnet tool install --global dotnet-ef
dotnet ef dbcontext scaffold "Server=.;Database=QL_ChiTieu;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models --context QlChiTieuContext --no-onconfiguring --force
```
`--no-onconfiguring` để connection string không bị hard-code trong code.
(Nếu cột `date` sinh ra `DateOnly`, đổi thành `DateTime` hoặc chỉnh form dùng `DateOnly.FromDateTime`.)

## 3. Cấu hình
- `appsettings.json`: connection string chính (Copy if newer).
- `appsettings.Development.json`: ghi đè khi lập trình (chạy F5 có debugger → tự dùng).
- `appsettings.example.json`: file mẫu để commit. Hai file chứa mật khẩu đã nằm trong `.gitignore`.
- Đọc bằng `ConfigurationBuilder` → bind vào lớp `AppSettings` (Config/AppSettings.cs).

## 4. Log
Serilog ghi `Logs/log-yyyyMMdd.txt` cạnh file .exe: Information khi Thêm/Sửa/Xóa (kèm MaCT), Error khi lỗi CSDL.

## 5. Publish (Framework-Dependent, win-x64)
```
dotnet publish -c Release -r win-x64 --self-contained false -o publish
```
Visual Studio: Build > Publish > Folder > Target runtime `win-x64`, Deployment mode `Framework-dependent`.

## 6. Chạy trên máy khác
1. Máy đích cài **.NET 8 Desktop Runtime (x64)** và có SQL Server (đã chạy file .sql).
2. Chép cả thư mục `publish` sang máy đích.
3. Mở `appsettings.json` bằng Notepad, sửa `Server=...`, user/password cho đúng máy đó (KHÔNG build lại).
4. Chạy `SoChiTieu.exe`. (Nên xóa `appsettings.Development.json` khỏi thư mục publish cho gọn.)

## 7. Checklist nghiệm thu
- [ ] Thêm/Sửa/Xóa xong lưới tự làm mới, kiểm tra lại trong SSMS.
- [ ] Lọc “Ăn uống” + tháng này → chỉ còn dòng phù hợp, Label tổng khớp tính tay.
- [ ] Đổi connection string trong appsettings.json → kết nối DB khác, không build lại.
- [ ] Có `Logs/log-yyyyMMdd.txt`; ngắt SQL Server thử → thấy hộp thoại thân thiện + dòng [ERR] trong log.
- [ ] Bản publish chạy được trên máy thứ hai.
