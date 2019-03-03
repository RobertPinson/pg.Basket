using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace pg.Basket.Dal.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "basket");

            migrationBuilder.CreateSequence(
                name: "orderitemseq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "orderseq",
                schema: "basket",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "baskets",
                schema: "basket",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baskets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "basketItems",
                schema: "basket",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    BasketId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_basketItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_basketItems_baskets_BasketId",
                        column: x => x.BasketId,
                        principalSchema: "basket",
                        principalTable: "baskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vouchers",
                schema: "basket",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BasketId = table.Column<int>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vouchers_baskets_BasketId",
                        column: x => x.BasketId,
                        principalSchema: "basket",
                        principalTable: "baskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_basketItems_BasketId",
                schema: "basket",
                table: "basketItems",
                column: "BasketId");

            migrationBuilder.CreateIndex(
                name: "IX_vouchers_BasketId",
                schema: "basket",
                table: "vouchers",
                column: "BasketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "basketItems",
                schema: "basket");

            migrationBuilder.DropTable(
                name: "vouchers",
                schema: "basket");

            migrationBuilder.DropTable(
                name: "baskets",
                schema: "basket");

            migrationBuilder.DropSequence(
                name: "orderitemseq");

            migrationBuilder.DropSequence(
                name: "orderseq",
                schema: "basket");
        }
    }
}
