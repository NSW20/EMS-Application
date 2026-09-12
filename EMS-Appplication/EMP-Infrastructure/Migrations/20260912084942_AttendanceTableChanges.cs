using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMP_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AttendanceTableChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Attendances",
                type: "int",
                maxLength: 20,
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Attendances",
                type: "date",
                nullable: false,
                defaultValueSql: "GetDate()",
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "CheckIn",
                table: "Attendances",
                type: "time",
                nullable: true,
                defaultValueSql: "CAST(GETDATE() AS TIME)",
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Attendances",
                type: "int",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Attendances",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValueSql: "GetDate()");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "CheckIn",
                table: "Attendances",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true,
                oldDefaultValueSql: "CAST(GETDATE() AS TIME)");
        }
    }
}
