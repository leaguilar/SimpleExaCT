from rest_framework.authtoken.models import Token
from django.core.management.base import BaseCommand, CommandError
from django.contrib.auth.models import User

class Command(BaseCommand):
    help = 'Closes the specified poll for voting'

    def add_arguments(self, parser):
        parser.add_argument('--user_id', type=str, required=True)
        parser.add_argument('--token', type=str)

    def handle(self, *args, **options):
        user_id = User.objects.get(username=options['user_id'])
        key = options['token']
        try:
            if key:
                token = Token.objects.create(user=user_id, key=key)
            else:
                token = Token.objects.create(user=user_id)
            print(token.key)
        except Exception as e:
            raise CommandError('Error "%s"' % e)
        self.stdout.write(self.style.SUCCESS(f'Successfully added the token {token.key}'))
