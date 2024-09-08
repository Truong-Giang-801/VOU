using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vou.Services.AuthAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserBrandTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserBrand",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBrand", x => new { x.BrandId, x.UserID });
                    table.ForeignKey(
                        name: "FK_UserBrand_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBrand_UserID",
                table: "UserBrand",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBrand");
        }
    }
}
