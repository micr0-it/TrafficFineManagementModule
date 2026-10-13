using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    AppUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PwdHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameSurname = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.AppUserId);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    DriverId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverNameSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationIdentityNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DriverLicenceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DriverLicenceClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DriverFinePoint = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.DriverId);
                });

            migrationBuilder.CreateTable(
                name: "TrafficFineTypes",
                columns: table => new
                {
                    TrafficFineTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Article = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TrafficFinePoint = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficFineTypes", x => x.TrafficFineTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehiclePlate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleModel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleBrand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleLicenceSerialNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleLicenseValidationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VehicleLicenseHolderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
                    table.ForeignKey(
                        name: "FK_Vehicles_Drivers_VehicleLicenseHolderId",
                        column: x => x.VehicleLicenseHolderId,
                        principalTable: "Drivers",
                        principalColumn: "DriverId");
                });

            migrationBuilder.CreateTable(
                name: "TrafikFines",
                columns: table => new
                {
                    TrafficFineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrafficFineRecordNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrafficFineDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrafficFineAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    FluentStaffId = table.Column<int>(type: "int", nullable: false),
                    FluentAdminId = table.Column<int>(type: "int", nullable: true),
                    FluentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefusalReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrafficFineTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafikFines", x => x.TrafficFineId);
                    table.ForeignKey(
                        name: "FK_TrafikFines_AppUsers_FluentAdminId",
                        column: x => x.FluentAdminId,
                        principalTable: "AppUsers",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrafikFines_AppUsers_FluentStaffId",
                        column: x => x.FluentStaffId,
                        principalTable: "AppUsers",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrafikFines_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "DriverId");
                    table.ForeignKey(
                        name: "FK_TrafikFines_TrafficFineTypes_TrafficFineTypeId",
                        column: x => x.TrafficFineTypeId,
                        principalTable: "TrafficFineTypes",
                        principalColumn: "TrafficFineTypeId");
                    table.ForeignKey(
                        name: "FK_TrafikFines_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_NameSurname",
                table: "AppUsers",
                column: "NameSurname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrafikFines_DriverId",
                table: "TrafikFines",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_TrafikFines_FluentAdminId",
                table: "TrafikFines",
                column: "FluentAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_TrafikFines_FluentStaffId",
                table: "TrafikFines",
                column: "FluentStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_TrafikFines_TrafficFineTypeId",
                table: "TrafikFines",
                column: "TrafficFineTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrafikFines_VehicleId",
                table: "TrafikFines",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleLicenseHolderId",
                table: "Vehicles",
                column: "VehicleLicenseHolderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrafikFines");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "TrafficFineTypes");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Drivers");
        }
    }
}
