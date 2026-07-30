using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrepaidCardApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPhoneAndBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "Cards",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Cards",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Cards");
        }
    }
}
