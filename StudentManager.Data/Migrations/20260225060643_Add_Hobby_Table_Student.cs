using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Hobby_Table_Student : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Hobby",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hobby",
                table: "Students");
        }
    }
}
