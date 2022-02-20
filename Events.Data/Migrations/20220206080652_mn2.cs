using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Events.Data.Migrations
{
    public partial class mn2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssessmentMethodologies",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentMethodologies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    Filename = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Content = table.Column<byte[]>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Is64base = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogNewsCategory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogNewsCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(nullable: true),
                    label = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(nullable: true),
                    Contenant = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExploitIpTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    typeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExploitIpTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExploitSeveritys",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExploitSeveritys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncidentCategories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ip",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ip = table.Column<string>(nullable: true),
                    port = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ip", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OwnerDetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subsId = table.Column<string>(nullable: true),
                    ownerSub = table.Column<string>(nullable: true),
                    ownerType = table.Column<string>(nullable: true),
                    cid = table.Column<string>(nullable: true),
                    phoneNum = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportCategories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Saverities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: true),
                    Lable = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Saverities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sectors",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticIPs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Mobile = table.Column<long>(nullable: false),
                    Ip = table.Column<string>(nullable: true),
                    EditDate = table.Column<DateTime>(nullable: false),
                    Provider = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticIPs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Urganceys",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: true),
                    Label = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urganceys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VerificationPossibilities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationPossibilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    DepartmentId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Orgname = table.Column<string>(nullable: true),
                    SectorId = table.Column<int>(nullable: false),
                    SectorId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_Sectors_SectorId1",
                        column: x => x.SectorId1,
                        principalTable: "Sectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EntityStatus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityStatus_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    sectionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_Sections_sectionId",
                        column: x => x.sectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    SectionId = table.Column<long>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false),
                    IsHead = table.Column<bool>(nullable: false),
                    IsSubHead = table.Column<bool>(nullable: false),
                    IsAssignedHead = table.Column<bool>(nullable: false),
                    IsAssignedSubeHead = table.Column<bool>(nullable: false),
                    OrganizationId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationContacts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    JobTitle = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationContacts_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationDomains",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrgId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationDomains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationDomains_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationIp",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrgId = table.Column<long>(nullable: false),
                    IpName = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationIp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationIp_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Apts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(nullable: true),
                    LastUpdateById = table.Column<long>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    StatusId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Apts_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Apts_AspNetUsers_LastUpdateById",
                        column: x => x.LastUpdateById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Apts_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogNews",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(nullable: true),
                    LastUpdateById = table.Column<long>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    NewsCategoryId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogNews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogNews_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlogNews_AspNetUsers_LastUpdateById",
                        column: x => x.LastUpdateById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlogNews_BlogNewsCategory_NewsCategoryId",
                        column: x => x.NewsCategoryId,
                        principalTable: "BlogNewsCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CloseReports",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(nullable: true),
                    LastUpdateById = table.Column<long>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    reportId = table.Column<long>(nullable: false),
                    report = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloseReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CloseReports_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CloseReports_AspNetUsers_LastUpdateById",
                        column: x => x.LastUpdateById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(nullable: true),
                    LastUpdateById = table.Column<long>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CommentString = table.Column<string>(nullable: true),
                    CommentId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_LastUpdateById",
                        column: x => x.LastUpdateById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Exploits",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(nullable: true),
                    LastUpdateById = table.Column<long>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    CVE = table.Column<string>(nullable: true),
                    Descriptin = table.Column<string>(nullable: true),
                    Solution = table.Column<string>(nullable: true),
                    SeverityId = table.Column<long>(nullable: true),
                    Resources = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exploits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exploits_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exploits_AspNetUsers_LastUpdateById",
                        column: x => x.LastUpdateById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exploits_ExploitSeveritys_SeverityId",
                        column: x => x.SeverityId,
                        principalTable: "ExploitSeveritys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GeneralReports",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastUpdateById = table.Column<long>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    StatusId = table.Column<long>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    ReportCategoryId = table.Column<long>(nullable: false),
                    UrganceyId = table.Column<long>(nullable: true),
                    SaverityId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralReports_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralReports_AspNetUsers_LastUpdateById",
                        column: x => x.LastUpdateById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralReports_ReportCategories_ReportCategoryId",
                        column: x => x.ReportCategoryId,
                        principalTable: "ReportCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralReports_Saverities_SaverityId",
                        column: x => x.SaverityId,
                        principalTable: "Saverities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralReports_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralReports_Urganceys_UrganceyId",
                        column: x => x.UrganceyId,
                        principalTable: "Urganceys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    depId = table.Column<int>(nullable: false),
                    publish = table.Column<bool>(nullable: false),
                    UsersId = table.Column<long>(nullable: true),
                    UrganceyId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                    table.ForeignKey(
                        name: "FK_News_Urganceys_UrganceyId",
                        column: x => x.UrganceyId,
                        principalTable: "Urganceys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_News_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    EntityId = table.Column<long>(nullable: false),
                    ParentEntityId = table.Column<long>(nullable: false),
                    EntityType = table.Column<int>(nullable: false),
                    StatusId = table.Column<long>(nullable: true),
                    ParentEntityType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserActivity",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(nullable: true),
                    UserNameId = table.Column<long>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserActivity_AspNetUsers_UserNameId",
                        column: x => x.UserNameId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DPEs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(nullable: true),
                    LastUpdateById = table.Column<long>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    InformingBy = table.Column<string>(nullable: true),
                    InformingNote = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<long>(nullable: true),
                    ContactedPersonId = table.Column<long>(nullable: true),
                    MethodOfInforming = table.Column<string>(nullable: true),
                    Recommendation = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    HeadDirections = table.Column<string>(nullable: true),
                    InformingDate = table.Column<DateTime>(nullable: false),
                    DateFrom = table.Column<DateTime>(nullable: false),
                    DateTo = table.Column<DateTime>(nullable: false),
                    StatusId = table.Column<long>(nullable: true),
                    Scope = table.Column<string>(nullable: true),
                    AssessmentMethodologyId = table.Column<long>(nullable: true),
                    AttachmentReportId = table.Column<long>(nullable: true),
                    IsOpen = table.Column<bool>(nullable: false),
                    IsDone = table.Column<bool>(nullable: false),
                    ExtraNote1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DPEs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DPEs_AssessmentMethodologies_AssessmentMethodologyId",
                        column: x => x.AssessmentMethodologyId,
                        principalTable: "AssessmentMethodologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DPEs_Attachments_AttachmentReportId",
                        column: x => x.AttachmentReportId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DPEs_OrganizationContacts_ContactedPersonId",
                        column: x => x.ContactedPersonId,
                        principalTable: "OrganizationContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DPEs_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DPEs_AspNetUsers_LastUpdateById",
                        column: x => x.LastUpdateById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DPEs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DPEs_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlternativeName",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    APTId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlternativeName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlternativeName_Apts_APTId",
                        column: x => x.APTId,
                        principalTable: "Apts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AptAttachment",
                columns: table => new
                {
                    AttachmentId = table.Column<long>(nullable: false),
                    APTId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AptAttachment", x => new { x.AttachmentId, x.APTId });
                    table.ForeignKey(
                        name: "FK_AptAttachment_Apts_APTId",
                        column: x => x.APTId,
                        principalTable: "Apts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AptAttachment_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttackStratigie",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    APTId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttackStratigie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttackStratigie_Apts_APTId",
                        column: x => x.APTId,
                        principalTable: "Apts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyName",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    APTId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyName_Apts_APTId",
                        column: x => x.APTId,
                        principalTable: "Apts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Content",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentString = table.Column<string>(nullable: true),
                    CreatedById = table.Column<long>(nullable: false),
                    createdDate = table.Column<DateTime>(nullable: false),
                    APTId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Content_Apts_APTId",
                        column: x => x.APTId,
                        principalTable: "Apts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Content_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OriginCountry",
                columns: table => new
                {
                    CountryId = table.Column<long>(nullable: false),
                    APTId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginCountry", x => new { x.CountryId, x.APTId });
                    table.ForeignKey(
                        name: "FK_OriginCountry_Apts_APTId",
                        column: x => x.APTId,
                        principalTable: "Apts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OriginCountry_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TargetedCountry",
                columns: table => new
                {
                    CountryId = table.Column<long>(nullable: false),
                    APTId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetedCountry", x => new { x.CountryId, x.APTId });
                    table.ForeignKey(
                        name: "FK_TargetedCountry_Apts_APTId",
                        column: x => x.APTId,
                        principalTable: "Apts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TargetedCountry_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TargetedSector",
                columns: table => new
                {
                    SectorId = table.Column<long>(nullable: false),
                    AptId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetedSector", x => new { x.SectorId, x.AptId });
                    table.ForeignKey(
                        name: "FK_TargetedSector_Apts_AptId",
                        column: x => x.AptId,
                        principalTable: "Apts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TargetedSector_Sectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThreatSignature",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Serial = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DomainName = table.Column<string>(nullable: true),
                    APTId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreatSignature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThreatSignature_Apts_APTId",
                        column: x => x.APTId,
                        principalTable: "Apts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ToolName",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    APTId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToolName_Apts_APTId",
                        column: x => x.APTId,
                        principalTable: "Apts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Incidents",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(nullable: true),
                    LastUpdateById = table.Column<long>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CategoryId = table.Column<long>(nullable: true),
                    Signature = table.Column<string>(nullable: true),
                    AptId = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CloseReportId = table.Column<long>(nullable: true),
                    ExtraNote1 = table.Column<string>(nullable: true),
                    ExtraNote2 = table.Column<string>(nullable: true),
                    ExtraNote3 = table.Column<string>(nullable: true),
                    StatusId = table.Column<long>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    SaverityId = table.Column<long>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Time = table.Column<string>(nullable: true),
                    UrganceyId = table.Column<long>(nullable: true),
                    IsIpsIdentificationRequested = table.Column<bool>(nullable: false),
                    IncidentCategoryId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incidents_Apts_AptId",
                        column: x => x.AptId,
                        principalTable: "Apts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incidents_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidents_CloseReports_CloseReportId",
                        column: x => x.CloseReportId,
                        principalTable: "CloseReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidents_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidents_IncidentCategories_IncidentCategoryId",
                        column: x => x.IncidentCategoryId,
                        principalTable: "IncidentCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidents_AspNetUsers_LastUpdateById",
                        column: x => x.LastUpdateById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidents_Saverities_SaverityId",
                        column: x => x.SaverityId,
                        principalTable: "Saverities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidents_EntityStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "EntityStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidents_Urganceys_UrganceyId",
                        column: x => x.UrganceyId,
                        principalTable: "Urganceys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportAttachment",
                columns: table => new
                {
                    attachmentId = table.Column<long>(nullable: false),
                    closeReportId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportAttachment", x => new { x.attachmentId, x.closeReportId });
                    table.ForeignKey(
                        name: "FK_ReportAttachment_Attachments_attachmentId",
                        column: x => x.attachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportAttachment_CloseReports_closeReportId",
                        column: x => x.closeReportId,
                        principalTable: "CloseReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentAttachment",
                columns: table => new
                {
                    commentId = table.Column<long>(nullable: false),
                    attachmentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentAttachment", x => new { x.commentId, x.attachmentId });
                    table.ForeignKey(
                        name: "FK_CommentAttachment_Attachments_attachmentId",
                        column: x => x.attachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentAttachment_Comments_commentId",
                        column: x => x.commentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralReportAttachment",
                columns: table => new
                {
                    GeneralReportId = table.Column<long>(nullable: false),
                    AttachmentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralReportAttachment", x => new { x.GeneralReportId, x.AttachmentId });
                    table.ForeignKey(
                        name: "FK_GeneralReportAttachment_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralReportAttachment_GeneralReports_GeneralReportId",
                        column: x => x.GeneralReportId,
                        principalTable: "GeneralReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GReportEntityAssignment",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(nullable: true),
                    LastUpdateById = table.Column<long>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<long>(nullable: true),
                    StatusId = table.Column<long>(nullable: true),
                    Request = table.Column<string>(nullable: true),
                    IsHandeled = table.Column<bool>(nullable: false),
                    GeneralReportId = table.Column<long>(nullable: true),
                    Response = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GReportEntityAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GReportEntityAssignment_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GReportEntityAssignment_GeneralReports_GeneralReportId",
                        column: x => x.GeneralReportId,
                        principalTable: "GeneralReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GReportEntityAssignment_AspNetUsers_LastUpdateById",
                        column: x => x.LastUpdateById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GReportEntityAssignment_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GReportEntityAssignment_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationOwner",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employeeId = table.Column<long>(nullable: false),
                    isNew = table.Column<bool>(nullable: false),
                    NotificationId = table.Column<long>(nullable: true),
                    IsUpdated = table.Column<bool>(nullable: false),
                    lastupdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationOwner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationOwner_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationOwner_AspNetUsers_employeeId",
                        column: x => x.employeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DPEWorkTeam",
                columns: table => new
                {
                    DpeId = table.Column<long>(nullable: false),
                    EUserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DPEWorkTeam", x => new { x.EUserId, x.DpeId });
                    table.ForeignKey(
                        name: "FK_DPEWorkTeam_DPEs_DpeId",
                        column: x => x.DpeId,
                        principalTable: "DPEs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DPEWorkTeam_AspNetUsers_EUserId",
                        column: x => x.EUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vulnerabilities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(nullable: true),
                    LastUpdateById = table.Column<long>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedById = table.Column<long>(nullable: true),
                    ExploitId = table.Column<long>(nullable: true),
                    DaysToFix = table.Column<int>(nullable: false),
                    AssessmentTypeId = table.Column<long>(nullable: true),
                    DpeId = table.Column<long>(nullable: true),
                    ExtraNote1 = table.Column<string>(nullable: true),
                    ExtraNote2 = table.Column<string>(nullable: true),
                    ExtraNote3 = table.Column<string>(nullable: true),
                    StatusId = table.Column<long>(nullable: true),
                    evidence = table.Column<string>(nullable: true),
                    hasTested = table.Column<bool>(nullable: false),
                    report = table.Column<string>(nullable: true),
                    TcReport = table.Column<string>(nullable: true),
                    OrgReport = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vulnerabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vulnerabilities_AssessmentTypes_AssessmentTypeId",
                        column: x => x.AssessmentTypeId,
                        principalTable: "AssessmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vulnerabilities_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vulnerabilities_DPEs_DpeId",
                        column: x => x.DpeId,
                        principalTable: "DPEs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vulnerabilities_Exploits_ExploitId",
                        column: x => x.ExploitId,
                        principalTable: "Exploits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vulnerabilities_AspNetUsers_LastUpdateById",
                        column: x => x.LastUpdateById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vulnerabilities_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vulnerabilities_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EntityAssignments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(nullable: true),
                    LastUpdateById = table.Column<long>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<long>(nullable: true),
                    StatusId = table.Column<long>(nullable: true),
                    Request = table.Column<string>(nullable: true),
                    IsHandeled = table.Column<bool>(nullable: false),
                    IncidentId = table.Column<long>(nullable: true),
                    Response = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityAssignments_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntityAssignments_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntityAssignments_AspNetUsers_LastUpdateById",
                        column: x => x.LastUpdateById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntityAssignments_EntityStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "EntityStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntityAssignments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncidentAttachment",
                columns: table => new
                {
                    AttachmentId = table.Column<long>(nullable: false),
                    IncidentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentAttachment", x => new { x.AttachmentId, x.IncidentId });
                    table.ForeignKey(
                        name: "FK_IncidentAttachment_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncidentAttachment_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncidentsComments",
                columns: table => new
                {
                    IncidentId = table.Column<long>(nullable: false),
                    CommentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentsComments", x => new { x.IncidentId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_IncidentsComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncidentsComments_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IpAddress",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceId = table.Column<long>(nullable: true),
                    DestId = table.Column<long>(nullable: true),
                    BeginTimestamp = table.Column<DateTime>(nullable: false),
                    EndTimestamp = table.Column<DateTime>(nullable: false),
                    IntrusionSet = table.Column<string>(nullable: true),
                    SourceCountry = table.Column<string>(nullable: true),
                    DestinationCountry = table.Column<string>(nullable: true),
                    DataLength = table.Column<string>(nullable: true),
                    SignatureTitle = table.Column<string>(nullable: true),
                    SignatureContent = table.Column<string>(nullable: true),
                    SignatureClassification = table.Column<string>(nullable: true),
                    TotalHits = table.Column<string>(nullable: true),
                    AptGroup = table.Column<string>(nullable: true),
                    IncidentId = table.Column<long>(nullable: false),
                    OwnerDetailId = table.Column<long>(nullable: true),
                    IsHandeled = table.Column<bool>(nullable: false),
                    IsKnown = table.Column<bool>(nullable: false),
                    IsRequestVarify = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IpAddress_Ip_DestId",
                        column: x => x.DestId,
                        principalTable: "Ip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IpAddress_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IpAddress_OwnerDetails_OwnerDetailId",
                        column: x => x.OwnerDetailId,
                        principalTable: "OwnerDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IpAddress_Ip_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Ip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrgsIncidentRel",
                columns: table => new
                {
                    IncidentId = table.Column<long>(nullable: false),
                    OrganizationId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgsIncidentRel", x => new { x.OrganizationId, x.IncidentId });
                    table.ForeignKey(
                        name: "FK_OrgsIncidentRel_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgsIncidentRel_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(nullable: true),
                    LastUpdateById = table.Column<long>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    TaskTypeId = table.Column<long>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Importance = table.Column<int>(nullable: false),
                    Urgent = table.Column<int>(nullable: false),
                    StatusId = table.Column<long>(nullable: false),
                    Assigned_forId = table.Column<long>(nullable: true),
                    Weight = table.Column<int>(nullable: false),
                    Date = table.Column<string>(nullable: true),
                    DueDate = table.Column<string>(nullable: true),
                    ParentIncidentId = table.Column<long>(nullable: true),
                    ClosingReportId = table.Column<long>(nullable: true),
                    ParentTaskId = table.Column<long>(nullable: true),
                    IsIncident = table.Column<bool>(nullable: false),
                    Progress = table.Column<int>(nullable: false),
                    Rate = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Sections_Assigned_forId",
                        column: x => x.Assigned_forId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_CloseReports_ClosingReportId",
                        column: x => x.ClosingReportId,
                        principalTable: "CloseReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_LastUpdateById",
                        column: x => x.LastUpdateById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Incidents_ParentIncidentId",
                        column: x => x.ParentIncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Tasks_ParentTaskId",
                        column: x => x.ParentTaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_TaskType_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "TaskType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DPEAttachment",
                columns: table => new
                {
                    AttachmentId = table.Column<long>(nullable: false),
                    DPEId = table.Column<long>(nullable: false),
                    VulnerabilityId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DPEAttachment", x => new { x.AttachmentId, x.DPEId });
                    table.ForeignKey(
                        name: "FK_DPEAttachment_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DPEAttachment_Vulnerabilities_VulnerabilityId",
                        column: x => x.VulnerabilityId,
                        principalTable: "Vulnerabilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExploitAffectedSources",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    source = table.Column<string>(nullable: true),
                    ExploitIpTypeId = table.Column<long>(nullable: true),
                    VulnerabilityId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExploitAffectedSources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExploitAffectedSources_ExploitIpTypes_ExploitIpTypeId",
                        column: x => x.ExploitIpTypeId,
                        principalTable: "ExploitIpTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExploitAffectedSources_Vulnerabilities_VulnerabilityId",
                        column: x => x.VulnerabilityId,
                        principalTable: "Vulnerabilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VulnerabilityVerificationPossibilities",
                columns: table => new
                {
                    VulnerabilityId = table.Column<long>(nullable: false),
                    VerificationPossibilityId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VulnerabilityVerificationPossibilities", x => new { x.VerificationPossibilityId, x.VulnerabilityId });
                    table.ForeignKey(
                        name: "FK_VulnerabilityVerificationPossibilities_VerificationPossibilities_VerificationPossibilityId",
                        column: x => x.VerificationPossibilityId,
                        principalTable: "VerificationPossibilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VulnerabilityVerificationPossibilities_Vulnerabilities_VulnerabilityId",
                        column: x => x.VulnerabilityId,
                        principalTable: "Vulnerabilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    incidentId = table.Column<long>(nullable: true),
                    aptId = table.Column<long>(nullable: true),
                    taskId = table.Column<long>(nullable: true),
                    CommentId = table.Column<long>(nullable: true),
                    GeneralReportId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tags_GeneralReports_GeneralReportId",
                        column: x => x.GeneralReportId,
                        principalTable: "GeneralReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tags_Apts_aptId",
                        column: x => x.aptId,
                        principalTable: "Apts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tags_Incidents_incidentId",
                        column: x => x.incidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tags_Tasks_taskId",
                        column: x => x.taskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskAttachments",
                columns: table => new
                {
                    AttachmentId = table.Column<long>(nullable: false),
                    TaskId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAttachments", x => new { x.AttachmentId, x.TaskId });
                    table.ForeignKey(
                        name: "FK_TaskAttachments_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskAttachments_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskComments",
                columns: table => new
                {
                    TaskId = table.Column<long>(nullable: false),
                    CommentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskComments", x => new { x.TaskId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_TaskComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskComments_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskEmpsRel",
                columns: table => new
                {
                    EUserId = table.Column<long>(nullable: false),
                    TaskId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskEmpsRel", x => new { x.EUserId, x.TaskId });
                    table.ForeignKey(
                        name: "FK_TaskEmpsRel_AspNetUsers_EUserId",
                        column: x => x.EUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskEmpsRel_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskEntityAssignments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(nullable: true),
                    LastUpdateById = table.Column<long>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<long>(nullable: true),
                    StatusId = table.Column<long>(nullable: true),
                    Request = table.Column<string>(nullable: true),
                    IsHandled = table.Column<bool>(nullable: false),
                    TaskId = table.Column<long>(nullable: true),
                    Response = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskEntityAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskEntityAssignments_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskEntityAssignments_AspNetUsers_LastUpdateById",
                        column: x => x.LastUpdateById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskEntityAssignments_EntityStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "EntityStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskEntityAssignments_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskEntityAssignments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChangeLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    changedById = table.Column<long>(nullable: false),
                    changeDate = table.Column<DateTime>(nullable: false),
                    APTId = table.Column<long>(nullable: true),
                    BlogNewsId = table.Column<long>(nullable: true),
                    CloseReportId = table.Column<long>(nullable: true),
                    CommentId = table.Column<long>(nullable: true),
                    DPEId = table.Column<long>(nullable: true),
                    EntityAssignmentId = table.Column<long>(nullable: true),
                    ExploitId = table.Column<long>(nullable: true),
                    GReportEntityAssignmentId = table.Column<long>(nullable: true),
                    GeneralReportId = table.Column<long>(nullable: true),
                    IncidentId = table.Column<long>(nullable: true),
                    TaskEntityAssignmentId = table.Column<long>(nullable: true),
                    TaskId = table.Column<long>(nullable: true),
                    VulnerabilityId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeLogs_Apts_APTId",
                        column: x => x.APTId,
                        principalTable: "Apts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeLogs_BlogNews_BlogNewsId",
                        column: x => x.BlogNewsId,
                        principalTable: "BlogNews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeLogs_CloseReports_CloseReportId",
                        column: x => x.CloseReportId,
                        principalTable: "CloseReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeLogs_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeLogs_DPEs_DPEId",
                        column: x => x.DPEId,
                        principalTable: "DPEs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeLogs_EntityAssignments_EntityAssignmentId",
                        column: x => x.EntityAssignmentId,
                        principalTable: "EntityAssignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeLogs_Exploits_ExploitId",
                        column: x => x.ExploitId,
                        principalTable: "Exploits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeLogs_GReportEntityAssignment_GReportEntityAssignmentId",
                        column: x => x.GReportEntityAssignmentId,
                        principalTable: "GReportEntityAssignment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeLogs_GeneralReports_GeneralReportId",
                        column: x => x.GeneralReportId,
                        principalTable: "GeneralReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeLogs_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeLogs_TaskEntityAssignments_TaskEntityAssignmentId",
                        column: x => x.TaskEntityAssignmentId,
                        principalTable: "TaskEntityAssignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeLogs_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeLogs_Vulnerabilities_VulnerabilityId",
                        column: x => x.VulnerabilityId,
                        principalTable: "Vulnerabilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeLogs_AspNetUsers_changedById",
                        column: x => x.changedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChangeLogField",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(nullable: true),
                    OldValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    ChangeLogId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeLogField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeLogField_ChangeLogs_ChangeLogId",
                        column: x => x.ChangeLogId,
                        principalTable: "ChangeLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlternativeName_APTId",
                table: "AlternativeName",
                column: "APTId");

            migrationBuilder.CreateIndex(
                name: "IX_AptAttachment_APTId",
                table: "AptAttachment",
                column: "APTId");

            migrationBuilder.CreateIndex(
                name: "IX_Apts_CreatedById",
                table: "Apts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Apts_LastUpdateById",
                table: "Apts",
                column: "LastUpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Apts_StatusId",
                table: "Apts",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_sectionId",
                table: "AspNetRoles",
                column: "sectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OrganizationId",
                table: "AspNetUsers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SectionId",
                table: "AspNetUsers",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttackStratigie_APTId",
                table: "AttackStratigie",
                column: "APTId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogNews_CreatedById",
                table: "BlogNews",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BlogNews_LastUpdateById",
                table: "BlogNews",
                column: "LastUpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_BlogNews_NewsCategoryId",
                table: "BlogNews",
                column: "NewsCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogField_ChangeLogId",
                table: "ChangeLogField",
                column: "ChangeLogId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_APTId",
                table: "ChangeLogs",
                column: "APTId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_BlogNewsId",
                table: "ChangeLogs",
                column: "BlogNewsId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_CloseReportId",
                table: "ChangeLogs",
                column: "CloseReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_CommentId",
                table: "ChangeLogs",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_DPEId",
                table: "ChangeLogs",
                column: "DPEId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_EntityAssignmentId",
                table: "ChangeLogs",
                column: "EntityAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_ExploitId",
                table: "ChangeLogs",
                column: "ExploitId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_GReportEntityAssignmentId",
                table: "ChangeLogs",
                column: "GReportEntityAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_GeneralReportId",
                table: "ChangeLogs",
                column: "GeneralReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_IncidentId",
                table: "ChangeLogs",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_TaskEntityAssignmentId",
                table: "ChangeLogs",
                column: "TaskEntityAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_TaskId",
                table: "ChangeLogs",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_VulnerabilityId",
                table: "ChangeLogs",
                column: "VulnerabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_changedById",
                table: "ChangeLogs",
                column: "changedById");

            migrationBuilder.CreateIndex(
                name: "IX_CloseReports_CreatedById",
                table: "CloseReports",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CloseReports_LastUpdateById",
                table: "CloseReports",
                column: "LastUpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_CommentAttachment_attachmentId",
                table: "CommentAttachment",
                column: "attachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentId",
                table: "Comments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CreatedById",
                table: "Comments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_LastUpdateById",
                table: "Comments",
                column: "LastUpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyName_APTId",
                table: "CompanyName",
                column: "APTId");

            migrationBuilder.CreateIndex(
                name: "IX_Content_APTId",
                table: "Content",
                column: "APTId");

            migrationBuilder.CreateIndex(
                name: "IX_Content_CreatedById",
                table: "Content",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DPEAttachment_VulnerabilityId",
                table: "DPEAttachment",
                column: "VulnerabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_DPEs_AssessmentMethodologyId",
                table: "DPEs",
                column: "AssessmentMethodologyId");

            migrationBuilder.CreateIndex(
                name: "IX_DPEs_AttachmentReportId",
                table: "DPEs",
                column: "AttachmentReportId");

            migrationBuilder.CreateIndex(
                name: "IX_DPEs_ContactedPersonId",
                table: "DPEs",
                column: "ContactedPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_DPEs_CreatedById",
                table: "DPEs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DPEs_LastUpdateById",
                table: "DPEs",
                column: "LastUpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_DPEs_OrganizationId",
                table: "DPEs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_DPEs_StatusId",
                table: "DPEs",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DPEWorkTeam_DpeId",
                table: "DPEWorkTeam",
                column: "DpeId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityAssignments_CreatedById",
                table: "EntityAssignments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EntityAssignments_IncidentId",
                table: "EntityAssignments",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityAssignments_LastUpdateById",
                table: "EntityAssignments",
                column: "LastUpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_EntityAssignments_StatusId",
                table: "EntityAssignments",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityAssignments_UserId",
                table: "EntityAssignments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityStatus_StatusId",
                table: "EntityStatus",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ExploitAffectedSources_ExploitIpTypeId",
                table: "ExploitAffectedSources",
                column: "ExploitIpTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExploitAffectedSources_VulnerabilityId",
                table: "ExploitAffectedSources",
                column: "VulnerabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Exploits_CreatedById",
                table: "Exploits",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Exploits_LastUpdateById",
                table: "Exploits",
                column: "LastUpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Exploits_SeverityId",
                table: "Exploits",
                column: "SeverityId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralReportAttachment_AttachmentId",
                table: "GeneralReportAttachment",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralReports_CreatedById",
                table: "GeneralReports",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralReports_LastUpdateById",
                table: "GeneralReports",
                column: "LastUpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralReports_ReportCategoryId",
                table: "GeneralReports",
                column: "ReportCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralReports_SaverityId",
                table: "GeneralReports",
                column: "SaverityId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralReports_StatusId",
                table: "GeneralReports",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralReports_UrganceyId",
                table: "GeneralReports",
                column: "UrganceyId");

            migrationBuilder.CreateIndex(
                name: "IX_GReportEntityAssignment_CreatedById",
                table: "GReportEntityAssignment",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GReportEntityAssignment_GeneralReportId",
                table: "GReportEntityAssignment",
                column: "GeneralReportId");

            migrationBuilder.CreateIndex(
                name: "IX_GReportEntityAssignment_LastUpdateById",
                table: "GReportEntityAssignment",
                column: "LastUpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_GReportEntityAssignment_StatusId",
                table: "GReportEntityAssignment",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_GReportEntityAssignment_UserId",
                table: "GReportEntityAssignment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentAttachment_IncidentId",
                table: "IncidentAttachment",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_AptId",
                table: "Incidents",
                column: "AptId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_CategoryId",
                table: "Incidents",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_CloseReportId",
                table: "Incidents",
                column: "CloseReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_CreatedById",
                table: "Incidents",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_IncidentCategoryId",
                table: "Incidents",
                column: "IncidentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_LastUpdateById",
                table: "Incidents",
                column: "LastUpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_SaverityId",
                table: "Incidents",
                column: "SaverityId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_StatusId",
                table: "Incidents",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_UrganceyId",
                table: "Incidents",
                column: "UrganceyId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentsComments_CommentId",
                table: "IncidentsComments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_IpAddress_DestId",
                table: "IpAddress",
                column: "DestId");

            migrationBuilder.CreateIndex(
                name: "IX_IpAddress_IncidentId",
                table: "IpAddress",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_IpAddress_OwnerDetailId",
                table: "IpAddress",
                column: "OwnerDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_IpAddress_SourceId",
                table: "IpAddress",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_News_UrganceyId",
                table: "News",
                column: "UrganceyId");

            migrationBuilder.CreateIndex(
                name: "IX_News_UsersId",
                table: "News",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationOwner_NotificationId",
                table: "NotificationOwner",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationOwner_employeeId",
                table: "NotificationOwner",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedById",
                table: "Notifications",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_StatusId",
                table: "Notifications",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationContacts_OrganizationId",
                table: "OrganizationContacts",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationDomains_OrganizationId",
                table: "OrganizationDomains",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationIp_OrganizationId",
                table: "OrganizationIp",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_SectorId1",
                table: "Organizations",
                column: "SectorId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrgsIncidentRel_IncidentId",
                table: "OrgsIncidentRel",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_OriginCountry_APTId",
                table: "OriginCountry",
                column: "APTId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportAttachment_closeReportId",
                table: "ReportAttachment",
                column: "closeReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_DepartmentId",
                table: "Sections",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_CommentId",
                table: "Tags",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_GeneralReportId",
                table: "Tags",
                column: "GeneralReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_aptId",
                table: "Tags",
                column: "aptId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_incidentId",
                table: "Tags",
                column: "incidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_taskId",
                table: "Tags",
                column: "taskId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetedCountry_APTId",
                table: "TargetedCountry",
                column: "APTId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetedSector_AptId",
                table: "TargetedSector",
                column: "AptId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAttachments_TaskId",
                table: "TaskAttachments",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_CommentId",
                table: "TaskComments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEmpsRel_TaskId",
                table: "TaskEmpsRel",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEntityAssignments_CreatedById",
                table: "TaskEntityAssignments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEntityAssignments_LastUpdateById",
                table: "TaskEntityAssignments",
                column: "LastUpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEntityAssignments_StatusId",
                table: "TaskEntityAssignments",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEntityAssignments_TaskId",
                table: "TaskEntityAssignments",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEntityAssignments_UserId",
                table: "TaskEntityAssignments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Assigned_forId",
                table: "Tasks",
                column: "Assigned_forId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ClosingReportId",
                table: "Tasks",
                column: "ClosingReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CreatedById",
                table: "Tasks",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_LastUpdateById",
                table: "Tasks",
                column: "LastUpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ParentIncidentId",
                table: "Tasks",
                column: "ParentIncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ParentTaskId",
                table: "Tasks",
                column: "ParentTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_StatusId",
                table: "Tasks",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskTypeId",
                table: "Tasks",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreatSignature_APTId",
                table: "ThreatSignature",
                column: "APTId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolName_APTId",
                table: "ToolName",
                column: "APTId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivity_UserNameId",
                table: "UserActivity",
                column: "UserNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Vulnerabilities_AssessmentTypeId",
                table: "Vulnerabilities",
                column: "AssessmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vulnerabilities_CreatedById",
                table: "Vulnerabilities",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Vulnerabilities_DpeId",
                table: "Vulnerabilities",
                column: "DpeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vulnerabilities_ExploitId",
                table: "Vulnerabilities",
                column: "ExploitId");

            migrationBuilder.CreateIndex(
                name: "IX_Vulnerabilities_LastUpdateById",
                table: "Vulnerabilities",
                column: "LastUpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Vulnerabilities_ModifiedById",
                table: "Vulnerabilities",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Vulnerabilities_StatusId",
                table: "Vulnerabilities",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_VulnerabilityVerificationPossibilities_VulnerabilityId",
                table: "VulnerabilityVerificationPossibilities",
                column: "VulnerabilityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlternativeName");

            migrationBuilder.DropTable(
                name: "AptAttachment");

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
                name: "AttackStratigie");

            migrationBuilder.DropTable(
                name: "ChangeLogField");

            migrationBuilder.DropTable(
                name: "CommentAttachment");

            migrationBuilder.DropTable(
                name: "CompanyName");

            migrationBuilder.DropTable(
                name: "Content");

            migrationBuilder.DropTable(
                name: "DPEAttachment");

            migrationBuilder.DropTable(
                name: "DPEWorkTeam");

            migrationBuilder.DropTable(
                name: "ExploitAffectedSources");

            migrationBuilder.DropTable(
                name: "GeneralReportAttachment");

            migrationBuilder.DropTable(
                name: "IncidentAttachment");

            migrationBuilder.DropTable(
                name: "IncidentsComments");

            migrationBuilder.DropTable(
                name: "IpAddress");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "NotificationOwner");

            migrationBuilder.DropTable(
                name: "OrganizationDomains");

            migrationBuilder.DropTable(
                name: "OrganizationIp");

            migrationBuilder.DropTable(
                name: "OrgsIncidentRel");

            migrationBuilder.DropTable(
                name: "OriginCountry");

            migrationBuilder.DropTable(
                name: "ReportAttachment");

            migrationBuilder.DropTable(
                name: "StaticIPs");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "TargetedCountry");

            migrationBuilder.DropTable(
                name: "TargetedSector");

            migrationBuilder.DropTable(
                name: "TaskAttachments");

            migrationBuilder.DropTable(
                name: "TaskComments");

            migrationBuilder.DropTable(
                name: "TaskEmpsRel");

            migrationBuilder.DropTable(
                name: "ThreatSignature");

            migrationBuilder.DropTable(
                name: "ToolName");

            migrationBuilder.DropTable(
                name: "UserActivity");

            migrationBuilder.DropTable(
                name: "VulnerabilityVerificationPossibilities");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ChangeLogs");

            migrationBuilder.DropTable(
                name: "ExploitIpTypes");

            migrationBuilder.DropTable(
                name: "Ip");

            migrationBuilder.DropTable(
                name: "OwnerDetails");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "VerificationPossibilities");

            migrationBuilder.DropTable(
                name: "BlogNews");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "EntityAssignments");

            migrationBuilder.DropTable(
                name: "GReportEntityAssignment");

            migrationBuilder.DropTable(
                name: "TaskEntityAssignments");

            migrationBuilder.DropTable(
                name: "Vulnerabilities");

            migrationBuilder.DropTable(
                name: "BlogNewsCategory");

            migrationBuilder.DropTable(
                name: "GeneralReports");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "AssessmentTypes");

            migrationBuilder.DropTable(
                name: "DPEs");

            migrationBuilder.DropTable(
                name: "Exploits");

            migrationBuilder.DropTable(
                name: "ReportCategories");

            migrationBuilder.DropTable(
                name: "Incidents");

            migrationBuilder.DropTable(
                name: "TaskType");

            migrationBuilder.DropTable(
                name: "AssessmentMethodologies");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "OrganizationContacts");

            migrationBuilder.DropTable(
                name: "ExploitSeveritys");

            migrationBuilder.DropTable(
                name: "Apts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "CloseReports");

            migrationBuilder.DropTable(
                name: "IncidentCategories");

            migrationBuilder.DropTable(
                name: "Saverities");

            migrationBuilder.DropTable(
                name: "EntityStatus");

            migrationBuilder.DropTable(
                name: "Urganceys");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Sectors");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
