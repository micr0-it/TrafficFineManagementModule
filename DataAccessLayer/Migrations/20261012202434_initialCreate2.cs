using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appeals_AppUsers_RecordedByPersonalId",
                table: "Appeals");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AppUsers_RecordedByPersonalId",
                table: "Payments");

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

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Drivers_VehicleLicenseHolderId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_NameSurname",
                table: "AppUsers");

            migrationBuilder.RenameColumn(
                name: "VehicleLicenseValidationDate",
                table: "Vehicles",
                newName: "VehicleRegistrationDate");

            migrationBuilder.RenameColumn(
                name: "RecordedByPersonalId",
                table: "TrafficFines",
                newName: "RecordedByPersonnelId");

            migrationBuilder.RenameColumn(
                name: "RecordedByDate",
                table: "TrafficFines",
                newName: "ApprovalDate");

            migrationBuilder.RenameColumn(
                name: "RecordedByAdminId",
                table: "TrafficFines",
                newName: "ApprovedByAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_TrafficFines_RecordedByPersonalId",
                table: "TrafficFines",
                newName: "IX_TrafficFines_RecordedByPersonnelId");

            migrationBuilder.RenameIndex(
                name: "IX_TrafficFines_RecordedByAdminId",
                table: "TrafficFines",
                newName: "IX_TrafficFines_ApprovedByAdminId");

            migrationBuilder.RenameColumn(
                name: "RecordedByPersonalId",
                table: "Payments",
                newName: "RecordedByPersonnelId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_RecordedByPersonalId",
                table: "Payments",
                newName: "IX_Payments_RecordedByPersonnelId");

            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "AppUsers",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "RecordedByPersonalId",
                table: "Appeals",
                newName: "RecordedByPersonnelId");

            migrationBuilder.RenameColumn(
                name: "OutcameDateDesc",
                table: "Appeals",
                newName: "OutcomeDescription");

            migrationBuilder.RenameIndex(
                name: "IX_Appeals_RecordedByPersonalId",
                table: "Appeals",
                newName: "IX_Appeals_RecordedByPersonnelId");

            migrationBuilder.AlterColumn<string>(
                name: "VehiclePlate",
                table: "Vehicles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleLicenseHolderId",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrafficFineTypeId",
                table: "TrafficFines",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrafficFineRecordNo",
                table: "TrafficFines",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ApprovalStatus",
                table: "TrafficFines",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "NameSurname",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserName",
                table: "AppUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehiclePlate",
                table: "Vehicles",
                column: "VehiclePlate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrafficFines_TrafficFineRecordNo",
                table: "TrafficFines",
                column: "TrafficFineRecordNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_AppUserName",
                table: "AppUsers",
                column: "AppUserName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appeals_AppUsers_RecordedByPersonnelId",
                table: "Appeals",
                column: "RecordedByPersonnelId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AppUsers_RecordedByPersonnelId",
                table: "Payments",
                column: "RecordedByPersonnelId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrafficFines_AppUsers_ApprovedByAdminId",
                table: "TrafficFines",
                column: "ApprovedByAdminId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrafficFines_AppUsers_RecordedByPersonnelId",
                table: "TrafficFines",
                column: "RecordedByPersonnelId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrafficFines_Drivers_DriverId",
                table: "TrafficFines",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "DriverId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrafficFines_TrafficFineTypes_TrafficFineTypeId",
                table: "TrafficFines",
                column: "TrafficFineTypeId",
                principalTable: "TrafficFineTypes",
                principalColumn: "TrafficFineTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrafficFines_Vehicles_VehicleId",
                table: "TrafficFines",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Drivers_VehicleLicenseHolderId",
                table: "Vehicles",
                column: "VehicleLicenseHolderId",
                principalTable: "Drivers",
                principalColumn: "DriverId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appeals_AppUsers_RecordedByPersonnelId",
                table: "Appeals");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AppUsers_RecordedByPersonnelId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_TrafficFines_AppUsers_ApprovedByAdminId",
                table: "TrafficFines");

            migrationBuilder.DropForeignKey(
                name: "FK_TrafficFines_AppUsers_RecordedByPersonnelId",
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

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Drivers_VehicleLicenseHolderId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_VehiclePlate",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_TrafficFines_TrafficFineRecordNo",
                table: "TrafficFines");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_AppUserName",
                table: "AppUsers");

            migrationBuilder.RenameColumn(
                name: "VehicleRegistrationDate",
                table: "Vehicles",
                newName: "VehicleLicenseValidationDate");

            migrationBuilder.RenameColumn(
                name: "RecordedByPersonnelId",
                table: "TrafficFines",
                newName: "RecordedByPersonalId");

            migrationBuilder.RenameColumn(
                name: "ApprovedByAdminId",
                table: "TrafficFines",
                newName: "RecordedByAdminId");

            migrationBuilder.RenameColumn(
                name: "ApprovalDate",
                table: "TrafficFines",
                newName: "RecordedByDate");

            migrationBuilder.RenameIndex(
                name: "IX_TrafficFines_RecordedByPersonnelId",
                table: "TrafficFines",
                newName: "IX_TrafficFines_RecordedByPersonalId");

            migrationBuilder.RenameIndex(
                name: "IX_TrafficFines_ApprovedByAdminId",
                table: "TrafficFines",
                newName: "IX_TrafficFines_RecordedByAdminId");

            migrationBuilder.RenameColumn(
                name: "RecordedByPersonnelId",
                table: "Payments",
                newName: "RecordedByPersonalId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_RecordedByPersonnelId",
                table: "Payments",
                newName: "IX_Payments_RecordedByPersonalId");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "AppUsers",
                newName: "isActive");

            migrationBuilder.RenameColumn(
                name: "RecordedByPersonnelId",
                table: "Appeals",
                newName: "RecordedByPersonalId");

            migrationBuilder.RenameColumn(
                name: "OutcomeDescription",
                table: "Appeals",
                newName: "OutcameDateDesc");

            migrationBuilder.RenameIndex(
                name: "IX_Appeals_RecordedByPersonnelId",
                table: "Appeals",
                newName: "IX_Appeals_RecordedByPersonalId");

            migrationBuilder.AlterColumn<string>(
                name: "VehiclePlate",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleLicenseHolderId",
                table: "Vehicles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TrafficFineTypeId",
                table: "TrafficFines",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TrafficFineRecordNo",
                table: "TrafficFines",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "ApprovalStatus",
                table: "TrafficFines",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "NameSurname",
                table: "AppUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserName",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_NameSurname",
                table: "AppUsers",
                column: "NameSurname",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appeals_AppUsers_RecordedByPersonalId",
                table: "Appeals",
                column: "RecordedByPersonalId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AppUsers_RecordedByPersonalId",
                table: "Payments",
                column: "RecordedByPersonalId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Drivers_VehicleLicenseHolderId",
                table: "Vehicles",
                column: "VehicleLicenseHolderId",
                principalTable: "Drivers",
                principalColumn: "DriverId");
        }
    }
}
