using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalOfPremises.Migrations
{
    /// <inheritdoc />
    public partial class FullNameImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date_Of_Construction",
                table: "Placement");

            migrationBuilder.RenameColumn(
                name: "Preriew_Image_Id",
                table: "Placement",
                newName: "YearConstruction");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Image",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Image");

            migrationBuilder.RenameColumn(
                name: "YearConstruction",
                table: "Placement",
                newName: "Preriew_Image_Id");

            migrationBuilder.AddColumn<int>(
                name: "Date_Of_Construction",
                table: "Placement",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
