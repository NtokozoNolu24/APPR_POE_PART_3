using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APPR_POE_FINAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDonorName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DonorName",
                table: "Donations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonorName",
                table: "Donations");
        }
    }
}
