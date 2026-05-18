using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PatientFollowUp.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDemoSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AuditEvents",
                columns: new[] { "Id", "Action", "Details", "EntityId", "EntityType", "Timestamp" },
                values: new object[,]
                {
                    { 1, "Created", "Initial demo follow-up task created during database seeding.", 1, "FollowUpTask", new DateTime(2026, 5, 10, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "Created", "Initial demo follow-up task created during database seeding.", 2, "FollowUpTask", new DateTime(2026, 5, 11, 15, 30, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "FollowUpTasks",
                columns: new[] { "Id", "CreatedAt", "Description", "DueDate", "PatientReferenceCode", "Status", "TaskType", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 10, 14, 0, 0, 0, DateTimeKind.Utc), "Review demonstration lab follow-up task.", new DateOnly(2026, 5, 15), "DEMO-001", 0, 0, new DateTime(2026, 5, 10, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2026, 5, 11, 15, 30, 0, 0, DateTimeKind.Utc), "Send demonstration appointment reminder.", new DateOnly(2026, 5, 20), "DEMO-002", 1, 1, new DateTime(2026, 5, 12, 16, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AuditEvents",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AuditEvents",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FollowUpTasks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FollowUpTasks",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
