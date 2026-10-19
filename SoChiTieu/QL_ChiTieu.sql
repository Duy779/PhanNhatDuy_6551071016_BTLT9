-- Chạy bằng SSMS. Tạo CSDL QL_ChiTieu + bảng ChiTieu + 12 dòng mẫu
IF DB_ID(N'QL_ChiTieu') IS NULL CREATE DATABASE QL_ChiTieu;
GO
USE QL_ChiTieu;
GO
IF OBJECT_ID(N'dbo.ChiTieu') IS NOT NULL DROP TABLE dbo.ChiTieu;
CREATE TABLE dbo.ChiTieu (
    MaCT    INT IDENTITY(1,1) CONSTRAINT PK_ChiTieu PRIMARY KEY,
    NgayChi DATE           NOT NULL,
    NoiDung NVARCHAR(100)  NOT NULL,
    LoaiChi NVARCHAR(30)   NOT NULL,
    SoTien  DECIMAL(18,0)  NOT NULL CONSTRAINT CK_ChiTieu_SoTien CHECK (SoTien > 0),
    GhiChu  NVARCHAR(200)  NULL
);
GO
INSERT INTO dbo.ChiTieu (NgayChi, NoiDung, LoaiChi, SoTien, GhiChu) VALUES
('2026-10-01', N'Cơm trưa',            N'Ăn uống', 35000,  NULL),
('2026-10-02', N'Xăng xe',             N'Đi lại',  60000,  NULL),
('2026-10-03', N'Photo giáo trình',    N'Học tập', 25000,  NULL),
('2026-10-04', N'Xem phim',            N'Giải trí',90000,  N'Đi cùng bạn'),
('2026-10-05', N'Trà sữa',             N'Ăn uống', 40000,  NULL),
('2026-10-06', N'Mua sắm',             N'Khác',    150000, NULL),
('2026-10-07', N'Tiền điện',           N'Khác',    500000, N'Hóa đơn tháng 9'),
('2026-10-08', N'Ăn sáng',             N'Ăn uống', 25000,  NULL),
('2026-10-09', N'Cà phê với bạn',      N'Ăn uống', 45000,  NULL),
('2026-10-10', N'Đổ xăng',             N'Đi lại',  70000,  NULL),
('2026-10-11', N'Sách lập trình',      N'Học tập', 180000, NULL),
('2026-10-12', N'Gửi xe',              N'Đi lại',  30000,  NULL);
GO
SELECT COUNT(*) AS SoDong, SUM(SoTien) AS TongChi FROM dbo.ChiTieu;  -- 12 dòng, 1.250.000
