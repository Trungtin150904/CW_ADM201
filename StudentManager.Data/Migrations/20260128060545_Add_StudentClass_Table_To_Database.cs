using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_StudentClass_Table_To_Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StudentClassId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "StudentClasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClasses", x => x.Id);
                });

            migrationBuilder.Sql(@" Insert into StudentClasses(Id, Name)
                                    Values('D8498690-EC3B-4B7B-A47C-C6EE1E7BC4EC', 'COS1205')");
            migrationBuilder.Sql(@"update Students 
                                    set StudentClassId = 'd8498690-ec3b-4b7b-a47c-c6ee1e7bc4ec'");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentClassId",
                table: "Students",
                column: "StudentClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_StudentClasses_StudentClassId",
                table: "Students",
                column: "StudentClassId",
                principalTable: "StudentClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudentClasses_StudentClassId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "StudentClasses");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudentClassId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudentClassId",
                table: "Students");
        }
    }
}
