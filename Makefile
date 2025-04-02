MIGRATIONS_DIR ?= ./src/WorkspaceService.Infrastructure/Migrations

migration-add:
	@current_date=$$(date +%Y%m%d); \
    prefix=Migration$${current_date}; \
    count=$$(ls $(MIGRATIONS_DIR) 2>/dev/null | grep -c "^$${prefix}"); \
    version=$$(printf "%02d" $$((count + 1))); \
    name="$${prefix}$${version}"; \
    echo "Создается миграция: $$name"; \
    dotnet ef migrations add $$name \
    	--project ./src/WorkspaceService.Infrastructure \
    	--startup-project ./src/WorkspaceService.Api \
    	--output-dir $(MIGRATIONS_DIR)