migration-add:
	dotnet ef migrations add "$(name)" \
	--project ./src/WorkspaceService.Infrastructure \
	--startup-project ./src/WorkspaceService.Api \
	--output-dir $(or $(migrationsPath), Migrations)