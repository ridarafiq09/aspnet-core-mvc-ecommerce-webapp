using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlipShop.Infrastructure.Migrations
{
    public partial class add_remaining_tbls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_CustomerID",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "tbl_orders");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CustomerID",
                table: "tbl_orders",
                newName: "IX_tbl_orders_CustomerID");

            migrationBuilder.AddColumn<int>(
                name: "orderAddressId",
                table: "tbl_orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "userTransactionId",
                table: "tbl_orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_orders",
                table: "tbl_orders",
                column: "OrderID");

            migrationBuilder.CreateTable(
                name: "tbl_order_address",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressLine1 = table.Column<string>(type: "varchar(500)", nullable: false),
                    AddressLine2 = table.Column<string>(type: "varchar(500)", nullable: false),
                    City = table.Column<string>(type: "varchar(100)", nullable: false),
                    State = table.Column<string>(type: "varchar(100)", nullable: false),
                    Postal = table.Column<short>(type: "smallint", nullable: false),
                    Country = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_order_address", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ordered_products_details",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Order_ID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Unit_Price = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ordered_products_details", x => new { x.ProductID, x.Order_ID });
                    table.ForeignKey(
                        name: "FK_tbl_ordered_products_details_tbl_orders_Order_ID",
                        column: x => x.Order_ID,
                        principalTable: "tbl_orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_ordered_products_details_tbl_product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "tbl_product",
                        principalColumn: "Product_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_transaction_details",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameOnCard = table.Column<string>(type: "varchar(100)", nullable: false),
                    CVV = table.Column<short>(type: "smallint", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    CardNumber = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_transaction_details", x => x.TransactionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_orders_orderAddressId",
                table: "tbl_orders",
                column: "orderAddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_orders_userTransactionId",
                table: "tbl_orders",
                column: "userTransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ordered_products_details_Order_ID",
                table: "tbl_ordered_products_details",
                column: "Order_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderAddress",
                table: "tbl_orders",
                column: "orderAddressId",
                principalTable: "tbl_order_address",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_orders_AspNetUsers_CustomerID",
                table: "tbl_orders",
                column: "CustomerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_orders_tbl_user_transaction_details_userTransactionId",
                table: "tbl_orders",
                column: "userTransactionId",
                principalTable: "tbl_user_transaction_details",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderAddress",
                table: "tbl_orders");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_orders_AspNetUsers_CustomerID",
                table: "tbl_orders");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_orders_tbl_user_transaction_details_userTransactionId",
                table: "tbl_orders");

            migrationBuilder.DropTable(
                name: "tbl_order_address");

            migrationBuilder.DropTable(
                name: "tbl_ordered_products_details");

            migrationBuilder.DropTable(
                name: "tbl_user_transaction_details");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_orders",
                table: "tbl_orders");

            migrationBuilder.DropIndex(
                name: "IX_tbl_orders_orderAddressId",
                table: "tbl_orders");

            migrationBuilder.DropIndex(
                name: "IX_tbl_orders_userTransactionId",
                table: "tbl_orders");

            migrationBuilder.DropColumn(
                name: "orderAddressId",
                table: "tbl_orders");

            migrationBuilder.DropColumn(
                name: "userTransactionId",
                table: "tbl_orders");

            migrationBuilder.RenameTable(
                name: "tbl_orders",
                newName: "Orders");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_orders_CustomerID",
                table: "Orders",
                newName: "IX_Orders_CustomerID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_CustomerID",
                table: "Orders",
                column: "CustomerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
