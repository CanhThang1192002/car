/*
  Seed thêm dữ liệu demo cho DB `oto`.
  - An toàn khi chạy nhiều lần: dùng key tự nhiên (Name/FeatureName) để tránh insert trùng.
  - Không đụng dữ liệu sẵn có; chỉ bổ sung những bản ghi còn thiếu.

  Cách chạy:
    USE oto;
    chạy toàn bộ file này trong SSMS.
*/

SET NOCOUNT ON;
GO

USE [oto];
GO

BEGIN TRY
  BEGIN TRAN;

  ---------------------------------------------------------------------------
  -- 1) Seed Showrooms (nếu chưa có theo tên)
  ---------------------------------------------------------------------------
  IF NOT EXISTS (SELECT 1 FROM dbo.Showrooms WHERE [Name] = N'VinFast - Quận 1')
    INSERT dbo.Showrooms([Name],[Province],[District],[StreetAddress],[Hotline])
    VALUES (N'VinFast - Quận 1', N'TP.HCM', N'Quận 1', N'12 Nguyễn Huệ, P.Bến Nghé', N'0333436743');

  IF NOT EXISTS (SELECT 1 FROM dbo.Showrooms WHERE [Name] = N'Toyota - Tân Bình')
    INSERT dbo.Showrooms([Name],[Province],[District],[StreetAddress],[Hotline])
    VALUES (N'Toyota - Tân Bình', N'TP.HCM', N'Tân Bình', N'68 Cộng Hòa, P.4', N'0333436743');

  IF NOT EXISTS (SELECT 1 FROM dbo.Showrooms WHERE [Name] = N'Ford - Hà Nội')
    INSERT dbo.Showrooms([Name],[Province],[District],[StreetAddress],[Hotline])
    VALUES (N'Ford - Hà Nội', N'Hà Nội', N'Cầu Giấy', N'102 Trần Duy Hưng', N'0333436743');

  IF NOT EXISTS (SELECT 1 FROM dbo.Showrooms WHERE [Name] = N'Mitsubishi - Đà Nẵng')
    INSERT dbo.Showrooms([Name],[Province],[District],[StreetAddress],[Hotline])
    VALUES (N'Mitsubishi - Đà Nẵng', N'Đà Nẵng', N'Hải Châu', N'55 Núi Thành', N'0333436743');

  ---------------------------------------------------------------------------
  -- 2) Seed Features (nếu chưa có theo FeatureName)
  ---------------------------------------------------------------------------
  DECLARE @features TABLE (FeatureName nvarchar(255) NOT NULL, Icon nvarchar(255) NULL);
  INSERT INTO @features(FeatureName, Icon) VALUES
    (N'Apple CarPlay / Android Auto', N'/uploads/Features/default-feature.png'),
    (N'Cruise Control', N'/uploads/Features/default-feature.png'),
    (N'Ghế da', N'/uploads/Features/default-feature.png'),
    (N'Cảm biến áp suất lốp', N'/uploads/Features/default-feature.png'),
    (N'Phanh tay điện tử', N'/uploads/Features/default-feature.png'),
    (N'Đèn pha LED', N'/uploads/Features/default-feature.png'),
    (N'Cảnh báo lệch làn', N'/uploads/Features/default-feature.png'),
    (N'Giữ làn chủ động', N'/uploads/Features/default-feature.png'),
    (N'6 túi khí', N'/uploads/Features/default-feature.png'),
    (N'Điều hòa tự động', N'/uploads/Features/default-feature.png');

  INSERT dbo.Features(FeatureName, Icon)
  SELECT f.FeatureName, f.Icon
  FROM @features f
  WHERE NOT EXISTS (SELECT 1 FROM dbo.Features x WHERE x.FeatureName = f.FeatureName);

  ---------------------------------------------------------------------------
  -- 3) Seed Cars (nếu chưa có theo Name + Year + Brand)
  ---------------------------------------------------------------------------
  DECLARE @cars TABLE (
    Name nvarchar(255) NOT NULL,
    Model nvarchar(255) NULL,
    [Year] int NULL,
    Condition int NOT NULL,
    Price decimal(18,2) NULL,
    Color nvarchar(50) NULL,
    [Description] nvarchar(max) NULL,
    Brand nvarchar(100) NULL,
    Mileage decimal(18,2) NULL,
    ImageUrl nvarchar(max) NULL,
    [Status] int NULL,
    FuelType nvarchar(50) NULL,
    Transmission nvarchar(max) NULL,
    BodyStyle nvarchar(max) NULL
  );

  INSERT INTO @cars VALUES
    (N'VinFast VF3',  N'VF3',  2024, 0, 281000000, N'Xanh',  N'Xe điện mini đô thị, nhỏ gọn, dễ lái.', N'VINFAST', 0, N'/uploads/Cars/default-car.jpg.png', 1, N'Điện', N'Số tự động', N'Hatchback'),
    (N'VinFast VF5',  N'VF5',  2024, 0, 497000000, N'Đỏ',    N'Xe điện gầm cao cỡ nhỏ, phù hợp gia đình.', N'VINFAST', 0, N'/uploads/Cars/default-car.jpg.png', 1, N'Điện', N'Số tự động', N'SUV'),
    (N'VinFast VF6',  N'VF6',  2025, 0, 647000000, N'Trắng', N'Xe điện cỡ C, tiện nghi và an toàn.', N'VINFAST', 0, N'/uploads/Cars/default-car.jpg.png', 1, N'Điện', N'Số tự động', N'SUV'),
    (N'VinFast VF7',  N'VF7',  2025, 0, 751000000, N'Đen',   N'SUV điện cỡ C thiết kế hiện đại.', N'VINFAST', 0, N'/uploads/Cars/default-car.jpg.png', 1, N'Điện', N'Số tự động', N'SUV'),
    (N'VinFast VF9',  N'VF9',  2025, 0, 1349000000, N'Xám',  N'SUV điện 7 chỗ cao cấp.', N'VINFAST', 0, N'/uploads/Cars/default-car.jpg.png', 1, N'Điện', N'Số tự động', N'SUV'),

    (N'Toyota Vios G', N'Vios G', 2024, 0, 545000000, N'Trắng', N'Sedan bền bỉ, tiết kiệm, phù hợp dịch vụ.', N'TOYOTA', 0, N'/uploads/Cars/default-car.jpg.png', 1, N'Xăng', N'Số tự động', N'Sedan'),
    (N'Toyota Corolla Cross', N'1.8V', 2024, 0, 820000000, N'Đen', N'Crossover đô thị, trang bị an toàn tốt.', N'TOYOTA', 0, N'/uploads/Cars/default-car.jpg.png', 1, N'Xăng', N'Số tự động', N'SUV'),
    (N'Ford Everest Titanium', N'Everest', 2024, 0, 1450000000, N'Trắng', N'SUV 7 chỗ mạnh mẽ, đi đường dài tốt.', N'FORD', 0, N'/uploads/Cars/default-car.jpg.png', 1, N'Dầu', N'Số tự động', N'SUV'),
    (N'Mitsubishi Xpander Cross', N'Xpander Cross', 2024, 0, 698000000, N'Cam', N'MPV gầm cao, rộng rãi, dễ sử dụng.', N'MITSUBISHI', 0, N'/uploads/Cars/default-car.jpg.png', 1, N'Xăng', N'Số tự động', N'MPV');

  INSERT dbo.Cars([Name],[Model],[Year],[Condition],[Price],[Color],[Description],[Brand],[Mileage],[ImageUrl],[Status],[CreatedAt],[UpdatedAt],[FuelType],[IsDeleted],[Transmission],[BodyStyle])
  SELECT
    c.[Name], c.[Model], c.[Year], c.[Condition], c.[Price], c.[Color], c.[Description], c.[Brand],
    c.[Mileage], c.[ImageUrl], c.[Status], GETDATE(), GETDATE(), c.[FuelType], 0, c.[Transmission], c.[BodyStyle]
  FROM @cars c
  WHERE NOT EXISTS (
    SELECT 1 FROM dbo.Cars x
    WHERE x.[Name] = c.[Name]
      AND ISNULL(x.[Year], -1) = ISNULL(c.[Year], -1)
      AND ISNULL(x.[Brand], N'') = ISNULL(c.[Brand], N'')
      AND x.IsDeleted = 0
  );

  ---------------------------------------------------------------------------
  -- 4) Seed CarInventories (phân tồn kho theo showroom)
  ---------------------------------------------------------------------------
  DECLARE @sid_vf_q1 int = (SELECT TOP 1 ShowroomId FROM dbo.Showrooms WHERE [Name] = N'VinFast - Quận 1');
  DECLARE @sid_toy_tb int = (SELECT TOP 1 ShowroomId FROM dbo.Showrooms WHERE [Name] = N'Toyota - Tân Bình');
  DECLARE @sid_ford_hn int = (SELECT TOP 1 ShowroomId FROM dbo.Showrooms WHERE [Name] = N'Ford - Hà Nội');
  DECLARE @sid_mit_dn int = (SELECT TOP 1 ShowroomId FROM dbo.Showrooms WHERE [Name] = N'Mitsubishi - Đà Nẵng');

  ;WITH CarMap AS (
    SELECT CarId, [Name] FROM dbo.Cars WHERE IsDeleted = 0
  )
  INSERT dbo.CarInventories(ShowroomId, CarId, Quantity, DisplayStatus, UpdatedAt)
  SELECT v.ShowroomId, v.CarId, v.Quantity, v.DisplayStatus, SYSDATETIME()
  FROM (
    SELECT @sid_vf_q1 AS ShowroomId, (SELECT TOP 1 CarId FROM CarMap WHERE [Name]=N'VinFast VF3') AS CarId, 8 AS Quantity, N'Available' AS DisplayStatus
    UNION ALL SELECT @sid_vf_q1, (SELECT TOP 1 CarId FROM CarMap WHERE [Name]=N'VinFast VF5'), 6, N'Available'
    UNION ALL SELECT @sid_vf_q1, (SELECT TOP 1 CarId FROM CarMap WHERE [Name]=N'VinFast VF6'), 3, N'OnDisplay'
    UNION ALL SELECT @sid_vf_q1, (SELECT TOP 1 CarId FROM CarMap WHERE [Name]=N'VinFast VF7'), 2, N'OnDisplay'
    UNION ALL SELECT @sid_vf_q1, (SELECT TOP 1 CarId FROM CarMap WHERE [Name]=N'VinFast VF9'), 1, N'Pending'

    UNION ALL SELECT @sid_toy_tb, (SELECT TOP 1 CarId FROM CarMap WHERE [Name]=N'Toyota Vios G'), 5, N'Available'
    UNION ALL SELECT @sid_toy_tb, (SELECT TOP 1 CarId FROM CarMap WHERE [Name]=N'Toyota Corolla Cross'), 2, N'OnDisplay'

    UNION ALL SELECT @sid_ford_hn, (SELECT TOP 1 CarId FROM CarMap WHERE [Name]=N'Ford Everest Titanium'), 2, N'Available'

    UNION ALL SELECT @sid_mit_dn, (SELECT TOP 1 CarId FROM CarMap WHERE [Name]=N'Mitsubishi Xpander Cross'), 4, N'Available'
  ) v
  WHERE v.CarId IS NOT NULL
    AND v.ShowroomId IS NOT NULL
    AND NOT EXISTS (
      SELECT 1 FROM dbo.CarInventories x
      WHERE x.ShowroomId = v.ShowroomId AND x.CarId = v.CarId
    );

  ---------------------------------------------------------------------------
  -- 5) Seed CarSpecifications (đủ để FE lọc: số chỗ, kiểu xe, hộp số, nhiên liệu…)
  ---------------------------------------------------------------------------
  DECLARE @specs TABLE(CarName nvarchar(255), Category nvarchar(100), SpecName nvarchar(255), SpecValue nvarchar(255));
  INSERT INTO @specs VALUES
    (N'VinFast VF3', N'Nội thất', N'Số chỗ ngồi', N'4'),
    (N'VinFast VF3', N'Pin', N'Quãng đường di chuyển', N'210 km'),
    (N'VinFast VF5', N'Nội thất', N'Số chỗ ngồi', N'5'),
    (N'VinFast VF6', N'Nội thất', N'Số chỗ ngồi', N'5'),
    (N'VinFast VF7', N'Nội thất', N'Số chỗ ngồi', N'5'),
    (N'VinFast VF9', N'Nội thất', N'Số chỗ ngồi', N'7'),

    (N'Toyota Vios G', N'Nội thất', N'Số chỗ ngồi', N'5'),
    (N'Toyota Corolla Cross', N'Nội thất', N'Số chỗ ngồi', N'5'),
    (N'Ford Everest Titanium', N'Nội thất', N'Số chỗ ngồi', N'7'),
    (N'Mitsubishi Xpander Cross', N'Nội thất', N'Số chỗ ngồi', N'7'),

    (N'Toyota Vios G', N'Vận hành', N'Hộp số', N'Tự động'),
    (N'Ford Everest Titanium', N'Vận hành', N'Hệ dẫn động', N'4x2/4x4 (tùy phiên bản)'),
    (N'Toyota Corolla Cross', N'Động cơ', N'Loại nhiên liệu', N'Xăng'),
    (N'VinFast VF6', N'Động cơ', N'Loại nhiên liệu', N'Điện');

  INSERT dbo.CarSpecifications(CarId, Category, SpecName, SpecValue)
  SELECT c.CarId, s.Category, s.SpecName, s.SpecValue
  FROM @specs s
  JOIN dbo.Cars c ON c.[Name] = s.CarName AND c.IsDeleted = 0
  WHERE NOT EXISTS (
    SELECT 1 FROM dbo.CarSpecifications x
    WHERE x.CarId = c.CarId AND x.Category = s.Category AND x.SpecName = s.SpecName
  );

  ---------------------------------------------------------------------------
  -- 6) Seed CarFeatures (gán tiện ích)
  ---------------------------------------------------------------------------
  DECLARE @carFeatures TABLE(CarName nvarchar(255), FeatureName nvarchar(255));
  INSERT INTO @carFeatures VALUES
    (N'VinFast VF3', N'Đèn pha LED'),
    (N'VinFast VF3', N'Điều hòa tự động'),
    (N'VinFast VF5', N'Apple CarPlay / Android Auto'),
    (N'VinFast VF5', N'Camera 360'),
    (N'VinFast VF6', N'Cruise Control'),
    (N'VinFast VF6', N'Cảm biến áp suất lốp'),
    (N'VinFast VF7', N'Cảnh báo lệch làn'),
    (N'VinFast VF7', N'Giữ làn chủ động'),
    (N'VinFast VF9', N'6 túi khí'),
    (N'VinFast VF9', N'Ghế da'),
    (N'Toyota Corolla Cross', N'Cruise Control'),
    (N'Toyota Corolla Cross', N'Cảm biến áp suất lốp'),
    (N'Ford Everest Titanium', N'Camera 360'),
    (N'Mitsubishi Xpander Cross', N'Phanh tay điện tử');

  INSERT dbo.CarFeatures(CarId, FeatureId)
  SELECT c.CarId, f.FeatureId
  FROM @carFeatures cf
  JOIN dbo.Cars c ON c.[Name] = cf.CarName AND c.IsDeleted = 0
  JOIN dbo.Features f ON f.FeatureName = cf.FeatureName
  WHERE NOT EXISTS (
    SELECT 1 FROM dbo.CarFeatures x
    WHERE x.CarId = c.CarId AND x.FeatureId = f.FeatureId
  );

  ---------------------------------------------------------------------------
  -- 7) Seed CarImages (main + gallery + 360)
  -- Lưu ý: ImageUrl đang là đường dẫn tương đối, bạn có thể thay bằng URL thật.
  ---------------------------------------------------------------------------
  DECLARE @img TABLE(
    CarName nvarchar(255),
    ImageUrl nvarchar(255),
    IsMainImage bit,
    ImageType nvarchar(max),
    Is360Degree bit,
    Title nvarchar(max),
    [Description] nvarchar(max)
  );

  INSERT INTO @img VALUES
    (N'VinFast VF3', N'/uploads/Cars/vf3-main.png', 1, N'main', 0, N'VF3 - Ảnh đại diện', N'Ảnh đại diện'),
    (N'VinFast VF3', N'/uploads/Cars/vf3-ngoai-that-1.png', 0, N'Ngoại thất', 0, N'Góc nghiêng', N'Ngoại thất'),
    (N'VinFast VF3', N'/uploads/Cars/vf3-noi-that-1.png', 0, N'Nội thất', 0, N'Khoang lái', N'Nội thất'),

    (N'VinFast VF5', N'/uploads/Cars/vf5-main.png', 1, N'main', 0, N'VF5 - Ảnh đại diện', N'Ảnh đại diện'),
    (N'VinFast VF5', N'/uploads/Cars/vf5-360-001.png', 0, N'360', 1, N'360-001', N'Ảnh 360'),
    (N'VinFast VF5', N'/uploads/Cars/vf5-360-002.png', 0, N'360', 1, N'360-002', N'Ảnh 360'),

    (N'Toyota Vios G', N'/uploads/Cars/vios-main.png', 1, N'main', 0, N'Vios - Ảnh đại diện', N'Ảnh đại diện'),
    (N'Toyota Corolla Cross', N'/uploads/Cars/cross-main.png', 1, N'main', 0, N'Corolla Cross - Ảnh đại diện', N'Ảnh đại diện');

  INSERT dbo.CarImages(CarId, ImageUrl, IsMainImage, ImageType, FileHash, CreatedAt, Is360Degree, [Description], Title)
  SELECT c.CarId, i.ImageUrl, i.IsMainImage, i.ImageType, NULL, GETDATE(), i.Is360Degree, i.[Description], i.Title
  FROM @img i
  JOIN dbo.Cars c ON c.[Name] = i.CarName AND c.IsDeleted = 0
  WHERE NOT EXISTS (
    SELECT 1 FROM dbo.CarImages x
    WHERE x.CarId = c.CarId AND x.ImageUrl = i.ImageUrl
  );

  -- Đồng bộ Cars.ImageUrl = ảnh main nếu đang null
  UPDATE c
  SET c.ImageUrl = mi.ImageUrl
  FROM dbo.Cars c
  CROSS APPLY (
    SELECT TOP 1 ImageUrl
    FROM dbo.CarImages
    WHERE CarId = c.CarId AND IsMainImage = 1
    ORDER BY CarImageId DESC
  ) mi
  WHERE c.IsDeleted = 0 AND (c.ImageUrl IS NULL OR LTRIM(RTRIM(c.ImageUrl)) = N'');

  COMMIT;
END TRY
BEGIN CATCH
  IF @@TRANCOUNT > 0 ROLLBACK;
  DECLARE @msg nvarchar(4000) = ERROR_MESSAGE();
  RAISERROR(N'Seed demo extra failed: %s', 16, 1, @msg);
END CATCH;
GO

