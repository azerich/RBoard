using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Simulation.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Gender = table.Column<byte>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    Locale = table.Column<int>(nullable: false),
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    ConfirmDate = table.Column<DateTime>(nullable: true),
                    LastLoginDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocalizedMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Locale = table.Column<int>(nullable: false),
                    Word = table.Column<int>(nullable: false),
                    Sentence = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizedMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3B1C34F1-C8E6-4013-AB5F-DF156968DAAE", "48bd3fe2-49d4-4c01-97af-efeeb7e6296b", "Administrator", "ADMINISTRATOR" },
                    { "81F5E7BF-CAD7-4EEE-8D8B-2ABB2B071849", "59d87a82-abde-4685-9102-cba2c9daa1b9", "Confirmed", "CONFIRMED" },
                    { "F090C70C-FFD2-49D2-9C57-A81DF9384206", "1f2d9d1b-eb7c-4b88-98df-2d3705103618", "Registered", "REGISTERED" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "ConfirmDate", "Email", "EmailConfirmed", "FirstName", "Gender", "LastLoginDate", "LastName", "Locale", "LockoutEnabled", "LockoutEnd", "MiddleName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RegisterDate", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "A34B367E-7677-4730-BAD0-13A419B0796A", 0, null, "48ce8ee8-f6d9-452c-8859-6d11719d6258", new DateTime(2020, 6, 25, 19, 42, 22, 405, DateTimeKind.Utc).AddTicks(191), "admin@rboard.com", true, "Site", null, null, "Administrator", 0, false, null, null, "ADMIN@RBOARD.COM", "ADMIN", "AQAAAAEAACcQAAAAEOF2/uktz1iUg8kxQdr8/XCxg4Redt7pg/GyQPY0wcteIZ+a5Zg5C482RngRvYDNeQ==", null, false, new DateTime(2020, 6, 25, 19, 42, 22, 404, DateTimeKind.Utc).AddTicks(9012), "d3dbd471-1332-4b00-9f06-b3e87c624a13", false, "admin" });

            migrationBuilder.InsertData(
                table: "LocalizedMessages",
                columns: new[] { "Id", "Locale", "Message", "Sentence", "Word" },
                values: new object[,]
                {
                    { new Guid("bf562d8b-b10f-47de-95e1-1cdb8dd236d5"), 0, "Danger", 0, 1 },
                    { new Guid("02d3ee00-2f51-4d89-8699-9ba120c26d18"), 0, "Your Email is confirmed", 17, 0 },
                    { new Guid("aa5b2b3f-a6b3-49c3-99d5-cca871f6a53f"), 0, "You must confirm it before login", 16, 0 },
                    { new Guid("a22aa690-5b3d-4445-8de4-4b742a2fedbe"), 0, "You are not allowed", 15, 0 },
                    { new Guid("4dfdaff6-603e-4859-bef7-0b33d7d74bf8"), 0, "Wrong user credentials to verify your email", 14, 0 },
                    { new Guid("afe92478-a9e0-4422-88bd-f206e9ab5db5"), 0, "Wrong password", 13, 0 },
                    { new Guid("e526e4e8-f85e-4467-b58d-9c1c2f042315"), 0, "Wrong Email verification code", 12, 0 },
                    { new Guid("b0e6ed47-9b54-4936-af41-200c22bd819a"), 0, "Verification code successfully send to your email", 11, 0 },
                    { new Guid("f180937d-d98a-4660-bc82-41dce5966b5b"), 0, "User not found", 10, 0 },
                    { new Guid("c696cf4e-7a29-41fb-97aa-cf94e78773ee"), 0, "This is your first authorized visit to site", 9, 0 },
                    { new Guid("eea5c066-842b-4608-bfec-d706e1ece19c"), 0, "Site error", 8, 0 },
                    { new Guid("b0ababd4-fcd0-43f9-8669-86815f95e700"), 0, "See you again", 7, 0 },
                    { new Guid("7f7cf1a6-0bf0-4378-baee-6146ce977489"), 0, "Passwords mismatch", 6, 0 },
                    { new Guid("d30fcd3d-c26d-4b2c-968d-08ad0888f76f"), 0, "Password must contain at least", 5, 0 },
                    { new Guid("b2ea1dab-1924-4ea6-9e43-a0194e21855d"), 0, "Bye", 0, 0 },
                    { new Guid("eb1d39ab-6946-4654-a2b7-d31da9a0b4bb"), 0, "or more", 4, 0 },
                    { new Guid("53704287-05c8-4a2c-8a8b-62d6f8d7a35b"), 0, "Your Email is not confirmed", 18, 0 },
                    { new Guid("aedbc25c-bb65-4f14-92ea-d7c91206e9e4"), 0, "Can not create a user", 1, 0 },
                    { new Guid("fe54e1f3-7707-4d81-9412-c0f8a847d176"), 0, "Account deleted", 0, 0 },
                    { new Guid("4f1c35ec-0bfc-44d7-b018-84353763f10e"), 0, "Welcome", 0, 11 },
                    { new Guid("bc26e07b-9a27-4a39-af0d-472373015a0f"), 0, "Warning", 0, 10 },
                    { new Guid("05da1568-92e2-4c13-8fb3-875b47ff7d99"), 0, "User name", 0, 9 },
                    { new Guid("575a4a2d-c1a3-4126-bcf1-d5dc8493c7a4"), 0, "Success", 0, 8 },
                    { new Guid("6c2e94a9-0fde-4114-91ee-5d9c50251fe5"), 0, "Secondary", 0, 7 },
                    { new Guid("30505114-973a-4f4e-bd98-ec28fd757a7c"), 0, "Primary", 0, 6 },
                    { new Guid("c9f5b633-7be0-4dfd-bba4-03d1d8ee6d66"), 0, "Tip", 0, 5 },
                    { new Guid("672ba02a-a64f-40a8-b73f-a72c22542300"), 0, "Information", 0, 4 },
                    { new Guid("e8bbb596-cadf-41d2-872c-a0d6c63002ba"), 0, "Email", 0, 3 },
                    { new Guid("087620e1-d585-4259-98b2-87801c9af9b2"), 0, "Dark", 0, 2 },
                    { new Guid("d7290a7f-4053-4765-92bd-9fda5d725efc"), 0, "Last time you were on the site is", 3, 0 },
                    { new Guid("8f858baa-2846-4611-91f8-df3d253f7a67"), 0, "is already taken", 2, 0 }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "A34B367E-7677-4730-BAD0-13A419B0796A", "F090C70C-FFD2-49D2-9C57-A81DF9384206" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "A34B367E-7677-4730-BAD0-13A419B0796A", "81F5E7BF-CAD7-4EEE-8D8B-2ABB2B071849" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "A34B367E-7677-4730-BAD0-13A419B0796A", "3B1C34F1-C8E6-4013-AB5F-DF156968DAAE" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "LocalizedMessages");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
