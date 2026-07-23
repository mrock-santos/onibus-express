using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnibusExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "passageiros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passageiros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rotas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Origem = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Destino = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DuracaoEstimada = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rotas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "viagens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RotaId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataHoraPartida = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PrecoBase = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    TotalAssentos = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_viagens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_viagens_rotas_RotaId",
                        column: x => x.RotaId,
                        principalTable: "rotas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reservas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ViagemId = table.Column<Guid>(type: "uuid", nullable: false),
                    PassageiroId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumeroAssento = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CodigoReserva = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CriadaEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reservas_passageiros_PassageiroId",
                        column: x => x.PassageiroId,
                        principalTable: "passageiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservas_viagens_ViagemId",
                        column: x => x.ViagemId,
                        principalTable: "viagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_passageiros_Cpf",
                table: "passageiros",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reservas_CodigoReserva",
                table: "reservas",
                column: "CodigoReserva",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reservas_PassageiroId",
                table: "reservas",
                column: "PassageiroId");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_ViagemId_NumeroAssento",
                table: "reservas",
                columns: new[] { "ViagemId", "NumeroAssento" },
                unique: true,
                filter: "\"Status\" = 'Confirmada'");

            migrationBuilder.CreateIndex(
                name: "IX_viagens_RotaId",
                table: "viagens",
                column: "RotaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservas");

            migrationBuilder.DropTable(
                name: "passageiros");

            migrationBuilder.DropTable(
                name: "viagens");

            migrationBuilder.DropTable(
                name: "rotas");
        }
    }
}
