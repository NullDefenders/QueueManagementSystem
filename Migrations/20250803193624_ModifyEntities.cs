using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectoryService.Migrations
{
    /// <inheritdoc />
    public partial class ModifyEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkplaceId",
                table: "Services",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_WorkplaceId",
                table: "Services",
                column: "WorkplaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Workplaces_WorkplaceId",
                table: "Services",
                column: "WorkplaceId",
                principalTable: "Workplaces",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Workplaces_WorkplaceId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_WorkplaceId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "WorkplaceId",
                table: "Services");
        }
    }
}
