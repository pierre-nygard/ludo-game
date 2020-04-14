using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameEngine.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Board",
                columns: table => new
                {
                    BoardID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Board", x => x.BoardID);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    GameID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastAction = table.Column<DateTime>(nullable: false),
                    Completed = table.Column<bool>(nullable: false),
                    BoardID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.GameID);
                    table.ForeignKey(
                        name: "FK_Game_Board_BoardID",
                        column: x => x.BoardID,
                        principalTable: "Board",
                        principalColumn: "BoardID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    PlayerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Score = table.Column<int>(nullable: false),
                    GameID = table.Column<int>(nullable: false),
                    PlayerType = table.Column<int>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.PlayerID);
                    table.ForeignKey(
                        name: "FK_Player_Game_GameID",
                        column: x => x.GameID,
                        principalTable: "Game",
                        principalColumn: "GameID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Piece",
                columns: table => new
                {
                    PieceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<int>(nullable: false),
                    Y = table.Column<int>(nullable: false),
                    PlayerID = table.Column<int>(nullable: false),
                    Steps = table.Column<int>(nullable: false),
                    Completed = table.Column<bool>(nullable: false),
                    BoardID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Piece", x => x.PieceID);
                    table.ForeignKey(
                        name: "FK_Piece_Board_BoardID",
                        column: x => x.BoardID,
                        principalTable: "Board",
                        principalColumn: "BoardID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Piece_Player_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Player",
                        principalColumn: "PlayerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Game_BoardID",
                table: "Game",
                column: "BoardID");

            migrationBuilder.CreateIndex(
                name: "IX_Piece_BoardID",
                table: "Piece",
                column: "BoardID");

            migrationBuilder.CreateIndex(
                name: "IX_Piece_PlayerID",
                table: "Piece",
                column: "PlayerID");

            migrationBuilder.CreateIndex(
                name: "IX_Player_GameID",
                table: "Player",
                column: "GameID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Piece");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "Board");
        }
    }
}
