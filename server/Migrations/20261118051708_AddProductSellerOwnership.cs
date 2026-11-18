using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ng_ecommerce_api.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSellerOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SellerProfileId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), 0, "DEMO-STORE-OWNER-CONCURRENCY-STAMP", "demo-store-owner@seed.local", false, true, null, "DEMO-STORE-OWNER@SEED.LOCAL", "DEMO-STORE-OWNER@SEED.LOCAL", null, null, false, "DEMO-STORE-OWNER-SECURITY-STAMP", false, "demo-store-owner@seed.local" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("11112222-3333-4444-5555-666677778888"),
                column: "SellerProfileId",
                value: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3301"),
                column: "SellerProfileId",
                value: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                column: "SellerProfileId",
                value: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("99998888-7777-6666-5555-444433332222"),
                column: "SellerProfileId",
                value: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                column: "SellerProfileId",
                value: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.InsertData(
                table: "SellerProfiles",
                columns: new[] { "Id", "CreatedAt", "Description", "StoreName", "UserId" },
                values: new object[] { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Default store for demonstration products.", "Demo Store", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") });

            migrationBuilder.Sql("""
                UPDATE [Products]
                SET [SellerProfileId] = 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'
                WHERE [SellerProfileId] = '00000000-0000-0000-0000-000000000000';
                """);
            
            migrationBuilder.CreateIndex(
                name: "IX_Products_SellerProfileId",
                table: "Products",
                column: "SellerProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SellerProfiles_SellerProfileId",
                table: "Products",
                column: "SellerProfileId",
                principalTable: "SellerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_SellerProfiles_SellerProfileId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SellerProfileId",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "SellerProfiles",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DropColumn(
                name: "SellerProfileId",
                table: "Products");
        }
    }
}
