# Script PowerShell để chạy migration tự động
# Sử dụng: .\database\run_migration.ps1

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  CHẠY MIGRATION: Thêm cột SoVeChoiGame" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Đọc connection string từ appsettings.json
$appsettingsPath = "backend\appsettings.json"

if (-not (Test-Path $appsettingsPath)) {
    Write-Host "❌ Không tìm thấy file appsettings.json!" -ForegroundColor Red
    Write-Host "   Đường dẫn mong đợi: $appsettingsPath" -ForegroundColor Yellow
    exit 1
}

Write-Host "📄 Đang đọc appsettings.json..." -ForegroundColor Yellow
$appsettings = Get-Content $appsettingsPath | ConvertFrom-Json
$connectionString = $appsettings.ConnectionStrings.LanguageAppDb

if (-not $connectionString) {
    Write-Host "❌ Không tìm thấy ConnectionStrings.LanguageAppDb!" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Connection string: $connectionString" -ForegroundColor Green
Write-Host ""

# Extract database name
$databaseName = $null
if ($connectionString -match "Database=([^;]+)") {
    $databaseName = $matches[1].Trim()
    Write-Host "📊 Database name: $databaseName" -ForegroundColor Cyan
} else {
    Write-Host "❌ Không thể extract database name từ connection string!" -ForegroundColor Red
    exit 1
}

# Extract server name
$serverName = $null
if ($connectionString -match "Server=([^;]+)") {
    $serverName = $matches[1].Trim()
    Write-Host "🖥️  Server name: $serverName" -ForegroundColor Cyan
} else {
    Write-Host "⚠️  Không tìm thấy Server trong connection string, dùng localhost" -ForegroundColor Yellow
    $serverName = "localhost"
}

Write-Host ""

# Kiểm tra file migration
$migrationFile = "database\add_ticket_column_migration.sql"
if (-not (Test-Path $migrationFile)) {
    Write-Host "❌ Không tìm thấy file migration!" -ForegroundColor Red
    Write-Host "   Đường dẫn mong đợi: $migrationFile" -ForegroundColor Yellow
    exit 1
}

Write-Host "📝 File migration: $migrationFile" -ForegroundColor Green
Write-Host ""

# Kiểm tra sqlcmd có sẵn không
$sqlcmdPath = Get-Command sqlcmd -ErrorAction SilentlyContinue
if (-not $sqlcmdPath) {
    Write-Host "❌ Không tìm thấy sqlcmd!" -ForegroundColor Red
    Write-Host "   Vui lòng cài SQL Server Command Line Utilities" -ForegroundColor Yellow
    Write-Host "   Hoặc dùng SQL Server Management Studio (SSMS)" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "   Xem hướng dẫn chi tiết trong: HUONG_DAN_CHAY_SQL.md" -ForegroundColor Cyan
    exit 1
}

Write-Host "🚀 Đang chạy migration..." -ForegroundColor Yellow
Write-Host ""

# Chạy migration với Windows Authentication
$result = sqlcmd -S $serverName -d $databaseName -E -i $migrationFile 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "✅ Migration thành công!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Kết quả:" -ForegroundColor Cyan
    $result | ForEach-Object { Write-Host $_ }
} else {
    Write-Host ""
    Write-Host "❌ Migration thất bại!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Lỗi:" -ForegroundColor Red
    $result | ForEach-Object { Write-Host $_ -ForegroundColor Red }
    Write-Host ""
    Write-Host "💡 Thử các cách sau:" -ForegroundColor Yellow
    Write-Host "   1. Kiểm tra SQL Server đang chạy" -ForegroundColor Yellow
    Write-Host "   2. Kiểm tra database '$databaseName' đã tồn tại" -ForegroundColor Yellow
    Write-Host "   3. Kiểm tra quyền truy cập database" -ForegroundColor Yellow
    Write-Host "   4. Dùng SQL Server Management Studio để chạy thủ công" -ForegroundColor Yellow
    exit 1
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  HOÀN TẤT!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan

