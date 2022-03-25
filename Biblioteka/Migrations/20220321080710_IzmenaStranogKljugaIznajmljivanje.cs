using Microsoft.EntityFrameworkCore.Migrations;

namespace Biblioteka.Migrations
{
    public partial class IzmenaStranogKljugaIznajmljivanje : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Iznajmljivanja_AspNetUsers_BibliotekarId",
                table: "Iznajmljivanja");

            migrationBuilder.AlterColumn<int>(
                name: "BibliotekarId",
                table: "Iznajmljivanja",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.AlterColumn<int>(
                name: "BibliotekarId",
                table: "Iznajmljivanja",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
