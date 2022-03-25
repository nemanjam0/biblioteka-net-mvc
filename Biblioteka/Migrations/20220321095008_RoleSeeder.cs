using Microsoft.EntityFrameworkCore.Migrations;

namespace Biblioteka.Migrations
{
    public partial class RoleSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Iznajmljivanja_AspNetUsers_BibliotekarId",
                table: "Iznajmljivanja");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 1, "943d3647-8dd0-484a-ab47-306fabd456da", "Korisnik", "KORISNIK" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 2, "bb7e5b95-412e-4af7-9f84-7ada93228570", "Bibliotekar", "BIBLIOTEKAR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 3, "ba109de8-6358-4e56-865e-079d201b59d8", "Admin", "ADMIN" });

            migrationBuilder.AddForeignKey(
                name: "FK_Iznajmljivanja_AspNetUsers_BibliotekarId",
                table: "Iznajmljivanja",
                column: "BibliotekarId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Iznajmljivanja_AspNetUsers_BibliotekarId",
                table: "Iznajmljivanja");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddForeignKey(
                name: "FK_Iznajmljivanja_AspNetUsers_BibliotekarId",
                table: "Iznajmljivanja",
                column: "BibliotekarId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
