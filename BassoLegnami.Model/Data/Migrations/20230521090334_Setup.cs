using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BassoLegnami.Model.Data.Migrations
{
	/// <inheritdoc />
	public partial class Setup : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "UserNameIndex",
				table: "AspNetUsers");

			migrationBuilder.DropIndex(
				name: "IX_AspNetUserRoles_UserId",
				table: "AspNetUserRoles");

			migrationBuilder.DropIndex(
				name: "RoleNameIndex",
				table: "AspNetRoles");

			migrationBuilder.AddColumn<bool>(
				name: "ChangePassword",
				table: "AspNetUsers",
				type: "bit",
				nullable: false,
				defaultValue: false);

			migrationBuilder.AddColumn<bool>(
				name: "Enabled",
				table: "AspNetUsers",
				type: "bit",
				nullable: false,
				defaultValue: false);

			migrationBuilder.AddColumn<DateTime>(
				name: "LastPasswordChangedDate",
				table: "AspNetUsers",
				type: "datetime2",
				nullable: true);

			migrationBuilder.CreateTable(
				name: "Authorizations",
				columns: table => new
				{
					AuthorizationID = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					RoleId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
					Controller = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Authorized = table.Column<bool>(type: "bit", nullable: false),
					Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
					CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Authorizations", x => x.AuthorizationID);
				});

			migrationBuilder.CreateTable(
				name: "EmailMessages",
				columns: table => new
				{
					EmailMessageID = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Status = table.Column<int>(type: "int", nullable: false),
					RecipientEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
					CCRecipientEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
					BCCRecipientEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
					ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
					SendDate = table.Column<DateTime>(type: "datetime2", nullable: true),
					Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
					CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_EmailMessages", x => x.EmailMessageID);
				});

			migrationBuilder.CreateTable(
				name: "ExternalSystems",
				columns: table => new
				{
					ExternalSystemID = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ExternalSystems", x => x.ExternalSystemID);
				});

			migrationBuilder.CreateTable(
				name: "Festivities",
				columns: table => new
				{
					FestivityID = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Day = table.Column<int>(type: "int", nullable: false),
					Month = table.Column<int>(type: "int", nullable: false),
					Year = table.Column<int>(type: "int", nullable: true),
					City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
					CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Festivities", x => x.FestivityID);
				});

			migrationBuilder.CreateTable(
				name: "FileFolders",
				columns: table => new
				{
					FileFolderID = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Path = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_FileFolders", x => x.FileFolderID);
				});

			migrationBuilder.CreateTable(
				name: "Logs",
				columns: table => new
				{
					LogID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
					EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
					UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Controller = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
					RecordID = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Method = table.Column<string>(type: "nvarchar(max)", nullable: true),
					QueryString = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Logs", x => x.LogID);
				});

			migrationBuilder.CreateTable(
				name: "RecordFilterRuleTypes",
				columns: table => new
				{
					RecordFilterRuleTypeID = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Flag1 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
					CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_RecordFilterRuleTypes", x => x.RecordFilterRuleTypeID);
				});

			migrationBuilder.CreateTable(
				name: "Role",
				columns: table => new
				{
					RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Role", x => x.RoleId);
				});

			migrationBuilder.CreateTable(
				name: "Settings",
				columns: table => new
				{
					SettingID = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Note = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
					CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Settings", x => x.SettingID);
					table.UniqueConstraint("AK_Settings_Key", x => x.Key);
				});

			migrationBuilder.CreateTable(
				name: "ExternalSystemValues",
				columns: table => new
				{
					ExternalSystemValueID = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					ExternalSystemID = table.Column<int>(type: "int", nullable: false),
					Value1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Value2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
					CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ExternalSystemValues", x => x.ExternalSystemValueID);
					table.ForeignKey(
						name: "FK_ExternalSystemValues_ExternalSystems_ExternalSystemID",
						column: x => x.ExternalSystemID,
						principalTable: "ExternalSystems",
						principalColumn: "ExternalSystemID",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Files",
				columns: table => new
				{
					FileID = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
					FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					MimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					FileFolderID = table.Column<int>(type: "int", nullable: false),
					EmailMessageID = table.Column<int>(type: "int", nullable: true),
					CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Files", x => x.FileID);
					table.ForeignKey(
						name: "FK_Files_EmailMessages_EmailMessageID",
						column: x => x.EmailMessageID,
						principalTable: "EmailMessages",
						principalColumn: "EmailMessageID",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Files_FileFolders_FileFolderID",
						column: x => x.FileFolderID,
						principalTable: "FileFolders",
						principalColumn: "FileFolderID",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Errors",
				columns: table => new
				{
					ErrorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LogID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ExceptionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Source = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Errors", x => x.ErrorID);
					table.ForeignKey(
						name: "FK_Errors_Logs_LogID",
						column: x => x.LogID,
						principalTable: "Logs",
						principalColumn: "LogID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "RecordFilterRules",
				columns: table => new
				{
					RecordFilterRuleID = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					RecordFilterRuleTypeID = table.Column<int>(type: "int", nullable: false),
					CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_RecordFilterRules", x => x.RecordFilterRuleID);
					table.ForeignKey(
						name: "FK_RecordFilterRules_RecordFilterRuleTypes_RecordFilterRuleTypeID",
						column: x => x.RecordFilterRuleTypeID,
						principalTable: "RecordFilterRuleTypes",
						principalColumn: "RecordFilterRuleTypeID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "RecordFilterRuleValues",
				columns: table => new
				{
					RecordFilterRuleValueID = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					RecordFilterRuleID = table.Column<int>(type: "int", nullable: false),
					RecordID = table.Column<int>(type: "int", nullable: false),
					CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_RecordFilterRuleValues", x => x.RecordFilterRuleValueID);
					table.ForeignKey(
						name: "FK_RecordFilterRuleValues_RecordFilterRules_RecordFilterRuleID",
						column: x => x.RecordFilterRuleID,
						principalTable: "RecordFilterRules",
						principalColumn: "RecordFilterRuleID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "UserNameIndex",
				table: "AspNetUsers",
				column: "NormalizedUserName",
				unique: true,
				filter: "[NormalizedUserName] IS NOT NULL");

			migrationBuilder.CreateIndex(
				name: "RoleNameIndex",
				table: "AspNetRoles",
				column: "NormalizedName",
				unique: true,
				filter: "[NormalizedName] IS NOT NULL");

			migrationBuilder.CreateIndex(
				name: "IX_Errors_LogID",
				table: "Errors",
				column: "LogID");

			migrationBuilder.CreateIndex(
				name: "IX_ExternalSystemValues_ExternalSystemID",
				table: "ExternalSystemValues",
				column: "ExternalSystemID");

			migrationBuilder.CreateIndex(
				name: "IX_Files_EmailMessageID",
				table: "Files",
				column: "EmailMessageID");

			migrationBuilder.CreateIndex(
				name: "IX_Files_FileFolderID",
				table: "Files",
				column: "FileFolderID");

			migrationBuilder.CreateIndex(
				name: "IX_RecordFilterRules_RecordFilterRuleTypeID",
				table: "RecordFilterRules",
				column: "RecordFilterRuleTypeID");

			migrationBuilder.CreateIndex(
				name: "IX_RecordFilterRuleValues_RecordFilterRuleID",
				table: "RecordFilterRuleValues",
				column: "RecordFilterRuleID");

			migrationBuilder.AddForeignKey(
				name: "FK_AspNetUserTokens_AspNetUsers_UserId",
				table: "AspNetUserTokens",
				column: "UserId",
				principalTable: "AspNetUsers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_AspNetUserTokens_AspNetUsers_UserId",
				table: "AspNetUserTokens");

			migrationBuilder.DropTable(
				name: "Authorizations");

			migrationBuilder.DropTable(
				name: "Errors");

			migrationBuilder.DropTable(
				name: "ExternalSystemValues");

			migrationBuilder.DropTable(
				name: "Festivities");

			migrationBuilder.DropTable(
				name: "Files");

			migrationBuilder.DropTable(
				name: "RecordFilterRuleValues");

			migrationBuilder.DropTable(
				name: "Role");

			migrationBuilder.DropTable(
				name: "Settings");

			migrationBuilder.DropTable(
				name: "Logs");

			migrationBuilder.DropTable(
				name: "ExternalSystems");

			migrationBuilder.DropTable(
				name: "EmailMessages");

			migrationBuilder.DropTable(
				name: "FileFolders");

			migrationBuilder.DropTable(
				name: "RecordFilterRules");

			migrationBuilder.DropTable(
				name: "RecordFilterRuleTypes");

			migrationBuilder.DropIndex(
				name: "UserNameIndex",
				table: "AspNetUsers");

			migrationBuilder.DropIndex(
				name: "RoleNameIndex",
				table: "AspNetRoles");

			migrationBuilder.DropColumn(
				name: "ChangePassword",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "Enabled",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "LastPasswordChangedDate",
				table: "AspNetUsers");

			migrationBuilder.CreateIndex(
				name: "UserNameIndex",
				table: "AspNetUsers",
				column: "NormalizedUserName",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserRoles_UserId",
				table: "AspNetUserRoles",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "RoleNameIndex",
				table: "AspNetRoles",
				column: "NormalizedName");
		}
	}
}
