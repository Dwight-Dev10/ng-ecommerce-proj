using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ng_ecommerce_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    ReviewCount = table.Column<int>(type: "int", nullable: false),
                    InStock = table.Column<bool>(type: "bit", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "ImageUrl", "InStock", "Name", "Price", "Rating", "ReviewCount" },
                values: new object[,]
                {
                    { new Guid("11112222-3333-4444-5555-666677778888"), "Electronics", "34-inch curved monitor for immersive productivity.", "https://example.com/images/monitor.jpg", true, "Ultra-Wide Monitor", 450.00m, 4.7000000000000002, 30 },
                    { new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3301"), "Furniture", "Comfortable mesh chair with lumbar support.", "https://example.com/images/chair.jpg", false, "Ergonomic Office Chair", 249.00m, 4.2000000000000002, 45 },
                    { new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"), "Accessories", "RGB backlit mechanical keyboard with blue switches.", "https://example.com/images/keyboard.jpg", true, "Mechanical Gaming Keyboard", 89.50m, 4.7999999999999998, 85 },
                    { new Guid("99998888-7777-6666-5555-444433332222"), "Home & Kitchen", "Stainless steel insulated water bottle, 32oz.", "https://example.com/images/bottle.jpg", true, "Reusable Water Bottle", 25.00m, 4.9000000000000004, 210 },
                    { new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), "Electronics", "High-quality over-ear headphones with noise cancellation.", "https://example.com/images/headphones.jpg", true, "Wireless Bluetooth Headphones", 199.99m, 4.5, 120 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
