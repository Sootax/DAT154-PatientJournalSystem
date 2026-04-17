using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DebriefReports",
                columns: table => new
                {
                    DebriefReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SimulationSessionId = table.Column<int>(type: "int", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DebriefReports", x => x.DebriefReportId);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeightKg = table.Column<double>(type: "float", nullable: false),
                    MedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentDiagnosis = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "VitalSigns",
                columns: table => new
                {
                    VitalSignsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystolicBp = table.Column<int>(type: "int", nullable: false),
                    DiastolicBp = table.Column<int>(type: "int", nullable: false),
                    HeartRate = table.Column<int>(type: "int", nullable: false),
                    RespiratoryRate = table.Column<int>(type: "int", nullable: false),
                    OxygenSaturation = table.Column<int>(type: "int", nullable: false),
                    TemperatureCelsius = table.Column<double>(type: "float", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VitalSigns", x => x.VitalSignsId);
                });

            migrationBuilder.CreateTable(
                name: "CaseScenarios",
                columns: table => new
                {
                    CaseScenarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    InitialVitalsVitalSignsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseScenarios", x => x.CaseScenarioId);
                    table.ForeignKey(
                        name: "FK_CaseScenarios_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId");
                    table.ForeignKey(
                        name: "FK_CaseScenarios_VitalSigns_InitialVitalsVitalSignsId",
                        column: x => x.InitialVitalsVitalSignsId,
                        principalTable: "VitalSigns",
                        principalColumn: "VitalSignsId");
                });

            migrationBuilder.CreateTable(
                name: "Allergies",
                columns: table => new
                {
                    AllergyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Substance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reaction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseScenarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergies", x => x.AllergyId);
                    table.ForeignKey(
                        name: "FK_Allergies_CaseScenarios_CaseScenarioId",
                        column: x => x.CaseScenarioId,
                        principalTable: "CaseScenarios",
                        principalColumn: "CaseScenarioId");
                });

            migrationBuilder.CreateTable(
                name: "CaseGoals",
                columns: table => new
                {
                    CaseGoalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeLimitSeconds = table.Column<int>(type: "int", nullable: false),
                    CaseScenarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseGoals", x => x.CaseGoalId);
                    table.ForeignKey(
                        name: "FK_CaseGoals_CaseScenarios_CaseScenarioId",
                        column: x => x.CaseScenarioId,
                        principalTable: "CaseScenarios",
                        principalColumn: "CaseScenarioId");
                });

            migrationBuilder.CreateTable(
                name: "MedicationOrders",
                columns: table => new
                {
                    MedicationOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrugName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Route = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseScenarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationOrders", x => x.MedicationOrderId);
                    table.ForeignKey(
                        name: "FK_MedicationOrders_CaseScenarios_CaseScenarioId",
                        column: x => x.CaseScenarioId,
                        principalTable: "CaseScenarios",
                        principalColumn: "CaseScenarioId");
                });

            migrationBuilder.CreateTable(
                name: "SimulationSessions",
                columns: table => new
                {
                    SimulationSessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseScenarioId = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationSessions", x => x.SimulationSessionId);
                    table.ForeignKey(
                        name: "FK_SimulationSessions_CaseScenarios_CaseScenarioId",
                        column: x => x.CaseScenarioId,
                        principalTable: "CaseScenarios",
                        principalColumn: "CaseScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Interventions",
                columns: table => new
                {
                    InterventionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SimulationSessionId = table.Column<int>(type: "int", nullable: false),
                    PerformedByUserId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DrugName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Route = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interventions", x => x.InterventionId);
                    table.ForeignKey(
                        name: "FK_Interventions_SimulationSessions_SimulationSessionId",
                        column: x => x.SimulationSessionId,
                        principalTable: "SimulationSessions",
                        principalColumn: "SimulationSessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherObservations",
                columns: table => new
                {
                    TeacherObservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SimulationSessionId = table.Column<int>(type: "int", nullable: false),
                    TeacherUserId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherObservations", x => x.TeacherObservationId);
                    table.ForeignKey(
                        name: "FK_TeacherObservations_SimulationSessions_SimulationSessionId",
                        column: x => x.SimulationSessionId,
                        principalTable: "SimulationSessions",
                        principalColumn: "SimulationSessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimelineEvents",
                columns: table => new
                {
                    TimelineEventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SimulationSessionId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimelineEvents", x => x.TimelineEventId);
                    table.ForeignKey(
                        name: "FK_TimelineEvents_SimulationSessions_SimulationSessionId",
                        column: x => x.SimulationSessionId,
                        principalTable: "SimulationSessions",
                        principalColumn: "SimulationSessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VitalSignRecords",
                columns: table => new
                {
                    VitalSignsRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SimulationSessionId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SystolicBp = table.Column<int>(type: "int", nullable: false),
                    DiastolicBp = table.Column<int>(type: "int", nullable: false),
                    HeartRate = table.Column<int>(type: "int", nullable: false),
                    RespiratoryRate = table.Column<int>(type: "int", nullable: false),
                    OxygenSaturation = table.Column<int>(type: "int", nullable: false),
                    TemperatureCelsius = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VitalSignRecords", x => x.VitalSignsRecordId);
                    table.ForeignKey(
                        name: "FK_VitalSignRecords_SimulationSessions_SimulationSessionId",
                        column: x => x.SimulationSessionId,
                        principalTable: "SimulationSessions",
                        principalColumn: "SimulationSessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Allergies_CaseScenarioId",
                table: "Allergies",
                column: "CaseScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseGoals_CaseScenarioId",
                table: "CaseGoals",
                column: "CaseScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseScenarios_InitialVitalsVitalSignsId",
                table: "CaseScenarios",
                column: "InitialVitalsVitalSignsId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseScenarios_PatientId",
                table: "CaseScenarios",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_SimulationSessionId",
                table: "Interventions",
                column: "SimulationSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationOrders_CaseScenarioId",
                table: "MedicationOrders",
                column: "CaseScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_SimulationSessions_CaseScenarioId",
                table: "SimulationSessions",
                column: "CaseScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherObservations_SimulationSessionId",
                table: "TeacherObservations",
                column: "SimulationSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelineEvents_SimulationSessionId",
                table: "TimelineEvents",
                column: "SimulationSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_VitalSignRecords_SimulationSessionId",
                table: "VitalSignRecords",
                column: "SimulationSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Allergies");

            migrationBuilder.DropTable(
                name: "CaseGoals");

            migrationBuilder.DropTable(
                name: "DebriefReports");

            migrationBuilder.DropTable(
                name: "Interventions");

            migrationBuilder.DropTable(
                name: "MedicationOrders");

            migrationBuilder.DropTable(
                name: "TeacherObservations");

            migrationBuilder.DropTable(
                name: "TimelineEvents");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "VitalSignRecords");

            migrationBuilder.DropTable(
                name: "SimulationSessions");

            migrationBuilder.DropTable(
                name: "CaseScenarios");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "VitalSigns");
        }
    }
}