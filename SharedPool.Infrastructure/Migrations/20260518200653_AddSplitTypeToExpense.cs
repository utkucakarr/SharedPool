using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedPool.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSplitTypeToExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SplitType",
                table: "Expenses",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SplitType",
                table: "Expenses");
        }
    }
}
