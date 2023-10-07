using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.Migrations
{
    /// <inheritdoc />
    public partial class Remove_bookOrigin_column_from_Book : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Books_BookAsOriginReference",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookAsOriginReference",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookAsOriginReference",
                table: "Books");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookAsOriginReference",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookAsOriginReference",
                table: "Books",
                column: "BookAsOriginReference");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Books_BookAsOriginReference",
                table: "Books",
                column: "BookAsOriginReference",
                principalTable: "Books",
                principalColumn: "Id");
        }
    }
}
