using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgenziaSpedizioni.Migrations
{
    /// <inheritdoc />
    public partial class SpedizioniToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Spedizioni",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    DataSpedizione = table.Column<DateOnly>(nullable: false),
                    DataConsegnaPrevista = table.Column<DateOnly>(nullable: false),
                    Peso = table.Column<double>(nullable: false),
                    CittaDestinataria = table.Column<string>(nullable: false),
                    Indirizzo = table.Column<string>(nullable: false),
                    CostoSpedizione = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spedizioni", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spedizioni_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Spedizioni_UserId",
                table: "Spedizioni",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Spedizioni");
        }
    }
}

