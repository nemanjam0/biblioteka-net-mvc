using Microsoft.EntityFrameworkCore.Migrations;

namespace Biblioteka.Migrations
{
    public partial class AddKorisnikIdForeignKeyIznajmljivanje : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Iznajmljivanja_AspNetUsers_KorisnikId",
                table: "Iznajmljivanja");

            migrationBuilder.DropForeignKey(
                name: "FK_Iznajmljivanja_PrimerciKnjige_PrimerakKnjigeId",
                table: "Iznajmljivanja");

            migrationBuilder.DropIndex(
                name: "IX_Iznajmljivanja_PrimerakKnjigeId",
                table: "Iznajmljivanja");

            migrationBuilder.DropColumn(
                name: "PrimerakKnjigeId",
                table: "Iznajmljivanja");

            migrationBuilder.AlterColumn<int>(
                name: "KorisnikId",
                table: "Iznajmljivanja",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrimerakId",
                table: "Iznajmljivanja",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Iznajmljivanja_PrimerakId",
                table: "Iznajmljivanja",
                column: "PrimerakId");

            migrationBuilder.AddForeignKey(
                name: "FK_Iznajmljivanja_AspNetUsers_KorisnikId",
                table: "Iznajmljivanja",
                column: "KorisnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Iznajmljivanja_PrimerciKnjige_PrimerakId",
                table: "Iznajmljivanja",
                column: "PrimerakId",
                principalTable: "PrimerciKnjige",
                principalColumn: "PrimerakKnjigeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Iznajmljivanja_AspNetUsers_KorisnikId",
                table: "Iznajmljivanja");

            migrationBuilder.DropForeignKey(
                name: "FK_Iznajmljivanja_PrimerciKnjige_PrimerakId",
                table: "Iznajmljivanja");

            migrationBuilder.DropIndex(
                name: "IX_Iznajmljivanja_PrimerakId",
                table: "Iznajmljivanja");

            migrationBuilder.DropColumn(
                name: "PrimerakId",
                table: "Iznajmljivanja");

            migrationBuilder.AlterColumn<int>(
                name: "KorisnikId",
                table: "Iznajmljivanja",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PrimerakKnjigeId",
                table: "Iznajmljivanja",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Iznajmljivanja_PrimerakKnjigeId",
                table: "Iznajmljivanja",
                column: "PrimerakKnjigeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Iznajmljivanja_AspNetUsers_KorisnikId",
                table: "Iznajmljivanja",
                column: "KorisnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Iznajmljivanja_PrimerciKnjige_PrimerakKnjigeId",
                table: "Iznajmljivanja",
                column: "PrimerakKnjigeId",
                principalTable: "PrimerciKnjige",
                principalColumn: "PrimerakKnjigeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
