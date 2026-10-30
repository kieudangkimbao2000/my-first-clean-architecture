FROM postgres:18

COPY ./docker/postgres/init.sql /docker-entrypoint-initdb.d/01-init.sql:ro