using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    AppUserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PwdHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
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
                    NationIdentityNo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DriverLicenceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DriverLicenceClass = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
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
                    VehiclePlate = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleModel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleBrand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleLicenceSerialNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleRegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
                });

            migrationBuilder.CreateTable(
                name: "TrafficFines",
                columns: table => new
                {
                    TrafficFineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrafficFineRecordNo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TrafficFineDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrafficFineAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DateOfNotification = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    TrafficFineTypeId = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecordedByPersonnelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficFines", x => x.TrafficFineId);
                    table.ForeignKey(
                        name: "FK_TrafficFines_AppUsers_RecordedByPersonnelId",
                        column: x => x.RecordedByPersonnelId,
                        principalTable: "AppUsers",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrafficFines_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "DriverId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrafficFines_TrafficFineTypes_TrafficFineTypeId",
                        column: x => x.TrafficFineTypeId,
                        principalTable: "TrafficFineTypes",
                        principalColumn: "TrafficFineTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrafficFines_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrafficFineApprovalHistories",
                columns: table => new
                {
                    TrafficFineApprovalHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrafficFineId = table.Column<int>(type: "int", nullable: false),
                    PerformedByUserId = table.Column<int>(type: "int", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PreviousStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficFineApprovalHistories", x => x.TrafficFineApprovalHistoryId);
                    table.ForeignKey(
                        name: "FK_TrafficFineApprovalHistories_AppUsers_PerformedByUserId",
                        column: x => x.PerformedByUserId,
                        principalTable: "AppUsers",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrafficFineApprovalHistories_TrafficFines_TrafficFineId",
                        column: x => x.TrafficFineId,
                        principalTable: "TrafficFines",
                        principalColumn: "TrafficFineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_AppUserName",
                table: "AppUsers",
                column: "AppUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_NationIdentityNo",
                table: "Drivers",
                column: "NationIdentityNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrafficFineApprovalHistories_PerformedByUserId",
                table: "TrafficFineApprovalHistories",
                column: "PerformedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrafficFineApprovalHistories_TrafficFineId",
                table: "TrafficFineApprovalHistories",
                column: "TrafficFineId");

            migrationBuilder.CreateIndex(
                name: "IX_TrafficFines_DriverId",
                table: "TrafficFines",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_TrafficFines_RecordedByPersonnelId",
                table: "TrafficFines",
                column: "RecordedByPersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_TrafficFines_TrafficFineRecordNo",
                table: "TrafficFines",
                column: "TrafficFineRecordNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrafficFines_TrafficFineTypeId",
                table: "TrafficFines",
                column: "TrafficFineTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrafficFines_VehicleId",
                table: "TrafficFines",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehiclePlate",
                table: "Vehicles",
                column: "VehiclePlate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrafficFineApprovalHistories");

            migrationBuilder.DropTable(
                name: "TrafficFines");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "TrafficFineTypes");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
