﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Do_An_Chuyen_Nganh.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "AspNetRoles",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            //    });

            //    migrationBuilder.CreateTable(
            //        name: "AspNetUsers",
            //        columns: table => new
            //        {
            //            Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //            FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            Introduction = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            BOD = table.Column<DateTime>(type: "datetime2", nullable: false),
            //            UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //            NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //            Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //            NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //            EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
            //            PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
            //            TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
            //            LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //            LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
            //            AccessFailedCount = table.Column<int>(type: "int", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "Category",
            //        columns: table => new
            //        {
            //            Id = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            CategoryName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
            //            Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            ParentId = table.Column<int>(type: "int", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_Category", x => x.Id);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "Colors",
            //        columns: table => new
            //        {
            //            Id = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            ColorName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_Colors", x => x.Id);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "Conditions",
            //        columns: table => new
            //        {
            //            Id = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            ConditionName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_Conditions", x => x.Id);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "Messages",
            //        columns: table => new
            //        {
            //            Id = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //            When = table.Column<DateTime>(type: "datetime2", nullable: false),
            //            SenderID = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //            ReceiverID = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_Messages", x => x.Id);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "Proveniences",
            //        columns: table => new
            //        {
            //            Id = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            ProvenienceName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_Proveniences", x => x.Id);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "Warranties",
            //        columns: table => new
            //        {
            //            Id = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            WarrantyName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_Warranties", x => x.Id);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "AspNetRoleClaims",
            //        columns: table => new
            //        {
            //            Id = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //            ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
            //            table.ForeignKey(
            //                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
            //                column: x => x.RoleId,
            //                principalTable: "AspNetRoles",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Cascade);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "Role",
            //        columns: table => new
            //        {
            //            Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_Role", x => x.Id);
            //            table.ForeignKey(
            //                name: "FK_Role_AspNetRoles_Id",
            //                column: x => x.Id,
            //                principalTable: "AspNetRoles",
            //                principalColumn: "Id");
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "AspNetUserClaims",
            //        columns: table => new
            //        {
            //            Id = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //            ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
            //            table.ForeignKey(
            //                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
            //                column: x => x.UserId,
            //                principalTable: "AspNetUsers",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Cascade);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "AspNetUserLogins",
            //        columns: table => new
            //        {
            //            LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
            //            ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
            //            ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
            //            table.ForeignKey(
            //                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
            //                column: x => x.UserId,
            //                principalTable: "AspNetUsers",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Cascade);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "AspNetUserRoles",
            //        columns: table => new
            //        {
            //            UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //            RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
            //            table.ForeignKey(
            //                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
            //                column: x => x.RoleId,
            //                principalTable: "AspNetRoles",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Cascade);
            //            table.ForeignKey(
            //                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
            //                column: x => x.UserId,
            //                principalTable: "AspNetUsers",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Cascade);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "AspNetUserTokens",
            //        columns: table => new
            //        {
            //            UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //            LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
            //            Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
            //            Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
            //            table.ForeignKey(
            //                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
            //                column: x => x.UserId,
            //                principalTable: "AspNetUsers",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Cascade);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "Orders",
            //        columns: table => new
            //        {
            //            Id = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            CustomerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
            //            PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //            Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //            Payment = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //            Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //            Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //            Paid = table.Column<int>(type: "int", nullable: false),
            //            CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            //            UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            //            UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_Orders", x => x.Id);
            //            table.ForeignKey(
            //                name: "FK_Orders_AspNetUsers_UserId",
            //                column: x => x.UserId,
            //                principalTable: "AspNetUsers",
            //                principalColumn: "Id");
            //        });

            //migrationBuilder.CreateTable(
            //    name: "Products",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
            //        Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
            //        Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        CategoryId = table.Column<int>(type: "int", nullable: false),
            //        ColorId = table.Column<int>(type: "int", nullable: false),
            //        ConditionId = table.Column<int>(type: "int", nullable: false),
            //        ProvenienceId = table.Column<int>(type: "int", nullable: false),
            //        Quantity = table.Column<int>(type: "int", nullable: false),
            //        Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        WarrantyId = table.Column<int>(type: "int", nullable: false),
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Status = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Products", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Products_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id");
            //        table.ForeignKey(
            //            name: "FK_Products_Category_CategoryId",
            //            column: x => x.CategoryId,
            //            principalTable: "Category",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Products_Colors_ColorId",
            //            column: x => x.ColorId,
            //            principalTable: "Colors",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Products_Conditions_ConditionId",
            //            column: x => x.ConditionId,
            //            principalTable: "Conditions",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Products_Proveniences_ProvenienceId",
            //            column: x => x.ProvenienceId,
            //            principalTable: "Proveniences",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Products_Warranties_WarrantyId",
            //            column: x => x.WarrantyId,
            //            principalTable: "Warranties",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //    migrationBuilder.CreateTable(
            //        name: "OrderDetails",
            //        columns: table => new
            //        {
            //            Id = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            OrderId = table.Column<int>(type: "int", nullable: false),
            //            ProductId = table.Column<int>(type: "int", nullable: false),
            //            ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //            Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //            Quantity = table.Column<int>(type: "int", nullable: false),
            //            Status = table.Column<int>(type: "int", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_OrderDetails", x => x.Id);
            //            table.ForeignKey(
            //                name: "FK_OrderDetails_Orders_OrderId",
            //                column: x => x.OrderId,
            //                principalTable: "Orders",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Cascade);
            //            table.ForeignKey(
            //                name: "FK_OrderDetails_Products_ProductId",
            //                column: x => x.ProductId,
            //                principalTable: "Products",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Cascade);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "ProductImages",
            //        columns: table => new
            //        {
            //            Id = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //            ProductId = table.Column<int>(type: "int", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_ProductImages", x => x.Id);
            //            table.ForeignKey(
            //                name: "FK_ProductImages_Products_ProductId",
            //                column: x => x.ProductId,
            //                principalTable: "Products",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Cascade);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "WishList",
            //        columns: table => new
            //        {
            //            Id = table.Column<int>(type: "int", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            ProductId = table.Column<int>(type: "int", nullable: false),
            //            UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_WishList", x => x.Id);
            //            table.ForeignKey(
            //                name: "FK_WishList_Products_ProductId",
            //                column: x => x.ProductId,
            //                principalTable: "Products",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Cascade);
            //        });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_AspNetRoleClaims_RoleId",
            //        table: "AspNetRoleClaims",
            //        column: "RoleId");

            //    migrationBuilder.CreateIndex(
            //        name: "RoleNameIndex",
            //        table: "AspNetRoles",
            //        column: "NormalizedName",
            //        unique: true,
            //        filter: "[NormalizedName] IS NOT NULL");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_AspNetUserClaims_UserId",
            //        table: "AspNetUserClaims",
            //        column: "UserId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_AspNetUserLogins_UserId",
            //        table: "AspNetUserLogins",
            //        column: "UserId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_AspNetUserRoles_RoleId",
            //        table: "AspNetUserRoles",
            //        column: "RoleId");

            //    migrationBuilder.CreateIndex(
            //        name: "EmailIndex",
            //        table: "AspNetUsers",
            //        column: "NormalizedEmail");

            //    migrationBuilder.CreateIndex(
            //        name: "UserNameIndex",
            //        table: "AspNetUsers",
            //        column: "NormalizedUserName",
            //        unique: true,
            //        filter: "[NormalizedUserName] IS NOT NULL");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_OrderDetails_OrderId",
            //        table: "OrderDetails",
            //        column: "OrderId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_OrderDetails_ProductId",
            //        table: "OrderDetails",
            //        column: "ProductId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Orders_UserId",
            //        table: "Orders",
            //        column: "UserId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_ProductImages_ProductId",
            //        table: "ProductImages",
            //        column: "ProductId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Products_CategoryId",
            //        table: "Products",
            //        column: "CategoryId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Products_ColorId",
            //        table: "Products",
            //        column: "ColorId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Products_ConditionId",
            //        table: "Products",
            //        column: "ConditionId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Products_ProvenienceId",
            //        table: "Products",
            //        column: "ProvenienceId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Products_UserId",
            //        table: "Products",
            //        column: "UserId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_Products_WarrantyId",
            //        table: "Products",
            //        column: "WarrantyId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_WishList_ProductId",
            //        table: "WishList",
            //        column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "WishList");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Conditions");

            migrationBuilder.DropTable(
                name: "Proveniences");

            migrationBuilder.DropTable(
                name: "Warranties");
        }
    }
}
