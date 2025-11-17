# Script to execute database schema
$sqlFile = Join-Path $PSScriptRoot "database_schema.sql"
$connectionString = "Server=localhost;Database=LanguageAppDb;Integrated Security=True;TrustServerCertificate=True;"

# Create database if not exists
sqlcmd -S localhost -E -Q "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'LanguageAppDb') CREATE DATABASE LanguageAppDb"

# Execute schema
sqlcmd -S localhost -E -d LanguageAppDb -i $sqlFile

Write-Host "Database schema executed successfully!"

