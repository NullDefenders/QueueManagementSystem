using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectoryService.Migrations
{
    /// <inheritdoc />
    public partial class WorkplacesServiceCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkplaceServiceCategory",
                columns: table => new
                {
                    ServiceCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkplaceId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkplaceServiceCategory", x => new { x.ServiceCategoryId, x.WorkplaceId });
                    table.ForeignKey(
                        name: "FK_WorkplaceServiceCategory_ServiceCategories_ServiceCategoryId",
                        column: x => x.ServiceCategoryId,
                        principalTable: "ServiceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkplaceServiceCategory_Workplaces_WorkplaceId",
                        column: x => x.WorkplaceId,
                        principalTable: "Workplaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkplaceServiceCategory_WorkplaceId",
                table: "WorkplaceServiceCategory",
                column: "WorkplaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkplaceServiceCategory");
        }
    }
}
