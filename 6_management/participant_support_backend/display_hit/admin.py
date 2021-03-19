from django.contrib import admin

# Register your models here.
from .models import HITAssignment

class HitAdmin(admin.ModelAdmin):
    list_display = ('worker_id' , 'assignment_id','challenge','used')  
    
admin.site.register(HITAssignment,HitAdmin)
