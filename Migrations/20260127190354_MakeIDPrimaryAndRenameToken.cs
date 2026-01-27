using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saturn.Migrations
{
    /// <inheritdoc />
    public partial class MakeIDPrimaryAndRenameToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "PfSessionToken",
                table: "Users",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Users",
                newName: "DeviceID");

            migrationBuilder.AlterColumn<ulong>(
                name: "PublicID",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "PublicID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "Users",
                newName: "PfSessionToken");

            migrationBuilder.RenameColumn(
                name: "DeviceID",
                table: "Users",
                newName: "UserID");

            migrationBuilder.AlterColumn<string>(
                name: "PublicID",
                table: "Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserID");
        }
    }
}
