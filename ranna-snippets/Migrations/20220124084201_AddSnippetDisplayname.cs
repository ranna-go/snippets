using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ranna_snippets.Migrations
{
    public partial class AddSnippetDisplayname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Displayname",
                table: "Snippets",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Displayname",
                table: "Snippets");
        }
    }
}
