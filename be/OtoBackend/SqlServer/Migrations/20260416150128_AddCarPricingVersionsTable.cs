using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddCarPricingVersionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Make this migration safe to run on databases where the table was created manually before
            // (or migrations history got out of sync).
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[dbo].[CarPricingVersions]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [dbo].[CarPricingVersions] (
                        [PricingVersionId] int NOT NULL IDENTITY,
                        [CarId] int NOT NULL,
                        [VersionName] nvarchar(255) NOT NULL,
                        [PriceVnd] decimal(18,2) NOT NULL,
                        [SortOrder] int NOT NULL,
                        [IsActive] bit NOT NULL CONSTRAINT [DF_CarPricingVersions_IsActive] DEFAULT CAST(1 AS bit),
                        [CreatedAt] datetime NULL CONSTRAINT [DF_CarPricingVersions_CreatedAt] DEFAULT ((getdate())),
                        [UpdatedAt] datetime NULL CONSTRAINT [DF_CarPricingVersions_UpdatedAt] DEFAULT ((getdate())),
                        CONSTRAINT [PK_CarPricingVersions] PRIMARY KEY ([PricingVersionId]),
                        CONSTRAINT [FK_CarPricingVersions_Cars] FOREIGN KEY ([CarId]) REFERENCES [dbo].[Cars] ([CarId]) ON DELETE CASCADE
                    );
                END

                IF COL_LENGTH(N'[dbo].[CarPricingVersions]', N'SortOrder') IS NULL
                BEGIN
                    ALTER TABLE [dbo].[CarPricingVersions] ADD [SortOrder] int NOT NULL CONSTRAINT [DF_CarPricingVersions_SortOrder] DEFAULT (0);
                END

                IF NOT EXISTS (
                    SELECT 1
                    FROM sys.indexes
                    WHERE name = N'IX_CarPricingVersions_CarId'
                      AND object_id = OBJECT_ID(N'[dbo].[CarPricingVersions]')
                )
                BEGIN
                    CREATE INDEX [IX_CarPricingVersions_CarId] ON [dbo].[CarPricingVersions] ([CarId]);
                END
                """);

            migrationBuilder.Sql(
                """
                INSERT INTO [CarPricingVersions] ([CarId], [VersionName], [PriceVnd], [SortOrder], [IsActive], [CreatedAt], [UpdatedAt])
                SELECT c.CarId, v.VersionName, v.PriceVnd, v.SortOrder, 1, GETDATE(), GETDATE()
                FROM [Cars] c
                INNER JOIN (
                    VALUES
                        ('VF7', N'VinFast VF7 Eco - Kèm Pin', CAST(799000000 AS decimal(18,2)), 1),
                        ('VF7', N'VinFast VF7 Plus Trần Thép - Nâng Cấp', CAST(999000000 AS decimal(18,2)), 2),
                        ('VF7', N'VinFast VF7 Plus Trần Kính - Nâng Cấp', CAST(1019000000 AS decimal(18,2)), 3),
                        ('VF8', N'VinFast VF8 Eco', CAST(1090000000 AS decimal(18,2)), 1),
                        ('VF8', N'VinFast VF8 Plus', CAST(1260000000 AS decimal(18,2)), 2),
                        ('VF3', N'VinFast VF3 Tiêu chuẩn', CAST(281000000 AS decimal(18,2)), 1),
                        ('VF5', N'VinFast VF5 Eco', CAST(458000000 AS decimal(18,2)), 1),
                        ('VF5', N'VinFast VF5 Plus', CAST(497000000 AS decimal(18,2)), 2),
                        ('VF6', N'VinFast VF6 Eco', CAST(599000000 AS decimal(18,2)), 1),
                        ('VF6', N'VinFast VF6 Plus', CAST(647000000 AS decimal(18,2)), 2),
                        ('VF9', N'VinFast VF9 Eco', CAST(1251000000 AS decimal(18,2)), 1),
                        ('VF9', N'VinFast VF9 Plus', CAST(1349000000 AS decimal(18,2)), 2)
                ) v (CarCode, VersionName, PriceVnd, SortOrder)
                    ON UPPER(ISNULL(c.[Name], '')) LIKE '%' + v.CarCode + '%'
                WHERE c.[IsDeleted] = 0
                  AND NOT EXISTS (
                      SELECT 1
                      FROM [CarPricingVersions] cpv
                      WHERE cpv.[CarId] = c.[CarId]
                        AND cpv.[VersionName] = v.[VersionName]
                  );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[dbo].[CarPricingVersions]', N'U') IS NOT NULL
                BEGIN
                    DROP TABLE [dbo].[CarPricingVersions];
                END
                """);
        }
    }
}
