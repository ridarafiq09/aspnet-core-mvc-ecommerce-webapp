using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlipShop.Infrastructure.Migrations
{
    public partial class AddProductAndCategoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "tbl_category",
                columns: table => new
                {
                    Category_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category_Title = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Category_Description = table.Column<string>(type: "nvarchar(400)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_category", x => x.Category_ID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_product",
                columns: table => new
                {
                    Product_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Product_Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Product_Description = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    Product_Price = table.Column<decimal>(type: "money", nullable: false),
                    Product_SKU = table.Column<int>(type: "int", nullable: false),
                    Product_Weight = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Supplier_Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Category_Id = table.Column<int>(type: "int", nullable: false),
                    Publish_Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Product_ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitOnOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product", x => x.Product_ID);
                    table.ForeignKey(
                        name: "FK_tbl_product_tbl_category_Category_Id",
                        column: x => x.Category_Id,
                        principalTable: "tbl_category",
                        principalColumn: "Category_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_Category_Id",
                table: "tbl_product",
                column: "Category_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_product");

            migrationBuilder.DropTable(
                name: "tbl_category");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");
        }
    }
}
