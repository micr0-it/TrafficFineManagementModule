using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class OdemeVeItiraz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrafikFines_AppUsers_FluentAdminId",
                table: "TrafikFines");

            migrationBuilder.DropForeignKey(
                name: "FK_TrafikFines_AppUsers_FluentStaffId",
                table: "TrafikFines");

            migrationBuilder.DropForeignKey(
                name: "FK_TrafikFines_Drivers_DriverId",
                table: "TrafikFines");

            migrationBuilder.DropForeignKey(
                name: "FK_TrafikFines_TrafficFineTypes_TrafficFineTypeId",
                table: "TrafikFines");

            migrationBuilder.DropForeignKey(
                name: "FK_TrafikFines_Vehicles_VehicleId",
                table: "TrafikFines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrafikFines",
                table: "TrafikFines");

            migrationBuilder.RenameTable(
                name: "TrafikFines",
                newName: "TrafficFines");

            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "TrafficFineTypes",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "FluentStaffId",
                table: "TrafficFines",
                newName: "RecordedByPersonalId");

            migrationBuilder.RenameColumn(
                name: "FluentDate",
                table: "TrafficFines",
                newName: "RecordedByDate");

            migrationBuilder.RenameColumn(
                name: "FluentAdminId",
                table: "TrafficFines",
                newName: "RecordedByAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_TrafikFines_VehicleId",
                table: "TrafficFines",
                newName: "IX_TrafficFines_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_TrafikFines_TrafficFineTypeId",
                table: "TrafficFines",
                newName: "IX_TrafficFines_TrafficFineTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_TrafikFines_FluentStaffId",
                table: "TrafficFines",
                newName: "IX_TrafficFines_RecordedByPersonalId");

            migrationBuilder.RenameIndex(
                name: "IX_TrafikFines_FluentAdminId",
                table: "TrafficFines",
                newName: "IX_TrafficFines_RecordedByAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_TrafikFines_DriverId",
                table: "TrafficFines",
                newName: "IX_TrafficFines_DriverId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfNotification",
                table: "TrafficFines",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "TrafficFines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrafficFines",
                table: "TrafficFines",
                column: "TrafficFineId");

            migrationBuilder.CreateTable(
                name: "Appeals",
                columns: table => new
                {
                    AppealId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrafficFineId = table.Column<int>(type: "int", nullable: false),
                    AppealDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppealReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppealStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutcomeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OutcameDateDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordedByPersonalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appeals", x => x.AppealId);
                    table.ForeignKey(
                        name: "FK_Appeals_AppUsers_RecordedByPersonalId",
                        column: x => x.RecordedByPersonalId,
                        principalTable: "AppUsers",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appeals_TrafficFines_TrafficFineId",
                        column: x => x.TrafficFineId,
                        principalTable: "TrafficFines",
                        principalColumn: "TrafficFineId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrafficFineId = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsDiscounted = table.Column<bool>(type: "bit", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiptNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordedByPersonalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_AppUsers_RecordedByPersonalId",
                        column: x => x.RecordedByPersonalId,
                        principalTable: "AppUsers",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_TrafficFines_TrafficFineId",
                        column: x => x.TrafficFineId,
                        principalTable: "TrafficFines",
                        principalColumn: "TrafficFineId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appeals_RecordedByPersonalId",
                table: "Appeals",
                column: "RecordedByPersonalId");

            migrationBuilder.CreateIndex(
                name: "IX_Appeals_TrafficFineId",
                table: "Appeals",
                column: "TrafficFineId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_RecordedByPersonalId",
                table: "Payments",
                column: "RecordedByPersonalId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TrafficFineId",
                table: "Payments",
                column: "TrafficFineId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TrafficFines_AppUsers_RecordedByAdminId",
                table: "TrafficFines",
                column: "RecordedByAdminId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrafficFines_AppUsers_RecordedByPersonalId",
                table: "TrafficFines",
                column: "RecordedByPersonalId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrafficFines_Drivers_DriverId",
                table: "TrafficFines",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrafficFines_TrafficFineTypes_TrafficFineTypeId",
                table: "TrafficFines",
                column: "TrafficFineTypeId",
                principalTable: "TrafficFineTypes",
                principalColumn: "TrafficFineTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrafficFines_Vehicles_VehicleId",
                table: "TrafficFines",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrafficFines_AppUsers_RecordedByAdminId",
                table: "TrafficFines");

            migrationBuilder.DropForeignKey(
                name: "FK_TrafficFines_AppUsers_RecordedByPersonalId",
                table: "TrafficFines");

            migrationBuilder.DropForeignKey(
                name: "FK_TrafficFines_Drivers_DriverId",
                table: "TrafficFines");

            migrationBuilder.DropForeignKey(
                name: "FK_TrafficFines_TrafficFineTypes_TrafficFineTypeId",
                table: "TrafficFines");

            migrationBuilder.DropForeignKey(
                name: "FK_TrafficFines_Vehicles_VehicleId",
                table: "TrafficFines");

            migrationBuilder.DropTable(
                name: "Appeals");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrafficFines",
                table: "TrafficFines");

            migrationBuilder.DropColumn(
                name: "DateOfNotification",
                table: "TrafficFines");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TrafficFines");

            migrationBuilder.RenameTable(
                name: "TrafficFines",
                newName: "TrafikFines");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "TrafficFineTypes",
                newName: "isActive");

            migrationBuilder.RenameColumn(
                name: "RecordedByPersonalId",
                table: "TrafikFines",
                newName: "FluentStaffId");

            migrationBuilder.RenameColumn(
                name: "RecordedByDate",
                table: "TrafikFines",
                newName: "FluentDate");

            migrationBuilder.RenameColumn(
                name: "RecordedByAdminId",
                table: "TrafikFines",
                newName: "FluentAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_TrafficFines_VehicleId",
                table: "TrafikFines",
                newName: "IX_TrafikFines_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_TrafficFines_TrafficFineTypeId",
                table: "TrafikFines",
                newName: "IX_TrafikFines_TrafficFineTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_TrafficFines_RecordedByPersonalId",
                table: "TrafikFines",
                newName: "IX_TrafikFines_FluentStaffId");

            migrationBuilder.RenameIndex(
                name: "IX_TrafficFines_RecordedByAdminId",
                table: "TrafikFines",
                newName: "IX_TrafikFines_FluentAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_TrafficFines_DriverId",
                table: "TrafikFines",
                newName: "IX_TrafikFines_DriverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrafikFines",
                table: "TrafikFines",
                column: "TrafficFineId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrafikFines_AppUsers_FluentAdminId",
                table: "TrafikFines",
                column: "FluentAdminId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrafikFines_AppUsers_FluentStaffId",
                table: "TrafikFines",
                column: "FluentStaffId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrafikFines_Drivers_DriverId",
                table: "TrafikFines",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrafikFines_TrafficFineTypes_TrafficFineTypeId",
                table: "TrafikFines",
                column: "TrafficFineTypeId",
                principalTable: "TrafficFineTypes",
                principalColumn: "TrafficFineTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrafikFines_Vehicles_VehicleId",
                table: "TrafikFines",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
