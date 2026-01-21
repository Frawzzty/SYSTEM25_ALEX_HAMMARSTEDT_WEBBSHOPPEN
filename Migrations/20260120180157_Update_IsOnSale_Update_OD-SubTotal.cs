using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class Update_IsOnSale_Update_ODSubTotal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OnSale",
                table: "Products",
                newName: "IsOnSale");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderDetails",
                newName: "SubTotal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsOnSale",
                table: "Products",
                newName: "OnSale");

            migrationBuilder.RenameColumn(
                name: "SubTotal",
                table: "OrderDetails",
                newName: "Price");
        }
    }
}
