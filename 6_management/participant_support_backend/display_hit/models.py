from django.db import models


# Create your models here.
class HITAssignment(models.Model):
    worker_id = models.CharField(max_length=120, default=None, blank=True, null=True)
    assignment_id = models.CharField(max_length=120, default=None, blank=True, null=True)
    hit_id = models.CharField(max_length=120, default=None, blank=True, null=True)
    challenge = models.CharField(max_length=120)
    group = models.IntegerField(default=0)
    used = models.BooleanField(default=False)

    def _str_(self):
        return self.challenge
