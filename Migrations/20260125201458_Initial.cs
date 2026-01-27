using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saturn.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "TEXT", nullable: false),
                    NID = table.Column<string>(type: "TEXT", nullable: false),
                    GNID = table.Column<string>(type: "TEXT", nullable: false),
                    PfSessionToken = table.Column<string>(type: "TEXT", nullable: false),
                    AgreedToPush = table.Column<bool>(type: "INTEGER", nullable: false),
                    AgreedToTerms = table.Column<bool>(type: "INTEGER", nullable: false),
                    PushAgreeTime = table.Column<int>(type: "INTEGER", nullable: false),
                    TermsAgreeTime = table.Column<int>(type: "INTEGER", nullable: false),
                    IsNew = table.Column<bool>(type: "INTEGER", nullable: false),
                    Coins = table.Column<int>(type: "INTEGER", nullable: false),
                    PropertyList = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
