using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarCreate.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StokKartBilgi",
                columns: table => new
                {
                    StokNo = table.Column<string>(type: "char(18)", nullable: false),
                    KasaIciMiktar = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    EksiltmeMiktar = table.Column<decimal>(type: "decimal(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StokKartBilgi", x => x.StokNo);
                });

            migrationBuilder.CreateTable(
                name: "Barkod",
                columns: table => new
                {
                    BarkodNo = table.Column<string>(type: "char(10)", nullable: false),
                    StokNo = table.Column<string>(type: "char(18)", nullable: false),
                    KasaIciMiktar = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    EksiltmeMiktar = table.Column<decimal>(type: "decimal(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barkod", x => x.BarkodNo);
                    table.ForeignKey(
                        name: "FK_Barkod_StokKartBilgi_StokNo",
                        column: x => x.StokNo,
                        principalTable: "StokKartBilgi",
                        principalColumn: "StokNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Barkod_StokNo",
                table: "Barkod",
                column: "StokNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Barkod");

            migrationBuilder.DropTable(
                name: "StokKartBilgi");
        }
    }
}
