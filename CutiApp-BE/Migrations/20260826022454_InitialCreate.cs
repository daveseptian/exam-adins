using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CutiApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USERNAME = table.Column<string>(type: "text", nullable: false),
                    PASSWORD = table.Column<string>(type: "text", nullable: false),
                    FULLNAME = table.Column<string>(type: "text", nullable: false),
                    ROLE = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LEAVE_BALANCE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USER_ID = table.Column<long>(type: "bigint", nullable: false),
                    REMAINING_DAYS = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEAVE_BALANCE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LEAVE_BALANCE_USER_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LEAVE_REQUEST",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USER_ID = table.Column<long>(type: "bigint", nullable: false),
                    START_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    END_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    REASON = table.Column<string>(type: "text", nullable: false),
                    STATUS = table.Column<string>(type: "text", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEAVE_REQUEST", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LEAVE_REQUEST_USER_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LEAVE_BALANCE_USER_ID",
                table: "LEAVE_BALANCE",
                column: "USER_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LEAVE_REQUEST_USER_ID",
                table: "LEAVE_REQUEST",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_USER_USERNAME",
                table: "USER",
                column: "USERNAME",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LEAVE_BALANCE");

            migrationBuilder.DropTable(
                name: "LEAVE_REQUEST");

            migrationBuilder.DropTable(
                name: "USER");
        }
    }
}
