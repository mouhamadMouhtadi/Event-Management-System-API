using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.Repository.Migrations.App
{
    /// <inheritdoc />
    public partial class AddPaymentFieldsToRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientSecret",
                table: "Registrations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "Registrations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Registrations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientSecret",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Registrations");
        }
    }
}
