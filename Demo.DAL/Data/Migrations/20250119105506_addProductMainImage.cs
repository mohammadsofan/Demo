﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class addProductMainImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainImage",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainImage",
                table: "Products");
        }
    }
}
