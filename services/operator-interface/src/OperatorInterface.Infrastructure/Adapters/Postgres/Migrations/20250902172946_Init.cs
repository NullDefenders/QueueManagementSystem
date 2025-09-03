using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OperatorInterface.Infrastructure.Adapters.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "operator_sessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    operator_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    workplace_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    session_start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    session_end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    assigned_services = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operator_sessions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "client_sessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticket_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    assignment_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    result = table.Column<string>(type: "text", nullable: true),
                    operator_session_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client_sessions", x => x.id);
                    table.ForeignKey(
                        name: "FK_client_sessions_operator_sessions_operator_session_id",
                        column: x => x.operator_session_id,
                        principalTable: "operator_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_client_sessions_operator_session_id",
                table: "client_sessions",
                column: "operator_session_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "client_sessions");

            migrationBuilder.DropTable(
                name: "operator_sessions");
        }
    }
}
