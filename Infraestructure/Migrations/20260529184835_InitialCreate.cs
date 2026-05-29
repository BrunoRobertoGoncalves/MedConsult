using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Crm = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Specialities = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClinicalCases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedSpecialistId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ClinicalContext = table.Column<string>(type: "text", nullable: false),
                    DiagnosticHypothesis = table.Column<string>(type: "text", nullable: false),
                    Question = table.Column<string>(type: "text", nullable: false),
                    SpecialityRequired = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicalCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicalCases_Users_AssignedSpecialistId",
                        column: x => x.AssignedSpecialistId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClinicalCases_Users_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UploadedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    StoragePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExtractedData = table.Column<string>(type: "text", nullable: true),
                    AlteredValues = table.Column<string>(type: "text", nullable: true),
                    ProcessingStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exams_Users_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaseReplies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClinicalCaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequesterId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseReplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseReplies_ClinicalCases_ClinicalCaseId",
                        column: x => x.ClinicalCaseId,
                        principalTable: "ClinicalCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseReplies_Users_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaseStatusHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClinicalCaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStatus = table.Column<string>(type: "text", nullable: true),
                    ToStatus = table.Column<string>(type: "text", nullable: false),
                    ChangedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseStatusHistories_ClinicalCases_ClinicalCaseId",
                        column: x => x.ClinicalCaseId,
                        principalTable: "ClinicalCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseStatusHistories_Users_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Opinions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClinicalCaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpecialistId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Agreement = table.Column<string>(type: "text", nullable: false),
                    ConductRecommendation = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opinions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Opinions_ClinicalCases_ClinicalCaseId",
                        column: x => x.ClinicalCaseId,
                        principalTable: "ClinicalCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Opinions_Users_SpecialistId",
                        column: x => x.SpecialistId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaseExams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClinicalCaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamId = table.Column<Guid>(type: "uuid", nullable: false),
                    LinkedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseExams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseExams_ClinicalCases_ClinicalCaseId",
                        column: x => x.ClinicalCaseId,
                        principalTable: "ClinicalCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseExams_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseExams_ClinicalCaseId_ExamId",
                table: "CaseExams",
                columns: new[] { "ClinicalCaseId", "ExamId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaseExams_ExamId",
                table: "CaseExams",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseReplies_ClinicalCaseId",
                table: "CaseReplies",
                column: "ClinicalCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseReplies_RequesterId",
                table: "CaseReplies",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseStatusHistories_ChangedByUserId",
                table: "CaseStatusHistories",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseStatusHistories_ClinicalCaseId",
                table: "CaseStatusHistories",
                column: "ClinicalCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalCases_AssignedSpecialistId",
                table: "ClinicalCases",
                column: "AssignedSpecialistId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalCases_RequestedByUserId",
                table: "ClinicalCases",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_UploadedByUserId",
                table: "Exams",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Opinions_ClinicalCaseId",
                table: "Opinions",
                column: "ClinicalCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Opinions_SpecialistId",
                table: "Opinions",
                column: "SpecialistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseExams");

            migrationBuilder.DropTable(
                name: "CaseReplies");

            migrationBuilder.DropTable(
                name: "CaseStatusHistories");

            migrationBuilder.DropTable(
                name: "Opinions");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "ClinicalCases");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
