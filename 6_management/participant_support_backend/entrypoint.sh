#!/bin/bash

python manage.py collectstatic --noinput
python manage.py makemigrations
python manage.py migrate --noinput
python manage.py createsuperuser \
        --noinput \
        --username $DJANGO_SUPERUSER_USERNAME \
        --email $DJANGO_EMAIL
python manage.py add_token \
	--user_id $DJANGO_SUPERUSER_USERNAME \
	--token $DJANGO_SUPERUSER_TOKEN
python manage.py runserver 0.0.0.0:8080
